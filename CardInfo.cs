using UnityEngine;

public class CardInfo
{
	public GameObject card;

	public int playerId;

	public CardInfo(GameObject card, int playerId)
	{
		this.card = card;
		this.playerId = playerId;
	}
}
