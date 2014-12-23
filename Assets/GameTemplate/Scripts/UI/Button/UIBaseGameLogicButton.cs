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
	public override void press (){
		base.press ();

		switch(action){
		case GameLogicAction.PAUSE:
			GameController.Instance.Manager.Paused = true;
			break;

		case GameLogicAction.RESUME:
			GameController.Instance.Manager.Paused = false;
			break;

		case GameLogicAction.RETRY:
			ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_GAME);
			break;

		case GameLogicAction.EXIT_GAME:
			ScreenLoaderVisualIndicator.Instance.LoadScene(GameSettings.SCENE_MAINMENU);
			break;
		}
	}
}
