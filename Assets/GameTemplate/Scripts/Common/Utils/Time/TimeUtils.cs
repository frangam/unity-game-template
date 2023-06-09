﻿/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
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

	public static IEnumerator getDateTimeFromServer(string serverURL)
	{
		GTDebug.log ("TimeUtils - Getting time from server "+serverURL);
		WWW www = new WWW(serverURL);
		yield return www;

		string response = www.text;

		GTDebug.log ("TimeUtils - Getting time from server Response: "+response);

		if (string.IsNullOrEmpty(www.error)){
			yield return response;
		} else {
			yield return "fail";
		}
	}
}
