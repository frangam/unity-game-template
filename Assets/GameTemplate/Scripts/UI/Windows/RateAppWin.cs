/***************************************************************************
Project:     Trivial Cofrade
Copyright (c) Altasy
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class RateAppWin : UIBaseWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void rate(){
		RateApp.Instance.rateUsNow ();
		UIController.Instance.Manager.close (this);
	}
	public void refuse(){
		RateApp.Instance.refuseRate ();
		UIController.Instance.Manager.close (this);
	}
}
