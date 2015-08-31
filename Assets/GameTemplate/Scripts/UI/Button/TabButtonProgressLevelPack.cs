/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class TabButtonProgressLevelPack : UIBaseTabButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string levelPackID = "1";

	[SerializeField]
	private bool noInteractIfProgressZero = true;

	[SerializeField]
	private SliderProgressLevelPack slider;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private BaseLevelPack levelPack;


	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public BaseLevelPack LevelPack {
		get {
			return this.levelPack;
		}
	}

	public float Progress {
		get {
			float res = 0;

			if(slider)
				res = slider.Progress;

			return res;
		}
	}


	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake();
		loadLevelPack();

		if(levelPack != null)
			loadProgress();
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void loadLevelPack(){
		levelPack = LevelPacks.Instance.getPackById(levelPackID);
	}

	private void loadProgress(){		
		button.interactable = !(noInteractIfProgressZero && levelPack.ProgressCompleted <= 0);

		if(slider)
			slider.loadProgress(levelPack);
	}
}
