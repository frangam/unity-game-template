/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

/// <summary>
/// Exit application.
/// </summary>
public class BtnExit : UIBaseButton {
	protected override void doPress ()
	{
		base.doPress ();

		#if UNITY_ANDROID && !UNITY_EDITOR
		using (AndroidJavaClass javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
			AndroidJavaObject unityActivity = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
			unityActivity.Call<bool>("moveTaskToBack", true);
		}
		#else
		Application.Quit();
		#endif
	}
}
