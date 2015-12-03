/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class UIBaseGameControlButtonsWindow : UIBaseWindow {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private AxisTouchButton[] axisTouchButtons;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		axisTouchButtons = GetComponentsInChildren<AxisTouchButton>();
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Releases the touch down.
	/// </summary>
	public void releaseTouchDown(){
		if(axisTouchButtons != null && axisTouchButtons.Length > 0){
			foreach(AxisTouchButton b in axisTouchButtons){
				GTDebug.log("Releasing button " +b.name);
				b.OnPointerUp(null);
			}
		}
	}
}
