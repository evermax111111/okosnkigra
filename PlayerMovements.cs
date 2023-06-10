using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
	public Transform borderRight;

	public Transform borderLeft;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			UnityEngine.Debug.LogError("Pos:" + Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition));
			Vector3 position = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			Vector3 position2 = base.transform.position;
			position.y = position2.y;
			float z = position.z;
			Vector3 position3 = borderLeft.position;
			float z2 = position3.z;
			Vector3 position4 = borderRight.position;
			position.z = Mathf.Clamp(z, z2, position4.z);
			base.transform.position = position;
		}
	}
}
