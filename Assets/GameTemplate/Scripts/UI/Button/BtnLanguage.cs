/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BtnLanguage : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	[Tooltip("Must be follows the same order than SupportedLanguages List from GameSettings")]
	private Sprite[] supportedLangsSprites;

//	[SerializeField]
//	private Text lbLanguage;

	[SerializeField]
	private Image imgLanguage;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int currentLanguageIndex;
	private SystemLanguage currentLanguage;

	//--------------------------------------
	// GETTERS && SETTERS
	//--------------------------------------


	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){ 
		loadCurrentLanguage();
	}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void loadCurrentLanguage(){
		currentLanguage = Languages.getCurrentLanguage();

		//get the index from GameSettings localizations list
		for(int i=0; i<GameSettings.Instance.localizations.Count; i++){
			if(currentLanguage == GameSettings.Instance.localizations[i]){
				currentLanguageIndex = i;
				break;
			}
		}

		loadButton();
	}

	private void loadButton(){
		if(imgLanguage != null && supportedLangsSprites != null && supportedLangsSprites.Length > currentLanguageIndex){
			imgLanguage.sprite = supportedLangsSprites[currentLanguageIndex];
		}
		else
			GTDebug.logErrorAlways("Not found Sprite for language " + GameSettings.Instance.localizations[currentLanguageIndex] +" with index " +currentLanguageIndex);

	}

	private void saveLanguageSelected(){
		//save the selected language
		Languages.selectLanguage(GameSettings.Instance.localizations[currentLanguageIndex], true);
		
		//load correct button
		loadButton();
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		base.doPress ();
		switchNextLanguage();
	}

	public void switchNextLanguage(){
		if(currentLanguageIndex+1 >= GameSettings.Instance.localizations.Count)
			currentLanguageIndex = 0;
		else
			currentLanguageIndex++;

		saveLanguageSelected();
	}

	public void switchPreviousLanguage(){
		if(currentLanguageIndex-1 <= 0)
			currentLanguageIndex = GameSettings.Instance.localizations.Count-1;
		else
			currentLanguageIndex--;
		
		saveLanguageSelected();
	}
}
