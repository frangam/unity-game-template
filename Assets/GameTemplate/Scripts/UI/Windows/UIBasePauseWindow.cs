/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
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
			lbCurrentScore.text = GameController.Instance.Manager.CurrentScore.ToString();
		}
		
		base.open ();
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void pause(){
		GameController.Instance.Manager.Paused = true;
	}
	
	public virtual void resume(){
		GameController.Instance.Manager.Paused = false;
	}
}
