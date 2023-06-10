using UnityEngine;

public class Card : MonoBehaviour
{
	public enum Suit
	{
		SPADE,
		HEART,
		CLUB,
		DIAMOND
	}

	public enum CardValue
	{
		ACE = 14,
		KING = 13,
		QUEEN = 12,
		JACK = 11,
		TEN = 10,
		NINE = 9,
		EIGHT = 8,
		SEVEN = 7,
		SIX = 6,
		FIVE = 5,
		FOUR = 4,
		THREE = 3,
		TWO = 2
	}

	public Suit suit;

	public CardValue value;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
