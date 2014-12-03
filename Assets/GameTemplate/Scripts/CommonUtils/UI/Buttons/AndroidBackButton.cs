using UnityEngine;
using System.Collections;
//using Chartboost;

public class AndroidBackButton : MonoBehaviour {	
	
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

				StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_GAME));
				break;


			case Action.POPUP_PAUSE:
				if(GameManager.Instance.InPause && !GameManager.Instance.ShowingGOPanel){
					GameManager.Instance.pause (false);
				}
				else if(!GameManager.Instance.InPause && !GameManager.Instance.ShowingGOPanel){
					GameManager.Instance.pause (true);
				}
				else if(GameManager.Instance.ShowingGOPanel){
					((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
					StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_MAINMENU));
				}
				
				break;

				
			case Action.MAIN_MENU_WITHOUT_CONFIRMATION:
					
				((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
				StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_MAINMENU));

				
				break;

			case Action.MAIN_MENU_WITH_CONFIRMATION:
				if(GameManager.gameStart && GameManager.Instance.InPause && !GameManager.Instance.ShowingGOPanel){
					UIHandler.Instance.abrir(GameScreen.EXIT, false);
				}
				else if(GameManager.gameStart && !GameManager.Instance.InPause && !GameManager.Instance.ShowingGOPanel){
					UIHandler.Instance.abrir(GameScreen.EXIT);
				}
				else if(GameManager.Instance.ShowingGOPanel){
					((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
					StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_LEVEL_SELECTION));
				}
				break;
			}				
		}
		#endif
	}
	#endregion
}
