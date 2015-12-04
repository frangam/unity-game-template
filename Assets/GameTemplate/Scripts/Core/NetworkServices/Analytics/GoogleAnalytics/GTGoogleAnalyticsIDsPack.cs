/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GTGoogleAnalyticsIDsPack{
	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public int 					gameVersion;

	public string				androidTrackingID;
	public string				iOSTrackingID;
	public string				otherTrackingID;

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public GTGoogleAnalyticsIDsPack(): this(0, "", "", ""){}
	public GTGoogleAnalyticsIDsPack(int pGameVersion): this(pGameVersion, "", "", ""){}
	public GTGoogleAnalyticsIDsPack(int pGameVersion, string pIOSTrackingID, string pAndroidTrackingID, string pOtherTrackingID){
		gameVersion = pGameVersion;
		
		//ios
		iOSTrackingID = pIOSTrackingID;

		//android
		androidTrackingID = pAndroidTrackingID;

		//other
		otherTrackingID = pOtherTrackingID;
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[GoogleAnalyticsIDsPack: gameVersion={0}, androidTrackingID={1}, iOSTrackingID={2}, otherTrackingID={3}]", gameVersion, androidTrackingID, iOSTrackingID, otherTrackingID);
	}
	
	
}
