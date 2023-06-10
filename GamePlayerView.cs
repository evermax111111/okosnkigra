using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GamePlayerView
{
	public GameObject view;

	public Text name;

	public Image loader;

	public Image pic;

	public Transform playerLocation;

	public Transform playeriPadLocation;

	public Transform playerCardLocation;

	public GameObject disconnectUI;

	public GameObject chatMessageUI;

	public GameObject medalUI;

	public GameObject cardHolderUI;

	public Animator profileAnim;

	public GameObject glow;
}
