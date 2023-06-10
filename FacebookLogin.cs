using Facebook.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacebookLogin : MonoBehaviour
{
	public static int DEFAULT_COINS = 5000;

	public static int DEFAULT_GEMS = 15;

	public static int SHARE_COINS = 500;

	public static int INVITE_COINS = 2000;

	public static int FACEBOOK_PROFILE_AVATAR = -1;

	public static string SHARE_URL = "YOUR WEBSITE HERE";

	public Image uPic;

	public Text playerName;

	public Text playerCoins;

	public Text playerGems;

	public InputField playerNameInput;

	public Text logView;

	public GameObject[] avatars;

	public GameObject selectedAvatar;

	public Color avatarSelectedColor;

	public Color avatarUnSelectedColor;

	public GameObject loginPanel;

	public GameObject menuPanel;

	public GameObject helpmenuPanel;

	public GameObject profilePanel;

	public GameObject editProfilePanel;

	public GameObject rewardCotainer;

	public GameObject leaderBoardPanel;

	public GameObject loader;

	public GameObject settingsPanel;

	public Sprite defaultAvatar;

	public Sprite logInSprite;

	public Sprite logOutSprite;

	public Image facebookButton;

	public Animator anim;

	private int selectedAvatarIndex;

	private bool isLoginFromSettings;

	public bool rewardExistingPlayerConfig;

	public int rewardCoins;

	public Text rewardCoinsText;

	public GameObject rewardPanel;

	private const string EXISTING_KEY = "existing_player_v1.5";

	private int lastRewardCoin;

	public Text coinsText;

	public Animator masterStrikerAnimator;

	private void Awake()
	{
		if (!FB.IsInitialized)
		{
			FB.Init(InitFacebookCallBack);
		}
		else
		{
			InitFacebookCallBack();
		}
	}

	private void Start()
	{
		string text = getPlayerName();
		selectedAvatarIndex = PlayerPrefs.GetInt("avatar", 0);
		if (selectedAvatarIndex >= 0)
		{
			setPlayerImage();
		}
		if (!string.IsNullOrEmpty(text))
		{
			anim.SetTrigger("fade");
			loginPanel.SetActive(value: false);
			showMenu();
			profilePanel.SetActive(value: true);
			rewardCotainer.SetActive(value: true);
			playerName.text = text;
			playerNameInput.text = text;
			if (PlayerPrefs.GetInt("existing_player_v1.5", 1) == 1)
			{
				AudioManager.getInstance().PlaySound(AudioManager.GAME_OVER);
				PlayerPrefs.SetInt("existing_player_v1.5", 0);
				rewardPanel.SetActive(value: true);
				rewardCoinsText.text = rewardCoins + string.Empty;
			}
			setPlayerCoins();
		}
		else
		{
			loginPanel.SetActive(value: true);
			PlayerPrefs.SetInt("existing_player_v1.5", 0);
		}
	}

	public void Update()
	{
		if (lastRewardCoin != rewardCoins && rewardPanel.activeSelf)
		{
			lastRewardCoin = rewardCoins;
			rewardCoinsText.text = rewardCoins + string.Empty;
		}
	}

	public void CollectRewardCoins()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_COIN_COLLECT);
		int @int = PlayerPrefs.GetInt("coins", DEFAULT_COINS);
		@int += rewardCoins;
		PlayerPrefs.SetInt("coins", @int);
		coinsText.text = MenuManager.getRepresentationcoins(@int);
		rewardPanel.SetActive(value: false);
	}

	private void setPlayerImage()
	{
		selectedAvatar = avatars[(selectedAvatarIndex != -1) ? selectedAvatarIndex : 0];
		selectedAvatar.GetComponent<Image>().color = avatarSelectedColor;
		uPic.sprite = selectedAvatar.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite;
		SavePlayerAvatar(uPic.sprite);
	}

	private void SavePlayerAvatar(Sprite sprite)
	{
		if (LevelManager.instance != null)
		{
			LevelManager.instance.SetPlayerImage(sprite);
		}
	}

	private void InitFacebookCallBack()
	{
		if (FB.IsLoggedIn)
		{
			loadProfileName();
			loadProfilePhoto();
			checkRequestIDPresent();
		}
		setFacebookButtonImage();
	}

	public void Login()
	{
		leaderBoardPanel.SetActive(value: false);
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		FB.LogInWithReadPermissions(null, FacebookLoginCallBack);
	}

	public void PlayAsGuest()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		int num = UnityEngine.Random.Range(1000, 10000);
		string value = "Guest" + num;
		PlayerPrefs.SetString("name", value);
		PlayerPrefs.SetInt("avatar", 0);
		setPlayerName(value);
		avatars[0].GetComponent<Image>().color = avatarSelectedColor;
		uPic.sprite = avatars[0].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite;
		SavePlayerAvatar(uPic.sprite);
		loginPanel.SetActive(value: false);
		showMenu();
		profilePanel.SetActive(value: true);
		rewardCotainer.SetActive(value: true);
		setPlayerCoins();
	}

	private void showMenu()
	{
		if ((PlayerPrefs.GetInt("show_help", 1) == 1) ? true : false)
		{
			helpmenuPanel.SetActive(value: true);
		}
		else
		{
			menuPanel.SetActive(value: true);
		}
	}

	public void FacebookLoginCallBack(ILoginResult result)
	{
		if (result.Error == null)
		{
			if (result.ResultDictionary.ContainsKey("cancelled") && (bool)result.ResultDictionary["cancelled"])
			{
				loginPanel.SetActive(value: true);
				return;
			}
			loginPanel.SetActive(value: false);
			PlayerPrefs.SetString("name", null);
			PlayerPrefs.SetInt("avatar", -1);
			LevelManager.instance.SetPlayerImage(null);
			LeaderBoardManager instance = LeaderBoardManager.getInstance();
			if (instance != null)
			{
				instance.ResetLoaderFlag();
				loader.SetActive(value: true);
				instance.GetUserScore();
			}
			else
			{
				loadProfileName();
				loadProfilePhoto();
			}
			checkRequestIDPresent();
			setFacebookButtonImage();
		}
		else
		{
			UnityEngine.Debug.Log("Error");
			loginPanel.SetActive(value: true);
		}
	}

	public void getUserFacebookData()
	{
		loadProfileName();
		loadProfilePhoto();
	}

	private void setFacebookButtonImage()
	{
		if (FB.IsLoggedIn)
		{
			facebookButton.sprite = logOutSprite;
		}
		else
		{
			facebookButton.sprite = logInSprite;
		}
	}

	private string getPlayerName()
	{
		return PlayerPrefs.GetString("name", null);
	}

	private void loadProfileName()
	{
		string value = getPlayerName();
		if (string.IsNullOrEmpty(value))
		{
			FB.API("me?fields=first_name", HttpMethod.GET, NameCallBack);
		}
	}

	public void loadProfilePhoto()
	{
		if (LevelManager.instance != null && LevelManager.instance.GetPlayerImage() != null)
		{
			avatars[0].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = LevelManager.instance.GetPlayerImage();
			int @int = PlayerPrefs.GetInt("avatar", -1);
			if (@int == -1)
			{
				uPic.sprite = LevelManager.instance.GetPlayerImage();
				SavePlayerAvatar(uPic.sprite);
			}
		}
		else
		{
			FB.API("me/picture?type=square&width=100&height=100&redirect=false", HttpMethod.GET, ProfiilePicCallBack);
		}
	}

	public void ProfiilePicCallBack(IGraphResult result)
	{
		UnityEngine.Debug.Log(result.RawResult);
		if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
		{
			IDictionary dictionary = result.ResultDictionary["data"] as IDictionary;
			string url = dictionary["url"] as string;
			StartCoroutine(fetchProfilePic(url));
		}
		else
		{
			UnityEngine.Debug.Log("ProfiilePicCallBack Error");
		}
	}

	private IEnumerator fetchProfilePic(string url)
	{
		WWW www = new WWW(url);
		yield return www;
		int playerAvatarId = PlayerPrefs.GetInt("avatar", -1);
		Sprite sprite = Sprite.Create(www.texture, new Rect(0f, 0f, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
		if (playerAvatarId == -1)
		{
			uPic.sprite = sprite;
			selectedAvatar = avatars[0];
		}
		if (playerAvatarId == -1)
		{
			avatars[0].GetComponent<Image>().color = avatarSelectedColor;
		}
		else
		{
			selectedAvatar.GetComponent<Image>().color = avatarUnSelectedColor;
			selectedAvatar = avatars[playerAvatarId];
			selectedAvatar.GetComponent<Image>().color = avatarSelectedColor;
		}
		avatars[0].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = sprite;
		LevelManager.instance.SetPlayerImage(sprite);
	}

	private void checkRequestIDPresent()
	{
		FB.GetAppLink(delegate(IAppLinkResult result)
		{
			if (!string.IsNullOrEmpty(result.Url))
			{
				int num = new Uri(result.Url).Query.IndexOf("request_ids");
				Text text = logView;
				string text2 = text.text;
				text.text = text2 + "GetAppLink index:" + num + "\n";
			}
			Text text3 = logView;
			text3.text = text3.text + "GetAppLink:" + result.RawResult + "\n";
			string query = new Uri(result.Url).Query;
			int num2 = query.IndexOf("request_ids") + "request_ids=".Length;
			int num3 = query.IndexOf("&ref=");
			string str = query.Substring(num2, num3 - num2);
			FB.API("/" + str + "_" + AccessToken.CurrentAccessToken.UserId, HttpMethod.GET, RequestCallBack);
		});
	}

	public void RequestCallBack(IGraphResult result)
	{
		if (string.IsNullOrEmpty(result.Error))
		{
			UnityEngine.Debug.LogError("Req Result:\n" + result.RawResult);
			IDictionary<string, object> resultDictionary = result.ResultDictionary;
			if (resultDictionary.ContainsKey("data"))
			{
				UnityEngine.Debug.LogError(resultDictionary["data"]);
				string code = (string)resultDictionary["data"];
				startPrivateMatch(code);
			}
			if (resultDictionary.ContainsKey("id"))
			{
				FB.API("/" + resultDictionary["id"], HttpMethod.DELETE);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Eror in request" + result.Error);
		}
	}

	private void startPrivateMatch(string code)
	{
		if (!code.Contains("invite"))
		{
			MenuManager menuManager = UnityEngine.Object.FindObjectOfType<MenuManager>();
			if (menuManager != null)
			{
				menuManager.OpenGameRoom(code);
			}
		}
	}

	private void getFriends()
	{
		FB.API("me/friends", HttpMethod.GET, FriendsResultCallBack);
	}

	public void FriendsResultCallBack(IGraphResult result)
	{
	}

	public void NameCallBack(IGraphResult result)
	{
		UnityEngine.Debug.Log(result.RawResult);
		string text = (string)result.ResultDictionary["first_name"];
		PlayerPrefs.SetString("name", text);
		LeaderBoardManager instance = LeaderBoardManager.getInstance();
		if (instance != null)
		{
			instance.UpdatePlayerDetail(text, PlayerPrefs.GetInt("avatar", -1));
		}
		if (isLoginFromSettings)
		{
			showHomeScreen();
		}
		else
		{
			showHomeScreen();
		}
	}

	public void showHomeScreen()
	{
		UnityEngine.Debug.LogError("showHomeScreen");
		int num = UnityEngine.Random.Range(1000, 10000);
		string defaultValue = "Guest" + num;
		setPlayerName(PlayerPrefs.GetString("name", defaultValue));
		setPlayerCoins();
		setPlayerAvatar();
		settingsPanel.SetActive(value: false);
		loginPanel.SetActive(value: false);
		showMenu();
		profilePanel.SetActive(value: true);
		rewardCotainer.SetActive(value: true);
		loader.SetActive(value: false);
		CanAnimateMasterStrike();
	}

	private void CanAnimateMasterStrike()
	{
		if (MenuManager.IsMasterStrikeReady())
		{
			masterStrikerAnimator.enabled = true;
			masterStrikerAnimator.gameObject.transform.GetChild(0).gameObject.SetActive(value: true);
		}
	}

	private void setPlayerAvatar()
	{
		int @int = PlayerPrefs.GetInt("avatar", -1);
		if (@int != -1)
		{
			avatars[@int].GetComponent<Image>().color = avatarSelectedColor;
			uPic.sprite = avatars[@int].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite;
			SavePlayerAvatar(uPic.sprite);
		}
	}

	public void Invite()
	{
		
	}

	public void Challenge()
	{
		FB.AppRequest("Download Carrom Online", null, null, null, 2, "invite", "Invite & Get Coins", RequestResponse);
	}

	public void ShareFacebook()
	{
		FB.ShareLink(new Uri(SHARE_URL), "Carrom Online", "Download Now!", new Uri("LINK TO PICTURE"), delegate(IShareResult result)
		{
			Text text = logView;
			text.text = text.text + "Request success:" + result.RawResult + "\n";
			if (result.Error == null && (bool)result.ResultDictionary["posted"])
			{
				MenuManager menuManager = UnityEngine.Object.FindObjectOfType<MenuManager>();
				if (menuManager != null)
				{
					int @int = PlayerPrefs.GetInt("coins", DEFAULT_COINS);
					@int += SHARE_COINS;
					PlayerPrefs.SetInt("coins", @int);
					LeaderBoardManager instance = LeaderBoardManager.getInstance();
					if (instance != null)
					{
						instance.UpdatePlayerPoints(instance.FACEBOOK_SHARE_XP);
					}
					int xp = (!(instance != null)) ? 300 : instance.FACEBOOK_SHARE_XP;
					LeaderBoardManager.synchPlayerCoins(@int);
					menuManager.showCoinRewardPanel(SHARE_COINS, xp);
				}
			}
		});
	}

	public void RequestResponse(IAppRequestResult result)
	{
		if (result.Cancelled)
		{
			logView.text += "Request Cancelled\n";
			return;
		}
		if (!string.IsNullOrEmpty(result.Error))
		{
			Text text = logView;
			text.text = text.text + "Request Error:" + result.Error + "\n";
			return;
		}
		string text2 = (string)result.ResultDictionary["to"];
		string[] array = text2.Split(',');
		Text text3 = logView;
		text3.text = text3.text + "Request success:" + result.RawResult + "\n";
		Text text4 = logView;
		text4.text = text4.text + array.Length + " coins\n";
		MenuManager menuManager = UnityEngine.Object.FindObjectOfType<MenuManager>();
		if (menuManager != null)
		{
			int @int = PlayerPrefs.GetInt("coins", DEFAULT_COINS);
			@int += array.Length * INVITE_COINS;
			PlayerPrefs.SetInt("coins", @int);
			LeaderBoardManager instance = LeaderBoardManager.getInstance();
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.INVITE_FRIEND_XP);
			}
			LeaderBoardManager.synchPlayerCoins(@int);
			int xp = (!(instance != null)) ? 300 : instance.INVITE_FRIEND_XP;
			menuManager.showCoinRewardPanel(array.Length * INVITE_COINS, xp);
		}
	}

	private void setPlayerName(string name)
	{
		playerName.text = name;
		playerNameInput.text = name;
	}

	private void setPlayerCoins()
	{
		int @int = PlayerPrefs.GetInt("coins", DEFAULT_COINS);
		playerCoins.text = MenuManager.getRepresentationcoins(@int);
		setGems();
	}

	private void setGems()
	{
		int @int = PlayerPrefs.GetInt("gems", DEFAULT_GEMS);
		playerGems.text = @int.ToString();
	}

	public void showEditProfilePanel()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		anim.SetTrigger("fade");
		editProfilePanel.SetActive(value: true);
	}

	public void closeEditProfilePanel()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		editProfilePanel.SetActive(value: false);
	}

	public void savePlayerName()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		string text = playerNameInput.text;
		if (!string.IsNullOrEmpty(text))
		{
			PlayerPrefs.SetString("name", text);
			setPlayerName(text);
			editProfilePanel.SetActive(value: false);
			PlayerPrefs.SetInt("avatar", selectedAvatarIndex);
			setPlayerImage();
			LeaderBoardManager instance = LeaderBoardManager.getInstance();
			if (instance != null)
			{
				instance.UpdatePlayerDetail(text, selectedAvatarIndex);
			}
		}
	}

	public void OnAvatarClicked(GameObject avatar)
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		if (selectedAvatar != null)
		{
			selectedAvatar.GetComponent<Image>().color = avatarUnSelectedColor;
		}
		selectedAvatar = avatar;
		selectedAvatar.GetComponent<Image>().color = avatarSelectedColor;
		selectedAvatarIndex = getAvatarIndex(avatar);
		if (selectedAvatarIndex == 0 && FB.IsLoggedIn)
		{
			selectedAvatarIndex = -1;
		}
	}

	private int getAvatarIndex(GameObject avatar)
	{
		for (int i = 0; i < avatars.Length; i++)
		{
			if (avatars[i] == avatar)
			{
				return i;
			}
		}
		return 0;
	}

	public void FacebookButtonClick()
	{
		if (FB.IsInitialized)
		{
			if (FB.IsLoggedIn)
			{
				FB.LogOut();
				int num = UnityEngine.Random.Range(1000, 10000);
				string value = "Guest" + num;
				PlayerPrefs.SetString("name", value);
				PlayerPrefs.SetInt("avatar", 0);
				PlayerPrefs.SetInt("gems", 0);
				setPlayerName(value);
				selectedAvatarIndex = 0;
				avatars[0].GetComponent<Image>().color = avatarSelectedColor;
				selectedAvatar.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = defaultAvatar;
				uPic.sprite = selectedAvatar.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite;
				SavePlayerAvatar(uPic.sprite);
				facebookButton.sprite = logInSprite;
				AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
			}
			else
			{
				isLoginFromSettings = true;
				Login();
			}
		}
	}
}
