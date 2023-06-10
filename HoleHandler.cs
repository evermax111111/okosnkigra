using Photon.Pun;
using System.Collections;
using UnityEngine;

public class HoleHandler : MonoBehaviour
{
	public GameObject shadowLayer;

	public Color delected;

	private AudioSource source;

	private Vector3 center;

	private void Start()
	{
		center = base.transform.GetChild(1).position;
		source = GetComponent<AudioSource>();
	}

	private void Update()
	{
	}

	private void moveCoinsToHole()
	{
	}

	public IEnumerator DestroyPuck(GameObject puck)
	{
		UnityEngine.Debug.LogError("DestroyPuck");
		yield return new WaitForSeconds(0.5f);
		if (CarromGameManager.Instance.isPlayerTurn)
		{
			PhotonNetwork.Destroy(puck);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
	}

	private void HideShadowLayer()
	{
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!(collision.tag == "Player") && !(collision.tag == "Puck"))
		{
			return;
		}
		CircleCollider2D component = collision.gameObject.GetComponent<CircleCollider2D>();
		if (component.isTrigger)
		{
			return;
		}
		source.Play();
		component.isTrigger = true;
		PuckColor component2 = collision.gameObject.GetComponent<PuckColor>();
		PhotonView component3 = collision.gameObject.GetComponent<PhotonView>();
		if (component3 != null)
		{
			Vector2 v = collision.gameObject.transform.localScale;
			v.x -= 0.05f;
			v.y -= 0.05f;
			collision.gameObject.transform.localScale = v;
			Rigidbody2D component4 = collision.gameObject.GetComponent<Rigidbody2D>();
			CircleCollider2D component5 = collision.gameObject.GetComponent<CircleCollider2D>();
			SpriteRenderer component6 = collision.gameObject.GetComponent<SpriteRenderer>();
			component4.velocity = Vector2.zero;
			component4.constraints = RigidbodyConstraints2D.FreezeAll;
			collision.gameObject.transform.position = center;
			component6.color = delected;
			component5.isTrigger = true;
			component5.enabled = false;
		}
		else
		{
			collision.gameObject.GetComponent<SpriteRenderer>().color = delected;
			Vector2 v2 = collision.gameObject.transform.localScale;
			v2.x -= 0.05f;
			v2.y -= 0.05f;
			collision.gameObject.transform.localScale = v2;
			Rigidbody2D component7 = collision.gameObject.GetComponent<Rigidbody2D>();
			component7.velocity = center - collision.gameObject.transform.position;
			component7.velocity = Vector2.zero;
			component7.constraints = RigidbodyConstraints2D.FreezePosition;
			collision.gameObject.transform.position = center;
			component.isTrigger = true;
			component.enabled = false;
		}
		if (collision.tag == "Puck")
		{
			if (CarromGameManager.Instance != null)
			{
				if (CarromGameManager.Instance.isPlayerTurn)
				{
					CarromGameManager.Instance.GoaledColor(new GoaledPuck(collision.gameObject, component2.suit));
				}
			}
			else if (OfflineGameManager.Instance != null)
			{
				OfflineGameManager.Instance.GoaledColor(new GoaledPuck(collision.gameObject, component2.suit));
			}
			else
			{
				MasterStrikeManager.Instance.GoaledPuck(collision.gameObject);
			}
		}
		else
		{
			if (!(collision.tag == "Player"))
			{
				return;
			}
			if (CarromGameManager.Instance != null)
			{
				PhotonView component8 = collision.gameObject.GetComponent<PhotonView>();
				component8.RPC("ChangeStrikerColor", RpcTarget.Others, 0);
				if (CarromGameManager.Instance.isPlayerTurn)
				{
					CarromGameManager.Instance.GoaledColor(new GoaledPuck(collision.gameObject, PuckColor.Color.STRIKER_COLOR));
				}
			}
			else if (OfflineGameManager.Instance != null)
			{
				OfflineGameManager.Instance.GoaledColor(new GoaledPuck(collision.gameObject, PuckColor.Color.STRIKER_COLOR));
			}
		}
	}
}
