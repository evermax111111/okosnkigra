using UnityEngine;

public class RateUsHelper : MonoBehaviour
{
	public GameObject rateUsPanel;

	private void Start()
	{
		if (LevelManager.getInstance().canShowRateUs())
		{
			rateUsPanel.SetActive(value: true);
		}
	}
}
