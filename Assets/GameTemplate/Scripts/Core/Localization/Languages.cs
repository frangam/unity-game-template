/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Languages {
	public static event Action OnChangingLanguage = delegate {};
	public static event Action OnLanguageSelected = delegate {};
	
	private const string PP_CURRENT_LANGUAGE = "pp_current_language";
	
	public static void loadAllLanguagesInGameSettings(){
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
	
	public static void selectLanguageOfDeviceIfSupported(){
		SystemLanguage deviceLanguage = Application.systemLanguage;
		SystemLanguage selected = deviceLanguage;
		
		if(GameSettings.Instance.localizations.Contains(deviceLanguage))
			selected = deviceLanguage;
		else
			selected = SystemLanguage.English;
		
		selectLanguage(selected);
	}
	
	public static void selectLanguage(SystemLanguage language, bool setFlagLanguageChanged = false){
		OnChangingLanguage(); //dispatch event
		Localization.language = language.ToString();
		OnLanguageSelected(); //dispatch event
		
		if(setFlagLanguageChanged)
			PlayerPrefs.SetInt(GameSettings.PP_LANGUAGE_CHANGED, 1);
	}
	
	public static SystemLanguage getCurrentLanguage(){
		SystemLanguage res = SystemLanguage.English;
		
		if(GameSettings.Instance.AllLanguages != null && GameSettings.Instance.AllLanguages.Count > 0){
			foreach(SystemLanguage sl in GameSettings.Instance.AllLanguages){
				if(sl.ToString().Equals(Localization.language)){
					res = sl;
					break;
				}
			}
		}
		
		return res;
	}
}
