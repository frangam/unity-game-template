/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class UIInAppShopWindow : UIBaseWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private UIBaseInAppWin gralInAppWin;
	
	
	public override void Awake ()
	{
		base.Awake ();
		
		if(!gralInAppWin)
			gralInAppWin = GetComponentInParent<UIBaseInAppWin>();
		
		if(gralInAppWin == null)
			Debug.LogError("UIInAppShopWindow - not found UIBaseInAppWin");
	}
	
	public override void open ()
	{
		//ANALYTICS
		if(StartClosed || (!StartClosed && !FirstOpen))
			GTAnalyticsHandler.Instance.logEvent (GAEventCategories.INAPP, GAEventActions.OPENED);
		
		base.open ();
		
		if(gralInAppWin ){
			gralInAppWin.checkMaxAmountOfMoneyRaised();
			gralInAppWin.showPanelLoading();
			StartCoroutine(gralInAppWin.waitAtStartForCloseIfNotLoaded(true));
		}
	}
	
	public override void close ()
	{
		//ANALYTICS
		if(!FirstClose)
			GTAnalyticsHandler.Instance.logEvent (GAEventCategories.INAPP, GAEventActions.OPENED);

		if(IsOpen && gralInAppWin)
			gralInAppWin.closeLoading(true);
		
		base.close ();
	}
	
	public void forceClose(){
		if(gralInAppWin )
			gralInAppWin.forceClose();
	}
	
	private void closeWinWhenErrorAtInit(){
		if(gralInAppWin )
			gralInAppWin.closeWinWhenErrorAtInit(true);
		
	}
	
	
}
