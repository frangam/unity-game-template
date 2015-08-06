using UnityEngine;
using System.Collections;

public class BtnShowAd : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private AdType adType = AdType.INTERSTITIAL; //the ad type to show
	
	[SerializeField]
	private int adZoneIndex = 0;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		base.doPress ();
		
		switch(adType){
		case AdType.INTERSTITIAL: AdsHandler.Instance.showInterstitial(); break;
		case AdType.VIDEO: AdsHandler.Instance.PlayAVideo(adZoneIndex); break;
		case AdType.RANDOM_INTERSTITIAL_VIDEO: AdsHandler.Instance.showRandomGameplayInterstitialOrVideoAd(adZoneIndex); break;
		}
	}
	
	protected override bool canPress ()
	{
		return !GameSettings.Instance.IS_PRO_VERSION && base.canPress ();
	}
}