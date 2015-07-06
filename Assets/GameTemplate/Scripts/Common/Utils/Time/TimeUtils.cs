﻿using UnityEngine;
using System;
using System.Collections;


public class TimeUtils{
	/// <summary>
	/// Waits for real seconds ignores Time.scale
	/// </summary>
	/// <returns>The for real seconds.</returns>
	/// <param name="delay">Delay.</param>
	public static IEnumerator WaitForRealSeconds( float delay )
	{
		float start = Time.realtimeSinceStartup;
		while( Time.realtimeSinceStartup < start + delay)
		{
			yield return null;
		}
	}

	public static IEnumerator getDateTimeFromServer()
	{
		WWW www = new WWW("http://www.frillsgames.com/gettime.php");
		yield return www;

		string response = www.text;
		dateTime = response;

//		Debug.Log("Current Time zone: " + TimeZone.CurrentTimeZone.StandardName);

	}
}
