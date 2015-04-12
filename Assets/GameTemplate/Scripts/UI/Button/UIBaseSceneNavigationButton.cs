using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBaseSceneNavigationButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
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
	public string SceneToGo {
		get {
			return this.sceneToGo;
		}
		set {
			sceneToGo = value;
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
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
			switch(section){
			case GameSection.MAIN_MENU: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_MAINMENU); break;
			case GameSection.GAME: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_GAME); break;
			case GameSection.CREDITS: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_CREDITS); break;
			case GameSection.LEVEL_SELECTION: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_LEVEL_SELECTION); break;
			case GameSection.SETTINGS: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_SETTINGS); break;
			case GameSection.CHARACTER_SELECTION: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_CHARACTER_SELECTION); break;
			case GameSection.ENVIRONMENT_SELECTION: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_ENVIRONMENT_SELECTION); break;
			case GameSection.ITEMS_SELECTION: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_ITEMS_SELECTION); break;
			case GameSection.TUTORIAL: ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_TUTORIAL); break;
			}
		}
		else{
			ScreenLoaderVisualIndicator.Instance.LoadScene(sceneToGo);
		}
	}
	
	
	
	private void goTo(){
		
	}
}
