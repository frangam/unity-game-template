/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class TestAdWindow : UIBaseWindow {
	public AdType adType;

	public override void open ()
	{
		base.open ();

		switch(adType){
		case AdType.INTERSTITIAL: AdsHandler.Instance.testOnInterstitialOpen(); break;
		case AdType.VIDEO: AdsHandler.Instance.testOnVideoStarted(); break;
		}
	}

	public override void close ()
	{
		base.close ();

		switch(adType){
		case AdType.INTERSTITIAL: AdsHandler.Instance.testOnInterstitialClose(); break;
		case AdType.VIDEO: AdsHandler.Instance.testOnVideoStarted(); break;
		}
	}
}
