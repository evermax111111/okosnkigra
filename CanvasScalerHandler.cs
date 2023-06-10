using BBModel;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScalerHandler : MonoBehaviour
{
	private CanvasScaler scaller;

	private void Awake()
	{
		scaller = GetComponent<CanvasScaler>();
		BBDevice cHDevice =BBDevice.iPhone;
		if (cHDevice == BBDevice.iPad)
		{
			scaller.matchWidthOrHeight = 1f;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
