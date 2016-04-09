/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIButtonOpenWin : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private List<UIBaseWindow> openWindows;
	
	[SerializeField]
	private List<UIBaseWindow> closeWindows;

	[SerializeField]
	private float delay = 0f;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		base.doPress ();
		
		if(closeWindows != null && closeWindows.Count > 0){
			foreach(UIBaseWindow w in closeWindows)
				UIController.Instance.Manager.waitForClose(w,delay);
		}
		
		if(openWindows != null && openWindows.Count > 0){
			foreach(UIBaseWindow w in openWindows)
				UIController.Instance.Manager.waitForOpen(w,delay);
		}
	}
}
