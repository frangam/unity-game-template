using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBasePauseWindow : UIBaseWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Text lbCurrentScore;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void open (){
		if(lbCurrentScore){
			lbCurrentScore.text = BaseGameController.Instance.CurrentScore.ToString();
		}

		base.open ();
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void pause(){
		BaseGameController.Instance.Paused = true;
	}

	public virtual void resume(){
		BaseGameController.Instance.Paused = false;
	}
}
