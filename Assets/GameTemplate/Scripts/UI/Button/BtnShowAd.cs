/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BtnShowAd : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private AdType adType = AdType.INTERSTITIAL; //the ad type to show
	
	[SerializeField]
	[Tooltip("Shows ads every number of clicks indicated for this value")]
	[Range(1,100)]
	private int numberOfClicksForShowAd = 1;
	
	
	
	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public int NumberOfClicksForShowAd
	{
		get
		{
			return numberOfClicksForShowAd;
		}
		
	}
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		if(base.ClicksCounter >= 0 && (base.ClicksCounter == 0 || (base.ClicksCounter % NumberOfClicksForShowAd == 0))){
			base.doPress ();
			
			GTDebug.log ("Showing Ad ["+adType+"]");
			
			switch(adType){
			case AdType.INTERSTITIAL: AdsHandler.Instance.showInterstitial(); break;
			case AdType.VIDEO: AdsHandler.Instance.PlayAVideo(); break;
			case AdType.RANDOM_INTERSTITIAL_VIDEO: AdsHandler.Instance.showRandomGameplayInterstitialOrVideoAd(); break;
			case AdType.VIDEO_V4VC: AdsHandler.Instance.playVideoV4VC(); break;
			}
		}
	}
	
	protected override bool canPress ()
	{
		return base.canPress () && AdsHandler.Instance.canShowAd(adType);
	}
	
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
}