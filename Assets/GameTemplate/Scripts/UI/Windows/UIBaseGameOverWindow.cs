/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
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
			lbCurrentScore.text = GameController.Instance.Manager.CurrentScore.ToString();
		}
		
		if(lbBestScore){
			lbBestScore.text = ScoresHandler.Instance.getBestScore(GameController.Instance.Manager.getRankingID()).ToString();
		}
		
		base.open ();
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	
}
