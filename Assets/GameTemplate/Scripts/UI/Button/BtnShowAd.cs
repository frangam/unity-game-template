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
	
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		base.doPress ();
		
		switch(adType){
		case AdType.INTERSTITIAL: AdsHandler.Instance.showInterstitial(); break;
		case AdType.VIDEO: AdsHandler.Instance.PlayAVideo(); break;
		case AdType.RANDOM_INTERSTITIAL_VIDEO: AdsHandler.Instance.showRandomGameplayInterstitialOrVideoAd(); break;
		case AdType.VIDEO_V4VC: AdsHandler.Instance.playVideoV4VC(); break;
		}
	}
	
	protected override bool canPress ()
	{
		return AdsHandler.Instance.canShowAd(adType) && base.canPress ();
	}
	
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
}