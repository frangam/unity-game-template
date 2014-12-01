////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using UnionAssets.FLE;
using System.Collections.Generic;

public class WPN_PopUpExamples : WPNFeaturePreview {

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	void OnGUI() {

		UpdateToStartPos();
		
		GUI.Label(new Rect(StartX, StartY, Screen.width, 40), "Native Pop Ups", style);
		StartY+= YLableStep;
		
		
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Rate PopUp with events")) {
			WP8RateUsPopUp rate = WP8RateUsPopUp.Create("Like this game?", "Please rate to support future updates!");
			rate.addEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
		}


		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Dialog PopUp")) {
			WP8Dialog dialog = WP8Dialog.Create("Dialog Titile", "Dialog message");
			dialog.addEventListener(BaseEvent.COMPLETE, onDialogClose);
		}
		
		
		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Message PopUp")) {
			WP8Message msg = WP8Message.Create("Message Titile", "Message message");
			msg.addEventListener(BaseEvent.COMPLETE, onMessageClose);
		}

		StartX += XButtonStep;
		if(GUI.Button(new Rect(StartX, StartY, buttonWidth, buttonHeight), "Show Preloader")) {
			WP8NativeUtils.ShowPreloader();
			Invoke("HidePreloader", 2f);
		}


	}
	

	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void HidePreloader() {
		WP8NativeUtils.HidePreloader();
	}

		
	private void onRatePopUpClose(CEvent e) {
		(e.dispatcher as WP8RateUsPopUp).removeEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
		string result = e.data.ToString();
		
		WP8PopUps.PopUp.ShowMessageWindow("Result", result + " button pressed");
	}
	
	private void onDialogClose(CEvent e) {

		//removing listner
		(e.dispatcher as WP8Dialog).removeEventListener(BaseEvent.COMPLETE, onDialogClose);

		string result = e.data.ToString();
		WP8PopUps.PopUp.ShowMessageWindow("Result", result + " button pressed");
	}
	
	private void onMessageClose(CEvent e) {
		(e.dispatcher as WP8Message).removeEventListener(BaseEvent.COMPLETE,  onMessageClose);
		WP8PopUps.PopUp.ShowMessageWindow("Result", "Message Closed");
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
