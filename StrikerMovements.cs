using UnityEngine;

public class StrikerMovements : MonoBehaviour
{
	public Rigidbody2D rb;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			UnityEngine.Debug.LogError("Pos:" + Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition));
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			vector.z = 0f;
			rb.AddForce(new Vector2(1f, 1f) * 200f);
		}
	}
}
