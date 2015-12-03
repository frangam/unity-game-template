/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Languages : PersistentSingleton<Languages>{
	public Font chineseFont;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int localizedLabelsInCurrentScene = 0;
	private int labelsTotalInCurrentScene = 0;
	private bool changingLanguage = false;
	private SystemLanguage currentLanguage = SystemLanguage.Unknown;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public SystemLanguage CurrentLanguage {
		get {
			SystemLanguage c = this.currentLanguage;
			SystemLanguage res = c;
			
			if(res == SystemLanguage.Unknown)
				res = getCurrentLanguage();
			
			return res;
		}
	}
	public bool IsCurrentLangLatin{
		get{
			SystemLanguage cur = CurrentLanguage;
			return cur != SystemLanguage.Chinese && cur != SystemLanguage.ChineseSimplified && cur != SystemLanguage.ChineseTraditional
				&& cur != SystemLanguage.Japanese && cur != SystemLanguage.Korean && cur != SystemLanguage.Turkish && cur != SystemLanguage.Russian;
		}
	}
	
	public static event Action OnChangingLanguage = delegate {};
	public static event Action OnAllLabelsLocalized = delegate {};
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	void OnLevelWasLoaded(int level) {
		GTDebug.log("Scene loaded");
		localizedLabelsInCurrentScene = 0;
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void loadAllLanguagesInGameSettings(){
		if(GameSettings.Instance.AllLanguages == null || (GameSettings.Instance.AllLanguages != null && GameSettings.Instance.AllLanguages.Count == 0)){
			GameSettings.Instance.AllLanguages = new List<SystemLanguage>(){
				SystemLanguage.Afrikaans, SystemLanguage.Arabic, SystemLanguage.Basque, SystemLanguage.Belarusian, SystemLanguage.Bulgarian
				, SystemLanguage.Catalan, SystemLanguage.Chinese, SystemLanguage.ChineseSimplified, SystemLanguage.ChineseTraditional
				, SystemLanguage.Czech, SystemLanguage.Danish, SystemLanguage.Dutch, SystemLanguage.English, SystemLanguage.Estonian
				, SystemLanguage.Faroese, SystemLanguage.Finnish, SystemLanguage.French, SystemLanguage.German, SystemLanguage.Greek
				, SystemLanguage.Hebrew, SystemLanguage.Hungarian, SystemLanguage.Icelandic, SystemLanguage.Indonesian, SystemLanguage.Italian
				, SystemLanguage.Japanese, SystemLanguage.Korean, SystemLanguage.Latvian, SystemLanguage.Lithuanian, SystemLanguage.Norwegian
				, SystemLanguage.Polish, SystemLanguage.Portuguese, SystemLanguage.Romanian, SystemLanguage.Russian, SystemLanguage.SerboCroatian
				, SystemLanguage.Slovak, SystemLanguage.Slovenian, SystemLanguage.Spanish, SystemLanguage.Swedish, SystemLanguage.Thai
				, SystemLanguage.Turkish, SystemLanguage.Ukrainian, SystemLanguage.Unknown, SystemLanguage.Vietnamese
			};
		}
	}
	
	public void selectLanguageOfDeviceIfSupported(){
		SystemLanguage deviceLanguage = Application.systemLanguage;
		SystemLanguage selected = deviceLanguage;
		
		if(GameSettings.Instance.localizations.Contains(deviceLanguage))
			selected = deviceLanguage;
		else if(deviceLanguage == SystemLanguage.Chinese && GameSettings.Instance.localizations.Contains(SystemLanguage.ChineseSimplified))
			selected = SystemLanguage.ChineseSimplified;
		//		else if(deviceLanguage == SystemLanguage.Chinese && GameSettings.Instance.localizations.Contains(SystemLanguage.ChineseTraditional))
		else
			selected = SystemLanguage.English;
		
		selectLanguage(selected);
	}
	
	public void selectCurrentLanguage(){
		selectLanguage(Localization.language);
	}
	
	public void selectLanguage(string language, bool setFlagLanguageChanged = false){
		currentLanguage = getSysLanguageFromString(language);
		changingLanguage = true;
		OnChangingLanguage(); //dispatch event
		
		Localization.language = language;
		
		if(setFlagLanguageChanged)
			PlayerPrefs.SetInt(GameSettings.PP_LANGUAGE_CHANGED, 1);
		
		Canvas.ForceUpdateCanvases();
	}
	public void selectLanguage(SystemLanguage language, bool setFlagLanguageChanged = false){
		currentLanguage = language;
		changingLanguage = true;
		OnChangingLanguage(); //dispatch event
		
		Localization.language = language.ToString();
		
		if(setFlagLanguageChanged)
			PlayerPrefs.SetInt(GameSettings.PP_LANGUAGE_CHANGED, 1);
	}
	
	public void setLabelsTotal(int total){
		labelsTotalInCurrentScene = total;
	}
	
	public void labelLocalized(){
		if(changingLanguage){
			localizedLabelsInCurrentScene++;
			
			if(localizedLabelsInCurrentScene >= labelsTotalInCurrentScene){
				OnAllLabelsLocalized();
				localizedLabelsInCurrentScene = 0;
				changingLanguage = false;
			}
		}
	}
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private SystemLanguage getCurrentLanguage(){
		SystemLanguage res = SystemLanguage.English;
		string current = Localization.language;
		
		if(GameSettings.Instance.AllLanguages != null && GameSettings.Instance.AllLanguages.Count > 0){
			foreach(SystemLanguage sl in GameSettings.Instance.AllLanguages){
				if(sl.ToString().Equals(current)){
					res = sl;
					break;
				}
			}
		}
		
		return res;
	}
	private SystemLanguage getSysLanguageFromString(string language){
		SystemLanguage res = SystemLanguage.English;
		List<SystemLanguage> all = GameSettings.Instance.AllLanguages;
		
		if(GameSettings.Instance.AllLanguages != null && all.Count > 0){
			foreach(SystemLanguage sl in all){
				if(sl.ToString().Equals(language)){
					res = sl;
					break;
				}
			}
		}
		
		return res;
	}
}
