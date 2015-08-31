/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class UIBaseConfirmationPurchaseButton : UICloseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private UIBaseConfimationPurchaseWindow window;


	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		
		getWindow();
	}
	protected override void doPress ()
	{
		base.doPress ();

		if(window != null)
			window.confirmPurchase();
		else
			Debug.LogError("Not found UIBaseConfimationPurchaseWindow");
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void getWindow(){
		if(window == null)
			window = GetComponentInParent<UIBaseConfimationPurchaseWindow>();
		
		if(window == null)
			Debug.LogError("Not found UIBaseConfimationPurchaseWindow");
	}
}
