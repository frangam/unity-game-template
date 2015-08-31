/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlaySoundButton : UIBaseButton {
	public enum Trigger{
		TOUCH_UP,
		TOUCH_DOWN,
	}

	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Trigger trigger;

	[SerializeField]
	[Tooltip("Use BaseSoundIDs class or inherited to set this value")]
	private string soundId = BaseSoundIDs.UI_BUTTON_CLICK_FX;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool movingFinger;

	public void OnPress (bool pressed = false){
		//touch up
		if(trigger == Trigger.TOUCH_UP && !pressed && !movingFinger){
			BaseSoundManager.Instance.play(soundId);
		}
		//touch down
		else if(trigger == Trigger.TOUCH_DOWN && pressed && !movingFinger){
			BaseSoundManager.Instance.play(soundId);
			movingFinger = false; //finger movement end
		}
		else if(!pressed){
			movingFinger = false; //finger movement end
		}
	}

	public void OnDrag(Vector2 delta){
		movingFinger = true;
	}
}
