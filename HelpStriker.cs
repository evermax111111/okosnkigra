using System.Collections;
using UnityEngine;

public class HelpStriker : MonoBehaviour
{
	public Sprite dotSprite;

	public bool changeSpriteAfterStart;

	public float initialDotSize;

	public int numberOfDots;

	public float dotSeparation;

	public float dotShift;

	public float idleTime;

	public GameObject trajectoryDots;

	private GameObject ball;

	private Rigidbody2D ballRB;

	private Vector3 ballPos;

	private Vector3 fingerPos;

	private Vector3 ballFingerDiff;

	private Vector2 shotForce;

	private float x1;

	private float y1;

	private GameObject helpGesture;

	private float idleTimer = 7f;

	private bool ballIsClicked;

	private bool ballIsClicked2;

	private GameObject ballClick;

	public float shootingPowerX;

	public float shootingPowerY;

	public bool usingHelpGesture;

	public bool explodeEnabled;

	public bool grabWhileMoving;

	public GameObject[] dots;

	public bool mask;

	private BoxCollider2D[] dotColliders;

	private GameObject arrow;

	public Color enabledColor;

	public Color disabledColor;

	public Color strikerInHoleColor;

	private HelpManager gameManager;

	private OfflineStrikerAnimator strikerHandler;

	private CircleCollider2D circleCollider2d;

	private SpriteRenderer spriteRender;

	private AudioSource source;

	private Vector3 lastfingurePos;

	private bool isMoving;

	private bool canStrike = true;

	private void Start()
	{
		gameManager = UnityEngine.Object.FindObjectOfType<HelpManager>();
		strikerHandler = GetComponent<OfflineStrikerAnimator>();
		source = GetComponent<AudioSource>();
		ball = base.gameObject;
		ballClick = base.gameObject.transform.GetChild(0).gameObject;
		arrow = base.gameObject.transform.GetChild(1).gameObject;
		circleCollider2d = ball.GetComponent<CircleCollider2D>();
		spriteRender = ball.GetComponent<SpriteRenderer>();
		if (ballClick == null)
		{
		}
		if (usingHelpGesture)
		{
			helpGesture = GameObject.Find("Help Gesture");
		}
		ballRB = GetComponent<Rigidbody2D>();
		Transform transform = trajectoryDots.transform;
		float x = initialDotSize;
		float y = initialDotSize;
		Vector3 localScale = trajectoryDots.transform.localScale;
		transform.localScale = new Vector3(x, y, localScale.z);
		for (int i = 0; i < numberOfDots; i++)
		{
			dots[i] = trajectoryDots.transform.GetChild(i).gameObject;
			if (dotSprite != null)
			{
				dots[i].GetComponent<SpriteRenderer>().sprite = dotSprite;
			}
		}
		trajectoryDots.SetActive(value: false);
		EnableOrDisableDots(state: false);
	}

