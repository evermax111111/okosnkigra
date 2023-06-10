using UnityEngine;

public class HelpStrikerMover : MonoBehaviour
{
	public Transform leftEnd;

	public Transform rightEnd;

	public Transform strikerPosition;

	public HelpManager helpManager;

	private Vector3 end;

	private AudioSource source;

	[SerializeField]
	private GameObject striker;

	private bool isDragging;

	private Vector3 lastMovement;

	private void Start()
	{
		end = base.transform.position;
		end.x = 0f;
		source = GetComponent<AudioSource>();
	}

	public void SetStriker(GameObject striker)
	{
		this.striker = striker;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition), Vector2.zero);
			if (raycastHit2D.collider != null)
			{
				if (raycastHit2D.collider.gameObject.name == base.gameObject.name)
				{
					isDragging = true;
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
		if (Input.GetMouseButton(0) && isDragging)
		{
			helpManager.HideDragHelp();
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
			}
			if (!source.isPlaying && (vector - lastMovement).magnitude > 0f)
			{
				source.Play();
			}
			lastMovement = vector;
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (isDragging)
			{
				helpManager.ShowDragHelp();
			}
			isDragging = false;
			if (source.isPlaying)
			{
				source.Stop();
			}
		}
	}
}
