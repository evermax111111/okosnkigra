using UnityEngine;
using UnityEngine.UI;

public class GamePlayerInfo
{
	public Image playerLoader;

	public Image playerPic;

	public Vector3 playerLocation;

	public Transform playerCardLocation;

	public GameObject disconnectUI;

	public GameObject chatMessageUI;

	public GameObject medalUI;

	public GameObject glow;

	public string playerName;

	public int rank;

	public GamePlayerInfo(string name, Image loader, Image pic, GameObject chat, GameObject medal, GameObject glow)
	{
		playerLoader = loader;
		playerName = name;
		chatMessageUI = chat;
		medalUI = medal;
		playerPic = pic;
		this.glow = glow;
	}

	public GamePlayerInfo(string name, Image loader, Image pic, Transform playerlocation, Transform cardLocation, GameObject disconnect, GameObject chat, GameObject medal)
	{
		playerLoader = loader;
		playerLocation = Camera.main.ScreenToWorldPoint(playerlocation.position);
		playerCardLocation = cardLocation;
		playerName = name;
		disconnectUI = disconnect;
		chatMessageUI = chat;
		medalUI = medal;
		playerPic = pic;
	}
}
