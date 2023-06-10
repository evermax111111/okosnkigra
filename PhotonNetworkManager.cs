using ExitGames.Client.Photon;
using Facebook.Unity;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
{
	private string gameVersion = "carrom_v1";

	public Color firstPlayerColor;

	public StrikerMover strikerMoverBottom;

	public StrikerMover strikerMoverTop;

	public GameObject roomJoinFailedPanel;

	public GameObject disconnectedPanel;

	public GameObject shareRoomIdPanel;

	public GameObject playOffline;

	public GameObject disconnect;

	public Text matchcode;

	public Text searchingText;

	public Text errorText;

	public Animator playerSearchingAnim;

	public PunLogLevel loglevel = PunLogLevel.Informational;

	public bool isFirstPlayer;

	private StrikerAnimator strikerAnimator;

	private CarromGameManager gamemanager;

	[Tooltip("The maximum number of players per room")]
	[SerializeField]
	private byte maxPlayersPerRoom = 2;

	private int roomId = -1;

	public bool connecToGameServer = true;

	private void Start()
	{
		gamemanager = UnityEngine.Object.FindObjectOfType<CarromGameManager>();
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_TICK);
		PhotonNetwork.LogLevel = loglevel;
		PhotonNetwork.NetworkingClient.LoadBalancingPeer.QuickResendAttempts = 3;
		PhotonNetwork.NetworkingClient.LoadBalancingPeer.SentCountAllowance = 7;
		PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 60000;
		Connect();
	}

	private void Update()
	{
	}

	private void Connect()
	{
		if (PhotonNetwork.IsConnected)
		{
			if (LevelManager.getInstance() != null)
			{
				gameVersion = LevelManager.getInstance().photonVersion;
			}
			PhotonNetwork.GameVersion = gameVersion;
			LogFeedback("Joining Room..." + PhotonNetwork.GameVersion);
			CreateRoom();
		}
		else
		{
			if (LevelManager.getInstance() != null)
			{
				gameVersion = LevelManager.getInstance().photonVersion;
			}
			PhotonNetwork.GameVersion = gameVersion;
			LogFeedback("Connecting..." + PhotonNetwork.GameVersion);
			PhotonNetwork.ConnectUsingSettings();
		}
		if (LevelManager.getInstance() != null && LevelManager.getInstance().gameMode == LevelManager.GameMode.CREATE_ROOM)
		{
			searchingText.text = "Creating Room...";
		}
	}

	private void SetPlayerProperties()
	{
		PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("name");
		int @int = PlayerPrefs.GetInt("avatar", 0);
		Hashtable hashtable = new Hashtable();
		hashtable.Add("pic", (!FB.IsLoggedIn || @int != FacebookLogin.FACEBOOK_PROFILE_AVATAR) ? ("Empty:" + @int) : AccessToken.CurrentAccessToken.UserId);
		hashtable.Add("striker", PlayerPrefs.GetInt("striker", 0));
		PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
	}

	private void CreateRoom()
	{
		LevelManager instance = LevelManager.getInstance();
		SetPlayerProperties();
		if (instance.gameMode == LevelManager.GameMode.MULTIPLAYER)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("coin", 2);
			PhotonNetwork.JoinRandomRoom(null, maxPlayersPerRoom);
		}
		else if (instance.gameMode == LevelManager.GameMode.CREATE_ROOM)
		{
			searchingText.text = "Creating Room...";
			if (roomId != -1)
			{
				LogFeedback("Rejoining");
				RoomOptions roomOptions = new RoomOptions();
				roomOptions.MaxPlayers = maxPlayersPerRoom;
				roomOptions.IsVisible = false;
				roomOptions.IsOpen = true;
				PhotonNetwork.JoinRoom(roomId.ToString());
			}
			else
			{
				int num = roomId = UnityEngine.Random.Range(1000, 10000);
				LogFeedback("Creatin Room " + num);
				RoomOptions roomOptions2 = new RoomOptions();
				roomOptions2.MaxPlayers = maxPlayersPerRoom;
				roomOptions2.IsVisible = false;
				roomOptions2.IsOpen = true;
				roomOptions2.EmptyRoomTtl = 120000;
				PhotonNetwork.JoinOrCreateRoom(num.ToString(), roomOptions2, null);
			}
		}
		else if (instance.gameMode == LevelManager.GameMode.JOIN_ROOM)
		{
			PhotonNetwork.JoinRoom(instance.roomName);
		}
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		LogFeedback("OnPhotonJoinRoomFailed");
		if (LevelManager.getInstance().gameMode != 0)
		{
			playerSearchingAnim.enabled = false;
			AudioManager.getInstance().StopSound(AudioManager.PLAY_TICK);
			roomJoinFailedPanel.SetActive(value: true);
		}
	}

	private void LogFeedback(string message)
	{
		UnityEngine.Debug.LogError(message);
	}

	public override void OnConnectedToMaster()
	{
		LogFeedback("OnConnectedToMaster: Next -> try to Join UnityEngine.Random Room");
		UnityEngine.Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");
		if (connecToGameServer)
		{
			CreateRoom();
		}
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
		UnityEngine.Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
		PhotonNetwork.CreateRoom(null, new RoomOptions
		{
			MaxPlayers = maxPlayersPerRoom
		});
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		playerSearchingAnim.enabled = false;
		AudioManager.getInstance().StopSound(AudioManager.PLAY_TICK);
		LogFeedback("<Color=Red>OnDisconnected</Color> " + cause);
		errorText.text = cause.ToString();
		LogFeedback("PUN Basics Tutorial/Launcher:Disconnected");
		disconnectedPanel.SetActive(value: true);
	}

	public override void OnJoinedRoom()
	{
		LogFeedback("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
		UnityEngine.Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");
		if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
		{
			UnityEngine.Debug.Log("Joined: " + PhotonNetwork.LocalPlayer.ActorNumber);
			HandleRoomJoined();
		}
		else
		{
			CloseTheRoom();
			shareRoomIdPanel.SetActive(value: false);
			playerSearchingAnim.enabled = false;
			AudioManager.getInstance().StopSound(AudioManager.PLAY_TICK);
			AudioManager.getInstance().PlaySound(AudioManager.PLAY_PLAYER_JOINED);
			gamemanager.OpponentJoined();
		}
		Invoke("ShowOfflineButton", LevelManager.getInstance().showOfflineMatchButton);
	}

	private void ShowOfflineButton()
	{
		playOffline.SetActive(value: true);
	}

	private void HandleRoomJoined()
	{
		LevelManager instance = LevelManager.getInstance();
		if (instance.gameMode == LevelManager.GameMode.CREATE_ROOM && PhotonNetwork.IsMasterClient)
		{
			shareRoomIdPanel.SetActive(value: true);
			matchcode.text = PhotonNetwork.CurrentRoom.Name;
		}
	}

	/*private void OnPlayerConnected(NetworkPlayer player)
	{
		Debug.Log("OnPlayerConnected" + player.guid);
	}*/

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		shareRoomIdPanel.SetActive(value: false);
		playerSearchingAnim.enabled = false;
		AudioManager.getInstance().StopSound(AudioManager.PLAY_TICK);
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_PLAYER_JOINED);
		LogFeedback("OnPlayerEnteredRoom" + newPlayer.ActorNumber);
		CloseTheRoom();
		gamemanager.StartGame(newPlayer);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		LogFeedback("OnPlayerLeftRoom" + otherPlayer.ActorNumber);
		if (!gamemanager.isGameOver)
		{
			AudioManager.getInstance().PlaySound(AudioManager.PLAYER_LEFT);
			disconnect.SetActive(value: true);
			Invoke("ShowGameOver", 1f);
		}
	}

	private void ShowGameOver()
	{
		CarromGameManager.Instance.GameOver(PhotonNetwork.LocalPlayer.ActorNumber);
	}

	private void CloseTheRoom()
	{
		PhotonNetwork.CurrentRoom.IsVisible = false;
		PhotonNetwork.CurrentRoom.IsOpen = false;
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		LogFeedback("OnCreateRoomFailed");
		CreateRoom();
	}

	public void Reconnect()
	{
		PhotonNetwork.ReconnectAndRejoin();
	}
}
