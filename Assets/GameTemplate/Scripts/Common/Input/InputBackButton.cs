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
				ScreenLoaderVisualIndicator.Instance.LoadScene (GameSettings.SCENE_GAME);
				break;
				
				
			case Action.POPUP_PAUSE:
				if(GameController.Instance.Manager.Paused && !GameController.Instance.Manager.Finished){
					GameController.Instance.Manager.Paused = false;
				}
				else if(!GameController.Instance.Manager.Paused && !GameController.Instance.Manager.Finished){
					GameController.Instance.Manager.Paused = true;
				}
				else if(GameController.Instance.Manager.Finished){
					ScreenLoaderVisualIndicator.Instance.LoadScene (GameSettings.SCENE_MAINMENU);
				}
				
				break;
				
				
			case Action.MAIN_MENU_WITHOUT_CONFIRMATION:
				ScreenLoaderVisualIndicator.Instance.LoadScene (GameSettings.SCENE_MAINMENU);
				break;
				
//			case Action.MAIN_MENU_WITH_CONFIRMATION:
//				if(GameController.gameStart && GameController.Instance.InPause && !GameController.Instance.Finished){
//					UIHandler.Instance.abrir(GameScreen.EXIT, false);
//				}
//				else if(GameController.gameStart && !GameController.Instance.InPause && !GameController.Instance.Finished){
//					UIHandler.Instance.abrir(GameScreen.EXIT);
//				}
//				else if(GameController.Instance.Finished){
//					((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
				//			ScreenLoaderIndicator.Instance.LoadScene (GameSettings.SCENE_LEVEL_SELECTION);
//				}
//				break;
			}				
		}
		#endif
	}
	#endregion
}
