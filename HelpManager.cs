using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour
{
	public GameObject dragHelpDown;

	public GameObject dragHelpRight;

	public GameObject hepCompletePanel;

	public GameObject tutorialWelcomePanel;

	public Text helpText;

	public GameObject helpPanel;

	public GameObject dragBubble;

	public GameObject gameoverPartile;

	public GameObject striker;

	public GameObject strikerHandler;

	public bool isDragged;

	private void Start()
	{
		if (AudioManager.getInstance() != null)
		{
			AudioManager.getInstance().PlaySound(AudioManager.PLAY_PLAYER_TURN);
		}
	}

	public void PlayerStriked()
	{
	}

	public void ShowDragHelp()
	{
		helpPanel.SetActive(value: true);
		helpText.text = "Now pull the striker down to aim and release";
		dragHelpDown.SetActive(value: true);
	}

	public void HideDragHelp()
	{
		isDragged = true;
		dragBubble.SetActive(value: false);
		dragHelpRight.SetActive(value: false);
	}

	public void HideDragDown()
	{
		dragHelpDown.SetActive(value: false);
	}

	public void HelpComplete()
	{
		gameoverPartile.SetActive(value: true);
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_WIN);
		hepCompletePanel.SetActive(value: true);
	}

	public void PlayGame()
	{
		AudioManager.getInstance().StopSound(AudioManager.PLAY_WIN);
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		PlayerPrefs.SetInt("show_help", 0);
		LevelManager.getInstance().setGameMode(LevelManager.GameMode.MULTIPLAYER);
		LevelManager.getInstance().setGameBet(LevelManager.Bet.BET_250);
		SceneManager.LoadScene("CarromOnline");
	}

	public void ContinueTutorial()
	{
		AudioManager.getInstance().PlaySound(AudioManager.PLAY_CLICK);
		dragBubble.SetActive(value: true);
		helpPanel.SetActive(value: true);
		helpText.text = "Drag the striker left or right";
		tutorialWelcomePanel.SetActive(value: false);
		dragHelpRight.SetActive(value: true);
		striker.SetActive(value: true);
		strikerHandler.SetActive(value: true);
	}
}
