using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBaseSceneNavigationButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private GameSection section;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void press (){
		base.press ();

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
}
