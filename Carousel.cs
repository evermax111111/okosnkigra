using UnityEngine;

public class Carousel : MonoBehaviour
{
	private bool moveRight;

	public bool processInput;

	private float time;

	private float position;

	private float lastPosition;

	private float offset;

	private float inputPositionStart;

	private float inputPositionEnd;

	private float positionDelta;

	private RectTransform rectTransform;

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
		UserInteractionEnabled = true;
		moveRight = false;
		processInput = true;
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
				UnityEngine.Debug.LogError("xOffset " + num);
				float num2 = num;
				Vector2 sizeDelta = Elements[0].sizeDelta;
				num = num2 + sizeDelta.x;
				UnityEngine.Debug.LogError("xOffset " + num);
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
		if (!UserInteractionEnabled)
		{
			processInput = false;
			return;
		}
		TouchPhase touchPhase = TouchPhase.Stationary;
		Vector2 zero = Vector2.zero;
		if (UnityEngine.Input.touchCount == 1)
		{
			Touch touch = Input.touches[0];
			touchPhase = touch.phase;
			zero = touch.position;
		}
		time += Time.deltaTime;
		if (time < 0.333f)
		{
			position = Mathf.Lerp(lastPosition, offset * -1f, time * 3f);
			lastPosition = position;
			lastPosition = 0f;
		}
		if (touchPhase == TouchPhase.Began && RectTransformUtility.RectangleContainsScreenPoint(rectTransform, zero, Camera.main))
		{
			processInput = true;
			inputPositionStart = zero.x;
			inputPositionEnd = zero.x;
			positionDelta = 0f;
		}
		if (touchPhase == TouchPhase.Moved && processInput)
		{
			inputPositionEnd = zero.x;
			positionDelta = inputPositionEnd - inputPositionStart;
			position = lastPosition + positionDelta;
			UnityEngine.Debug.LogError("Moving " + position);
		}
		if (touchPhase == TouchPhase.Ended || touchPhase == TouchPhase.Canceled)
		{
			time = ((!(Mathf.Abs(positionDelta) < (float)SwipeThreshold)) ? time : 0f);
		}
		if (Mathf.Abs(positionDelta) > (float)SwipeThreshold && processInput)
		{
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
