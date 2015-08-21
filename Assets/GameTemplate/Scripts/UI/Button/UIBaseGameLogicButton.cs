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
			break;
			
		case GameLogicAction.RESUME:
			GameController.Instance.Manager.Paused = false;
			break;
			
		case GameLogicAction.RETRY:
			GameController.Instance.Manager.RestartGameButtonPressed();
			break;
			
		case GameLogicAction.EXIT_GAME:
			ScreenLoaderVisualIndicator.Instance.LoadScene(GameSection.MAIN_MENU);
			break;
		}
	}
}
