using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
	[Serializable]
	public class Striker
	{
		public Text text;

		public Image selector;

		public Image buttonImage;

		public Image coinImage;

		public StrikerState state;

		public int price;

		public StrikerBuyMode buyMode;
	}

	public enum StrikerState
	{
		BUY,
		EQIP,
		EQIPPED
	}

	public enum StrikerBuyMode
	{
		COIN,
		GEM
	}

	public GameObject storePanel;

	public GameObject coinsPurchasePanel;

	public GameObject noCoinsPanel;

	public Animator anim;

	public Text coinsMessage;

	public Text coinsText;

	public Text gemText;

	public Sprite buySprite;

	public Sprite useSprite;

	public Sprite inUseSprite;

	public static StoreManager instance;

	public Striker[] strikers;

	private Striker clickedStriker;

	private int strikerIndex;

	public Text buyMessage;

	public GameObject buyConfirmDialog;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
	}

	private void UpdateButtonStates()
	{
		int @int = PlayerPrefs.GetInt("striker", 0);
		string @string = PlayerPrefs.GetString("strikers", "0");
		if (string.IsNullOrEmpty(@string))
		{
			for (int i = 0; i < strikers.Length; i++)
			{
				if (i == @int)
				{
					strikers[i].state = StrikerState.EQIPPED;
				}
				else
				{
					strikers[i].state = StrikerState.BUY;
				}
			}
			return;
		}
		string[] array = @string.Split(':');
		List<int> list = new List<int>();
		string[] array2 = array;
		foreach (string s in array2)
		{
			list.Add(int.Parse(s));
		}
		for (int k = 0; k < strikers.Length; k++)
		{
			if (list.Contains(k))
			{
				if (k == @int)
				{
					strikers[k].state = StrikerState.EQIPPED;
				}
				else
				{
					strikers[k].state = StrikerState.EQIP;
				}
			}
			else if (k == 0)
			{
				strikers[k].state = StrikerState.EQIP;
			}
			else
			{
				strikers[k].state = StrikerState.BUY;
			}
		}
	}

	private void UpdateButtonText()
	{
		for (int i = 0; i < strikers.Length; i++)
		{
			Striker striker = strikers[i];
			if (striker.state == StrikerState.EQIP)
			{
				striker.text.text = string.Empty;
				striker.coinImage.enabled = false;
				striker.buttonImage.sprite = useSprite;
			}
			else if (striker.state == StrikerState.EQIPPED)
			{
				striker.text.text = string.Empty;
				striker.selector.gameObject.SetActive(value: true);
				striker.coinImage.enabled = false;
				striker.buttonImage.sprite = inUseSprite;
			}
			else
			{
				striker.coinImage.enabled = true;
				striker.text.text = striker.price.ToString();
				striker.buttonImage.sprite = buySprite;
			}
		}
	}

	public void BoughtStriker(int index)
	{
		clickedStriker = strikers[index];
		StrikerBuyMode buyMode = clickedStriker.buyMode;
		if (clickedStriker.state == StrikerState.BUY)
		{
			strikerIndex = index;
			AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
			int @int = PlayerPrefs.GetInt("coins", FacebookLogin.DEFAULT_COINS);
			int int2 = PlayerPrefs.GetInt("gems", FacebookLogin.DEFAULT_GEMS);
			if (buyMode == StrikerBuyMode.COIN)
			{
				if (@int >= clickedStriker.price)
				{
					buyMessage.text = "Buy this striker for " + clickedStriker.price + " coins?";
					buyConfirmDialog.SetActive(value: true);
				}
				else
				{
					coinsMessage.text = "You need " + (clickedStriker.price - @int) + " more coins";
					noCoinsPanel.SetActive(value: true);
				}
			}
			else if (int2 >= clickedStriker.price)
			{
				buyMessage.text = "Buy this striker for " + clickedStriker.price + " gems?";
				buyConfirmDialog.SetActive(value: true);
			}
			else
			{
				coinsMessage.text = "You need " + (clickedStriker.price - int2) + " more gems";
				noCoinsPanel.SetActive(value: true);
			}
		}
		else if (clickedStriker.state == StrikerState.EQIP)
		{
			AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
			EquipStriker(index);
		}
	}

	public void BuyStriker()
	{
		buyConfirmDialog.SetActive(value: false);
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		if (clickedStriker.buyMode == StrikerBuyMode.COIN)
		{
			int @int = PlayerPrefs.GetInt("coins", FacebookLogin.DEFAULT_COINS);
			@int -= clickedStriker.price;
			PlayerPrefs.SetInt("coins", @int);
			coinsText.text = MenuManager.getRepresentationcoins(@int);
		}
		else
		{
			int int2 = PlayerPrefs.GetInt("gems", FacebookLogin.DEFAULT_COINS);
			int2 -= clickedStriker.price;
			PlayerPrefs.SetInt("gems", int2);
			gemText.text = MenuManager.getRepresentationcoins(int2);
		}
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_COIN_COLLECT);
		OnStrikerPurchased(strikerIndex);
	}

	public void CanceBuyStriker()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		buyConfirmDialog.SetActive(value: false);
	}

	private void EquipStriker(int index)
	{
		Striker striker = strikers[index];
		striker.state = StrikerState.EQIPPED;
		striker.text.text = string.Empty;
		striker.buttonImage.sprite = inUseSprite;
		striker.coinImage.enabled = false;
		striker.selector.gameObject.SetActive(value: true);
		PlayerPrefs.SetInt("striker", index);
		for (int i = 0; i < strikers.Length; i++)
		{
			Striker striker2 = strikers[i];
			if (striker2 != striker && striker2.state == StrikerState.EQIPPED)
			{
				striker2.state = StrikerState.EQIP;
				striker2.text.text = string.Empty;
				striker2.text.text = string.Empty;
				striker2.buttonImage.sprite = useSprite;
				striker2.coinImage.enabled = false;
				striker2.selector.gameObject.SetActive(value: false);
			}
		}
		LeaderBoardManager.getInstance().UpdatePlayerStriker();
	}

	public void OnStrikerPurchased(int index)
	{
		string @string = PlayerPrefs.GetString("strikers", "0");
		if (string.IsNullOrEmpty(@string))
		{
			PlayerPrefs.SetString("strikers", index.ToString());
			UnityEngine.Debug.LogError("Strikers: " + index.ToString());
		}
		else
		{
			UnityEngine.Debug.LogError("existingStriers: " + @string);
			string[] array = @string.Split(':');
			List<int> list = new List<int>();
			string text = null;
			string[] array2 = array;
			foreach (string text2 in array2)
			{
				UnityEngine.Debug.LogError("key: " + text2);
				if (!list.Contains(int.Parse(text2)))
				{
					list.Add(int.Parse(text2));
					if (!string.IsNullOrEmpty(text))
					{
						text += ":";
					}
					text += int.Parse(text2);
				}
			}
			if (!list.Contains(index))
			{
				list.Add(index);
				if (!string.IsNullOrEmpty(text))
				{
					text += ":";
				}
				text += index;
				PlayerPrefs.SetString("strikers", text);
			}
			UnityEngine.Debug.LogError("Strikers: " + text);
		}
		EquipStriker(index);
		PlayerPrefs.SetInt("striker", index);
		LeaderBoardManager.getInstance().UpdatePlayerStriker();
	}

	public void SetPurchasePrice(int index, string price)
	{
		Striker striker = strikers[index];
	}

	public void ShowStrore()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		UpdateButtonStates();
		UpdateButtonText();
		storePanel.SetActive(value: true);
	}

	public void CloseStore()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		anim.SetTrigger("fade");
		storePanel.SetActive(value: false);
	}

	public void ShowCoinsPurchase()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		anim.SetTrigger("fade");
		CloseBuyCoins();
		coinsPurchasePanel.SetActive(value: true);
		storePanel.SetActive(value: false);
	}

	public void Clear()
	{
		PlayerPrefs.DeleteAll();
	}

	public void BuyCoins()
	{
		ShowCoinsPurchase();
	}

	public void CloseBuyCoins()
	{
		noCoinsPanel.SetActive(value: false);
	}
}
