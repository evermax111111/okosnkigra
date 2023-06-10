using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Carousel2 : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IEventSystemHandler
{
	private bool moveRight;

	private bool processInput;

	private float time;

	private float position;

	private float lastPosition;

	private float offset;

	private float inputPositionStart;

	private float inputPositionEnd;

	private float positionDelta;

	private RectTransform rectTransform;

	private PointerEventData pointerEventData;

	public RectTransform[] Elements;

	public float ScaleMin = 0.7f;

	public float ScaleMax = 1f;

	public Vector2 Spacing = new Vector2(30f, 0f);

	public int SwipeThreshold = 30;

	public int currentSelection = 1;

	public bool UserInteractionEnabled
	{
		get;
		set;
	}

	public float ScrollDelta => Mathf.Abs(positionDelta);

	private void Awake()
	{
		UserInteractionEnabled = false;
		moveRight = false;
		processInput = false;
		time = 0f;
		position = 0f;
		lastPosition = 0f;
		offset = 0f;
		currentSelection = 1;
	}

	private void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		float num = 0f;
		for (int i = 0; i < Elements.Length; i++)
		{
			if (i == 0)
			{
				Vector2 anchoredPosition = Elements[0].anchoredPosition;
				num = anchoredPosition.x;
				float num2 = num;
				Vector2 sizeDelta = Elements[0].sizeDelta;
				num = num2 + sizeDelta.x;
			}
			else
			{
				num += Spacing.x;
				Vector2 anchoredPosition2 = Elements[i].anchoredPosition;
				anchoredPosition2.x = num;
				Elements[i].anchoredPosition = anchoredPosition2;
				float num3 = num;
				Vector2 sizeDelta2 = Elements[i].sizeDelta;
				num = num3 + sizeDelta2.x;
				Elements[i].localScale = Vector2.one * ScaleMin;
			}
		}
		moveRight = true;
		UserInteractionEnabled = true;
	}

	private void Update()
	{
		if (pointerEventData != null && processInput)
		{
			UnityEngine.Debug.Log("pointerEventData:  Drag = " + pointerEventData.delta);
			Vector2 delta = pointerEventData.delta;
			positionDelta = delta.x;
		}
		time += Time.deltaTime;
		if (time < 0.333f)
		{
			position = Mathf.Lerp(lastPosition, offset * -1f, time * 3f);
			lastPosition = position;
		}
		if (Mathf.Abs(positionDelta) > (float)SwipeThreshold && processInput)
		{
			processInput = false;
			lastPosition = position;
			if (currentSelection < Elements.Length)
			{
				SwipeDidEnd();
			}
			else if (currentSelection == Elements.Length)
			{
				if (positionDelta > 0f)
				{
					SwipeDidEnd();
				}
				else if (positionDelta < 0f)
				{
					time = 0f;
				}
			}
		}
		float num = 0f;
		Vector2 vector = Vector2.one * ScaleMin;
		Vector2 vector2 = Vector2.one * ScaleMax;
		for (int i = 0; i < Elements.Length; i++)
		{
			Vector2 anchoredPosition = Elements[i].anchoredPosition;
			anchoredPosition.x = position + num;
			Elements[i].anchoredPosition = anchoredPosition;
			float num2 = num;
			Vector2 sizeDelta = Elements[i].sizeDelta;
			num = num2 + sizeDelta.x;
			num += Spacing.x;
			int num3 = (!moveRight) ? currentSelection : (currentSelection - 1);
			Vector2 v = (i != num3) ? vector : vector2;
			Elements[i].localScale = Vector3.Lerp(Elements[i].localScale, v, Time.deltaTime * 5f);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (UserInteractionEnabled)
		{
			UnityEngine.Debug.Log("OnPointerDown");
			pointerEventData = eventData;
			SetRacyastingInChildrenEnabled(enabled: false);
			if (eventData.dragging)
			{
				Vector2 delta = eventData.delta;
				positionDelta = delta.x;
			}
			else
			{
				processInput = true;
				positionDelta = 0f;
			}
		}
	}

	public void OnPointerUp(PointerEventData pointerEventData)
	{
		if (UserInteractionEnabled)
		{
			time = ((!(Mathf.Abs(positionDelta) < (float)SwipeThreshold)) ? time : 0f);
			processInput = false;
			pointerEventData = null;
			SetRacyastingInChildrenEnabled(enabled: true);
			UnityEngine.Debug.Log("pointerEventData: " + pointerEventData + " -- OnPointerUp");
		}
	}

	private void SetRacyastingInChildrenEnabled(bool enabled)
	{
		Image[] componentsInChildren = GetComponentsInChildren<Image>();
		Image[] array = componentsInChildren;
		foreach (Image image in array)
		{
			image.raycastTarget = enabled;
		}
		Image component = GetComponent<Image>();
		if ((bool)component)
		{
			component.raycastTarget = true;
		}
	}

	private void SwipeDidEnd()
	{
		lastPosition = position;
		int num = Elements.Length;
		int num2 = 0;
		bool flag = false;
		if (positionDelta > 0f)
		{
			time = 0f;
			if (Mathf.Abs(positionDelta) > (float)SwipeThreshold)
			{
				if (currentSelection == 0)
				{
					offset = 0f;
				}
				else
				{
					if (moveRight)
					{
						currentSelection--;
					}
					moveRight = false;
					currentSelection--;
					currentSelection = ((currentSelection >= 0) ? currentSelection : 0);
					num2 = currentSelection;
					flag = true;
				}
			}
		}
		else if (positionDelta < 0f)
		{
			time = 0f;
			if (Mathf.Abs(positionDelta) > (float)SwipeThreshold)
			{
				if (currentSelection == num)
				{
					num2 = num - 1;
					flag = true;
				}
				else
				{
					if (!moveRight)
					{
						currentSelection++;
					}
					moveRight = true;
					num2 = currentSelection;
					flag = true;
					currentSelection++;
				}
			}
		}
		if (!flag)
		{
			return;
		}
		offset = 0f;
		for (int i = 0; i < num2; i++)
		{
			if (i < num)
			{
				float num3 = offset;
				Vector2 sizeDelta = Elements[i].sizeDelta;
				offset = num3 + sizeDelta.x;
				offset += Spacing.x;
			}
		}
	}
}
