/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class UITestAddMoneyButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	[Tooltip("True if add gems. False to add money")]
	private bool addGems = false;
	
	[SerializeField]
	private int quantity;
	
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		base.doPress ();
		
		if(!addGems){
			if(quantity > 0)
				GameMoneyManager.Instance.addMoney(quantity);
			else
				GameMoneyManager.Instance.resetMoney();
		}
		else{
			if(quantity > 0)
				GameMoneyManager.Instance.addGems(quantity);
			else
				GameMoneyManager.Instance.resetGems();
		}
	}
}
