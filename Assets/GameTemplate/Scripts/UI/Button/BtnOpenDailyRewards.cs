/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
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

		GTDebug.log("BtnOpenDailyRewards Can press ? " + can + ". Internet ? " + InternetChecker.Instance.IsconnectedToInternet + ". DailyRewards initialized ? " + DailyRewards.Instance.IsInitialized);

		return can;
	}

	protected override void doPress ()
	{
		base.doPress ();
		UIController.Instance.Manager.open (drWin);
	}
}
