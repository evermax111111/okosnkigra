using UnityEngine;
using UnityEngine.UI;

public class CreateProfile : MonoBehaviour
{
	public RectTransform window;

	public bool startWelcomeScreen;

	public RectTransform[] introImages;

	private float wide;

	private float mousePositionStartX;

	private float mousePositionEndX;

	private float dragAmount;

	public float screenPosition;

	public float lastScreenPosition;

	private float lerpTimer;

	private float lerpPage;

	public int pageCount = 1;

	public string side = string.Empty;

	public int swipeThrustHold = 30;

	public int spaceBetweenProfileImages = 30;

	private bool canSwipe;

	public GameObject cartoonWindow;

	public Texture2D userPic;

	private void Start()
	{
		wide = cartoonWindow.GetComponent<RectTransform>().rect.width;
		for (int i = 1; i < introImages.Length; i++)
		{
			introImages[i].anchoredPosition = new Vector2((wide + (float)spaceBetweenProfileImages) * (float)i, 0f);
		}
		side = "right";
		startWelcomeScreen = true;
	}

	private void Update()
	{
		if (!startWelcomeScreen)
		{
			return;
		}
		lerpTimer += Time.deltaTime;
		if ((double)lerpTimer < 0.333)
		{
			screenPosition = Mathf.Lerp(lastScreenPosition, lerpPage * -1f, lerpTimer * 3f);
			lastScreenPosition = screenPosition;
		}
		if (Input.GetMouseButtonDown(0))
		{
			canSwipe = true;
			Vector3 mousePosition = UnityEngine.Input.mousePosition;
			mousePositionStartX = mousePosition.x;
		}
		if (Input.GetMouseButton(0) && canSwipe)
		{
			Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
			mousePositionEndX = mousePosition2.x;
			dragAmount = mousePositionEndX - mousePositionStartX;
			screenPosition = lastScreenPosition + dragAmount;
		}
		if (Mathf.Abs(dragAmount) > (float)swipeThrustHold && canSwipe)
		{
			canSwipe = false;
			lastScreenPosition = screenPosition;
			if (pageCount < introImages.Length)
			{
				OnSwipeComplete();
			}
			else if (pageCount == introImages.Length && dragAmount < 0f)
			{
				lerpTimer = 0f;
			}
			else if (pageCount == introImages.Length && dragAmount > 0f)
			{
				OnSwipeComplete();
			}
		}
		if (Input.GetMouseButtonUp(0) && Mathf.Abs(dragAmount) < (float)swipeThrustHold)
		{
			lerpTimer = 0f;
		}
		for (int i = 0; i < introImages.Length; i++)
		{
			introImages[i].anchoredPosition = new Vector2(screenPosition + (wide + (float)spaceBetweenProfileImages) * (float)i, 0f);
			if (side == "right")
			{
				if (i == pageCount - 1)
				{
					introImages[i].localScale = Vector3.Lerp(introImages[i].localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 5f);
					Color color = introImages[i].GetComponent<Image>().color;
					introImages[i].GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1f);
				}
				else
				{
					introImages[i].localScale = Vector3.Lerp(introImages[i].localScale, new Vector3(0.7f, 0.7f, 0.7f), Time.deltaTime * 5f);
					Color color2 = introImages[i].GetComponent<Image>().color;
					introImages[i].GetComponent<Image>().color = new Color(color2.r, color2.g, color2.b, 0.5f);
				}
			}
			else if (i == pageCount)
			{
				introImages[i].localScale = Vector3.Lerp(introImages[i].localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 5f);
				Color color3 = introImages[i].GetComponent<Image>().color;
				introImages[i].GetComponent<Image>().color = new Color(color3.r, color3.g, color3.b, 1f);
			}
			else
			{
				introImages[i].localScale = Vector3.Lerp(introImages[i].localScale, new Vector3(0.7f, 0.7f, 0.7f), Time.deltaTime * 5f);
				Color color4 = introImages[i].GetComponent<Image>().color;
				introImages[i].GetComponent<Image>().color = new Color(color4.r, color4.g, color4.b, 0.5f);
			}
		}
	}

	private void MoveTo()
	{
		for (int i = 0; i < introImages.Length; i++)
		{
			introImages[i].anchoredPosition = new Vector2(screenPosition + (wide + (float)spaceBetweenProfileImages) * (float)i, 0f);
			if (side == "right")
			{
				if (i == pageCount - 1)
				{
					introImages[i].localScale = Vector3.Lerp(introImages[i].localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 5f);
					Color color = introImages[i].GetComponent<Image>().color;
					introImages[i].GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1f);
				}
				else
				{
					introImages[i].localScale = Vector3.Lerp(introImages[i].localScale, new Vector3(0.7f, 0.7f, 0.7f), Time.deltaTime * 5f);
					Color color2 = introImages[i].GetComponent<Image>().color;
					introImages[i].GetComponent<Image>().color = new Color(color2.r, color2.g, color2.b, 0.5f);
				}
			}
			else if (i == pageCount)
			{
				introImages[i].localScale = Vector3.Lerp(introImages[i].localScale, new Vector3(1.2f, 1.2f, 1.2f), Time.deltaTime * 5f);
				Color color3 = introImages[i].GetComponent<Image>().color;
				introImages[i].GetComponent<Image>().color = new Color(color3.r, color3.g, color3.b, 1f);
			}
			else
			{
				introImages[i].localScale = Vector3.Lerp(introImages[i].localScale, new Vector3(0.7f, 0.7f, 0.7f), Time.deltaTime * 5f);
				Color color4 = introImages[i].GetComponent<Image>().color;
				introImages[i].GetComponent<Image>().color = new Color(color4.r, color4.g, color4.b, 0.5f);
			}
		}
	}

	private void OnSwipeComplete()
	{
		lastScreenPosition = screenPosition;
		if (dragAmount > 0f)
		{
			if (Mathf.Abs(dragAmount) > (float)swipeThrustHold)
			{
				if (pageCount == 0)
				{
					lerpTimer = 0f;
					lerpPage = 0f;
					return;
				}
				if (side == "right")
				{
					pageCount--;
				}
				side = "left";
				pageCount--;
				lerpTimer = 0f;
				if (pageCount < 0)
				{
					pageCount = 0;
				}
				lerpPage = (wide + (float)spaceBetweenProfileImages) * (float)pageCount;
			}
			else
			{
				lerpTimer = 0f;
			}
		}
		else
		{
			if (!(dragAmount < 0f))
			{
				return;
			}
			if (Mathf.Abs(dragAmount) > (float)swipeThrustHold)
			{
				if (pageCount == introImages.Length)
				{
					lerpTimer = 0f;
					lerpPage = (wide + (float)spaceBetweenProfileImages) * (float)introImages.Length - 1f;
					return;
				}
				if (side == "left")
				{
					pageCount++;
				}
				side = "right";
				lerpTimer = 0f;
				lerpPage = (wide + (float)spaceBetweenProfileImages) * (float)pageCount;
				pageCount++;
			}
			else
			{
				lerpTimer = 0f;
			}
		}
	}
}
