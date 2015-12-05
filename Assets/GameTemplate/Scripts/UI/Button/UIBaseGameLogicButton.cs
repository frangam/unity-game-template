/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class UIBaseGameLogicButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private GameLogicAction action;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		base.doPress ();
		
		switch(action){
		case GameLogicAction.PAUSE:
			GameController.Instance.Manager.Paused = true;
			GTAnalyticsHandler.Instance.logEvent(GAEventCategories.GAME, GAEventActions.PAUSED);
			break;
			
		case GameLogicAction.RESUME:
			GameController.Instance.Manager.Paused = false;
			GTAnalyticsHandler.Instance.logEvent(GAEventCategories.GAME, GAEventActions.RESUMED);
			break;
			
		case GameLogicAction.RETRY:
			GameController.Instance.Manager.RestartGameButtonPressed();
			GTAnalyticsHandler.Instance.logEvent(GAEventCategories.GAME, GAEventActions.RETRIED);
			break;
			
		case GameLogicAction.EXIT_GAME:
			ScreenLoaderVisualIndicator.Instance.LoadScene(GameSection.MAIN_MENU);
			GTAnalyticsHandler.Instance.logEvent(GAEventCategories.GAME, GAEventActions.EXITED);
			break;
		}
	}
}