	private void Update()
	{
		if (numberOfDots > 40)
		{
			numberOfDots = 40;
		}
		if (usingHelpGesture)
		{
			helpGesture.transform.position = new Vector3(ballPos.x, ballPos.y, ballPos.z);
		}
		RaycastHit2D raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector2.zero);
		if (raycastHit2D.collider != null && !ballIsClicked2)
		{
			if (raycastHit2D.collider.gameObject.name == ballClick.gameObject.name)
			{
				ballIsClicked = true;
			}
			else
			{
				ballIsClicked = false;
			}
		}
		else
		{
			ballIsClicked = false;
		}
		if (ballIsClicked2)
		{
			ballIsClicked = true;
		}
		Vector2 velocity = ballRB.velocity;
		float x = velocity.x;
		Vector2 velocity2 = ballRB.velocity;
		float num = x * velocity2.x;
		Vector2 velocity3 = ballRB.velocity;
		float y = velocity3.y;
		Vector2 velocity4 = ballRB.velocity;
		if (num + y * velocity4.y <= 0.0085f)
		{
			ballRB.velocity = new Vector2(0f, 0f);
			idleTimer -= Time.deltaTime;
		}
		else if (trajectoryDots.activeSelf)
		{
			trajectoryDots.SetActive(value: false);
			arrow.SetActive(value: false);
			ResetTrajectoryDotPositions();
		}
		if (usingHelpGesture && idleTimer <= 0f)
		{
			helpGesture.GetComponent<Animator>().SetBool("Inactive", value: true);
		}
		ballPos = ball.transform.position;
		if (changeSpriteAfterStart)
		{
			for (int i = 0; i < numberOfDots; i++)
			{
				if (dotSprite != null)
				{
					dots[i].GetComponent<SpriteRenderer>().sprite = dotSprite;
				}
			}
		}
		if (UnityEngine.Input.GetKey(KeyCode.Mouse0) && ballIsClicked)
		{
			Vector2 velocity5 = ballRB.velocity;
			if (velocity5.x == 0f)
			{
				Vector2 velocity6 = ballRB.velocity;
				if (velocity6.y == 0f)
				{
					goto IL_029f;
				}
			}
			if (grabWhileMoving)
			{
				goto IL_029f;
			}
		}
		goto IL_0554;
		IL_0554:
		if (UnityEngine.Input.GetKeyUp(KeyCode.Mouse0) && ballIsClicked)
		{
			ballIsClicked2 = false;
			if (trajectoryDots.activeInHierarchy)
			{
				if (explodeEnabled)
				{
					StartCoroutine(explode());
				}
				if (canStrike)
				{
					isMoving = true;
					gameManager.PlayerStriked();
					trajectoryDots.SetActive(value: false);
					arrow.SetActive(value: false);
					ResetTrajectoryDotPositions();
					EnableOrDisableDots(state: false);
					strikerHandler.SetStrikerMoving();
					DisableBallTrigger();
					ballRB.velocity = new Vector2(shotForce.x, shotForce.y);
					if (ballRB.isKinematic)
					{
						ballRB.isKinematic = false;
					}
				}
			}
		}
		if (isMoving && AllStoppedMoving())
		{
			isMoving = false;
			gameManager.HelpComplete();
		}
		return;
		IL_029f:
		if (canStrike && gameManager.isDragged)
		{
			ballIsClicked2 = true;
			if (usingHelpGesture)
			{
				idleTimer = idleTime;
				helpGesture.GetComponent<Animator>().SetBool("Inactive", value: false);
			}
			fingerPos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			fingerPos.z = 0f;
			if (grabWhileMoving)
			{
				ballRB.velocity = new Vector2(0f, 0f);
				ballRB.isKinematic = true;
			}
			ballFingerDiff = ballPos - fingerPos;
			ballFingerDiff = Vector3.ClampMagnitude(ballFingerDiff, 1.5f);
			shotForce = new Vector2(ballFingerDiff.x * shootingPowerX, ballFingerDiff.y * shootingPowerY);
			float num2 = Mathf.Min(1f + ballFingerDiff.magnitude * 2f, 7f);
			arrow.transform.localScale = new Vector3(num2, num2, 1f);
			if (Mathf.Sqrt(ballFingerDiff.x * ballFingerDiff.x + ballFingerDiff.y * ballFingerDiff.y) > 0.4f)
			{
				trajectoryDots.SetActive(value: true);
				arrow.SetActive(value: true);
				gameManager.HideDragDown();
				EnableOrDisableDots(state: true);
			}
			else
			{
				trajectoryDots.SetActive(value: false);
				arrow.SetActive(value: false);
				ResetTrajectoryDotPositions();
				EnableOrDisableDots(state: false);
				if (ballRB.isKinematic)
				{
					ballRB.isKinematic = false;
				}
			}
			for (int j = 0; j < numberOfDots; j++)
			{
				x1 = ballPos.x + shotForce.x * Time.fixedDeltaTime * (dotSeparation * (float)j + dotShift);
				y1 = ballPos.y + shotForce.y * Time.fixedDeltaTime * (dotSeparation * (float)j + dotShift);
				Transform transform = dots[j].transform;
				float x2 = x1;
				float y2 = y1;
				Vector3 position = dots[j].transform.position;
				transform.position = new Vector3(x2, y2, position.z);
			}
		}
		goto IL_0554;
	}

	private bool AllStoppedMoving()
	{
		if (ballRB.velocity.magnitude > 0f)
		{
			return false;
		}
		return true;
	}

	public void DisableBallTrigger()
	{
		circleCollider2d.isTrigger = false;
		source.Play();
	}

	public void StopAudio()
	{
		if (source.isPlaying)
		{
			source.Play();
		}
	}

	public void EnableBallTrigger()
	{
		circleCollider2d.isTrigger = true;
	}

	private void HideTrajectory()
	{
		for (int i = 0; i < numberOfDots; i++)
		{
			dots[i].transform.gameObject.SetActive(value: false);
		}
	}

	private void showTrajectory()
	{
		for (int i = 0; i < numberOfDots; i++)
		{
			dots[i].transform.gameObject.SetActive(value: true);
		}
	}

	public IEnumerator explode()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime * (dotSeparation * ((float)numberOfDots - 1f)));
		UnityEngine.Debug.Log("exploded");
	}

	public void collided(GameObject dot)
	{
		UnityEngine.Debug.LogError("******************");
		for (int i = 0; i < numberOfDots; i++)
		{
			if (dot.name == "Dot (" + i + ")")
			{
				for (int j = i + 1; j < numberOfDots; j++)
				{
					dots[j].gameObject.GetComponent<SpriteRenderer>().enabled = false;
				}
			}
		}
	}

	public void uncollided(GameObject dot)
	{
		for (int i = 0; i < numberOfDots; i++)
		{
			if (!(dot.name == "Dot (" + i + ")"))
			{
				continue;
			}
			for (int num = i - 1; num > 0; num--)
			{
				if (!dots[num].gameObject.GetComponent<SpriteRenderer>().enabled)
				{
					UnityEngine.Debug.Log("nigggssss");
					return;
				}
			}
			if (!dots[i].gameObject.GetComponent<SpriteRenderer>().enabled)
			{
				for (int num2 = i; num2 > 0; num2--)
				{
					dots[num2].gameObject.GetComponent<SpriteRenderer>().enabled = true;
				}
			}
		}
	}

	private void ResetTrajectoryDotPositions()
	{
		for (int i = 0; i < numberOfDots; i++)
		{
			dots[i].transform.position = Vector3.zero;
		}
	}

	public void EnableOrDisableDots(bool state)
	{
		if (trajectoryDots != null)
		{
			trajectoryDots.SetActive(state);
		}
	}

	public void ChangeStrikerColor(int color)
	{
		if (color == 0)
		{
			spriteRender.color = strikerInHoleColor;
		}
		else
		{
			spriteRender.color = Color.white;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Puck")
		{
			canStrike = false;
			spriteRender.color = disabledColor;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Puck")
		{
			canStrike = true;
			spriteRender.color = enabledColor;
		}
	}
}
