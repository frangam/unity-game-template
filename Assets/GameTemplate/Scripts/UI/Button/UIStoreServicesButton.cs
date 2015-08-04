﻿using UnityEngine;
using System.Collections;


public class UIStoreServicesButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private StoreService service;
	
	[SerializeField]
	private ScoreType scoreType;
	
	[SerializeField]
	private int scoreIndexForGameMultiversion;
	
	//
	//	[SerializeField]
	//	[Tooltip("Leave empty if you want to show all items. Don't leave empty if you want to show an specific item")]
	//	private string id;
	
	[SerializeField]
	[Tooltip("True if we want to get the ranking id from de GameManager")]
	private bool handledByGameManager = false;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public int ScoreIndexForGameMultiversion {
		get {
			return this.scoreIndexForGameMultiversion;
		}
		set {
			scoreIndexForGameMultiversion = value;
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		base.doPress ();
		
		switch(service){
		case StoreService.SCORES:
			string rankId = "";
			
			if(handledByGameManager)
				rankId = GameController.Instance.Manager.getRankingID();
			else{
				switch(scoreType){
				case ScoreType.UNIQUE:	rankId = GameSettings.Instance.CurrentUniqueRankingID;	break;
				case ScoreType.SURVIVAL:	rankId = GameSettings.Instance.CurrentUniqueSurvivalRankingID;	break;
				case ScoreType.BY_INDEX_MULTIGAME: 
					if(GameSettings.Instance.CurrentScoresIDs.Count > scoreIndexForGameMultiversion)
						rankId = GameSettings.Instance.CurrentScoresIDs[scoreIndexForGameMultiversion];
					else
						Debug.LogError("UIStoreServicesButton ("+name+") - score index out of range");
					break;
				}
			}
			
			ScoresHandler.Instance.showRanking(rankId);
			break;
			
		case StoreService.ACHIEVEMENTS:
			BaseAchievementsManager.Instance.showAchievementsFromServer();
			break;
		}
	}
}
