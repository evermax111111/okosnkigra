using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class MonetizeManager : MonoBehaviour, IStoreListener
{
	public int GEM_100 = 100;

	public int GEM_200 = 225;

	public int GEM_300 = 350;

	public int GEM_400 = 475;

	public int GEM_500 = 600;

	public int GEM_700 = 1500;

	public int GEM_900 = 6000;

	public string chance = "$2";

	public string earlyPrice = "$2";

	public Text log;

	public Text[] coins;

	private static IStoreController m_StoreController;

	private static IExtensionProvider m_StoreExtensionProvider;

	public static string No_AD = "no_ad";

	public static string Coin_10K = "coin_1";

	public static string Coin_25K = "coin_2";

	public static string Coin_50K = "coin_3";

	public static string Coin_100K = "coin_4";

	public static string Coin_250K = "coin_5";

	public static string Coin_750K = "coin_6";

	public static string Gem_100 = "gem_1";

	public static string Gem_200 = "gem_2";

	public static string Gem_300 = "gem_3";

	public static string Gem_400 = "gem_4";

	public static string Gem_500 = "gem_5";

	public static string Gem_700 = "gem_6";

	public static string Gem_900 = "gem_7";

	public static string Master_Striker = "master_striker";

	public static string Master_Striker_Early = "master_striker_early";

	public static MonetizeManager Instance
	{
		get;
		set;
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
	}

	private void Start()
	{
		if (m_StoreController == null)
		{
			InitializePurchasing();
		}
		else
		{
			initInAppButtons(m_StoreController);
		}
	}

	private void initInAppButtons(IStoreController controller)
	{
		int num = 1;
		for (int i = 0; i < controller.products.all.Length; i++)
		{
			Product product = controller.products.all[i];
			string str = product.metadata.localizedTitle + ":" + product.metadata.localizedPriceString;
			if (!(log != null))
			{
				continue;
			}
			Text text = log;
			text.text = text.text + str + "\n";
			if (coins.Length > i)
			{
				coins[i].text = product.metadata.localizedPriceString;
				continue;
			}
			if (string.Equals(product.definition.id, Master_Striker, StringComparison.Ordinal))
			{
				chance = product.metadata.localizedPriceString;
			}
			else if (string.Equals(product.definition.id, Master_Striker_Early, StringComparison.Ordinal))
			{
				earlyPrice = product.metadata.localizedPriceString;
				UnityEngine.Debug.LogError("Early: " + earlyPrice);
			}
			if (StoreManager.instance == null)
			{
				break;
			}
		}
	}

	public void InitializePurchasing()
	{
		if (!IsInitialized())
		{
			ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
			configurationBuilder.AddProduct(Coin_10K, ProductType.Consumable);
			configurationBuilder.AddProduct(Coin_25K, ProductType.Consumable);
			configurationBuilder.AddProduct(Coin_50K, ProductType.Consumable);
			configurationBuilder.AddProduct(Coin_100K, ProductType.Consumable);
			configurationBuilder.AddProduct(Coin_250K, ProductType.Consumable);
			configurationBuilder.AddProduct(Coin_750K, ProductType.Consumable);
			configurationBuilder.AddProduct(No_AD, ProductType.NonConsumable);
			configurationBuilder.AddProduct(Gem_100, ProductType.Consumable);
			configurationBuilder.AddProduct(Gem_200, ProductType.Consumable);
			configurationBuilder.AddProduct(Gem_300, ProductType.Consumable);
			configurationBuilder.AddProduct(Gem_400, ProductType.Consumable);
			configurationBuilder.AddProduct(Gem_500, ProductType.Consumable);
			configurationBuilder.AddProduct(Gem_700, ProductType.Consumable);
			configurationBuilder.AddProduct(Gem_900, ProductType.Consumable);
			configurationBuilder.AddProduct(Master_Striker, ProductType.Consumable);
			configurationBuilder.AddProduct(Master_Striker_Early, ProductType.Consumable);
			UnityPurchasing.Initialize(this, configurationBuilder);
		}
	}

	private bool IsInitialized()
	{
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void Buy10KCoins()
	{
		BuyProductID(Coin_10K);
	}

	public void Buy25KCoins()
	{
		BuyProductID(Coin_25K);
	}

	public void Buy50KCoins()
	{
		BuyProductID(Coin_50K);
	}

	public void Buy100KCoins()
	{
		BuyProductID(Coin_100K);
	}

	public void Buy250KCoins()
	{
		BuyProductID(Coin_250K);
	}

	public void Buy750KCoins()
	{
		BuyProductID(Coin_750K);
	}

	public void BuyNoAd()
	{
		BuyProductID(No_AD);
	}

	public void Buy100Gems()
	{
		BuyProductID(Gem_100);
	}

	public void Buy200Gems()
	{
		BuyProductID(Gem_200);
	}

	public void Buy300Gems()
	{
		BuyProductID(Gem_300);
	}

	public void Buy400Gems()
	{
		BuyProductID(Gem_400);
	}

	public void Buy500Gems()
	{
		BuyProductID(Gem_500);
	}

	public void Buy700Gems()
	{
		BuyProductID(Gem_700);
	}

	public void Buy900Gems()
	{
		BuyProductID(Gem_900);
	}

	public void BuyMoreMasterStrike()
	{
		BuyProductID(Master_Striker);
	}

	public void BuyMasterStrikeEarly()
	{
		BuyProductID(Master_Striker_Early);
	}

	private void BuyProductID(string productId)
	{
		if (IsInitialized())
		{
			Product product = m_StoreController.products.WithID(productId);
			if (product != null && product.availableToPurchase)
			{
				UnityEngine.Debug.Log($"Purchasing product asychronously: '{product.definition.id}'");
				m_StoreController.InitiatePurchase(product);
			}
			else
			{
				UnityEngine.Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		else
		{
			UnityEngine.Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}

	public void RestorePurchases()
	{
		if (!IsInitialized())
		{
			UnityEngine.Debug.Log("RestorePurchases FAIL. Not initialized.");
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			UnityEngine.Debug.Log("RestorePurchases started ...");
			IAppleExtensions extension = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			extension.RestoreTransactions(delegate(bool result)
			{
				UnityEngine.Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		else
		{
			UnityEngine.Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		initInAppButtons(controller);
		UnityEngine.Debug.Log("OnInitialized: PASS");
		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		UnityEngine.Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		LeaderBoardManager instance = LeaderBoardManager.getInstance();
		UnityEngine.Debug.Log("ProcessPurchase");
		if (string.Equals(args.purchasedProduct.definition.id, Coin_10K, StringComparison.Ordinal))
		{
			UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
			addCoins(10000);
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.BUY_10K_COIN_XP);
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Coin_25K, StringComparison.Ordinal))
		{
			addCoins(25000);
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.BUY_25K_COIN_XP);
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Coin_50K, StringComparison.Ordinal))
		{
			addCoins(50000);
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.BUY_50K_COIN_XP);
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Coin_100K, StringComparison.Ordinal))
		{
			addCoins(100000);
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.BUY_100K_COIN_XP);
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Coin_250K, StringComparison.Ordinal))
		{
			addCoins(250000);
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.BUY_250K_COIN_XP);
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Coin_750K, StringComparison.Ordinal))
		{
			addCoins(750000);
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.BUY_750K_COIN_XP);
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Gem_100, StringComparison.Ordinal))
		{
			addGems(GEM_100);
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.BUY_10K_COIN_XP);
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Gem_200, StringComparison.Ordinal))
		{
			addGems(GEM_200);
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.BUY_25K_COIN_XP);
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Gem_300, StringComparison.Ordinal))
		{
			addGems(GEM_300);
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.BUY_50K_COIN_XP);
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Gem_400, StringComparison.Ordinal))
		{
			addGems(GEM_400);
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.BUY_100K_COIN_XP);
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Gem_500, StringComparison.Ordinal))
		{
			addGems(GEM_500);
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.BUY_250K_COIN_XP);
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Gem_700, StringComparison.Ordinal))
		{
			addGems(GEM_700);
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.BUY_750K_COIN_XP);
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Gem_900, StringComparison.Ordinal))
		{
			addGems(GEM_900);
			if (instance != null)
			{
				instance.UpdatePlayerPoints(instance.BUY_900_GEM_XP);
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, No_AD, StringComparison.Ordinal))
		{
			int @int = PlayerPrefs.GetInt("no_ad", 1);
			MenuManager menuManager = UnityEngine.Object.FindObjectOfType<MenuManager>();
			if (@int == 1 && instance != null)
			{
				UnityEngine.Debug.LogError("no_ad xp");
				instance.UpdatePlayerPoints(instance.BUY_NO_AD_XP);
			}
			PlayerPrefs.SetInt("no_ad", 0);
			@int = PlayerPrefs.GetInt("no_ad", 1);
			UnityEngine.Debug.LogError("adStatus:" + @int);
			if (menuManager != null)
			{
				menuManager.handleNoAd();
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Master_Striker, StringComparison.Ordinal))
		{
			if (MasterStrikeManager.Instance != null)
			{
				MasterStrikeManager.Instance.OnPurchasedMasterStriker();
			}
		}
		else if (string.Equals(args.purchasedProduct.definition.id, Master_Striker_Early, StringComparison.Ordinal))
		{
			if (MasterStrikeManager.Instance != null)
			{
				MasterStrikeManager.Instance.OnPurchasedMasterStrikerEarly();
			}
		}
		else
		{
			UnityEngine.Debug.Log($"ProcessPurchase: FAIL. Unrecognized product: '{args.purchasedProduct.definition.id}'");
		}
		return PurchaseProcessingResult.Complete;
	}

	private void addCoins(int rewardCoins)
	{
		MenuManager menuManager = UnityEngine.Object.FindObjectOfType<MenuManager>();
		int @int = PlayerPrefs.GetInt("coins", FacebookLogin.DEFAULT_COINS);
		@int += rewardCoins;
		PlayerPrefs.SetInt("coins", @int);
		LeaderBoardManager.synchPlayerCoins(@int);
		if (menuManager != null)
		{
			menuManager.handleCoinPurchase(rewardCoins);
		}
	}

	private void addGems(int gems)
	{
		MenuManager menuManager = UnityEngine.Object.FindObjectOfType<MenuManager>();
		int @int = PlayerPrefs.GetInt("gems", FacebookLogin.DEFAULT_GEMS);
		@int += gems;
		PlayerPrefs.SetInt("gems", @int);
		LeaderBoardManager.synchPlayerDetails();
		if (menuManager != null)
		{
			menuManager.handleGemPurchase(gems);
		}
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		UnityEngine.Debug.Log($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
	}
}
