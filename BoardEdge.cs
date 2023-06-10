using UnityEngine;

public class BoardEdge : MonoBehaviour
{
	public AudioSource source;

	private bool audioEnabled;

	private void Start()
	{
		audioEnabled = ((PlayerPrefs.GetInt("audio", 1) == 1) ? true : false);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!(collision.gameObject == null))
		{
			Rigidbody2D component = collision.gameObject.GetComponent<Rigidbody2D>();
			if (component != null && audioEnabled)
			{
				float a = collision.relativeVelocity.magnitude / 10f;
				source.volume = Mathf.Min(a, 1f);
				source.Play();
			}
		}
	}
}
