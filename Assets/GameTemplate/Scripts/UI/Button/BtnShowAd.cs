using UnityEngine;
using System.Collections;

public class BtnShowAd : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private AdType adType = AdType.INTERSTITIAL; //the ad type to show

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		base.doPress ();

		switch(adType){
		case AdType.INTERSTITIAL: AdsHandler.Instance.mostrarPantallazo(); break;
		}
	}

	protected override bool canPress ()
	{
		return !GameSettings.Instance.IS_PRO_VERSION && base.canPress ();
	}
}