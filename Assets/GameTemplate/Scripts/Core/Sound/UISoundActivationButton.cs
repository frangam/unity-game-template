/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISoundActivationButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private SoundType type;
	
	[SerializeField]
	private Image imgActive;
	
	[SerializeField]
	private Image imgInactive;
	
	[SerializeField]
	private bool hideActive = false;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool active = true;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake(){
		checkShowActive();
	}

	public override void Update ()
	{
		base.Update ();
		checkShowActive();
	}
	
	protected override void doPress ()
	{
		base.doPress ();
		
		active = BaseSoundManager.Instance.muteOrActiveOncesMuteOncesActive(type);

		/*** ANALYTICS **/
		string gaEventAction = active ? GAEventActions.ENABLED : GAEventActions.DISABLED;
		string gaEventCat = GAEventCategories.SOUND;
		switch(type){
		case SoundType.FX:
			gaEventCat = GAEventCategories.SOUND; break;
		case SoundType.MUSIC:
			gaEventCat = GAEventCategories.MUSIC; break;
		}
		GTAnalyticsHandler.Instance.logEvent (gaEventCat, gaEventAction);
		/*** ANALYTICS **/


//		if(hideActive)
//			imgActive.gameObject.SetActive(active);
//		
//		imgInactive.gameObject.SetActive(!active);
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void checkShowActive(){
		switch(type){
		case SoundType.FX:
			active = BaseSoundManager.Instance.IsSoundActive();
			break;
			
		case SoundType.MUSIC:
			active = BaseSoundManager.Instance.IsMusicActive();
			break;
		}
		
		if(hideActive)
			imgActive.gameObject.SetActive(active);
		else{
			imgActive.gameObject.SetActive(true);
		}
		
		imgInactive.gameObject.SetActive(!active);
	}
}
