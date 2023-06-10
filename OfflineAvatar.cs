using UnityEngine;

public class OfflineAvatar : MonoBehaviour
{
	public OfflinePlayer[] offlineAvatars;

	private int firstPlayerIndex;

	public OfflinePlayer getFirstOfflinePlayer()
	{
		firstPlayerIndex = UnityEngine.Random.Range(0, offlineAvatars.Length);
		UnityEngine.Debug.LogError("Offline Player1 Index:" + firstPlayerIndex);
		return offlineAvatars[firstPlayerIndex];
	}

	public OfflinePlayer getSecondOfflinePlayer()
	{
		int num;
		for (num = UnityEngine.Random.Range(0, offlineAvatars.Length); num == firstPlayerIndex; num = UnityEngine.Random.Range(0, offlineAvatars.Length))
		{
		}
		UnityEngine.Debug.LogError("Offline Player2 Index:" + num);
		return offlineAvatars[num];
	}
}
