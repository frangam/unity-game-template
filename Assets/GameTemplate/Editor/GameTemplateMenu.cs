/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections;

public class GameTemplateMenu : EditorWindow {
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	#if UNITY_EDITOR
	
	[MenuItem("Game Template/Game Settings", false, 0)]
	public static void Edit() {
		Selection.activeObject = GameSettings.Instance;
	}
	[MenuItem("Game Template/Ads Settings/AdMob", false, 1)]
	public static void SettingsAdmob() {
		Selection.activeObject = GoogleAdmobSettings.Instance;
	}
	[MenuItem("Game Template/Ads Settings/AdColony", false, 1)]
	public static void SettingsAdColony() {
		Selection.activeObject = AdColonySettings.Instance;
	}
	[MenuItem("Game Template/Level Settings/Level Packs Settings", false, 1)]
	public static void EditLevelPacks() {
		Selection.activeObject = LevelPacks.Instance;
	}
	
	//--------------------------------------
	//  Documentation
	//--------------------------------------	
	[MenuItem("Game Template/Documentation/All Docs", false)]
	public static void AllDocs() {
		string url = "https://drive.google.com/drive/u/0/folders/0B9SJ925Use_uSjlXZFU2d2ZfODQ";
		Application.OpenURL(url);
	}
	
	//--------------------------------------
	//  Achievements Documentation
	//--------------------------------------
	
	[MenuItem("Game Template/Documentation/Achievements/Getting Started")]
	public static void AchievementSystemGettingStartedDoc() {
		string url = "https://docs.google.com/document/d/17kOcOnMRrLfwEKrG9xs-shzGYEzBCSOTwJe8akRC0vo/#heading=h.ujroj168a0eg";
		Application.OpenURL(url);
	}
	
	[MenuItem("Game Template/Documentation/Achievements/Workflow")]
	public static void AchievementSystemWorkflowDoc() {
		string url = "https://docs.google.com/document/d/17kOcOnMRrLfwEKrG9xs-shzGYEzBCSOTwJe8akRC0vo/#heading=h.4rrfds5d75hd";
		Application.OpenURL(url);
	}
	
	[MenuItem("Game Template/Documentation/Achievements/Setup")]
	public static void AchievementSystemSetupDoc() {
		string url = "https://docs.google.com/document/d/17kOcOnMRrLfwEKrG9xs-shzGYEzBCSOTwJe8akRC0vo/edit#heading=h.wynqvbtoesnn";
		Application.OpenURL(url);
	}
	
	
	
	#endif
	
}
