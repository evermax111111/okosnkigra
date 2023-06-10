using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class MenuManager : MonoBehaviour
{
	public float AppVersion = 1f;

	public float MarketAndroidVersion = 0.9f;

	public float MarketiOSVersion = 0.9f;

	public const int COIN_250 = 250;

	public const int COIN_500 = 500;

	public const int COIN_1000 = 1000;

	public const int COIN_2500 = 2500;

	public bool showPlayersDropDown;

	public bool canHaveFivePlayer;

	public GameObject roomIdPanel;

	public GameObject joinPanel;

	public GameObject betPanel;

	public GameObject noCoinsPanel;

	public GameObject adRewardedPanel;

	public GameObject adRewardedRetryPanel;

	public GameObject watchAdPanel;

	public GameObject invitePanel;

	public GameObject purchasePanel;

	public GameObject forceUpdatePanel;

	public GameObject settingsPanel;

	public GameObject settingsButton;

	public GameObject playerPanel;

	public GameObject rateUsPanel;

	public GameObject quitPanel;

	public GameObject leaderBoardPanel;

	public GameObject levelUpPanel;

	public GameObject storePanel;

	public GameObject privateGamePanel;


	public GameObject masterStrikeIcon;

	public GameObject masterStrikeLable;

	public GameObject playButton;

	public GameObject createRoomButton;

	public GameObject noAdButton;

	public GameObject gameTitle;

	public GameObject menuPanel;

	public GameObject menuPanelHelp;

	public GameObject rewardContainer;

	public GameObject helpButton;

	public GameObject bet250Selected;

	public GameObject bet500Selected;

	public GameObject bet1000Selected;

	public GameObject bet2500Selected;

	public InputField roomNameField;

	public Button ShareButton;

	public Button RestoreButton;

	private GameObject currentSelected;

	public Text coinsText;

	public Text gemstext;

	public Text screenTitle;

	public Text adRewardMessage;

	public Text watchAddMessage;

	public Text levelNumber;

	public Text facebookXp;

	public Text inviteXp;

	public Text watchApXp;

	public Text noCoinsDetail;

	public Text forceUpdateText;

	public string forceUpdateMessage;

	public Image audioToggle;

	public Image vibrationToggle;

	public Image messageToggle;

	public Sprite enabledAudio;

	public Sprite diabledAudio;

	public Dropdown dropDown;

	public Animator anim;

	public Animator masterStrikerAnimator;

	private int coins;

	public bool showCustomMessages;

	public bool showBannerInGame = true;

	public bool showBannerOffline = true;

	public Text rewardMessage;

	public GameObject adRewardedNotAvailablePanel;

	public Color tabUnselectedColor;

	public Color tabSelectedColor;

	public Text coinsViewText;

	public Text gemsViewText;

	public GameObject coinsPurchasePanel;

	public GameObject gemsPurchasePanel;

	public Image gemsTab;

	public Image coinsTab;

	public Text[] gemsTextViews;

	public string packagname = "com.Badboy.carrom";

	private static IExtensionProvider m_StoreExtensionProvider;

	private void Start()
	{
		currentSelected = bet250Selected;
		currentSelected.SetActive(value: true);
		AppVersion = float.Parse(Application.version);
		coins = PlayerPrefs.GetInt("coins", 10000);
		coinsText.text = getRepresentationcoins(coins);
		if ((PlayerPrefs.GetInt("audio", 1) == 1) ? true : false)
		{
			audioToggle.sprite = enabledAudio;
		}
		else
		{
			audioToggle.sprite = diabledAudio;
		}
		bool flag = (PlayerPrefs.GetInt("vibrate", 1) == 1) ? true : false;
		vibrationToggle.sprite = ((!flag) ? diabledAudio : enabledAudio);
		flag = ((PlayerPrefs.GetInt("show_custom_message", 0) != 0) ? true : false);
		messageToggle.sprite = ((!flag) ? diabledAudio : enabledAudio);
		if (IsMasterStrikeReady())
		{
			masterStrikerAnimator.enabled = true;
			masterStrikerAnimator.gameObject.transform.GetChild(0).gameObject.SetActive(value: true);
		}
		LevelManager.getInstance().maxPlayer = 2;
		dropDown.value = 0;
	}

	private void Awake()
	{
	}

	public void CanShowLevelUp()
	{
		int @int = PlayerPrefs.GetInt("level_up", 0);
		if (@int == 1)
		{
			PlayerPrefs.SetInt("level_up", 0);
			AudioManager.getInstance().PlaySound(AudioManager.PLAY_WIN);
			levelUpPanel.SetActive(value: true);
			levelNumber.text = PlayerPrefs.GetInt("level", 1).ToString();
		}
	}

	private void Update()
	{
		checkUpdateNeeded();
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
			quitPanel.SetActive(value: true);
		}
	}

	private void checkUpdateNeeded()
	{
		if (MarketAndroidVersion > AppVersion && !forceUpdatePanel.activeSelf)
		{
			forceUpdatePanel.SetActive(value: true);
			forceUpdateText.text = forceUpdateMessage;
		}
	}

	public static string getRepresentationcoins(int coins)
	{
		if (coins < 10000)
		{
			return coins.ToString();
		}
		if (coins >= 1000000)
		{
			if (coins % 1000000 == 0)
			{
				return coins / 1000000 + "M";
			}
			return ((float)coins / 1000000f).ToString("F2") + "M";
		}
		if (coins % 1000 == 0)
		{
			return coins / 1000 + "K";
		}
		return ((float)coins / 1000f).ToString("F2") + "K";
	}

	public void OnMultiplyerClicked()
	{
		anim.SetTrigger("fade");
		hideMenuItems();
		playClickAudio();
		screenTitle.text = "MULTIPLAYER";
		LevelManager.getInstance().setGameMode(LevelManager.GameMode.MULTIPLAYER);
		betPanel.SetActive(value: true);
		settingsButton.SetActive(value: false);
		joinPanel.SetActive(value: false);
		createRoomButton.SetActive(value: false);
		playButton.SetActive(value: true);
		helpButton.SetActive(value: false);
		masterStrikeIcon.SetActive(value: false);
		masterStrikeLable.SetActive(value: false);
		if (showPlayersDropDown)
		{
			dropDown.gameObject.SetActive(value: true);
		}
		if (showPlayersDropDown)
		{
			List<string> list = new List<string>();
			list.Add("3 Players");
			list.Add("4 Players");
			List<string> list2 = list;
			if (canHaveFivePlayer)
			{
				list2.Add("5 Players");
			}
			dropDown.ClearOptions();
			dropDown.AddOptions(list2);
		}
	}

	public void bet250Coins()
	{
		if (currentSelected != bet250Selected)
		{
			playClickAudio();
			currentSelected.SetActive(value: false);
			currentSelected = bet250Selected;
			bet250Selected.SetActive(value: true);
		}
	}

	public void bet500Coins()
	{
		if (currentSelected != bet500Selected)
		{
			playClickAudio();
			currentSelected.SetActive(value: false);
			currentSelected = bet500Selected;
			bet500Selected.SetActive(value: true);
		}
	}

	public void bet1000Coins()
	{
		if (currentSelected != bet1000Selected)
		{
			playClickAudio();
			currentSelected.SetActive(value: false);
			currentSelected = bet1000Selected;
			bet1000Selected.SetActive(value: true);
		}
	}

	public void bet1500Coins()
	{
		if (currentSelected != bet2500Selected)
		{
			playClickAudio();
			currentSelected.SetActive(value: false);
			currentSelected = bet2500Selected;
			bet2500Selected.SetActive(value: true);
		}
	}

	public void OnOfflineClicked()
	{
		LevelManager.getInstance().setGameMode(LevelManager.GameMode.OFFLINE_MODE);
		playClickAudio();
		HandleBannerAd(showBannerOffline);
		SceneManager.LoadScene("CarromOffline");
	}

	public void GoBackToMenu()
	{
		anim.SetTrigger("fade");
		showMenuItems();
		playClickAudio();
		settingsButton.SetActive(value: true);
		helpButton.SetActive(value: true);
		AdManager.getInstance().showAd();
	}

	private void hideMenuItems()
	{
		gameTitle.SetActive(value: false);
		menuPanel.SetActive(value: false);
		menuPanelHelp.SetActive(value: false);
		rewardContainer.SetActive(value: false);
	}

	private void showMenuItems()
	{
		gameTitle.SetActive(value: true);
		rewardContainer.SetActive(value: true);
		betPanel.SetActive(value: false);
		masterStrikeIcon.SetActive(value: true);
		masterStrikeLable.SetActive(value: true);
		if ((PlayerPrefs.GetInt("show_help", 1) == 1) ? true : false)
		{
			menuPanelHelp.SetActive(value: true);
		}
		else
		{
			menuPanel.SetActive(value: true);
		}
	}

	public void OnPlayWithFriendsClicked()
	{
		anim.SetTrigger("fade");
		if (AdManager.getInstance() != null)
		{
		}
		playClickAudio();
		privateGamePanel.SetActive(value: true);
	}

	public void ClosePrivateMatchPanel()
	{
		playClickAudio();
		anim.SetTrigger("fade");
		privateGamePanel.SetActive(value: false);
	}

	public void OpenGameRoom(string roomId)
	{
		OnPlayWithFriendsClicked();
		roomIdPanel.SetActive(value: true);
		roomNameField.text = roomId;
	}

	public void OnCreateRoomClicked()
	{
		HandleBannerAd(showBannerInGame);
		LevelManager.getInstance().setGameMode(LevelManager.GameMode.CREATE_ROOM);
		playClickAudio();
		SceneManager.LoadScene("CarromOnline");
	}

	private void HandleBannerAd(bool flag)
	{
		AdManager instance = AdManager.getInstance();
		if (instance != null)
		{
			if (flag)
			{
				instance.showAd();
			}
			else
			{
				instance.hideAd();
			}
		}
	}

	public void OnJoinRoomClicked()
	{
		playClickAudio();
		roomIdPanel.SetActive(value: true);
	}

	public void OnJoinUserEnteredRoom()
	{
		string text = roomNameField.text;
		playClickAudio();
		if (!string.IsNullOrEmpty(text))
		{
			HandleBannerAd(showBannerInGame);
			LevelManager.getInstance().roomName = text;
			LevelManager.getInstance().setGameMode(LevelManager.GameMode.JOIN_ROOM);
			SceneManager.LoadScene("CarromOnline");
		}
	}

	private LevelManager.City GetCity(int city)
	{
		return (LevelManager.City)city;
	}

	private LevelManager.Bet GetBetFromCity(LevelManager.City city)
	{
		switch (city)
		{
		case LevelManager.City.DELHI:
			return LevelManager.Bet.BET_250;
		case LevelManager.City.DUBAI:
			return LevelManager.Bet.BET_500;
		case LevelManager.City.LONDON:
			return LevelManager.Bet.BET_2500;
		case LevelManager.City.THAILAND:
			return LevelManager.Bet.BET_5K;
		case LevelManager.City.SYDNEY:
			return LevelManager.Bet.BET_10K;
		case LevelManager.City.NEWYORK:
			return LevelManager.Bet.BET_50K;
		default:
			return LevelManager.Bet.BET_250;
		}
	}

	public void OnPlayGameClicked(int city)
	{
		int num = 10;
		LevelManager.instance.city = GetCity(city);
		LevelManager.Bet betFromCity = GetBetFromCity(LevelManager.instance.city);
		LevelManager.instance.setGameBet(betFromCity);
		num = (int)betFromCity;
		UnityEngine.Debug.LogError("Coins:" + num);
		playClickAudio();
		coins = PlayerPrefs.GetInt("coins", FacebookLogin.DEFAULT_COINS);
		if (coins < num)
		{
			showNoCoinsAlert(num - coins);
			return;
		}
		HandleBannerAd(showBannerInGame);
		LevelManager.getInstance().setGameMode(LevelManager.GameMode.MULTIPLAYER);
		SceneManager.LoadScene("CarromOnline");
	}

	private bool coinAvailableToBetGame(LevelManager.Bet bet)
	{
		int @int = PlayerPrefs.GetInt("coins", 10000);
		if (@int < (int)bet)
		{
			return false;
		}
		return true;
	}

	public LevelManager.Bet getUserSelectedBet()
	{
		if (currentSelected == bet250Selected)
		{
			return LevelManager.Bet.BET_250;
		}
		if (currentSelected == bet500Selected)
		{
			return LevelManager.Bet.BET_500;
		}
		return LevelManager.Bet.BET_250;
	}

	public void closeEnterJoinPanel()
	{
		playClickAudio();
		roomIdPanel.SetActive(value: false);
	}

	private void showNoCoinsAlert(int coins)
	{
		noCoinsPanel.SetActive(value: true);
		noCoinsDetail.text = "You need " + coins + " more coins.";
	}

	public void closeNoCoinsAlert()
	{
		playClickAudio();
		noCoinsPanel.SetActive(value: false);
	}

	private void playClickAudio()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
	}

	public void BuyCoinsClicked()
	{
		OnShowInAppPurchasePanel();
	}

	public void WatchVideoClicked()
	{
		playClickAudio();
		if (AdManager.getInstance().showRewardedVideo())
		{
		}
	}

	public void showCoinRewardPanel(int coins, int xp)
	{
		adRewardedPanel.SetActive(value: true);
		watchAdPanel.SetActive(value: false);
		rewardMessage.text = "You have received " + coins + " coins.";
		coins = PlayerPrefs.GetInt("coins", 10000);
		coinsText.text = getRepresentationcoins(coins);
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_COIN_COLLECT);
	}

	public void showGemsRewardPanel(int gems)
	{
		adRewardedPanel.SetActive(value: true);
		watchAdPanel.SetActive(value: false);
		rewardMessage.text = "You have received " + gems + " gems.";
		gems = PlayerPrefs.GetInt("gems", FacebookLogin.DEFAULT_GEMS);
		gemstext.text = getRepresentationcoins(gems);
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_GEM_AQUIRED);
	}

	public void handleCoinPurchase(int coins)
	{
		if (purchasePanel.activeSelf)
		{
			purchasePanel.SetActive(value: false);
		}
		LeaderBoardManager instance = LeaderBoardManager.getInstance();
		int xp = 300;
		if (instance != null)
		{
			switch (coins)
			{
			case 10000:
				xp = instance.BUY_10K_COIN_XP;
				break;
			case 25000:
				xp = instance.BUY_25K_COIN_XP;
				break;
			case 50000:
				xp = instance.BUY_50K_COIN_XP;
				break;
			case 100000:
				xp = instance.BUY_100K_COIN_XP;
				break;
			case 250000:
				xp = instance.BUY_250K_COIN_XP;
				break;
			case 750000:
				xp = instance.BUY_750K_COIN_XP;
				break;
			}
		}
		else
		{
			xp = 300;
		}
		showCoinRewardPanel(coins, xp);
	}

	public void handleGemPurchase(int coins)
	{
		if (purchasePanel.activeSelf)
		{
			purchasePanel.SetActive(value: false);
		}
		showGemsRewardPanel(coins);
	}

	public void rewardedDialogOkClicked()
	{
		adRewardedPanel.SetActive(value: false);
	}

	public void showRewardedReTryPanel()
	{
		watchAdPanel.SetActive(value: false);
		adRewardedRetryPanel.SetActive(value: true);
	}

	public void showRewardedNotAvailable()
	{
		watchAdPanel.SetActive(value: false);
		adRewardedNotAvailablePanel.SetActive(value: true);
	}

	public void CloseRewardNotAvailable()
	{
		playClickAudio();
		adRewardedNotAvailablePanel.SetActive(value: false);
	}

	public void rewardedDialogRetryClicked()
	{
		adRewardedRetryPanel.SetActive(value: false);
		AdManager.getInstance().RetryRewardedVideo();
	}

	public void watchAdCloseClicked()
	{
		playClickAudio();
		watchAdPanel.SetActive(value: false);
	}

	public void watchAdClicked()
	{
		playClickAudio();
		watchAdPanel.SetActive(value: true);
		if (adRewardMessage != null && AdManager.getInstance() != null)
		{
			watchAddMessage.text = "Watch a video to get " + AdManager.getInstance().WATCH_AD_COINS + " coins.";
		}
		LeaderBoardManager instance = LeaderBoardManager.getInstance();
		if (instance != null)
		{
			watchApXp.text = instance.REWARDED_AD_XP + " XP";
		}
	}

	public void FacebookInviteCloseClicked()
	{
		playClickAudio();
		invitePanel.SetActive(value: false);
	}

	public void FacebooInviteClicked()
	{
		playClickAudio();
		invitePanel.SetActive(value: true);
		if (LeaderBoardManager.getInstance() != null)
		{
			facebookXp.text = LeaderBoardManager.getInstance().FACEBOOK_SHARE_XP + " XP";
			inviteXp.text = LeaderBoardManager.getInstance().INVITE_FRIEND_XP + " XP";
		}
	}

	public void InviteFacebookFriends()
	{
		playClickAudio();
		FacebookLogin facebookLogin = UnityEngine.Object.FindObjectOfType<FacebookLogin>();
		if (facebookLogin != null)
		{
			facebookLogin.Challenge();
		}
	}

	public void ShareFacebook()
	{
		playClickAudio();
		FacebookLogin facebookLogin = UnityEngine.Object.FindObjectOfType<FacebookLogin>();
		if (facebookLogin != null)
		{
			facebookLogin.ShareFacebook();
		}
	}

	public void OnShowInAppPurchasePanel()
	{
		playClickAudio();
		anim.SetTrigger("fade");
		purchasePanel.SetActive(value: true);
		OnCoinsTabClicked();
	}

	public void OnCoinsTabClicked()
	{
		coinsPurchasePanel.SetActive(value: true);
		gemsPurchasePanel.SetActive(value: false);
		coinsViewText.color = tabSelectedColor;
		gemsViewText.color = tabUnselectedColor;
		gemsTab.color = tabUnselectedColor;
		coinsTab.color = tabSelectedColor;
	}

	public void OnShowGemsPurchasePanel()
	{
		playClickAudio();
		anim.SetTrigger("fade");
		purchasePanel.SetActive(value: true);
		OnGemsTabClicked();
	}

	public void OnGemsTabClicked()
	{
		coinsPurchasePanel.SetActive(value: false);
		gemsPurchasePanel.SetActive(value: true);
		coinsViewText.color = tabUnselectedColor;
		gemsViewText.color = tabSelectedColor;
		gemsTab.color = tabSelectedColor;
		coinsTab.color = tabUnselectedColor;
		setGemsValue();
	}

	private void setGemsValue()
	{
		int num = 10;
		for (int i = 0; i < gemsTextViews.Length; i++)
		{
			switch (i)
			{
			case 0:
				num = MonetizeManager.Instance.GEM_100;
				break;
			case 1:
				num = MonetizeManager.Instance.GEM_200;
				break;
			case 2:
				num = MonetizeManager.Instance.GEM_300;
				break;
			case 3:
				num = MonetizeManager.Instance.GEM_400;
				break;
			case 4:
				num = MonetizeManager.Instance.GEM_500;
				break;
			case 5:
				num = MonetizeManager.Instance.GEM_700;
				break;
			case 6:
				num = MonetizeManager.Instance.GEM_900;
				break;
			}
			gemsTextViews[i].text = num.ToString();
		}
	}

	public void LaunchMasterStrike()
	{
		playClickAudio();
		SceneManager.LoadScene("CarromMasterStrike");
	}

	public void CloseInAppPurchasePanel()
	{
		playClickAudio();
		anim.SetTrigger("fade");
		purchasePanel.SetActive(value: false);
	}

	public void Buy10KCoins()
	{
		MonetizeManager.Instance.Buy10KCoins();
	}

	public void Buy25KCoins()
	{
		MonetizeManager.Instance.Buy25KCoins();
	}

	public void Buy50KCoins()
	{
		MonetizeManager.Instance.Buy50KCoins();
	}

	public void Buy100KCoins()
	{
		MonetizeManager.Instance.Buy100KCoins();
	}

	public void Buy250KCoins()
	{
		MonetizeManager.Instance.Buy250KCoins();
	}

	public void Buy750KCoins()
	{
		MonetizeManager.Instance.Buy750KCoins();
	}

	public void BuyNoAd()
	{
		MonetizeManager.Instance.BuyNoAd();
	}

	public void Buy100Gems()
	{
		MonetizeManager.Instance.Buy100Gems();
	}

	public void Buy200Gems()
	{
		MonetizeManager.Instance.Buy200Gems();
	}

	public void Buy300Gems()
	{
		MonetizeManager.Instance.Buy300Gems();
	}

	public void Buy400Gems()
	{
		MonetizeManager.Instance.Buy400Gems();
	}

	public void Buy500Gems()
	{
		MonetizeManager.Instance.Buy500Gems();
	}

	public void Buy700Gems()
	{
		MonetizeManager.Instance.Buy700Gems();
	}

	public void Buy900Gems()
	{
		MonetizeManager.Instance.Buy900Gems();
	}

	public void handleNoAd()
	{
		if (purchasePanel.activeSelf)
		{
			purchasePanel.SetActive(value: false);
		}
		AdManager.getInstance().removeAd();
		noAdButton.SetActive(value: false);
	}

	public void UpdateApp()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		Application.OpenURL("market://details?id=" + packagname);
	}

	public void ShowSettings()
	{
		anim.SetTrigger("fade");
		AdManager.getInstance().hideAd();
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		hideMenuItems();
		settingsPanel.SetActive(value: true);
		playerPanel.SetActive(value: false);
		if (!showCustomMessages)
		{
			if ((PlayerPrefs.GetInt("enabled_custom_message", 0) == 0) ? true : false)
			{
				messageToggle.sprite = diabledAudio;
			}
		}
		else if ((PlayerPrefs.GetInt("enabled_custom_message", 0) == 0) ? true : false)
		{
			messageToggle.sprite = enabledAudio;
		}
	}

	public void CloseSettings()
	{
		anim.SetTrigger("fade");
		playerPanel.SetActive(value: true);
		showMenuItems();
		if (AdManager.getInstance() != null)
		{
			AdManager.getInstance().showAd();
		}
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		settingsPanel.SetActive(value: false);
	}

	public void LikeUs()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		Application.OpenURL("https://your facebook here.com");
	}

	public void FollowUs()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		Application.OpenURL("https://your Site here.com");
	}

	public void RateUs()
	{
		UpdateApp();
	}

	public void ShareApp()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		ShareManager.insatnce.ShareRoomId("I'm playing the most popular family game - Carrom Online.\nDownload today and join me for an exciting match\n" + FacebookLogin.SHARE_URL);
	}

	public void ToggleAudio()
	{
		if ((PlayerPrefs.GetInt("audio", 1) == 1) ? true : false)
		{
			PlayerPrefs.SetInt("audio", 0);
			audioToggle.sprite = diabledAudio;
		}
		else
		{
			audioToggle.sprite = enabledAudio;
			PlayerPrefs.SetInt("audio", 1);
			AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		}
	}

	public void ToggleVibration()
	{
		string key = "vibrate";
		if ((PlayerPrefs.GetInt(key, 1) == 1) ? true : false)
		{
			PlayerPrefs.SetInt(key, 0);
			vibrationToggle.sprite = diabledAudio;
		}
		else
		{
			vibrationToggle.sprite = enabledAudio;
			PlayerPrefs.SetInt(key, 1);
		}
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
	}

	public void ToggleCustomMessage()
	{
		PlayerPrefs.SetInt("enabled_custom_message", 1);
		string key = "show_custom_message";
		if ((PlayerPrefs.GetInt(key, 0) == 1) ? true : false)
		{
			PlayerPrefs.SetInt(key, 0);
			messageToggle.sprite = diabledAudio;
		}
		else
		{
			messageToggle.sprite = enabledAudio;
			PlayerPrefs.SetInt(key, 1);
		}
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
	}

	public void LaunchHelpMode()
	{
		playClickAudio();
		HandleBannerAd(showBannerInGame);
		SceneManager.LoadScene("CarromHelp");
	}

	public void CloseStore()
	{
		anim.SetTrigger("fade");
		playClickAudio();
		storePanel.SetActive(value: false);
	}

	public void HelpItemClicked()
	{
		LaunchHelpMode();
	}

	public void RestorePurchases()
	{
		        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
	}

	public void RateUsClicked()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		PlayerPrefs.SetInt("can_show_rate_us", 0);
		RateUs();
		rateUsPanel.SetActive(value: false);
	}

	public void RateUsLaterClicked()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		rateUsPanel.SetActive(value: false);
	}

	public void QuitGame()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		Application.Quit();
	}

	public void CancelQuit()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		quitPanel.SetActive(value: false);
	}

	public void ShowLeaderBoard()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		LeaderBoardManager instance = LeaderBoardManager.getInstance();
		if (instance != null)
		{
			instance.LoadData();
		}
		AdManager.getInstance().hideAd();
		leaderBoardPanel.SetActive(value: true);
	}

	public void OnClickAllTimeLeaderBoard()
	{
		LeaderBoardManager instance = LeaderBoardManager.getInstance();
		if (instance != null)
		{
			instance.GetAllTimeLeaderBoard();
		}
	}

	public void OnClickTodaysLeaderBoard()
	{
		LeaderBoardManager instance = LeaderBoardManager.getInstance();
		if (instance != null)
		{
			instance.GetTodaysLeaderBoard();
		}
	}

	public void OnClickWeeksLeaderBoard()
	{
		LeaderBoardManager instance = LeaderBoardManager.getInstance();
		if (instance != null)
		{
			instance.GetWeeklyLeaderBoard();
		}
	}

	public void ClearLeadeboard()
	{
		LeaderBoardManager instance = LeaderBoardManager.getInstance();
		if (instance != null)
		{
			instance.Clear();
		}
	}

	public void HideLeaderBoard()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		leaderBoardPanel.SetActive(value: false);
		AdManager.getInstance().showAd();
	}

	public void ListenerMethod(Vector2 value)
	{
		UnityEngine.Debug.LogError("ListenerMethod: " + value);
	}

	public void CollectLevelUpCoins()
	{
		levelUpPanel.SetActive(value: false);
		PlayerPrefs.SetInt("level_up", 0);
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_COIN_COLLECT);
		int @int = PlayerPrefs.GetInt("coins", 10000);
		@int += 1000;
		PlayerPrefs.SetInt("coins", @int);
		coinsText.text = getRepresentationcoins(@int);
	}

	public void OnPlayerSelected(int playerIndex)
	{
		LevelManager.getInstance().maxPlayer = 3 + playerIndex;
		if (AudioManager.getInstance() != null)
		{
			AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		}
	}

	public static bool IsMasterStrikeReady()
	{
		long num = 86400000L;
		string @string = PlayerPrefs.GetString("name", null);
		bool flag = (PlayerPrefs.GetInt("show_help", 1) == 1) ? true : false;
		bool flag2 = !string.IsNullOrEmpty(@string);
		long ticks = DateTime.Now.Ticks;
		long num2 = long.Parse(PlayerPrefs.GetString(MasterStrikeManager.KEY_TIME, "636913312439118356"));
		long num3 = num - (ticks - num2) / 10000;
		if (num3 < 0 && !flag && flag2)
		{
			return true;
		}
		return false;
	}
}
