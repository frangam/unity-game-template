/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using UnionAssets.FLE;

public class UIInAppManager : UIBaseManager {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private UIBaseWindow purchaseCompletedWin;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		CoreIAPManager.dispatcher.addEventListener(CoreIAPManager.PURCHASE_COMPLETED, OnPurchaseCompleted);
	}
	public override void OnDestroy ()
	{
		base.OnDestroy ();
		CoreIAPManager.dispatcher.removeEventListener(CoreIAPManager.PURCHASE_COMPLETED, OnPurchaseCompleted);
	}
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	public void OnPurchaseCompleted(CEvent e){
		if(purchaseCompletedWin){
			open (purchaseCompletedWin);
		}
	}
}
