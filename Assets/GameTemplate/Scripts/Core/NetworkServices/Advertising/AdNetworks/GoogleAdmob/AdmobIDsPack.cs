using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AdmobIDsPack{
	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public int 					gameVersion;

	public string				android_BannerID;
	public string				android_InterstitialID;

	public string				iOS_BannerID;
	public string				iOS_InterstitialID;
	
	public string				wp_BannerID;
	public string				wp_InterstitialID;

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public AdmobIDsPack(int pGameVersion, string piOS_BannerID, string piOS_InterstitialID
	                    , string pandroid_BannerID, string pandroid_InterstitialID, string pwp_BannerID, string pwp_InterstitialID){
		gameVersion = pGameVersion;
		
		//ios
		iOS_BannerID = piOS_BannerID;
		iOS_InterstitialID = piOS_InterstitialID;

		//android
		android_BannerID = pandroid_BannerID;
		android_InterstitialID = pandroid_InterstitialID;

		//windows phone
		wp_BannerID = pwp_BannerID;
		wp_InterstitialID = pwp_InterstitialID;
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[AdmobIDsPack: gameVersion={0}, iOS_BannerID={1}, iOS_InterstitialID={2}, android_BannerID={3}, android_InterstitialID={4}, wp_BannerID={5}, wp_InterstitialID={6}]", gameVersion, iOS_BannerID, iOS_InterstitialID, android_BannerID, android_InterstitialID, wp_BannerID, wp_InterstitialID);
	}
	
}
