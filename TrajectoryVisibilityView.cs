using Photon.Pun;
using UnityEngine;

public class TrajectoryVisibilityView : MonoBehaviour, IPunObservable
{
	private trajectoryScript trajectoryScript;

	private CircleCollider2D circleColllider;

	public void Start()
	{
		trajectoryScript = GetComponent<trajectoryScript>();
		circleColllider = GetComponent<CircleCollider2D>();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!stream.IsWriting)
		{
			bool flag = (bool)stream.ReceiveNext();
			if (!(circleColllider != null))
			{
			}
		}
	}

	private void Update()
	{
	}
}
