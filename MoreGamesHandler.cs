using UnityEngine;

public class MoreGamesHandler : MonoBehaviour
{
	public GameObject panel;

	public Animator anim;

	public void OpenPlayStoreGame(string androidpackageId)
	{
		//AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		Application.OpenURL("market://details?id=" + androidpackageId);
	}

	public void OpenAppstoreStoreGame(string appIdentifier)
	{
	}

	public void GoBackToMenu()
	{
		anim.SetTrigger("fade");
		//AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		panel.SetActive(value: false);
	}

	public void ShowMoreGames()
	{
		anim.SetTrigger("fade");
		//AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		panel.SetActive(value: true);
	}
}
