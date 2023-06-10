using System;
using System.Collections;
using UnityEngine;

public class ShareManager : MonoBehaviour
{
	private void Awake()
	{
		ShareManager.insatnce = this;
	}

	public void ShareRoomId(string message)
	{
		base.StartCoroutine(this.ShareAndroidText(message));
	}

	private IEnumerator ShareAndroidText(string message)
	{
		yield return new WaitForEndOfFrame();
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent", new object[0]);
		intentObject.Call<AndroidJavaObject>("setAction", new object[]
		{
			intentClass.GetStatic<string>("ACTION_SEND")
		});
		intentObject.Call<AndroidJavaObject>("setType", new object[]
		{
			"text/plain"
		});
		intentObject.Call<AndroidJavaObject>("putExtra", new object[]
		{
			intentClass.GetStatic<string>("EXTRA_SUBJECT"),
			this.subject
		});
		intentObject.Call<AndroidJavaObject>("putExtra", new object[]
		{
			intentClass.GetStatic<string>("EXTRA_TEXT"),
			message
		});
		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", new object[]
		{
			intentObject,
			"Share Via"
		});
		currentActivity.Call("startActivity", new object[]
		{
			jChooser
		});
		yield break;
	}

	public void RateUs()
	{
		Application.OpenURL("market://details?id=YOUR_ID");
	}

	public static ShareManager insatnce;

	private string subject = "Subject text";

	private string body = "Actual text (Link)";
}
