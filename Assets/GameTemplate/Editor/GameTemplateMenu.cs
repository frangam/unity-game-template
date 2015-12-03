/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GameTemplateMenu : EditorWindow {
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	#if UNITY_EDITOR
	
	[MenuItem("GameTemplate/► Play-Stop", false, -10000000)]
	public static void PlayFromCurrentScene()
	{
		if ( EditorApplication.isPlaying == true ){
			EditorApplication.isPlaying = false;
			return;
		}
		
		EditorApplication.SaveCurrentSceneIfUserWantsTo();
		
		List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
		foreach(string scene in GTBuildSettingsConfig.Instance.CurrentBuildPack.build.ScenesNames)
			scenes.Add(new EditorBuildSettingsScene(scene, true));
		
		EditorBuildSettings.scenes = scenes.ToArray();
		//		EditorApplication.OpenScene(scenes[0].path);
		
		EditorApplication.isPlaying = true;
	}
	
	[MenuItem("GameTemplate/► Play-Stop from Scene 0", false, -10000000)]
	public static void PlayFromPrelaunchScene()
	{
		if ( EditorApplication.isPlaying == true ){
			EditorApplication.isPlaying = false;
			return;
		}
		
		EditorApplication.SaveCurrentSceneIfUserWantsTo();
		
		List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
		foreach(string scene in GTBuildSettingsConfig.Instance.CurrentBuildPack.build.ScenesNames)
			scenes.Add(new EditorBuildSettingsScene(scene, true));
		
		EditorBuildSettings.scenes = scenes.ToArray();
		EditorApplication.OpenScene(scenes[0].path);
		
		EditorApplication.isPlaying = true;
	}
	
	
	[MenuItem("GameTemplate/Game Settings", false, -40)]
	public static void GameTemplateGameSettings() {
		Selection.activeObject = GameSettings.Instance;
	}
	
	[MenuItem("GameTemplate/Level Settings/Level Packs Settings", false, -30)]
	public static void GameTemplateLevelSettingsLevelPacksSettings() {
		Selection.activeObject = LevelPacks.Instance;
	}
	
	//--------------------------------------
	//  Documentation
	//--------------------------------------
	
	[MenuItem("GameTemplate/Documentation/Getting Started", false, -10)]
	public static void GettingStarted() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.boccrtu9rp44";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/All Docs Folder", false, 100)]
	public static void GameTemplateDocumentationAllDocsFolder() {
		string url = "https://drive.google.com/drive/u/0/folders/0B9SJ925Use_uSjlXZFU2d2ZfODQ";
		Application.OpenURL(url);
	}
	
	//--------------------------------------
	//  Game Settings
	//--------------------------------------	
	[MenuItem("GameTemplate/Documentation/Game Settings/Game Info")]
	public static void GameSettingsGameInfo() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.rzctmeoh3bqf";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Game Settings/App Links")]
	public static void GameSettingsAppLinks() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.lucy8bjlbfte";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Game Settings/Money Setup")]
	public static void GameSettingsMoneySetup() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.raol53p9wujm";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Game Settings/Character Control")]
	public static void GameSettingsCharacterControl() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.byo606orm9ke";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Game Settings/Ads Showing Settings")]
	public static void GameSettingsAdsShowingSettings() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.iscok2yxg05y";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Game Settings/Social Network Setup")]
	public static void GameSettingsSocialNetworkSetup() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.we2be264z1jg";
		Application.OpenURL(url);
	}
	
	
	//--------------------------------------
	//  Achievements Documentation
	//--------------------------------------
	[MenuItem("GameTemplate/Documentation/Achievements/Getting Started")]
	public static void AchievementsGettingStarted() {
		string url = "https://docs.google.com/document/d/17kOcOnMRrLfwEKrG9xs-shzGYEzBCSOTwJe8akRC0vo/edit#heading=h.ujroj168a0eg";
		Application.OpenURL(url);
	}
	
	[MenuItem("GameTemplate/Documentation/Achievements/Workflow")]
	public static void AchievementsWorkflow() {
		string url = "https://docs.google.com/document/d/17kOcOnMRrLfwEKrG9xs-shzGYEzBCSOTwJe8akRC0vo/edit#heading=h.4rrfds5d75hd";
		Application.OpenURL(url);
	}
	
	[MenuItem("GameTemplate/Documentation/Achievements/Use")]
	public static void AchievementsSetup() {
		string url = "https://docs.google.com/document/d/17kOcOnMRrLfwEKrG9xs-shzGYEzBCSOTwJe8akRC0vo/edit#heading=h.wynqvbtoesnn";
		Application.OpenURL(url);
	}
	
	
	
	#endif
	
}
