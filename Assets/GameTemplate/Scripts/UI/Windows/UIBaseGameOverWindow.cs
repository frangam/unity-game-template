using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBaseGameOverWindow : UIBaseWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Text lbCurrentScore;

	[SerializeField]
	private Text lbBestScore;


	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void open (){
		if(lbCurrentScore){
			lbCurrentScore.text = BaseGameController.Instance.CurrentScore.ToString();
		}

		if(lbBestScore){
			lbBestScore.text = ScoresHandler.Instance.getBestScore().ToString();
		}
		
		base.open ();
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	
}
