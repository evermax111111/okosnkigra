using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class StrikerMover : MonoBehaviour
{
	public Transform leftEnd;

	public Transform rightEnd;

	public Transform strikerPosition;

	private Vector3 end;

	private AudioSource source;

	private CircleCollider2D strikerCollider;

	private CircleCollider2D scollider;

	public static byte MOVE_STRIKER = 101;

	[SerializeField]
	private GameObject striker;

	private PhotonView gamePhotonView;

	private bool isDragging;

	private Vector3 lastMovement;

	private bool animateStriker;

	private float animateStrikerDuration;

	private float animateStrikerEndDuration = 1f;

	private void Start()
	{
		end = base.transform.position;
		end.x = 0f;
		source = GetComponent<AudioSource>();
	}

	public void SetStriker(GameObject striker, int strikerIndex)
	{
		this.striker = striker;
		strikerCollider = striker.transform.GetChild(0).GetComponent<CircleCollider2D>();
		scollider = striker.GetComponent<CircleCollider2D>();
		gamePhotonView = striker.GetComponent<PhotonView>();
		GetComponent<SpriteRenderer>().sprite = ((strikerIndex >= LevelManager.instance.strikers.Length) ? LevelManager.instance.strikers[0] : LevelManager.instance.strikers[strikerIndex]);
	}

	private void Update()
	{
		if (animateStrikerDuration > animateStrikerEndDuration && animateStriker)
		{
			animateStriker = false;
		}
		if (animateStriker)
		{
			animateStrikerDuration += Time.deltaTime;
			float t = animateStrikerDuration / animateStrikerEndDuration;
			base.transform.position = Vector3.Lerp(base.transform.position, end, t);
		}
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector2.zero);
			if (raycastHit2D.collider != null)
			{
				if (raycastHit2D.collider.gameObject.name == base.gameObject.name)
				{
					isDragging = true;
					strikerCollider.enabled = false;
				}
				else
				{
					isDragging = false;
				}
			}
			else
			{
				isDragging = false;
			}
		}
		if (Input.GetMouseButton(0) && isDragging && ((CarromGameManager.Instance != null) ? (CarromGameManager.Instance.isPlayerTurn && scollider.isTrigger) : ((!(OfflineGameManager.Instance != null)) ? MasterStrikeManager.Instance.canMoveStriker : OfflineGameManager.Instance.isPlayerTurn)))
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			Vector3 position = leftEnd.position;
			vector.y = position.y;
			vector.z = 0f;
			float x = vector.x;
			Vector3 position2 = leftEnd.position;
			float x2 = position2.x;
			Vector3 position3 = rightEnd.position;
			vector.x = Mathf.Clamp(x, x2, position3.x);
			base.transform.position = vector;
			Vector3 position4 = strikerPosition.position;
			vector.y = position4.y;
			if (striker != null)
			{
				striker.transform.position = vector;
				if (gamePhotonView != null && !object.Equals(vector, lastMovement))
				{
					object[] eventContent = new object[1]
					{
						vector
					};
					SendOptions sendOptions = default(SendOptions);
					sendOptions.Reliability = false;
					SendOptions sendOptions2 = sendOptions;
					RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
					raiseEventOptions.Receivers = ReceiverGroup.Others;
					RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
					PhotonNetwork.RaiseEvent(MOVE_STRIKER, eventContent, raiseEventOptions2, sendOptions2);
				}
			}
			if (!source.isPlaying && (vector - lastMovement).magnitude > 0f)
			{
				source.Play();
			}
			lastMovement = vector;
		}
		if (Input.GetMouseButtonUp(0) && strikerCollider != null)
		{
			isDragging = false;
			strikerCollider.enabled = true;
			if (source.isPlaying)
			{
				source.Stop();
			}
		}
	}

	[PunRPC]
	public void MoveStriker()
	{
	}

	public void AnimateStrikerHandlerToCenter()
	{
		animateStrikerDuration = 0f;
		animateStriker = true;
	}
}
