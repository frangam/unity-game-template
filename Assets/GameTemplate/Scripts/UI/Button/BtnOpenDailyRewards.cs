﻿using UnityEngine;
using System.Collections;

public class BtnOpenDailyRewards : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private DailyRewardsInterface drWin;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override bool canPress ()
	{
		bool can = drWin && InternetChecker.Instance.IsconnectedToInternet && DailyRewards.Instance.IsInitialized && base.canPress ();
		
		GTDebug.log("Can press ? " + can);
		
		return can;
	}
	
	protected override void doPress ()
	{
		base.doPress ();
		UIController.Instance.Manager.open (drWin);
	}
}