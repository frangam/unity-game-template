using UnityEngine;
using System.Collections;

public class InputBackButton : MonoBehaviour {
	public enum Action{
		QUIT
		,MAIN_MENU_WITHOUT_CONFIRMATION
		,POPUP_PAUSE
		,RESUME_GAME
		,GAME_SCREEN
		,WORLD_SELECTION_SCREEN
		,ELEGIR_JUGADOR
		,MAIN_MENU_WITH_CONFIRMATION
	}
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Action action;
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void LateUpdate () {
		
		#if UNITY_ANDROID || UNITY_WP8 || UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Escape)) {
			
			switch(action){
			case Action.QUIT:
				Application.Quit(); 
				break;
				
			case Action.GAME_SCREEN:
				StartCoroutine( ScreenLoaderVisualIndicator.Instance.Load (GameSettings.SCENE_GAME));
				break;
				
				
			case Action.POPUP_PAUSE:
				if(BaseGameController.Instance.Paused && !BaseGameController.Instance.Finished){
					BaseGameController.Instance.Paused = false;
				}
				else if(!BaseGameController.Instance.Paused && !BaseGameController.Instance.Finished){
					BaseGameController.Instance.Paused = true;
				}
				else if(BaseGameController.Instance.Finished){
					StartCoroutine( ScreenLoaderVisualIndicator.Instance.Load (GameSettings.SCENE_MAINMENU));
				}
				
				break;
				
				
			case Action.MAIN_MENU_WITHOUT_CONFIRMATION:
				StartCoroutine( ScreenLoaderVisualIndicator.Instance.Load (GameSettings.SCENE_MAINMENU));
				break;
				
//			case Action.MAIN_MENU_WITH_CONFIRMATION:
//				if(BaseGameController.gameStart && BaseGameController.Instance.InPause && !BaseGameController.Instance.Finished){
//					UIHandler.Instance.abrir(GameScreen.EXIT, false);
//				}
//				else if(BaseGameController.gameStart && !BaseGameController.Instance.InPause && !BaseGameController.Instance.Finished){
//					UIHandler.Instance.abrir(GameScreen.EXIT);
//				}
//				else if(BaseGameController.Instance.Finished){
//					((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
//					StartCoroutine( ScreenLoaderIndicator.Instance.Load (GameSettings.SCENE_LEVEL_SELECTION));
//				}
//				break;
			}				
		}
		#endif
	}
	#endregion
}
