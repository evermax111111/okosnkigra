using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class trajectoryScript : MonoBehaviourPun, IPunInstantiateMagicCallback, IOnEventCallback
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

	private CarromGameManager gameManager;

	private StrikerAnimator strikerHandler;

	private CircleCollider2D circleCollider2d;

	private SpriteRenderer spriteRender;

	private AudioSource source;

	private bool audioEnabled;

	private Vector3 lastfingurePos;

	private Vector2 velocity;

	private bool canStrike = true;

	private void Start()
	{
		gameManager = UnityEngine.Object.FindObjectOfType<CarromGameManager>();
		strikerHandler = GetComponent<StrikerAnimator>();
		source = GetComponent<AudioSource>();
		audioEnabled = ((PlayerPrefs.GetInt("audio", 1) == 1) ? true : false);
		ball = base.gameObject;
		ballClick = base.gameObject.transform.GetChild(0).gameObject;
		arrow = base.gameObject.transform.GetChild(1).gameObject;
		circleCollider2d = ball.GetComponent<CircleCollider2D>();
		spriteRender = ball.GetComponent<SpriteRenderer>();
		if (ballClick == null)
		{
		}
		if (base.photonView.IsMine || !PhotonNetwork.IsConnected)
		{
			trajectoryDots = GameObject.Find("Trajectory Dots");
		}
		else
		{
			trajectoryDots = GameObject.Find("Trajectory Dots(Clone)");
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
		for (int j = numberOfDots; j < 40; j++)
		{
		}
		trajectoryDots.SetActive(value: false);
		base.photonView.RPC("EnableOrDisableDots", RpcTarget.All, false);
	}

	private void Update()
	{
		if (!base.photonView.IsMine && PhotonNetwork.IsConnected)
		{
			return;
		}
		if (numberOfDots > 40)
		{
			numberOfDots = 40;
		}
		if (usingHelpGesture)
		{
			helpGesture.transform.position = new Vector3(ballPos.x, ballPos.y, ballPos.z);
		}
		RaycastHit2D raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector2.zero);
		if (raycastHit2D.collider != null && !ballIsClicked2 && !gameManager.isGameOver && !gameManager.IsChatPannelOpen())
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
		Vector2 vector = ballRB.velocity;
		float x = vector.x;
		Vector2 vector2 = ballRB.velocity;
		float num = x * vector2.x;
		Vector2 vector3 = ballRB.velocity;
		float y = vector3.y;
		Vector2 vector4 = ballRB.velocity;
		if (num + y * vector4.y <= 0.0085f)
		{
			ballRB.velocity = new Vector2(0f, 0f);
			idleTimer -= Time.deltaTime;
		}
		else
		{
			arrow.SetActive(value: false);
			ResetTrajectoryDotPositions();
			if (trajectoryDots.activeSelf)
			{
				base.photonView.RPC("EnableOrDisableDots", RpcTarget.All, false);
			}
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
			Vector2 vector5 = ballRB.velocity;
			if (vector5.x == 0f)
			{
				Vector2 vector6 = ballRB.velocity;
				if (vector6.y == 0f)
				{
					goto IL_02ee;
				}
			}
			if (grabWhileMoving)
			{
				goto IL_02ee;
			}
		}
		goto IL_05c2;
		IL_05c2:
		if (!Input.GetKeyUp(KeyCode.Mouse0))
		{
			return;
		}
		ballIsClicked2 = false;
		if (!trajectoryDots.activeInHierarchy)
		{
			return;
		}
		if (explodeEnabled)
		{
			StartCoroutine(explode());
		}
		if (gameManager.isPlayerTurn && canStrike)
		{
			gameManager.FinishTurn();
			trajectoryDots.SetActive(value: false);
			arrow.SetActive(value: false);
			ResetTrajectoryDotPositions();
			base.photonView.RPC("EnableOrDisableDots", RpcTarget.All, false);
			base.photonView.RPC("MoveStriker", RpcTarget.All, new Vector2(shotForce.x, shotForce.y), base.transform.position, base.transform.rotation);
			if (!ballRB.isKinematic)
			{
			}
		}
		return;
		IL_02ee:
		if (canStrike)
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
			float num2 = Mathf.Min(1f + ballFingerDiff.magnitude * 4f, 7f);
			arrow.transform.localScale = new Vector3(num2, num2, 1f);
			if (Mathf.Sqrt(ballFingerDiff.x * ballFingerDiff.x + ballFingerDiff.y * ballFingerDiff.y) > 0.4f)
			{
				arrow.SetActive(value: true);
				if (!trajectoryDots.activeSelf)
				{
					base.photonView.RPC("EnableOrDisableDots", RpcTarget.All, true);
				}
			}
			else
			{
				arrow.SetActive(value: false);
				ResetTrajectoryDotPositions();
				if (trajectoryDots.activeSelf)
				{
					base.photonView.RPC("EnableOrDisableDots", RpcTarget.All, false);
				}
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
		goto IL_05c2;
	}

	[PunRPC]
	public void MoveStriker(Vector2 velocity, Vector3 position, Quaternion rotation)
	{
		canStrike = false;
		strikerHandler.SetStrikerMoving();
		circleCollider2d.enabled = true;
		circleCollider2d.isTrigger = false;
		ballRB.constraints = RigidbodyConstraints2D.None;
		ballRB.constraints = RigidbodyConstraints2D.FreezeRotation;
		if (!gameManager.isPlayerTurn)
		{
		}
		base.transform.position = position;
		if (!gameManager.isPlayerTurn)
		{
		}
		if (audioEnabled)
		{
			source.Play();
		}
		ballRB.velocity = velocity;
		if (ballRB.isKinematic)
		{
			ballRB.isKinematic = false;
		}
	}

	private void PushStriker()
	{
		if (audioEnabled)
		{
			source.Play();
		}
		ballRB.velocity = velocity;
	}

	[PunRPC]
	public void DragStriker(Vector3 movement)
	{
		base.transform.position = movement;
	}

	[PunRPC]
	public void StopPuck(Vector3 position)
	{
		ballRB.velocity = Vector2.zero;
		ballRB.constraints = RigidbodyConstraints2D.FreezePosition;
		base.transform.position = position;
		spriteRender.color = strikerInHoleColor;
		circleCollider2d.isTrigger = true;
		circleCollider2d.enabled = false;
		Vector2 v = base.transform.localScale;
		v.x -= 0.05f;
		v.y -= 0.05f;
		base.transform.localScale = v;
	}

	[PunRPC]
	public void DisableBallTrigger()
	{
		circleCollider2d.isTrigger = false;
		if (audioEnabled)
		{
			source.Play();
		}
	}

	[PunRPC]
	public void StopAudio()
	{
		if (audioEnabled && source.isPlaying)
		{
			source.Play();
		}
	}

	[PunRPC]
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

	[PunRPC]
	public void EnableOrDisableDots(bool state)
	{
		if (trajectoryDots != null)
		{
			trajectoryDots.SetActive(state);
		}
	}

	[PunRPC]
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
		if (base.photonView.IsMine && collision.tag == "Puck")
		{
			canStrike = false;
			base.photonView.RPC("StrikerCantStrike", RpcTarget.All, null);
		}
	}

	[PunRPC]
	public void StrikerCanStrike()
	{
		spriteRender.color = enabledColor;
	}

	[PunRPC]
	public void StrikerCantStrike()
	{
		spriteRender.color = disabledColor;
	}

	public void MakeStrikerStrike()
	{
		canStrike = true;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (base.photonView.IsMine && collision.tag == "Puck")
		{
			canStrike = true;
			base.photonView.RPC("StrikerCanStrike", RpcTarget.All, null);
		}
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		int num = (int)info.Sender.CustomProperties["striker"];
		GetComponent<SpriteRenderer>().sprite = LevelManager.instance.strikers[num];
		if (info.photonView.IsMine)
		{
		}
	}

	public void OnEvent(EventData photonEvent)
	{
		byte code = photonEvent.Code;
		if (code == StrikerMover.MOVE_STRIKER && !base.photonView.IsMine)
		{
			object[] array = (object[])photonEvent.CustomData;
			Vector3 position = (Vector3)array[0];
			base.transform.position = position;
		}
	}

	public void OnEnable()
	{
		PhotonNetwork.AddCallbackTarget(this);
	}

	public void OnDisable()
	{
		PhotonNetwork.RemoveCallbackTarget(this);
	}
}
