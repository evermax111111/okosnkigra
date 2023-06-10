using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OfflineGameManager : MonoBehaviour, IPunTurnManagerCallbacks
{
	public class OfflinePlayer
	{
		public PuckColor.Color color;

		public int score;

		public bool playerPutQueen;

		public OfflinePlayer(PuckColor.Color color)
		{
			this.color = color;
		}
	}

	private const int FIRST_PLAYER = 0;

	private const int SECOND_PLAYER = 1;

	public Color disabledColor;

	public Color enabledColor;

	public Sprite whiteCoin;

	public Sprite blackCoin;

	public GameObject playerStriker;

	public GameObject opponentStriker;

	public GameObject blackPuck;

	public GameObject whitePuck;

	public GameObject redPuck;

	public GameObject redPuckOffline;

	public GameObject strikerMoverUp;

	public GameObject strikerMoverDown;

	private SpriteRenderer strikerMoverUpRender;

	private SpriteRenderer strikerMoverDownRender;

	public SpriteRenderer strikerStripUpRender;

	public SpriteRenderer strikerStripDownRender;

	private Collider2D strikerMoverUpCollider;

	private Collider2D strikerMoverDownCollider;

	public Text leftScoreView;

	public Text rightScoreView;

	public Image playerCoin;

	public Image opponentCoin;

	public Image playerImageView;

	public Image opponentImageView;

	public Text playerNameView;

	public Text opponentNameView;

	public Text foulDetail;

	private Text playerScoreView;

	private Text opponentScoreView;

	public static OfflineGameManager Instance;

	public GameObject movementPadBottom;

	public GameObject movementPadTop;

	public GameObject gameOverPanel;

	public GameObject gameOverPartilce;

	public GameObject quitPanel;

	public GameObject leftAvatar;

	public GameObject rightAvatar;

	public Animator foulAnimator;

	public Text queenRecoverLeft;

	public Text queenRecoverRight;

	public Transform restPositionTop;

	public Transform restPositionBottom;

	public CarromCoin[] puckPositions;

	public List<Puck> pucks = new List<Puck>();

	private OfflinePlayer player;

	private OfflinePlayer opponentPlayer;

	private OfflinePlayer playingPlayer;

	private OfflineStrikerAnimator playerStrikerAnimator;

	private OfflineStrikerAnimator opponentStrikerAnimator;

	public bool isPlayerTurn = true;

	private bool strikedAnyPucks;

	private PunTurnManager turnManager;

	private float TURN_DELAY = 30f;

	private Image currentLoader;

	public Image playerLoader;

	public Image opponentLoader;

	public Animator queenPuckAnimator;

	public Animator queenPuckAnimatorRight;

	private List<GoaledPuck> goaledColors;

	private StrikerMover downStrikerMover;

	private StrikerMover upStrikerMover;

	public Button GOShareButton;

	private float fillAmount;

	private float startTime;

	private bool IsPlayerWon;

	private bool isGameOver;

	private int winscore;

	private bool queenAcquired;

	public Transform redCoinLeft;

	public Transform redCoinRight;

	private GameObject queenPuck;

	private bool timesUp;

	public GameObject home;

	public GameObject share;

	private void Start()
	{
		downStrikerMover = strikerMoverDown.GetComponent<StrikerMover>();
		upStrikerMover = strikerMoverUp.GetComponent<StrikerMover>();
		strikerMoverUpRender = strikerMoverUp.GetComponent<SpriteRenderer>();
		strikerMoverDownRender = strikerMoverDown.GetComponent<SpriteRenderer>();
		strikerMoverUpCollider = strikerMoverUp.GetComponent<CircleCollider2D>();
		strikerMoverDownCollider = strikerMoverDown.GetComponent<CircleCollider2D>();
		SetWinScore((puckPositions.Length - 1) / 2);
		Instance = this;
		goaledColors = new List<GoaledPuck>();
		StartGame();
	}

	private void Update()
	{
		fillAmount = 1f - Mathf.Abs(startTime - Time.fixedTime) / LevelManager.getInstance().turnTime;
		if (!timesUp && currentLoader != null)
		{
			if (fillAmount < 0f)
			{
				OnTurnTimeEnds(0);
			}
			if (fillAmount > 0f)
			{
				currentLoader.fillAmount = fillAmount;
			}
		}
	}

	private void AddTunrManager()
	{
		turnManager = base.gameObject.AddComponent<PunTurnManager>();
		turnManager.TurnManagerListener = this;
		TURN_DELAY = LevelManager.getInstance().turnTime;
		turnManager.TurnDuration = TURN_DELAY;
	}

	public void SetStrikerAnimator(OfflineStrikerAnimator strikerAnimator)
	{
		playerStrikerAnimator = strikerAnimator;
	}

	public void SetOpponentStrikerAnimator(OfflineStrikerAnimator strikerAnimator)
	{
		opponentStrikerAnimator = strikerAnimator;
	}

	public void StartGame()
	{
		int num = 0;
		InstantiatePucks();
		InstantiateOpponetStriker();
		BeginGame(0);
		PlayerTurn();
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_PLAYER_TURN);
	}

	private void InstantiatePucks()
	{
		CarromCoin[] array = puckPositions;
		GameObject gameObject;
		foreach (CarromCoin carromCoin in array)
		{
			gameObject = UnityEngine.Object.Instantiate(carromCoin.prefab, carromCoin.prefab.transform.position, Quaternion.identity);
			PuckColor component = gameObject.GetComponent<PuckColor>();
			pucks.Add(new Puck(gameObject.GetComponent<Rigidbody2D>(), component));
		}
		gameObject = playerStriker;
		playerStriker.GetComponent<SpriteRenderer>().sprite = LevelManager.instance.strikers[PlayerPrefs.GetInt("striker", 0)];
		playerStrikerAnimator = gameObject.GetComponent<OfflineStrikerAnimator>();
		SetStrikerAnimator(playerStrikerAnimator);
		gameObject.GetComponent<CircleCollider2D>().enabled = true;
		downStrikerMover.SetStriker(gameObject, PlayerPrefs.GetInt("striker"));
	}

	private void InstantiateOpponetStriker()
	{
		GameObject gameObject = opponentStriker;
		opponentStriker.GetComponent<SpriteRenderer>().sprite = LevelManager.instance.strikers[PlayerPrefs.GetInt("striker", 0)];
		gameObject.GetComponent<CircleCollider2D>().enabled = true;
		opponentStrikerAnimator = gameObject.GetComponent<OfflineStrikerAnimator>();
		SetOpponentStrikerAnimator(opponentStrikerAnimator);
		upStrikerMover.SetStriker(gameObject, PlayerPrefs.GetInt("striker"));
	}

	private void BeginGame(int playetToPlay)
	{
		if (playetToPlay == 0)
		{
			PuckColor.Color color = (playetToPlay == 0) ? PuckColor.Color.WHITE : PuckColor.Color.BLACK;
			currentLoader = ((playetToPlay != 0) ? opponentLoader : playerLoader);
			playerCoin.sprite = ((color != PuckColor.Color.WHITE) ? blackCoin : whiteCoin);
			opponentCoin.sprite = ((color != PuckColor.Color.WHITE) ? whiteCoin : blackCoin);
			PuckColor.Color color2 = (color != PuckColor.Color.WHITE) ? PuckColor.Color.WHITE : PuckColor.Color.BLACK;
			player = new OfflinePlayer(color);
			opponentPlayer = new OfflinePlayer(color2);
			playingPlayer = player;
			playerScoreView = leftScoreView;
			opponentScoreView = rightScoreView;
			movementPadBottom.SetActive(value: true);
		}
		playerCoin.enabled = true;
		opponentCoin.enabled = true;
		playerNameView.text = "Player 1";
		opponentNameView.text = "Player 2";
	}

	public void PlayerTurn()
	{
		EnableStrikerHandler();
		if (player == playingPlayer)
		{
			playerStrikerAnimator.MoveStrikerIn();
		}
		else
		{
			opponentStrikerAnimator.MoveStrikerIn();
		}
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_PLAYER_TURN);
		RoundBegin();
	}

	public void RoundBegin()
	{
		strikedAnyPucks = false;
		if (turnManager != null)
		{
			turnManager.BeginTurn();
		}
		startTime = (int)Time.time;
		timesUp = false;
	}

	private void MakeChangesToOtherPlayer()
	{
		playerStrikerAnimator.MoveStrikerOut();
	}

	private void MakeChangesToGameBeginner()
	{
	}

	private void HandleOwnerShip()
	{
		OfflinePlayer offlinePlayer = playingPlayer = ((playingPlayer != player) ? player : opponentPlayer);
	}

	public void OpponentScoredPoint(OfflinePlayer opponnentId, int score)
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_POINT_SCORED);
		Text text = (opponnentId == player) ? playerScoreView : opponentScoreView;
		text.text = score.ToString();
	}

	public void PlayerScoredPoint(OfflinePlayer playerId, int score)
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_POINT_SCORED);
		Text text = (playerId != player) ? opponentScoreView : playerScoreView;
		text.text = score.ToString();
	}

	public void GameOver(OfflinePlayer playerId)
	{
		currentLoader.fillAmount = 1f;
		currentLoader = null;
		AudioManager.getInstance().PlaySound(AudioManager.GAME_OVER);
		gameOverPanel.SetActive(value: true);
		gameOverPartilce.SetActive(value: true);
		isGameOver = true;
		if (playerId == player)
		{
			leftAvatar.GetComponent<GameOverAnimator>().AnimateWinner();
			rightAvatar.GetComponent<GameOverAnimator>().AnimateLooser();
		}
		else
		{
			rightAvatar.GetComponent<GameOverAnimator>().AnimateWinner();
			leftAvatar.GetComponent<GameOverAnimator>().AnimateLooser();
		}
	}

	public void PlayerLostPoint(OfflinePlayer playerId, int score)
	{
		Text text = (playerId != player) ? opponentScoreView : playerScoreView;
		text.text = score.ToString();
	}

	public void AnimatePuck(PuckAnimator animator)
	{
	}

	private void MoveStrikerHandlerToCenter()
	{
		if (playingPlayer == player)
		{
			downStrikerMover.AnimateStrikerHandlerToCenter();
		}
		else
		{
			upStrikerMover.AnimateStrikerHandlerToCenter();
		}
	}

	private Foul IsFoul()
	{
		int num = 0;
		int num2 = 0;
		foreach (Puck puck in pucks)
		{
			if (puck.puckcolor.suit != PuckColor.Color.RED)
			{
				if (puck.puckcolor.suit == playingPlayer.color)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		foulDetail.text = string.Empty;
		foreach (GoaledPuck goaledColor in goaledColors)
		{
			if (goaledColor.color == PuckColor.Color.STRIKER_COLOR)
			{
				foulDetail.text = "You cannot put the Striker";
				return new Foul(num, num2, isFoul: true, "Foul");
			}
		}
		if (num == 1 || num2 == 1)
		{
			int num3 = 0;
			int num4 = 0;
			foreach (GoaledPuck goaledColor2 in goaledColors)
			{
				if (goaledColor2.color != PuckColor.Color.RED)
				{
					if (goaledColor2.color == playingPlayer.color)
					{
						num3++;
					}
					else
					{
						num4++;
					}
				}
			}
			if ((!queenAcquired || IsQueenGoaled()) && num2 == 1 && num4 == 1)
			{
				foulDetail.text = "Putting opponent's last puck when\n Red is not recovered is Foul";
				return new Foul(num, num2, isFoul: true, "Plotting opponent's last puck when\n Red is not recovered is Foul");
			}
			if (IsQueenPresent() && !IsQueenGoaled() && num == 1 && num3 == 1)
			{
				foulDetail.text = "First Put Red Puck";
				return new Foul(num, num2, isFoul: true, "First Put Queen(Red) Puck");
			}
			if (IsQueenPresent() && !IsQueenGoaled() && num2 == 1 && num4 == 1)
			{
				foulDetail.text = "First Put Red Puck";
				return new Foul(num, num2, isFoul: true, "First Put Queen(Red) Puck");
			}
		}
		return new Foul(num, num2, isFoul: false, string.Empty);
	}

	private bool IsQueenPresent()
	{
		foreach (Puck puck in pucks)
		{
			if (puck.puckcolor.suit == PuckColor.Color.RED)
			{
				return true;
			}
		}
		return false;
	}

	private bool IsQueenGoaled()
	{
		foreach (GoaledPuck goaledColor in goaledColors)
		{
			if (goaledColor.color == PuckColor.Color.RED)
			{
				return true;
			}
		}
		return false;
	}

	private bool HasStrikedAnyPucksToHole()
	{
		return goaledColors.Count > 0;
	}

	public void SetWinScore(int score)
	{
		winscore = score;
	}

	private bool IsGameOver()
	{
		if (player.score >= winscore)
		{
			GameOver(player);
			return true;
		}
		if (opponentPlayer.score >= winscore)
		{
			GameOver(opponentPlayer);
			return true;
		}
		return false;
	}

	public void RoundComplete(float delay)
	{
		Foul foul = IsFoul();
		if (foul.isFoul)
		{
			ShowFoulMessage();
			if (playingPlayer.playerPutQueen)
			{
				RespawnQueen();
			}
			foreach (GoaledPuck goaledColor in goaledColors)
			{
				if (goaledColor.color != PuckColor.Color.STRIKER_COLOR)
				{
					if (goaledColor.color == playingPlayer.color || goaledColor.color == PuckColor.Color.RED)
					{
						goaledColor.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
						goaledColor.gameObject.GetComponent<PuckAnimator>().AnimatePuck(Vector3.zero);
					}
					else if (foul.opponentCoinsCount == 1 && IsQueenPresent())
					{
						goaledColor.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
						goaledColor.gameObject.GetComponent<PuckAnimator>().AnimatePuck(Vector3.zero);
					}
					else
					{
						DestroyPuck(goaledColor.gameObject);
						OfflinePlayer offlinePlayer = (playingPlayer != player) ? player : opponentPlayer;
						offlinePlayer.score++;
						OpponentScoredPoint(offlinePlayer, offlinePlayer.score);
					}
				}
			}
			int score = playingPlayer.score;
			if (score > 0)
			{
				GameObject gameObject = (playingPlayer.color != 0) ? UnityEngine.Object.Instantiate(whitePuck, (playingPlayer != player) ? strikerMoverUp.transform.position : strikerMoverDown.transform.position, Quaternion.identity) : UnityEngine.Object.Instantiate(blackPuck, (playingPlayer != player) ? strikerMoverUp.transform.position : strikerMoverDown.transform.position, Quaternion.identity);
				PuckAnimator component = gameObject.GetComponent<PuckAnimator>();
				component.AnimatePuck(Vector3.zero);
				pucks.Add(new Puck(gameObject.GetComponent<Rigidbody2D>(), gameObject.GetComponent<PuckColor>()));
				playingPlayer.score--;
				PlayerLostPoint(playingPlayer, playingPlayer.score);
			}
			if (!IsGameOver())
			{
				goaledColors.Clear();
				Invoke("NextPlayerTurn", 2f);
			}
			return;
		}
		strikedAnyPucks = HasStrikedAnyPucksToHole();
		if (strikedAnyPucks)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (GoaledPuck goaledColor2 in goaledColors)
			{
				if (goaledColor2.color == playingPlayer.color)
				{
					flag = true;
					playingPlayer.score++;
					PlayerScoredPoint(playingPlayer, playingPlayer.score);
					DestroyPuck(goaledColor2.gameObject);
				}
				else if (goaledColor2.color == PuckColor.Color.RED)
				{
					playingPlayer.playerPutQueen = true;
					flag2 = true;
					DestroyPuck(goaledColor2.gameObject);
				}
				else
				{
					DestroyPuck(goaledColor2.gameObject);
					OfflinePlayer offlinePlayer2 = (playingPlayer != player) ? player : opponentPlayer;
					offlinePlayer2.score++;
					OpponentScoredPoint(offlinePlayer2, offlinePlayer2.score);
				}
			}
			MoveStrikerHandlerToCenter();
			if (flag && playingPlayer.playerPutQueen)
			{
				playingPlayer.playerPutQueen = false;
				RoundBegin();
				if (player == playingPlayer)
				{
					playerStrikerAnimator.MoveBackStriker();
				}
				else
				{
					opponentStrikerAnimator.MoveBackStriker();
				}
				if (queenPuck != null)
				{
					queenAcquired = true;
					queenPuck.GetComponent<SpriteRenderer>().color = Color.white;
					if (player == playingPlayer)
					{
						queenRecoverLeft.text = "Red recovered";
						queenPuckAnimator.gameObject.SetActive(value: true);
						queenPuckAnimator.SetTrigger("show");
					}
					else
					{
						queenRecoverRight.text = "Red recovered";
						queenPuckAnimatorRight.gameObject.SetActive(value: true);
						queenPuckAnimatorRight.SetTrigger("show");
					}
				}
			}
			else if (flag || flag2)
			{
				RoundBegin();
				if (player == playingPlayer)
				{
					playerStrikerAnimator.MoveBackStriker();
				}
				else
				{
					opponentStrikerAnimator.MoveBackStriker();
				}
			}
			else
			{
				if (playingPlayer.playerPutQueen)
				{
					RespawnQueen();
				}
				Invoke("NextPlayerTurn", 2f);
			}
			if (IsGameOver())
			{
				return;
			}
		}
		else if (playingPlayer.playerPutQueen)
		{
			RespawnQueen();
			Invoke("NextPlayerTurn", 2f);
		}
		else
		{
			Invoke("NextPlayerTurn", delay);
		}
		goaledColors.Clear();
	}

	public void ShowFoulMessage()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_FOUL);
		foulAnimator.SetTrigger("foul");
	}

	private void RespawnQueen()
	{
		playingPlayer.playerPutQueen = false;
		GameObject gameObject = UnityEngine.Object.Instantiate(redPuck, (playingPlayer != player) ? strikerMoverUp.transform.position : strikerMoverDown.transform.position, Quaternion.identity);
		PuckAnimator component = gameObject.GetComponent<PuckAnimator>();
		component.AnimatePuck(Vector3.zero);
		pucks.Add(new Puck(gameObject.GetComponent<Rigidbody2D>(), gameObject.GetComponent<PuckColor>()));
		if (queenPuck != null)
		{
			if (player == playingPlayer)
			{
				queenRecoverLeft.text = "Red not recovered";
				queenPuckAnimator.gameObject.SetActive(value: true);
				queenPuckAnimator.SetTrigger("show");
			}
			else
			{
				queenRecoverRight.text = "Red not recovered";
				queenPuckAnimatorRight.gameObject.SetActive(value: true);
				queenPuckAnimatorRight.SetTrigger("show");
			}
			UnityEngine.Object.Destroy(queenPuck);
		}
	}

	public void DestroyPuck(GameObject puck)
	{
		PuckColor component = puck.GetComponent<PuckColor>();
		foreach (Puck puck2 in pucks)
		{
			if (puck2.puckcolor == component)
			{
				pucks.Remove(puck2);
				break;
			}
		}
		if (component.suit == PuckColor.Color.RED)
		{
			queenPuck = UnityEngine.Object.Instantiate(redPuckOffline, puck.transform.position, Quaternion.identity);
			if (player == playingPlayer)
			{
				queenPuck.GetComponent<PuckAnimator>().AnimateQueen(Camera.main.ScreenToWorldPoint(redCoinLeft.position));
			}
			else
			{
				queenPuck.GetComponent<PuckAnimator>().AnimateQueen(Camera.main.ScreenToWorldPoint(redCoinRight.position));
			}
			UnityEngine.Object.Destroy(puck);
		}
		else if (playingPlayer.color == component.suit)
		{
			if (playingPlayer == player)
			{
				puck.GetComponent<PuckAnimator>().AnimatePuckToScore(Camera.main.ScreenToWorldPoint(playerCoin.transform.position));
			}
			else
			{
				puck.GetComponent<PuckAnimator>().AnimatePuckToScore(Camera.main.ScreenToWorldPoint(opponentCoin.transform.position));
			}
		}
		else if (playingPlayer == player)
		{
			puck.GetComponent<PuckAnimator>().AnimatePuckToScore(Camera.main.ScreenToWorldPoint(opponentCoin.transform.position));
		}
		else
		{
			puck.GetComponent<PuckAnimator>().AnimatePuckToScore(Camera.main.ScreenToWorldPoint(playerCoin.transform.position));
		}
	}

	private void NextPlayerTurn()
	{
		disableStrikerMover();
		strikedAnyPucks = false;
		if (playingPlayer == player)
		{
			playerStrikerAnimator.MoveStrikerOut();
		}
		else
		{
			opponentStrikerAnimator.MoveStrikerOut();
		}
		MoveStrikerHandlerToCenter();
		HandleOwnerShip();
		PlayerTurn();
		UpdateLoader();
	}

	public void UpdateLoader()
	{
		if (!(currentLoader == null))
		{
			currentLoader.fillAmount = 1f;
			if (playingPlayer != player)
			{
				currentLoader = opponentLoader;
			}
			else
			{
				currentLoader = playerLoader;
			}
		}
	}

	private void disableStrikerMover()
	{
		if (playingPlayer == player)
		{
			strikerMoverDownRender.color = disabledColor;
			strikerMoverDownCollider.enabled = false;
			strikerStripDownRender.color = disabledColor;
		}
		else
		{
			strikerMoverUpRender.color = disabledColor;
			strikerMoverUpCollider.enabled = false;
			strikerStripUpRender.color = disabledColor;
		}
	}

	private void EnableStrikerHandler()
	{
		if (playingPlayer == player)
		{
			strikerMoverDownRender.color = enabledColor;
			strikerMoverDownCollider.enabled = true;
			strikerStripDownRender.color = enabledColor;
		}
		else
		{
			strikerMoverUpRender.color = enabledColor;
			strikerMoverUpCollider.enabled = true;
			strikerStripUpRender.color = enabledColor;
		}
	}

	public void SetScorePoint()
	{
	}

	public void SetFoul()
	{
	}

	public void GoaledColor(GoaledPuck puck)
	{
		goaledColors.Add(puck);
	}

	private void LogFeedback(string message)
	{
	}

	public void OnTurnBegins(int turn)
	{
		UnityEngine.Debug.LogError("OnTurnBegins");
	}

	public void OnTurnCompleted(int turn)
	{
	}

	public void OnPlayerMove(Player player, int turn, object move)
	{
	}

	public void OnPlayerFinished(Player player, int turn, object move)
	{
	}

	public void OnTurnTimeEnds(int turn)
	{
		if (isGameOver)
		{
			return;
		}
		timesUp = true;
		RoundComplete(0f);
	}

	public void FinishTurn()
	{
		timesUp= true;
	}

	public void OnQuitClicked()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		quitPanel.SetActive(value: true);
	}

	public void OnCancelQuitClicked()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		quitPanel.SetActive(value: false);
	}

	public void LeaveAndGoHome()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		AudioManager.getInstance().StopSound(AudioManager.GAME_OVER);
		GoBackToMenu();
	}

	private void GoBackToMenu()
	{
		AudioManager.getInstance().StopSound(AudioManager.PLAY_TICK);
		SceneManager.LoadScene("menu");
		AdManager.getInstance().ShowInterstitial();
		AdManager.getInstance().showAd();
	}

	public void ShareScore()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		ShareManager.insatnce.ShareRoomId("I'm playing Carrom Master.\nDownload today and join me!\n" + FacebookLogin.SHARE_URL);
	}

	public void CurtainClosed()
	{
		home.SetActive(value: true);
		share.SetActive(value: true);
	}
}
