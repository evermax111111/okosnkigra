using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdManager : MonoBehaviour
{
	public void Awake()
	{
		if (AdManager.instance == null)
		{
			manager = UnityEngine.Object.FindObjectOfType<MenuManager>();
			AdManager.instance = this;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			return;
		}
		AdManager.instance.manager = UnityEngine.Object.FindObjectOfType<MenuManager>();
		AdManager.instance.gameManager = UnityEngine.Object.FindObjectOfType<CarromGameManager>();
		UnityEngine.Object.Destroy(gameObject);
	}

	public static AdManager getInstance()
	{
		return AdManager.instance;
	}

    [Obsolete]
    public void Start()
	{

#if UNITY_ANDROID
		string appId = "4565935";
#elif UNITY_IPHONE
        string appId = "4565934";
#else
		string appId = "unexpected_platform";
#endif

#if UNITY_ANDROID
		string Rewarded_ID = Android_Rewarded_ID;
#elif UNITY_IPHONE
        string Rewarded_ID = IOS_Rewarded_ID;
#else
        string Rewarded_ID = "unexpected_platform";
#endif

		rewardedAd = new RewardedAd(Rewarded_ID);
		//string appId = "ca-app-pub-5502594396951181~1490398266";
		//AppLovin.Initialize();
		MobileAds.Initialize(appId);
		RequestBanner();
		/*rewardBasedVideo = RewardBasedVideoAd.Instance;
		rewardBasedVideo.OnAdLoaded += HandleOnAdLoadedRewarded;
		rewardBasedVideo.OnAdFailedToLoad += HandleOnAdFailedToLoadRewarded;
		rewardBasedVideo.OnAdClosed += HandleOnAdClosedRewarded;*/
		//rewardBasedVideo.OnAdRewarded += HandleOnRewarded;
		this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
		this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
		this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
		this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
		this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
		this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
		RequestRewardedAd();
		if (PlayerPrefs.GetInt("no_ad", 1) != 1)
		{
			Debug.LogError("No Ad:Interstital Already paid");
			return;
		}
		RequestIntersttialAd();
	}

	public void Update()
	{
		if (isRewarded)
		{
			isRewarded = false;
			if (!isDoubleCoins)
			{
				rewardUser(WATCH_AD_COINS);
			}
			LeaderBoardManager leaderBoardManager = LeaderBoardManager.getInstance();
			if (manager != null)
			{
				UnityEngine.Debug.LogError("manager != null");
				int xp = (!(leaderBoardManager != null)) ? 20 : leaderBoardManager.REWARDED_AD_XP;
				manager.showCoinRewardPanel(WATCH_AD_COINS, xp);
			}
			if (AdManager.instance.gameManager != null)
			{
				int xp2 = (!(leaderBoardManager != null)) ? 20 : leaderBoardManager.REWARDED_AD_XP;
				if (isDoubleCoins)
				{
					UnityEngine.Debug.LogError("showDoubledCoins");
					AdManager.instance.gameManager.showDoubledCoins(xp2);
				}
				else
				{
					UnityEngine.Debug.LogError("showCoinRewardPanel");
					AdManager.instance.gameManager.showCoinRewardPanel(WATCH_AD_COINS, xp2);
				}
			}
			else
			{
				UnityEngine.Debug.LogError("gameManager is nulll");
			}
			if (leaderBoardManager != null)
			{
				leaderBoardManager.UpdatePlayerPoints(leaderBoardManager.REWARDED_AD_XP);
			}
		}
		if (isClosedRewardedVideo)
		{
			isClosedRewardedVideo = false;
			RequestRewardedAd();
		}
	}

	public bool showRewardedVideoToDoubleCoins()
	{
		isDoubleCoins = true;
		if (rewardedAd.IsLoaded())
		{
			UnityEngine.Debug.LogError("Rewarded available");
			rewardedAd.Show();
			return true;
		}
		if (failedToloadRewarded)
		{
			if (manager != null)
			{
				manager.showRewardedReTryPanel();
			}
			return false;
		}
		return true;
	}

	private void RequestBanner()
	{
		if (PlayerPrefs.GetInt("no_ad", 1) != 1)
		{
			UnityEngine.Debug.LogError("No Ad:Already paid");
			return;
		}
		UnityEngine.Debug.LogError("RequestBanner");
#if UNITY_ANDROID
		string Banner_ID = Android_Banner_ID;
#elif UNITY_IPHONE
            string Banner_ID = IOS_Banner_ID;
#else
            string Banner_ID = "unexpected_platform";
#endif
		//string android_Banner_ID = Android_Banner_ID;
		AdSize banner = AdSize.Banner;
		bannerView = new BannerView(Banner_ID, banner, AdPosition.Bottom);
		AdRequest request = new AdRequest.Builder().Build();
		bannerView.OnAdLoaded += BannerLoaded;
		bannerView.OnAdFailedToLoad += BannerFailed;
		bannerView.LoadAd(request);
	}

	public void BannerLoaded(object sender, EventArgs args)
	{
		Debug.LogError("BannerLoaded");
	}

	public void BannerFailed(object sender, AdFailedToLoadEventArgs args)
	{
		Debug.LogError("BannerFailed " + args.Message);
	}

	private void RequestRewardedAd()
	{
		//string android_Rewarded_ID = Android_Rewarded_ID;
		AdRequest request = new AdRequest.Builder().Build();
		rewardedAd.LoadAd(request);
		failedToloadRewarded = false;
	}
	/*
	public void HandleOnRewarded(object sender, Reward args)
	{
		Debug.Log("Rewarded rewarded");
		isRewarded = true;
	}
	*/
	public void HandleUserEarnedReward(object sender, Reward args)
	{
		Debug.Log("Rewarded rewarded");
		isRewarded = true;
	}

	/*
	 public void HandleOnAdLoadedRewarded(object sender, EventArgs args)
	{
		Debug.Log("Rewarded loaded");
	}
	*/
	public void HandleRewardedAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdLoaded event received");
		Debug.Log("Rewarded loaded");
	}
	/*
	public void HandleOnAdFailedToLoadRewarded(object sender, AdFailedToLoadEventArgs args)
	{
		Debug.Log("Rewarded failed to load");
		failedToloadRewarded = true;
	}
	*/

	public void HandleRewardedAdOpening(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdOpening event received");
	}

	public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
	{
		MonoBehaviour.print(
			"HandleRewardedAdFailedToLoad event received with message: "
							 + args.Message);
		failedToloadRewarded = true;
	}
	public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
	{
		MonoBehaviour.print(
			"HandleRewardedAdFailedToShow event received with message: "
							 + args.Message);
	}
	/*
	public void HandleOnAdClosedRewarded(object sender, EventArgs args)
	{
		Debug.Log("Rewarded closed");
		isClosedRewardedVideo = true;
	}
	*/
	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdClosed event received");
		isClosedRewardedVideo = true;
	}
	public bool showRewardedVideo()
	{
		isDoubleCoins = false;
		if (rewardedAd.IsLoaded())
		{
			UnityEngine.Debug.Log("Rewarded available");
			rewardedAd.Show();
			return true;
		}
		if (failedToloadRewarded)
		{
			if (manager != null)
			{
				manager.showRewardedReTryPanel();
			}
			return false;
		}
		if (manager != null)
		{
			manager.showRewardedNotAvailable();
		}
		return true;
	}

	public void RetryRewardedVideo()
	{
		RequestRewardedAd();
	}

	private void rewardUser(int reward)
	{
		int num = PlayerPrefs.GetInt("coins", FacebookLogin.DEFAULT_COINS);
		num += reward;
		PlayerPrefs.SetInt("coins", num);
		LeaderBoardManager leaderBoardManager = LeaderBoardManager.getInstance();
		if (leaderBoardManager != null)
		{
			leaderBoardManager.UpdatePlayerCoins(num);
		}
	}

	public void hideAd()
	{
		Debug.LogError("hideAd");
		if (bannerView != null)
		{
			Debug.LogError("Hidden");
			bannerView.Hide();
		}
	}

	public void removeAd()
	{
		if (bannerView != null)
		{
			bannerView.Destroy();
			bannerView = null;
		}
		if (interstitial != null)
		{
			interstitial.Destroy();
			interstitial = null;
		}
	}

	public void showAd()
	{
		Debug.LogError("showAd");
		if (bannerView != null)
		{
			Debug.LogError("shown");
			bannerView.Show();
		}
	}

	private void RequestIntersttialAd()
	{
#if UNITY_ANDROID
		string Interstitial_ID = Android_Interstitial_ID;
#elif UNITY_IPHONE
            string Interstitial_ID = IOS_Interstitial_ID;
#else
            string Interstitial_ID = "unexpected_platform";
#endif
		//string android_Interstitial_ID = Android_Interstitial_ID;
		if (interstitial != null)
		{
			interstitial.Destroy();
		}
		interstitial = new InterstitialAd(Interstitial_ID);
		interstitial.OnAdLoaded += HandleOnAdLoaded;
		interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		interstitial.OnAdOpening += HandleOnAdOpened;
		interstitial.OnAdClosed += HandleOnAdClosed;
		AdRequest request = new AdRequest.Builder().Build();
		interstitial.LoadAd(request);
	}

	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		Debug.LogError("Interstitial loaded");
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		Debug.LogError("Interstitial failed " + args.Message);
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		print("HandleAdOpened event received");
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		Debug.LogError("Interstitial close");
		RequestIntersttialAd();
	}

	public void HandleOnAdLeftApplication(object sender, EventArgs args)
	{
		Debug.LogError("Interstitial left app");
	}

	public void ShowInterstitial()
	{
		if (interstitial != null && interstitial.IsLoaded())
		{
			interstitial.Show();
		}
	}

	public bool IsRewardedVideoAvailable()
	{
		return rewardedAd != null && rewardedAd.IsLoaded();
	}

	public int WATCH_AD_COINS = 200;

	private BannerView bannerView;

	private RewardBasedVideoAd rewardBasedVideo;

	private InterstitialAd interstitial;

	private bool failedToloadRewarded;

	private bool isRewarded;

	private bool isClosedRewardedVideo;

	private MenuManager manager;

	private static AdManager instance;

	private CarromGameManager gameManager;

	private bool isDoubleCoins;

	public string Android_Banner_ID;

	public string Android_Interstitial_ID;

	public string Android_Rewarded_ID;

	public string IOS_Banner_ID;

	public string IOS_Interstitial_ID;

	public string IOS_Rewarded_ID;

	private RewardedAd rewardedAd;
}
