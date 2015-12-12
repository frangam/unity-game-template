/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBaseSceneNavigationButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool showLoadingPanelImmediately = false;
	
	[SerializeField]
	[Tooltip("Game Section represented")]
	private GameSection section;
	
	[SerializeField]
	[Tooltip("Leave empty if do navigate with Section. If we go to an specific scene represented by another game section fill it")]
	private string sceneToGo = "";
	
	[SerializeField]
	private bool localMultiplayer = false;
	
	[SerializeField]
	private bool onlineMultiplayer = false;
	
	//--------------------------------------
	// GETTERS && SETTERS
	//--------------------------------------
	#region Getters/Setters
	public string SceneToGo {
		get {
			return this.sceneToGo;
		}
		set {
			sceneToGo = value;
		}
	}
	
	public bool ShowLoadingPanelImmediately {
		get {
			return this.showLoadingPanelImmediately;
		}
		set {
			showLoadingPanelImmediately = value;
		}
	}
	
	public GameSection Section {
		get {
			return this.section;
		}
		set {
			section = value;
		}
	}
	
	public bool LocalMultiplayer {
		get {
			return this.localMultiplayer;
		}
		set {
			localMultiplayer = value;
		}
	}
	
	public bool OnlineMultiplayer {
		get {
			return this.onlineMultiplayer;
		}
		set {
			onlineMultiplayer = value;
		}
	}
	#endregion
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override IEnumerator doPressBeforeAWhile ()
	{
		if(showLoadingPanelImmediately)
			UILoadingPanel.Instance.show();
		
		return base.doPressBeforeAWhile ();
	}
	
	protected override void doPress ()
	{
		base.doPress ();
		
		//select game mode
		switch(section){
		case GameSection.QUICKGAME_MODE: 
			PlayerPrefs.SetInt(GameSettings.PP_GAME_MODE, (int) GameMode.QUICKGAME);
			break;
		case GameSection.CAMPAIGN_MODE: 
			PlayerPrefs.SetInt(GameSettings.PP_GAME_MODE, (int) GameMode.CAMPAIGN);
			break;
		case GameSection.SURVIVAL_MODE: 
			PlayerPrefs.SetInt(GameSettings.PP_GAME_MODE, (int) GameMode.SURVIVAL);
			break;
		}
		
		//multiplayer options
		PlayerPrefs.SetInt(GameSettings.PP_LOCAL_MULTIPLAYER, localMultiplayer ? 1:0);
		PlayerPrefs.SetInt(GameSettings.PP_ONLINE_MULTIPLAYER, onlineMultiplayer ? 1:0);
		
		//Load scene
		if(string.IsNullOrEmpty(sceneToGo)){
			if(!ScreenLoaderVisualIndicator.Instance.LoadScene(section)){
				switch(section){
				case GameSection.MAIN_MENU: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_MAINMENU, true, !showLoadingPanelImmediately); break;
				case GameSection.GAME: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_GAME, true, !showLoadingPanelImmediately); break;
				case GameSection.CREDITS: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_CREDITS, true, !showLoadingPanelImmediately); break;
				case GameSection.LEVEL_SELECTION: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_LEVEL_SELECTION, true, !showLoadingPanelImmediately); break;
				case GameSection.SETTINGS: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_SETTINGS, true, !showLoadingPanelImmediately); break;
				case GameSection.CHARACTER_SELECTION: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_CHARACTER_SELECTION, true, !showLoadingPanelImmediately); break;
				case GameSection.ENVIRONMENT_SELECTION: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_ENVIRONMENT_SELECTION, true, !showLoadingPanelImmediately); break;
				case GameSection.ITEMS_SELECTION: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_ITEMS_SELECTION, true, !showLoadingPanelImmediately); break;
				case GameSection.TUTORIAL: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_TUTORIAL, true, !showLoadingPanelImmediately); break;
				}
			}
		}
		else{
			ScreenLoaderVisualIndicator.Instance.LoadScene(sceneToGo, true, !showLoadingPanelImmediately);
		}
	}
}
