using System;
using UnityEngine;

[Serializable]
public class GamePlayer
{
	[SerializeField]
	public int ID = -1;

	public GamePlayer(int id)
	{
		ID = id;
	}
}
