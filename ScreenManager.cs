using UnityEngine;

public class ScreenManager : MonoBehaviour
{
	private void OnEnable()
	{
		Screen.sleepTimeout = -1;
	}

	private void OnDisable()
	{
		Screen.sleepTimeout = -2;
	}
}
