/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class BntRateUs : UIBaseButton {
	protected override void doPress ()
	{
		base.doPress ();
		RateApp.Instance.rateUsNow();
	}
}
