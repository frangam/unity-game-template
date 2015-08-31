/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AdColonyIDsPack {
	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public int 					gameVersion;

	public string 				android_appID;
	public string 				iOS_appID;
//	public string 				wp_appID;

	public List<string>			android_adZoneIDs;
	public List<string>			iOS_adZoneIDs;
//	public List<string>			wp_adZoneIDs;

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public AdColonyIDsPack(int pGameVersion, string piOS_appID, string pandroid_appID, string pwp_appID
	                       , List<string> piOS_adZoneIDs, List<string> pandroid_adZoneIDs){

		gameVersion = pGameVersion;

		//ios
		iOS_appID = piOS_appID;
		if(piOS_adZoneIDs != null)
			iOS_adZoneIDs = piOS_adZoneIDs;
		else
			iOS_adZoneIDs = new List<string>();

		//android
		android_appID = pandroid_appID;
		if(pandroid_adZoneIDs != null)
			android_adZoneIDs = pandroid_adZoneIDs;
		else
			android_adZoneIDs = new List<string>();

//		//windows phone
//		wp_appID = pwp_appID;
//		if(pwp_adZoneIDs != null)
//			wp_adZoneIDs = pwp_adZoneIDs;
//		else
//			wp_adZoneIDs = new List<string>();
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[AdColonyIDsPack: gameVersion={0}, iOS_appID={1}, android_appID={2}, iOS_adZoneIDs={3}, android_adZoneIDs={4}]", gameVersion, iOS_appID, android_appID, iOS_adZoneIDsToString(), android_adZoneIDsToString());
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public string iOS_adZoneIDsToString(){
		string res = "(";
		
		for(int i=0; i<iOS_adZoneIDs.Count; i++){
			res += iOS_adZoneIDs[i].ToString();
			
			if(i<iOS_adZoneIDs.Count-1)
				res += ",";
		}
		res += ")";

		return res;
	}

	public string android_adZoneIDsToString(){
		string res = "(";
		
		for(int i=0; i<android_adZoneIDs.Count; i++){
			res += android_adZoneIDs[i].ToString();
			
			if(i<android_adZoneIDs.Count-1)
				res += ",";
		}
		res += ")";
		
		return res;
	}
}
