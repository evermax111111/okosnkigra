using System;
using UnityEngine;

[Serializable]
public class Sound
{
	public string name;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume;

	public bool loop;

	[Range(-3f, 3f)]
	public float pitch = 1f;

	[HideInInspector]
	public AudioSource source;
}
