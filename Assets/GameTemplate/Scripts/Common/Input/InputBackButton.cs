/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputBackButton : Singleton<InputBackButton> {
	public enum Action{
		QUIT
		,MAIN_MENU_WITHOUT_CONFIRMATION
		,POPUP_PAUSE
		,RESUME_GAME
		,GAME_SCREEN
		,WORLD_SELECTION_SCREEN
		,CHARACTER_SELECTION
		,MAIN_MENU_WITH_CONFIRMATION
		,SURVIVAL_MENU_SELECTION
		,HANDLED_BY_UICONTROLLER //SET BY THE UICONTROLLER
	}
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Action action;
	
	[SerializeField]
	[Tooltip("Leave empty if do navigate with Action. If we go to an specific scene represented by another game section fill it")]
	private string specificScreenToGO;
	
	[SerializeField]
	private UIBaseManager uiManagerToDoAnAction;
	
	[SerializeField]
	private List<UIBaseWindow> openWindows;
	
	[SerializeField]
	private List<UIBaseWindow> closeWindows;
	
	[SerializeField]
	private bool resetEasterEggs = false;
	
	//--------------------------------------
	// GETTERS && SETTERS
	//--------------------------------------
	public Action CurrentAction {
		get {
			return this.action;
		}
		set {
			action = value;
		}
	}
	
	public string SpecificScreenToGO {
		get {
			return this.specificScreenToGO;
		}
		set {
			specificScreenToGO = value;
		}
	}
	
	public UIBaseManager UiManagerToDoAnAction {
		get {
			return this.uiManagerToDoAnAction;
		}
		set {
			uiManagerToDoAnAction = value;
		}
	}
	
	public List<UIBaseWindow> OpenWindows {
		get {
			return this.openWindows;
		}
		set {
			openWindows = value;
		}
	}
	
	public List<UIBaseWindow> CloseWindows {
		get {
			return this.closeWindows;
		}
		set {
			closeWindows = value;
		}
	}
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void LateUpdate () {
		
		#if UNITY_ANDROID || UNITY_WP8 || UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Escape)) {
			//easter eggs
			if(resetEasterEggs)
				EasterEggsController.Instance.resetAllInputs();
			
			
			//logic with priority
			
			if(uiManagerToDoAnAction != null){
				uiManagerToDoAnAction.doActionWhenInputBackButtonIsPressed();
			}
			else if(openWindows != null && openWindows.Count > 0){
				//first need close
				if(closeWindows != null && closeWindows.Count > 0){
					foreach(UIBaseWindow w in closeWindows)
						UIController.Instance.Manager.close(w);
				}
				
				foreach(UIBaseWindow w in openWindows)
					UIController.Instance.Manager.open(w);
			}
			else if(closeWindows != null && closeWindows.Count > 0){
				foreach(UIBaseWindow w in closeWindows)
					UIController.Instance.Manager.close(w);
			}
			else if(!string.IsNullOrEmpty(specificScreenToGO)){
				ScreenLoaderVisualIndicator.Instance.LoadScene (specificScreenToGO);
			}
			else{
				switch(action){
				case Action.QUIT:
					Application.Quit(); 
					break;
					
				case Action.GAME_SCREEN:
					ScreenLoaderVisualIndicator.Instance.LoadScene (GameSection.GAME);
					break;
					
					
				case Action.POPUP_PAUSE:
					if(GameController.Instance.Manager.Paused && GameController.Instance.Manager.Started && !GameController.Instance.Manager.Finished){
						GameController.Instance.Manager.Paused = false;
					}
					else if(!GameController.Instance.Manager.Paused && GameController.Instance.Manager.Started && !GameController.Instance.Manager.Finished){
						GameController.Instance.Manager.Paused = true;
					}
					else if(GameController.Instance.Manager.Finished){
						ScreenLoaderVisualIndicator.Instance.LoadScene (GameSection.MAIN_MENU);
					}
					
					break;
					
					
				case Action.MAIN_MENU_WITHOUT_CONFIRMATION:
					ScreenLoaderVisualIndicator.Instance.LoadScene (GameSection.MAIN_MENU);
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
		}
		#endif
	}
	#endregion
}
