using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public enum Reward
	{
		OFFLINE_WIN_COINS = 250,
		WATCH_VIDEO_COINS = 500
	}

	public enum GameMode
	{
		MULTIPLAYER,
		JOIN_ROOM,
		CREATE_ROOM,
		HELP_MODE,
		OFFLINE_MODE
	}

	public enum Bet
	{
		NO_BET = 0,
		BET_250 = 250,
		BET_500 = 500,
		BET_2500 = 2500,
		BET_5K = 5000,
		BET_10K = 10000,
		BET_50K = 50000
	}

	public enum City
	{
		DELHI,
		DUBAI,
		LONDON,
		THAILAND,
		SYDNEY,
		NEWYORK
	}

	public bool canShowRateUsPanel = true;

	public static LevelManager instance;

	private bool showRateUsFlag;

	public GameMode gameMode;

	public Bet gameBet = Bet.BET_250;

	public City city;

	public Sprite[] strikers;

	public string roomName;

	public string photonVersion = "carrom_v1";

	public float turnTime = 60f;

	public float showOfflineMatchButton = 1f;

	public float opponentWaitingTime = 5f;

	public int botStartTime = 15;

	public int botStartTimeFourPlayer = 15;

	public int botStartTimeFivePlayer = 15;

	public int maxPlayer = 2;

	public int initialMaxCoins = 7500;

	public int initilaMaxGems = 10;

	public int initialMinCoins = 500;

	public int buyMaxCoins = 25000;

	public int buyMaxGems = 125;

	public int buyMinCoins = 15000;

	public int earlyMaxCoins = 50000;

	public int earlyMaxGems = 250;

	public int earlyMinCoins = 30000;

	public int initialChance = 3;

	public int earlyAccessChance = 5;

	public int rateUsIntervals;

	private Sprite playerAvatar;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
	}

	public static LevelManager getInstance()
	{
		return instance;
	}

	public void setGameMode(GameMode mode)
	{
		gameMode = mode;
	}

	public void setGameBet(Bet mode)
	{
		gameBet = mode;
	}

	public void showRateUs()
	{
		showRateUsFlag = (PlayerPrefs.GetInt("can_show_rate_us", 1) == 1);
	}

	public bool canShowRateUs()
	{
		if (showRateUsFlag)
		{
			int @int = PlayerPrefs.GetInt("last_day", 100);
			if (Mathf.Abs(@int - DateTime.Now.Day) > rateUsIntervals && canShowRateUsPanel)
			{
				PlayerPrefs.SetInt("last_day", DateTime.Now.Day);
				return true;
			}
		}
		return false;
	}

	public void SetPlayerImage(Sprite avatar)
	{
		playerAvatar = avatar;
	}

	public Sprite GetPlayerImage()
	{
		return playerAvatar;
	}
}
