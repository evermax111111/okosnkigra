using Facebook.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeaderBoardManager : MonoBehaviour
{
	public string LEVELS = "200:450:7500:1100:2900:4400:6400:9000:12800:17000:22000:27800:34000:41000:48500:56500:65250:74500:85000:100000";

	public int WIN_MATCH_XP = 50;

	public int LOST_MATCH_XP = 25;

	public int BUY_10K_COIN_XP = 20;

	public int BUY_25K_COIN_XP = 35;

	public int BUY_50K_COIN_XP = 60;

	public int BUY_100K_COIN_XP = 100;

	public int BUY_250K_COIN_XP = 150;

	public int BUY_750K_COIN_XP = 200;

	public int BUY_NO_AD_XP = 200;

	public int BUY_900_GEM_XP = 500;

	public int BUY_100_GEM_XP = 20;

	public int REWARDED_AD_XP = 20;

	public int INVITE_FRIEND_XP = 30;

	public int FACEBOOK_SHARE_XP = 30;

	public int LEADER_BOARD_PAGE_SIZE = 100;

	public const string KEY_AVATAR = "avatar";

	public const string KEY_COINS = "coins";

	public const string KEY_TOKEN = "token";

	public const string KEY_LEVEL = "levels";

	public const string KEY_STRIKER = "striker";

	public const string KEY_STRIKERS = "strikers";

	public const string KEY_GEMS = "gems";

	public const string KEY_TIME = "master_strike_time";

	private const string DONKEY_MASTER = "carrom-master";

	private static LeaderBoardManager instance;

	public Text logView;

	public GameObject listItem;

	public Transform parentContainer;

	public Sprite[] avatars;

	public Sprite selectedSprite;

	private bool dataLoaded;

	public Text[] titles;

	public Color[] states;

	private Text selectedText;

	public GameObject loader;

	public ScrollRect scrollRect;

	public Text playerName;

	public Text playerRank;

	public Text playerPoint;

	public Text level;

	public Text levelNumber;

	public Image playerImage;

	public GameObject fb;

	public GameObject playerItem;

	public int[] levels;

	public static LeaderBoardManager getInstance()
	{
		return instance;
	}

	private void Awake()
	{
		scrollRect.onValueChanged.AddListener(ListenerMethod);
		if (instance == null)
		{
			instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
		instance.dataLoaded = dataLoaded;
		instance.avatars = avatars;
		instance.listItem = listItem;
		instance.parentContainer = parentContainer;
		instance.selectedSprite = selectedSprite;
		instance.titles = titles;
		instance.states = states;
		instance.loader = loader;
		instance.playerName = playerName;
		instance.playerRank = playerRank;
		instance.playerPoint = playerPoint;
		instance.playerImage = playerImage;
		instance.fb = fb;
		instance.playerItem = playerItem;
		instance.level = level;
		instance.levelNumber = levelNumber;
		setLevelAndXP();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Start()
	{
		Invoke("setLevelAndXP", 1f);
	}

	private void createLevels()
	{
		levels = new int[200];
		int num = 50;
		int num2 = 4;
		string text = string.Empty;
		for (int i = 0; i < 200; i++)
		{
			if (i != 0)
			{
				levels[i] = levels[i - 1] + num * (i + num2);
			}
			else
			{
				levels[i] = num * (i + num2);
			}
			text = text + levels[i] + ":";
		}
		UnityEngine.Debug.LogError(text);
	}

	private void fillLevels()
	{
		if (string.IsNullOrEmpty(LEVELS))
		{
			createLevels();
			return;
		}
		string[] array = LEVELS.Split(':');
		if (array.Length != 1)
		{
			levels = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				levels[i] = int.Parse(array[i]);
			}
		}
	}

	private void setLevelAndXP()
	{
		UnityEngine.Debug.LogError("setLevelAndXP");
		fillLevels();
		UnityEngine.Debug.LogError("fillLevels");
		if (instance.levels == null || instance.levels.Length <= 0)
		{
			return;
		}
		int @int = PlayerPrefs.GetInt("level", 1);
		int int2 = PlayerPrefs.GetInt("xp", 0);
		UnityEngine.Debug.LogError("xp " + int2);
		int num = 0;
		int[] array = instance.levels;
		foreach (int num2 in array)
		{
			num++;
			if (int2 < num2)
			{
				break;
			}
		}
		@int = num;
		PlayerPrefs.SetInt("level", @int);
		if (instance.levelNumber != null)
		{
			instance.levelNumber.text = @int.ToString();
			instance.level.text = int2 + "<color=#ffff00ff>/</color>" + ((@int > instance.levels.Length) ? instance.levels[instance.levels.Length - 1] : instance.levels[@int - 1]);
			GameObject gameObject = GameObject.Find("MenuManager");
			if (gameObject != null && gameObject.GetComponent<MenuManager>() != null)
			{
				gameObject.GetComponent<MenuManager>().CanShowLevelUp();
			}
		}
	}

	public void LoadData()
	{
		if (!dataLoaded)
		{
			UnityEngine.Debug.LogError("LoadData");
			instance.playerName.text = PlayerPrefs.GetString("name", string.Empty);
			/*PlayerScore playerScore = null;
			if (FB.IsLoggedIn)
			{
				//playerScore = new PlayerScore();
				playerScore.playerid = AccessToken.CurrentAccessToken.UserId;
			}
			loadAvatar(playerImage, PlayerPrefs.GetInt("avatar", -1), playerScore);
			GetAllTimeLeaderBoard();*/
		}
	}

	public void ResetLoaderFlag()
	{
		dataLoaded = false;
		selectedText = null;
		UnityEngine.Debug.LogError("ResetLoaderFlag");
	}

	public void GetUserScore()
	{
		if (FB.IsLoggedIn)
		{
			logMessage("GetUserScore");
			string userId = AccessToken.CurrentAccessToken.UserId;
			/*PLeaderboardOptions pLeaderboardOptions = new PLeaderboardOptions();
			pLeaderboardOptions.Add("table", "highscores");
			pLeaderboardOptions.Add("playerid", userId);
			pLeaderboardOptions.Add("perpage", 1);
			pLeaderboardOptions.Add("highest", true);
			pLeaderboardOptions.Add("source", "carrom-master");
			PLeaderboardOptions options = pLeaderboardOptions;
			Playtomic.Leaderboards.List(options, GetUserDataCallback);*/
		}
		else
		{
			logMessage("Not loggedIn");
		}
	}

	/*private void GetUserDataCallback(List<PlayerScore> scores, int numscores, PResponse response)
	{
		FacebookLogin facebookLogin = UnityEngine.Object.FindObjectOfType<FacebookLogin>();
		if (response.success)
		{
			logMessage("GetUserScore success");
			UnityEngine.Debug.LogError("GetUserDataCallback");
			if (scores.Count > 0)
			{
				logMessage("User data saved");
				PlayerScore playerScore = scores[0];
				PlayerPrefs.SetString("name", playerScore.playername);
				PlayerPrefs.SetInt("xp", (int)playerScore.points);
				logMessage("Xp:" + playerScore.points);
				Dictionary<string, object> dictionary = (Dictionary<string, object>)playerScore["fields"];
				if (dictionary.ContainsKey("avatar"))
				{
					logMessage("Avatar available");
					string text = (string)dictionary["avatar"];
					logMessage("Avatar:" + text);
					PlayerPrefs.SetInt("avatar", int.Parse(text));
				}
				if (dictionary.ContainsKey("coins"))
				{
					string text2 = (string)dictionary["coins"];
					logMessage("coins:" + text2);
					PlayerPrefs.SetInt("coins", int.Parse(text2));
					PlayerPrefs.SetInt("show_help", 0);
				}
				if (dictionary.ContainsKey("striker"))
				{
					string text3 = (string)dictionary["striker"];
					logMessage("striker:" + text3);
					PlayerPrefs.SetInt("striker", int.Parse(text3));
				}
				if (dictionary.ContainsKey("master_strike_time"))
				{
					string text4 = (string)dictionary["master_strike_time"];
					logMessage("time:" + text4);
					PlayerPrefs.SetString("master_strike_time", text4);
				}
				UnityEngine.Debug.LogError("KEY_STRIKER");
				if (dictionary.ContainsKey("strikers"))
				{
					string value = (string)dictionary["strikers"];
					PlayerPrefs.SetString("strikers", value);
				}
				UnityEngine.Debug.LogError("KEY_STRIKERS");
				if (dictionary.ContainsKey("gems"))
				{
					string s = (string)dictionary["gems"];
					PlayerPrefs.SetInt("gems", int.Parse(s));
				}
				UnityEngine.Debug.LogError("KEY_GEMS");
				setLevelAndXP();
				if (facebookLogin != null)
				{
					logMessage("fblogin != null");
					facebookLogin.loadProfilePhoto();
					facebookLogin.showHomeScreen();
				}
			}
			else
			{
				logMessage("User data not saved");
				if (facebookLogin != null)
				{
					facebookLogin.getUserFacebookData();
				}
			}
		}
		else
		{
			logMessage("GetUserScore Failed " + response.errorcode);
			if (facebookLogin != null)
			{
				facebookLogin.getUserFacebookData();
			}
		}
	}*/

	public void UpdatePlayerDetail(string name, int avatar)
	{
		if (FB.IsLoggedIn)
		{
			string userId = AccessToken.CurrentAccessToken.UserId;
			logMessage(userId);
			logMessage("avatar:" + avatar);
			/*PlayerScore playerScore = new PlayerScore();
			playerScore.playername = name;
			playerScore.playerid = userId;
			playerScore.points = PlayerPrefs.GetInt("xp", 0);
			playerScore.table = "highscores";
			playerScore.source = "carrom-master";
			playerScore.fields = new PDictionary
			{
				{
					"avatar",
					avatar + string.Empty
				},
				{
					"coins",
					PlayerPrefs.GetInt("coins", FacebookLogin.DEFAULT_COINS) + string.Empty
				},
				{
					"levels",
					PlayerPrefs.GetInt("level", 1).ToString()
				},
				{
					"striker",
					PlayerPrefs.GetInt("striker", 0).ToString()
				},
				{
					"strikers",
					PlayerPrefs.GetString("strikers", null).ToString()
				},
				{
					"gems",
					PlayerPrefs.GetInt("gems", 0).ToString()
				},
				{
					"master_strike_time",
					PlayerPrefs.GetString("master_strike_time", "636913312439118356")
				}
			};
			PlayerScore score = playerScore;
			Playtomic.Leaderboards.Save(score, UpdateUserDetailCallback);*/
		}
	}

	public void UpdatePlayerCoins(int coins)
	{
		if (FB.IsLoggedIn)
		{
			string userId = AccessToken.CurrentAccessToken.UserId;
			logMessage(userId);
			/*PlayerScore playerScore = new PlayerScore();
			playerScore.playername = PlayerPrefs.GetString("name", "Guest");
			playerScore.playerid = userId;
			playerScore.points = PlayerPrefs.GetInt("xp", 0);
			playerScore.table = "highscores";
			playerScore.source = "carrom-master";
			playerScore.fields = new PDictionary
			{
				{
					"coins",
					coins + string.Empty
				},
				{
					"avatar",
					PlayerPrefs.GetInt("avatar", 0) + string.Empty
				},
				{
					"levels",
					PlayerPrefs.GetInt("level", 1).ToString()
				},
				{
					"striker",
					PlayerPrefs.GetInt("striker", 0).ToString()
				},
				{
					"strikers",
					PlayerPrefs.GetString("strikers", null).ToString()
				},
				{
					"gems",
					PlayerPrefs.GetInt("gems", 0).ToString()
				},
				{
					"master_strike_time",
					PlayerPrefs.GetString("master_strike_time", "636913312439118356")
				}
			};
			PlayerScore score = playerScore;
			Playtomic.Leaderboards.Save(score, UpdateUserDetailCallback);*/
		}
	}

	public void UpdatePlayerDetails()
	{
		if (FB.IsLoggedIn)
		{
			string userId = AccessToken.CurrentAccessToken.UserId;
			logMessage(userId);
			/*PlayerScore playerScore = new PlayerScore();
			playerScore.playername = PlayerPrefs.GetString("name", "Guest");
			playerScore.playerid = userId;
			playerScore.points = PlayerPrefs.GetInt("xp", 0);
			playerScore.table = "highscores";
			playerScore.source = "carrom-master";
			playerScore.fields = new PDictionary
			{
				{
					"coins",
					PlayerPrefs.GetInt("coins", 0) + string.Empty
				},
				{
					"avatar",
					PlayerPrefs.GetInt("avatar", 0) + string.Empty
				},
				{
					"levels",
					PlayerPrefs.GetInt("level", 1).ToString()
				},
				{
					"striker",
					PlayerPrefs.GetInt("striker", 0).ToString()
				},
				{
					"strikers",
					PlayerPrefs.GetString("strikers", null).ToString()
				},
				{
					"gems",
					PlayerPrefs.GetInt("gems", 0).ToString()
				},
				{
					"master_strike_time",
					PlayerPrefs.GetString("master_strike_time", "636913312439118356")
				}
			};
			PlayerScore score = playerScore;
			Playtomic.Leaderboards.Save(score, UpdateUserDetailCallback);*/
		}
	}

	public int UpdatePlayerPoints(int xps)
	{
		if (levels == null)
		{
			return -1;
		}
		UnityEngine.Debug.LogError("UpdatePlayerPoints:" + xps);
		int @int = PlayerPrefs.GetInt("xp", 0);
		@int += xps;
		PlayerPrefs.SetInt("xp", @int);
		int int2 = PlayerPrefs.GetInt("level", 1);
		if (int2 > levels.Length)
		{
			saveXpAndLevels(@int);
			return @int;
		}
		int num = 0;
		for (num = 0; num < levels.Length && @int > levels[num]; num++)
		{
		}
		int int3 = PlayerPrefs.GetInt("next_xp", levels[int2 - 1]);
		if (@int >= int3)
		{
			UnityEngine.Debug.LogError("Level Up!");
			PlayerPrefs.SetInt("level_up", 1);
			int2++;
			PlayerPrefs.SetInt("level", int2);
		}
		setLevelAndXP();
		saveXpAndLevels(@int);
		return @int;
	}

	private void saveXpAndLevels(int xp)
	{
		if (FB.IsLoggedIn)
		{
			string userId = AccessToken.CurrentAccessToken.UserId;
			logMessage(userId);
			/*PlayerScore playerScore = new PlayerScore();
			playerScore.playername = PlayerPrefs.GetString("name", "Guest");
			playerScore.playerid = userId;
			playerScore.points = xp;
			playerScore.table = "highscores";
			playerScore.source = "carrom-master";
			playerScore.fields = new PDictionary
			{
				{
					"coins",
					PlayerPrefs.GetInt("coins", FacebookLogin.DEFAULT_COINS).ToString()
				},
				{
					"avatar",
					PlayerPrefs.GetInt("avatar", 0).ToString()
				},
				{
					"levels",
					PlayerPrefs.GetInt("level", 1).ToString()
				},
				{
					"striker",
					PlayerPrefs.GetInt("striker", 0).ToString()
				},
				{
					"strikers",
					PlayerPrefs.GetString("strikers", null).ToString()
				},
				{
					"gems",
					PlayerPrefs.GetInt("gems", 0).ToString()
				},
				{
					"master_strike_time",
					PlayerPrefs.GetString("master_strike_time", "636913312439118356")
				}
			};
			PlayerScore score = playerScore;
			Playtomic.Leaderboards.Save(score, UpdateUserDetailCallback);*/
		}
	}

	public void UpdatePlayerStriker()
	{
		if (FB.IsLoggedIn)
		{
			string userId = AccessToken.CurrentAccessToken.UserId;
			logMessage(userId);
			/*PlayerScore playerScore = new PlayerScore();
			playerScore.playername = PlayerPrefs.GetString("name", "Guest");
			playerScore.playerid = userId;
			playerScore.points = PlayerPrefs.GetInt("xp", 0);
			playerScore.table = "highscores";
			playerScore.source = "carrom-master";
			playerScore.fields = new PDictionary
			{
				{
					"coins",
					PlayerPrefs.GetInt("coins", FacebookLogin.DEFAULT_COINS).ToString()
				},
				{
					"avatar",
					PlayerPrefs.GetInt("avatar", 0).ToString()
				},
				{
					"levels",
					PlayerPrefs.GetInt("level", 1).ToString()
				},
				{
					"striker",
					PlayerPrefs.GetInt("striker", 1).ToString()
				},
				{
					"strikers",
					PlayerPrefs.GetString("strikers", null).ToString()
				},
				{
					"gems",
					PlayerPrefs.GetInt("gems", 0).ToString()
				},
				{
					"master_strike_time",
					PlayerPrefs.GetString("master_strike_time", "636913312439118356")
				}
			};
			PlayerScore score = playerScore;
			Playtomic.Leaderboards.Save(score, UpdateUserDetailCallback);*/
		}
	}

	public void Clear()
	{
		if (FB.IsLoggedIn)
		{
			string userId = AccessToken.CurrentAccessToken.UserId;
			logMessage(userId);
			/*PlayerScore playerScore = new PlayerScore();
			playerScore.playername = PlayerPrefs.GetString("name", "Guest");
			playerScore.playerid = userId;
			playerScore.points = 0L;
			playerScore.table = "highscores";
			playerScore.source = "carrom-master";
			playerScore.fields = new PDictionary
			{
				{
					"coins",
					"0"
				},
				{
					"avatar",
					"0"
				},
				{
					"levels",
					"1"
				},
				{
					"striker",
					"0"
				},
				{
					"strikers",
					null
				},
				{
					"gems",
					"0"
				},
				{
					"master_strike_time",
					"636913312439118356"
				}
			};
			PlayerScore score = playerScore;
			Playtomic.Leaderboards.Save(score, UpdateUserDetailCallback);*/
		}
	}

	/*private void UpdateUserDetailCallback(PResponse response)
	{
		if (response.success)
		{
			UnityEngine.Debug.LogError("Name saved!");
			logMessage("Name saved!");
		}
		else
		{
			logMessage("Name save Failed!");
			UnityEngine.Debug.LogError("Name Failed! " + response.errorcode + ":" + response.errormessage);
		}
	}*/

	public void GetAllTimeLeaderBoard()
	{
		if (!(selectedText == titles[0]))
		{
			AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
			if (selectedText != null)
			{
				selectedText.color = states[1];
			}
			selectedText = titles[0];
			selectedText.color = states[0];
			clearList();
			GetPlayerRank("alltime");
			loadList("alltime");
		}
	}

	public void GetTodaysLeaderBoard()
	{
		if (!(selectedText == titles[1]))
		{
			AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
			if (selectedText != null)
			{
				selectedText.color = states[1];
			}
			selectedText = titles[1];
			selectedText.color = states[0];
			clearList();
			GetPlayerRank("today");
			loadList("today");
		}
	}

	public void GetWeeklyLeaderBoard()
	{
		if (!(selectedText == titles[2]))
		{
			AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
			if (selectedText != null)
			{
				selectedText.color = states[1];
			}
			selectedText = titles[2];
			selectedText.color = states[0];
			clearList();
			GetPlayerRank("last7days");
			loadList("last7days");
		}
	}

	private void loadList(string mode)
	{
		/*PLeaderboardOptions pLeaderboardOptions = new PLeaderboardOptions();
		pLeaderboardOptions.Add("table", "highscores");
		pLeaderboardOptions.Add("page", 1);
		pLeaderboardOptions.Add("perpage", LEADER_BOARD_PAGE_SIZE);
		pLeaderboardOptions.Add("mode", mode);
		pLeaderboardOptions.Add("highest", true);
		pLeaderboardOptions.Add("source", "carrom-master");
		PLeaderboardOptions options = pLeaderboardOptions;
		Playtomic.Leaderboards.List(options, ListResponseCallback);*/
	}

	/*private void ListResponseCallback(List<PlayerScore> scores, int numscores, PResponse response)
	{
		UnityEngine.Debug.LogError("DailListComplete");
		loader.SetActive(value: false);
		if (response.success)
		{
			dataLoaded = true;
			string text = null;
			if (FB.IsLoggedIn)
			{
				text = AccessToken.CurrentAccessToken.UserId;
			}
			RectTransform component = parentContainer.gameObject.GetComponent<RectTransform>();
			Rect rect = component.rect;
			rect.height = 168 * scores.Count + scores.Count * 40 + 20;
			component.sizeDelta = new Vector2(0f, rect.height);
			for (int i = 0; i < scores.Count; i++)
			{
				PlayerScore playerScore = scores[i];
				if (text == null || !text.Equals(playerScore.playerid))
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(listItem);
					gameObject.transform.Find("Rank").GetComponent<Text>().text = playerScore.rank + string.Empty;
					gameObject.transform.Find("Name").GetComponent<Text>().text = playerScore.playername;
					gameObject.transform.Find("Points").GetComponent<Text>().text = playerScore.points + string.Empty;
					if (text != null && text.Equals(playerScore.playerid))
					{
						gameObject.GetComponent<Image>().sprite = selectedSprite;
					}
					Dictionary<string, object> dictionary = (Dictionary<string, object>)playerScore["fields"];
					if (dictionary.ContainsKey("avatar"))
					{
						string s = (string)dictionary["avatar"];
						int avatarId = int.Parse(s);
						loadAvatar(gameObject.transform.Find("Avatar").GetChild(0).GetChild(0)
							.GetComponent<Image>(), avatarId, playerScore);
					}
					gameObject.transform.SetParent(parentContainer, worldPositionStays: false);
				}
			}
		}
		else
		{
			dataLoaded = false;
		}
	}*/

	/*private void loadAvatar(Image view, int avatarId, PlayerScore score)
	{
		if (avatarId != -1)
		{
			view.sprite = avatars[avatarId];
			return;
		}
		if (score != null)
		{
			logMessage("%#%#:" + score.playerid);
		}
		FB.API("/" + score.playerid + "/picture?type=square&width=100&height=100&redirect=false", HttpMethod.GET, delegate(IGraphResult result)
		{
			if (string.IsNullOrEmpty(result.Error) && !result.Cancelled)
			{
				IDictionary dictionary = result.ResultDictionary["data"] as IDictionary;
				string url = dictionary["url"] as string;
				StartCoroutine(fetchProfilePic(url, view));
			}
			else
			{
				UnityEngine.Debug.Log("ProfiilePicCallBack Error");
			}
		});
	}*/

	private IEnumerator fetchProfilePic(string url, Image pic)
	{
		WWW www = new WWW(url);
		yield return www;
		if (pic != null)
		{
			pic.gameObject.SetActive(value: true);
			pic.sprite = Sprite.Create(www.texture, new Rect(0f, 0f, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
		}
	}

	private void GetPlayerRank(string mode)
	{
		if (FB.IsLoggedIn)
		{
			fb.SetActive(value: false);
			playerItem.SetActive(value: true);
			/*PLeaderboardOptions pLeaderboardOptions = new PLeaderboardOptions();
			pLeaderboardOptions.Add("table", "highscores");
			pLeaderboardOptions.Add("playerid", AccessToken.CurrentAccessToken.UserId);
			pLeaderboardOptions.Add("mode", mode);
			pLeaderboardOptions.Add("source", "carrom-master");
			PLeaderboardOptions options = pLeaderboardOptions;
			Playtomic.Leaderboards.GetPlayerRank(options, PlayerRankResponse);*/
		}
		else
		{
			fb.SetActive(value: true);
			playerItem.SetActive(value: false);
		}
	}

	/*private void PlayerRankResponse(PlayerScore scores, PResponse response)
	{
		UnityEngine.Debug.LogError("PlayerRankResponse");
		loader.SetActive(value: false);
		if (response.success)
		{
			playerRank.text = (scores.rank + 1).ToString();
			playerPoint.text = getPlayerFormattedPoints(scores);
			UnityEngine.Debug.LogError("PlayerRankResponse success");
		}
		else
		{
			UnityEngine.Debug.LogError("PlayerRankResponse fail");
		}
	}*/

	private void clearList()
	{
		playerPoint.text = "-";
		playerRank.text = "-";
		UnityEngine.Debug.LogError("childCount:" + parentContainer.childCount);
		for (int i = 0; i < parentContainer.childCount; i++)
		{
			GameObject gameObject = parentContainer.GetChild(i).gameObject;
			UnityEngine.Object.Destroy(gameObject);
		}
		loader.SetActive(value: true);
	}

	public void ListenerMethod(Vector2 value)
	{
		if (scrollRect.verticalNormalizedPosition == 0f)
		{
			UnityEngine.Debug.LogError("Load More: " + value);
		}
	}

	public void OnEndDrag(PointerEventData data)
	{
		UnityEngine.Debug.LogError("Stopped dragging " + base.name + "!");
	}

	public void ScrollDragEnd(PointerEventData data)
	{
		UnityEngine.Debug.LogError("Stopped dragging " + base.name + "!");
	}

	public static void synchPlayerCoins(int coins)
	{
		if (instance != null)
		{
			instance.UpdatePlayerCoins(coins);
		}
	}

	public static void synchPlayerDetails()
	{
		if (instance != null)
		{
			instance.UpdatePlayerDetails();
		}
	}

	/*private string getPlayerFormattedPoints(PlayerScore scores)
	{
		return scores.points + string.Empty;
	}*/

	private void logMessage(string message)
	{
		if (logView != null)
		{
			Text text = logView;
			text.text = text.text + message + "\n";
		}
	}

	public void addToLeaderBoard(int scored)
	{
		UnityEngine.Debug.LogError("addToLeaderBoard");
		/*PlayerScore playerScore = new PlayerScore();
		playerScore.playername = "Zeel";
		playerScore.points = 205L;
		playerScore.playerid = "11";
		playerScore.table = "highscores";
		playerScore.fields = new PDictionary
		{
			{
				"avatar",
				"10"
			},
			{
				"coins",
				1000
			}
		};
		PlayerScore score = playerScore;
		Playtomic.Leaderboards.Save(score, SubmitComplete);*/
	}

	/*private void SubmitComplete(PResponse response)
	{
		if (response.success)
		{
			UnityEngine.Debug.LogError("Score saved!");
		}
		else
		{
			UnityEngine.Debug.LogError("Score Failed! " + response.errorcode + ":" + response.errormessage);
		}
	}*/

	private void ShowScores()
	{
		/*PLeaderboardOptions pLeaderboardOptions = new PLeaderboardOptions();
		pLeaderboardOptions.Add("table", "highscores");
		pLeaderboardOptions.Add("page", 1);
		pLeaderboardOptions.Add("perpage", 10);
		pLeaderboardOptions.Add("highest", true);
		PLeaderboardOptions options = pLeaderboardOptions;
		Playtomic.Leaderboards.List(options, ListComplete);*/
	}

	/*private void ListComplete(List<PlayerScore> scores, int numscores, PResponse response)
	{
		if (response.success)
		{
			UnityEngine.Debug.LogError(scores.Count + " scores returned out of " + numscores);
			for (int i = 0; i < scores.Count; i++)
			{
				PlayerScore playerScore = scores[i];
				UnityEngine.Debug.LogError(" - " + playerScore.playername + " got " + playerScore.points + " on " + playerScore.playerid);
				Dictionary<string, object> dictionary = (Dictionary<string, object>)playerScore["fields"];
				foreach (string key in dictionary.Keys)
				{
					UnityEngine.Debug.LogError("****" + key);
				}
			}
		}
	}*/
}
