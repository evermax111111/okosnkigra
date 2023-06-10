using UnityEngine;

public class GameOverAnimator : MonoBehaviour
{
	public Transform winPosition;

	public Transform looserPosition;

	public GameObject winnerTag;

	private bool animateWinner;

	private bool animateLooser;

	private float animateWinnerDuration;

	private float animateWinnerEndDuration = 1f;

	private void Start()
	{
	}

	private void Update()
	{
		if (animateWinnerDuration > animateWinnerEndDuration && animateWinner)
		{
			if (animateWinner)
			{
				winnerTag.SetActive(value: true);
			}
			animateWinner = false;
			animateLooser = false;
		}
		if (animateWinner)
		{
			animateWinnerDuration += Time.deltaTime;
			float t = animateWinnerDuration / animateWinnerEndDuration;
			base.transform.position = Vector3.Lerp(base.transform.position, winPosition.position, t);
		}
		if (animateLooser)
		{
			animateWinnerDuration += Time.deltaTime;
			float t2 = animateWinnerDuration / animateWinnerEndDuration;
			base.transform.position = Vector3.Lerp(base.transform.position, looserPosition.position, t2);
		}
	}

	public void AnimateWinner()
	{
		RectTransform component = GetComponent<RectTransform>();
		component.pivot = new Vector2(0.5f, 0.5f);
		animateWinnerDuration = 0f;
		animateWinner = true;
	}

	public void AnimateLooser()
	{
		RectTransform component = GetComponent<RectTransform>();
		component.pivot = new Vector2(0.5f, 0.5f);
		animateWinnerDuration = 0f;
		animateLooser = true;
	}
}
