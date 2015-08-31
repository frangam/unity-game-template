/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;

public class GCConnect : PersistentSingleton<GCConnect> {
	private static bool IsInited = false;
	private bool leaderBoardsLoaded = false;
	private bool achievementsLoaded = false;
	private bool achievementsChecked = false; //it has checked if there are achievements that need to be updated in server side beacuse they were unlocked locally
	private bool notifiedLoader = false;
	private int totalScoresToLoad = 0;
	private int scoresLoaded = 0;
	
	void Update(){
		if(BaseGameScreenController.Instance.Section == GameSection.LOAD_SCREEN && !notifiedLoader && leaderBoardsLoaded && achievementsLoaded && achievementsChecked){
			GTDebug.log("Notifying to GameLoaderManager that GameCenter is prepared");
			GameLoaderManager.Instance.GCPrepared = true;
			notifiedLoader = true;
		}
	}
	
	void OnEnable(){
		GTDebug.log("Enabling");
		
		if(GameSettings.Instance.CurrentAchievements != null && GameSettings.Instance.CurrentAchievements.Count > 0)
			BaseAchievementsManager.dispatcher.addEventListener(BaseAchievementsManager.ACHIEVEMENTS_INITIAL_CHEKING, OnAchievementsChecked);		
	}
	
	void OnDisable(){
		GTDebug.log("Disabling");
		
		if(GameSettings.Instance.CurrentAchievements != null && GameSettings.Instance.CurrentAchievements.Count > 0)
			BaseAchievementsManager.dispatcher.removeEventListener(BaseAchievementsManager.ACHIEVEMENTS_INITIAL_CHEKING, OnAchievementsChecked);	
	}
	
	public void init (bool showLoginWindowGameServices = true) {
		if(!IsInited) {
			GameCenterManager.OnAuthFinished += OnAuthFinished;
			
			//--
			// Achievements
			//--
			if(GameSettings.Instance.CurrentAchievements != null && GameSettings.Instance.CurrentAchievements.Count > 0){
				foreach(Achievement a in GameSettings.Instance.CurrentAchievements){
					GameCenterManager.RegisterAchievement(a.IdForIOS);
				}
			}
			
			
			GameCenterManager.OnAchievementsLoaded += OnAchievementsLoaded;
			//			GameCenterManager.OnAchievementsProgress += OnAchievementProgress;
			//			GameCenterManager.OnAchievementsReset += OnAchievementsReset;
			
			//--
			// Player Scores
			//--
			GameCenterManager.OnPlayerScoreLoaded += OnPlayerScoreLoaded;
			GameCenterManager.OnScoreSubmitted += OnScoreSubmitted;
			
			//Initializing Game Cneter class. This action will triger authentication flow
			if(showLoginWindowGameServices){
				GTDebug.log("Showing authentication flow of Game Center");
				
				GameCenterManager.Init();
				IsInited = true;
			}
			else{
				GTDebug.log("NOT showing authentication flow of Game Center");
			}
		}
	}
	
	private void loadScores(){
		List<Score> scoresToLoad = new List<Score>();
		
		Score uniqueScore = new Score(GameSettings.Instance.CurrentUniqueRankingID);
		if(uniqueScore != null && !string.IsNullOrEmpty(uniqueScore.Id)){
			scoresToLoad.Add(uniqueScore);
		}
		
		Score uniqueSurvivalScore = new Score(GameSettings.Instance.CurrentUniqueRankingID);
		if(uniqueSurvivalScore != null && !string.IsNullOrEmpty(uniqueSurvivalScore.Id)){
			scoresToLoad.Add(uniqueSurvivalScore);
		}
		
		//the rest of scores
		if(GameSettings.Instance.CurrentScores != null && GameSettings.Instance.CurrentScores.Count > 0){
			
			
			foreach(Score score in GameSettings.Instance.CurrentScores){
				if(score != null && !string.IsNullOrEmpty(score.Id)){
					scoresToLoad.Add(score);
				}
			}
		}
		
		//finaly load scores
		totalScoresToLoad = scoresToLoad != null ? scoresToLoad.Count : 0;
		
		if(totalScoresToLoad > 0){
			foreach(Score score in scoresToLoad)
				GameCenterManager.LoadCurrentPlayerScore(score.IdForSaveOniOSStore);
		}
	}
	
	/*--------------------------------
	 * Events
	 -------------------------------*/
	void OnAuthFinished (ISN_Result res) {
		if (res.IsSucceeded) {
			GTDebug.log("Player connected");
			PlayerPrefs.SetInt(GameSettings.PP_LAST_OPENNING_USER_CONNECTED_TO_STORE_SERVICE, 1);
			loadScores();
		} else {
			GTDebug.log("Player NOT connected");
			PlayerPrefs.SetInt(GameSettings.PP_LAST_OPENNING_USER_CONNECTED_TO_STORE_SERVICE, 0);
		}
	}
	
	private void OnPlayerScoreLoaded (GK_PlayerScoreLoadedResult result) {
		if(result.IsSucceeded) {
			scoresLoaded++;
			
			if(scoresLoaded >= totalScoresToLoad)
				GameCenterManager.OnPlayerScoreLoaded -= OnPlayerScoreLoaded;
			
			GK_Score score = result.loadedScore;
			GTDebug.log("Leaderboard ID: "+ score.leaderboardId + " Player score loaded: [Long: " + score.GetLongScore()+"] [Double: "+score.GetDoubleScore()+"]");
			ScoresHandler.Instance.loadBestScoreFromStore(score);
		}
		else{
			GTDebug.log("Player score load was failed");
		}
	}
	
	private void OnScoreSubmitted (ISN_Result result) {
		if(result.IsSucceeded)  {
			GTDebug.log("Score Submitted");
		} else {
			GTDebug.log("Score Submit Failed");
		}
	}
	
	private void OnAchievementsLoaded(ISN_Result res) {
		if(res.IsSucceeded && GameSettings.Instance.CurrentAchievements != null && GameSettings.Instance.CurrentAchievements.Count > 0){
			GTDebug.log("Starting initial checking in server side");
			BaseAchievementsManager.Instance.initialCheckingInServerSide();
		}
	}
	
	//	private void OnAchievementsReset(ISN_Result res) {
	//
	//	}
	//	
	//	private void OnAchievementProgress(ISN_Result res) {
	//
	//	}
	
	
	void OnAchievementsChecked (){
		achievementsChecked = true;
	}
}
