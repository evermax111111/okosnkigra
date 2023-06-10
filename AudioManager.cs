using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static string PLAY_PLACE_CARD = "place_card";

	public static string PLAY_HIT_CARD = "hit";

	public static string PLAY_ADD_CARD = "add_cards";

	public static string PLAY_WIN = "win";

	public static string PLAY_LOSE = "lost";

	public static string PLAY_CLICK = "click";

	public static string PLAY_PLAYER_JOINED = "player_joined";

	public static string PLAY_PLAYER_TURN = "player_turn";

	public static string PLAY_TICK = "tick";

	public static string PLAY_CHAT = "chat";

	public static string PLAY_COIN_COLLECT = "coin_collect";

	public static string PLAYER_LEFT = "player_left";

	public static string GAME_OVER = "gameover";

	public static string PLAY_POINT_SCORED = "point_scored";

	public static string PLAY_FOUL = "foul";

	public static string PLAY_STRIKER_DRAG = "striker_drag";

	public static string PLAY_GEM_AQUIRED = "gem_aquired";

	public Sound[] sounds;

	private static AudioManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public static AudioManager getInstance()
	{
		return instance;
	}

	private void Start()
	{
		Sound[] array = sounds;
		foreach (Sound sound in array)
		{
			sound.source = base.gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;
			sound.source.volume = sound.volume;
			sound.source.loop = sound.loop;
			sound.source.pitch = sound.pitch;
		}
	}

	public void PlaySound(string soundName)
	{
		if (PlayerPrefs.GetInt("audio", 1) != 1 || 1 == 0)
		{
			return;
		}
		Sound[] array = sounds;
		int num = 0;
		Sound sound;
		while (true)
		{
			if (num < array.Length)
			{
				sound = array[num];
				if (sound.name == soundName && sound.source != null)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		sound.source.Play();
	}

	public void StopSound(string soundName)
	{
		Sound[] array = sounds;
		int num = 0;
		Sound sound;
		while (true)
		{
			if (num < array.Length)
			{
				sound = array[num];
				if (sound.name == soundName && sound.source.isPlaying)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		sound.source.Stop();
	}
}
