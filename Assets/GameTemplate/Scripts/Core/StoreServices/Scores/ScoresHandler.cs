/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class ScoresHandler : PersistentSingleton<ScoresHandler> {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private string scoreIDToShow = "";
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public Score getScoreByIndex(int scoreIndex){
		Score res = null;
		
		if(GameSettings.Instance.CurrentScores.Count > scoreIndex){
			res = GameSettings.Instance.CurrentScores[scoreIndex];
		}
		else{
			GTDebug.logErrorAlways("The index " + scoreIndex + "is out of range");
		}
		
		return res;
	}
	public Score getScoreByID(string scoreID){
		Score res = null;
		
		if(GameSettings.Instance.CurrentScores != null && GameSettings.Instance.CurrentScores.Count > 0){
			foreach(Score score in GameSettings.Instance.CurrentScores){
				if(score.Id.Equals(scoreID)){
					res = score;
					break;
				}
			}
		}
		else{
			GTDebug.logErrorAlways("Must to fill scores in GameSettings asset");
		}
		
		if(res == null)
			GTDebug.logErrorAlways("Not found score ID " + scoreID + " in GameSettings asset");
		
		return res;
	}
	
	public void loadBestScoreFromStoreByIndex(int leaderboardIndex, long score){
		if(GameSettings.Instance.CurrentScores.Count > leaderboardIndex){
			loadBestScoreFromStore(GameSettings.Instance.CurrentScores[leaderboardIndex].Id, score);
		}
		else{
			GTDebug.logErrorAlways("The index " + leaderboardIndex + "is out of range");
		}
	}
	
	public void loadBestScoreFromStore(GK_Score gkScore){
		if(gkScore != null){
			string id = gkScore.leaderboardId;
			id = id.Replace("_", "-");
			id = id.Replace(GameSettings.Instance.prefixScoresGroupOnIOS, "");
			
			GTDebug.log("Get score from GameSettings asset with LeaderboardID: "+ id);
			
			//get local score objhec
			Score score = getScoreByID(id);
			
			if(score != null){
				GTDebug.log("Local score: LeaderboardID: "+ id + " score: " +score.Value);
				
				//When platform is iOS and formart is elapsed time we work with milliseconds (neScoreValue is in ms)
				//we ned to do a coversion from milliseconds score to HUNDREDTHS_OF_A_SECOND (convert ms to s and * 100)
				if(Application.platform == RuntimePlatform.IPhonePlayer && score.Format == ScoreFormat.ELAPSED_TIME_HUNDREDTHS_OF_A_SECOND){
					double scoreSavedInServerDouble = gkScore.GetDoubleScore();
					System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(scoreSavedInServerDouble);
					long convertedValue = ((long) (timeSpan.TotalMilliseconds)); //milliseconds
					GTDebug.log("LeaderboardID: "+ score.IdForSaveOniOSStore + " Server score (in Secs) - Converting Seconds to MilliSeconds score. Before Conversion: "+scoreSavedInServerDouble+"Secs. After: "+ convertedValue+"Ms");
					loadBestScoreFromStore(score.Id, convertedValue);
				}
				else{
					loadBestScoreFromStore(score.Id, gkScore.GetLongScore());
				}
			}
			else{
				GTDebug.log("Local score not found with LeaderboardID: "+ id);
			}
		}
	}
	
	/// <summary>
	/// Loads the best score from store.
	/// </summary>
	/// <param name="scoreID">Score ID.</param>
	/// <param name="scoreFromStore">Score from store.</param>
	public void loadBestScoreFromStore(string scoreID, long scoreFromStore){
		if(!string.IsNullOrEmpty(scoreID)){
			Score score = getScoreByID(scoreID);
			if(score == null) return;
			
			long savedLocally = getBestScoreByID (scoreID); //the score saved on the device (locally)
			
			GTDebug.log("Leaderboard ID: " + scoreID + " with score saved on the Store: "+  scoreFromStore.ToString());
			GTDebug.log("Leaderboard ID: " + scoreID + " with score saved on the device: "+  savedLocally.ToString());
			
			//update the score saved on the device by the online score is greater
			if(scoreFromStore > savedLocally){
				GTDebug.log("LeaderboardID: "+ scoreID + "Saving locally score from store because is greater than the previous saved on the device. From Store: "+scoreFromStore+", SavedLocally: "+savedLocally);
				saveScoreOnlyLocallyByID(scoreID, scoreFromStore);
			}
			//send to server the greatest score that is the saved score locally
			else if(scoreFromStore < savedLocally){
				GTDebug.log("LeaderboardID: "+ scoreID + "Sending to server the best score saved locally: " +savedLocally +". From Store: " + scoreFromStore);
				
				ScoresHandler.Instance.sendScoreToServerByID(scoreID, savedLocally);
			}
			
		}
	}
	
	public void showRankingByIndex(int rankingIndex){
		if(GameSettings.Instance.CurrentScores.Count > rankingIndex){
			string id = "";
			
			#if UNITY_IPHONE
			id = GameSettings.Instance.CurrentScores[rankingIndex].IdForSaveOniOSStore;
			#elif UNITY_ANDROID
			id = GameSettings.Instance.CurrentScores[rankingIndex].Id;
			#endif
			showRanking(id);
		}
		else{
			GTDebug.logErrorAlways("The index " + rankingIndex + "is out of range");
		}
	}
	public void showRanking(string rankingID = ""){
		if(Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor) return;
		
		#if UNITY_IPHONE
		if(!GameSettings.Instance.USE_GAMECENTER)
			return;
		#elif UNITY_ANDROID
		if(!GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES)
			return;
		#endif
		
		
		#if UNITY_ANDROID
		if(GooglePlayConnection.state == GPConnectionState.STATE_UNCONFIGURED){
			GPSConnect.Instance.init();
			
			AndroidDialog dialog = AndroidDialog.Create(Localization.Localize(ExtraLocalizations.POPUP_TITLE_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.POPUP_DESC_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.OK_BUTTON_GPS_LOGIN_POPUP)
			                                            , Localization.Localize(ExtraLocalizations.CANCEL_BUTTON_GPS_LOGIN_POPUP));
			dialog.ActionComplete += OnAndroidDialogClose;
		}
		else if(GooglePlayConnection.state == GPConnectionState.STATE_DISCONNECTED){
			AndroidDialog dialog = AndroidDialog.Create(Localization.Localize(ExtraLocalizations.POPUP_TITLE_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.POPUP_DESC_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.OK_BUTTON_GPS_LOGIN_POPUP)
			                                            , Localization.Localize(ExtraLocalizations.CANCEL_BUTTON_GPS_LOGIN_POPUP));
			dialog.ActionComplete += OnAndroidDialogClose;
		}
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
			//Show all rankings
			if(string.IsNullOrEmpty(rankingID))
				GooglePlayManager.instance.ShowLeaderBoardsUI();
			//show specific ranking
			else
				GooglePlayManager.instance.ShowLeaderBoardById(rankingID);
			
		}
		
		#elif UNITY_IPHONE
		
		
		scoreIDToShow = rankingID;
		
		if(GameCenterManager.IsPlayerAuthenticated){
			//Show all rankings
			if(string.IsNullOrEmpty(rankingID))
				GameCenterManager.ShowLeaderboards();
			//show specific ranking
			else
				GameCenterManager.ShowLeaderboard(rankingID);
		}
		else{
			IOSDialog dialog = IOSDialog.Create(Localization.Localize(ExtraLocalizations.POPUP_TITLE_GC_LOGIN)
			                                    ,Localization.Localize(ExtraLocalizations.POPUP_DESC_GC_LOGIN)
			                                    , Localization.Localize(ExtraLocalizations.OK_BUTTON_GC_LOGIN_POPUP)
			                                    , Localization.Localize(ExtraLocalizations.CANCEL_BUTTON_GC_LOGIN_POPUP));
			dialog.OnComplete += onDialogIOSClose;
		}
		#endif
	}
	
	void OnAuthIOSFinished (ISN_Result res) {
		GameCenterManager.OnAuthFinished -= OnAuthIOSFinished;
		if (res.IsSucceeded) {
			if(!string.IsNullOrEmpty(scoreIDToShow))
				GameCenterManager.ShowLeaderboard(scoreIDToShow);
			else
				GameCenterManager.ShowLeaderboards();
		} 
	}
	private void OnAndroidDialogClose(AndroidDialogResult res) {
		switch(res) {
		case AndroidDialogResult.YES:
			GooglePlayConnection.instance.connect();
			GooglePlayManager.instance.showAchievementsUI();
			break;
		case AndroidDialogResult.NO:
			break;
			
		}
	}
	private void onDialogIOSClose(IOSDialogResult result) {
		
		//parsing result
		switch(result) {
		case IOSDialogResult.YES:
			GameCenterManager.OnAuthFinished += OnAuthIOSFinished;
			GameCenterManager.init();
			break;
		case IOSDialogResult.NO:
			break;
			
		}
	}
	public void saveScoreOnlyLocallyByIndex(int rankingIndex, long score){
		if(GameSettings.Instance.CurrentScores.Count > rankingIndex){
			saveScoreOnlyLocallyByID(GameSettings.Instance.CurrentScores[rankingIndex].Id, score);
		}
		else{
			GTDebug.logErrorAlways("The index " + rankingIndex + "is out of range");
		}
	}
	
	//--------------------------------------
	// Save locally on PlayerPrefs
	//--------------------------------------
	public void saveScoreOnlyLocallyByID(string scoreID, long newScoreValue){
		string id = scoreID;
		Score score = getScoreByID(id);
		if(score == null) return;
		
		long best = getBestScoreByID (id);
		bool save = false;
		
		//criteria order to know which is a best score
		switch(score.Order){
		case ScoreOrder.DESCENDING: save = newScoreValue > best; break;
		case ScoreOrder.ASCENDING: save = best == 0 || newScoreValue < best; break;
		}
		
		if(save)
			saveBestScoreByID(id, newScoreValue);
		
		//save the last score
		saveLastScoreByID (id, newScoreValue);
	}
	
	/// <summary>
	/// Sends the score to server.
	/// This methods is useful when use game multiversion
	/// </summary>
	/// <param name="rankingIndex">Ranking by index.</param>
	/// <param name="score">Score.</param>
	public void sendScoreToServerByIndex(int rankingIndex, long score){
		if(GameSettings.Instance.CurrentScores.Count > rankingIndex){
			sendScoreToServerByID(GameSettings.Instance.CurrentScores[rankingIndex].Id, score);
		}
		else{
			GTDebug.log("The index " + rankingIndex + "is out of range");
		}
	}
	
	/// <summary>
	/// Sends the score to server by I.
	/// </summary>
	/// <param name="scoreID">Score I.</param>
	/// <param name="newScoreValue">New score value (If score format is ElapsedTime, Pass the value in Milliseconds".</param>
	public void sendScoreToServerByID(string scoreID, long newScoreValue){
		string id = scoreID;
		Score score = getScoreByID(id); //get the score by its id from gamesettings asset
		if(score == null) return;
		
		
		#if UNITY_IPHONE
		if(!GameSettings.Instance.USE_GAMECENTER)
			return;
		#elif UNITY_ANDROID
		if(!GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES)
			return;
		#endif
		
		GTDebug.log("LeaderboardID: "+ id + ". New score: "+newScoreValue);
		
		bool sendScoreToServer = false;
		long best = getBestScoreByID (id);
		long scoreToSend = 0;
		long scoreSavedInServer = 0;
		
		//criteria order to know which is a best score
		switch(score.Order){
		case ScoreOrder.DESCENDING: scoreToSend = newScoreValue >= best ? newScoreValue : best; break;
		case ScoreOrder.ASCENDING: scoreToSend = best == 0 || newScoreValue <= best ? newScoreValue : best; break;
		}
		
		GTDebug.log("LeaderboardID: "+ id + ". Best Score saved in device: "+best);
		GTDebug.log("LeaderboardID: "+ id + ". Score to send: "+scoreToSend);
		
		
		#if UNITY_ANDROID
		if(GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES){
			if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){				
				if(GooglePlayManager.instance.GetLeaderBoard (id) != null){
					scoreSavedInServer = GooglePlayManager.instance.GetLeaderBoard (id).GetCurrentPlayerScore(GPBoardTimeSpan.ALL_TIME, GPCollectionType.GLOBAL).score;
					
					GTDebug.log("ScoresHandler - Server score: "+scoreSavedInServer + ". Score to send to the server: "+scoreToSend);
					
					
					//criteria order to know which is a best score to send to the server or not
					switch(score.Order){
					case ScoreOrder.DESCENDING: sendScoreToServer = scoreSavedInServer < scoreToSend; break;
					case ScoreOrder.ASCENDING: sendScoreToServer = scoreSavedInServer <= 0 || ( scoreSavedInServer > scoreToSend); break;
					}
				}
				else{
					sendScoreToServer = true;
				}
				
				//send score to the server
				if(sendScoreToServer)
					GooglePlayManager.instance.SubmitScoreById (id, scoreToSend);
				
				if(sendScoreToServer)
					GTDebug.log("Sending score to the server: " + scoreToSend + " a ranking: " + id);
			}
			
			
		}
		#elif UNITY_IPHONE
		if(GameSettings.Instance.USE_GAMECENTER && GameCenterManager.IsPlayerAuthenticated){
			//get player score saved in server side
			if(GameCenterManager.GetLeaderboard(score.IdForSaveOniOSStore) != null){
				//When platform is iOS and formart is elapsed time we work with milliseconds (neScoreValue is in ms)
				//we ned to do a coversion from milliseconds score to HUNDREDTHS_OF_A_SECOND (convert ms to s and * 100)
				if(score.Format == ScoreFormat.ELAPSED_TIME_HUNDREDTHS_OF_A_SECOND){
					double scoreSavedInServerDouble = GameCenterManager.GetLeaderboard(score.IdForSaveOniOSStore).GetCurrentPlayerScore(GK_TimeSpan.ALL_TIME, GK_CollectionType.GLOBAL).GetDoubleScore();
					System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(scoreSavedInServerDouble);
					long convertedValue = ((long) (timeSpan.TotalMilliseconds)); //milliseconds
					GTDebug.log("LeaderboardID: "+ score.IdForSaveOniOSStore + "Server score (in Secs) - Converting Seconds to MilliSeconds score. Before Conversion: "+scoreSavedInServer+"Secs. After: "+ convertedValue+"Ms");
					scoreSavedInServer = convertedValue;
				}
				else
					scoreSavedInServer = GameCenterManager.GetLeaderboard(score.IdForSaveOniOSStore).GetCurrentPlayerScore(GK_TimeSpan.ALL_TIME, GK_CollectionType.GLOBAL).GetLongScore();
				
				GTDebug.log("LeaderboardID: "+ score.IdForSaveOniOSStore + "Server score: "+scoreSavedInServer + ". Score to send to the server: "+scoreToSend);
				GTDebug.log("LeaderboardID: "+ score.IdForSaveOniOSStore + "Score Order: "+score.Order);
				
				//criteria order to know which is a best score to send to the server or not
				switch(score.Order){
				case ScoreOrder.DESCENDING: sendScoreToServer = scoreSavedInServer < scoreToSend; break;
				case ScoreOrder.ASCENDING: sendScoreToServer = scoreSavedInServer == 0 || ( scoreSavedInServer > scoreToSend); break;
				}
				
				
			}
			else{
				sendScoreToServer = true;
			}
			
			GTDebug.log("LeaderboardID: " + score.IdForSaveOniOSStore + " .Must we send the score " + scoreToSend + "to server ? " + sendScoreToServer); 
			
			//send score to the server
			if(sendScoreToServer){
				//When platform is iOS and formart is elapsed time we work with milliseconds (neScoreValue is in ms)
				//we ned to do a coversion from milliseconds score to HUNDREDTHS_OF_A_SECOND (convert ms to s and * 100)
				if(score.Format == ScoreFormat.ELAPSED_TIME_HUNDREDTHS_OF_A_SECOND){
					System.TimeSpan timeSpan = System.TimeSpan.FromMilliseconds(scoreToSend);
					double scoreToSendInDouble = timeSpan.TotalSeconds; //seconds to send to Game Center
					
					GTDebug.log("LeaderboardID: "+ score.IdForSaveOniOSStore + "Reporting Score [" + scoreToSendInDouble + "] (seconds) to server (format elapset time hudredths of a second)");
					
					GameCenterManager.ReportScore(scoreToSendInDouble, score.IdForSaveOniOSStore); //sending
					GTDebug.log("LeaderboardID: "+ score.IdForSaveOniOSStore + "Score sent In Secs - Converted Ms to Seconds score."+scoreToSendInDouble);
				}
				//send long score
				else{
					GameCenterManager.ReportScore(scoreToSend, score.IdForSaveOniOSStore); //sending
				}
				
				GTDebug.log ("LeaderboardID: "+ score.IdForSaveOniOSStore + "Sending score to the server: " + scoreToSend);
			}
		}
		#endif
		
		
		//--------------------------------------
		// Save locally on PlayerPrefs
		//--------------------------------------
		//save the best score
		switch(score.Order){
		case ScoreOrder.DESCENDING:
			if(scoreSavedInServer > 0 && scoreSavedInServer > best && scoreSavedInServer > scoreToSend){
				GTDebug.log("Saving locally best score is in the server side: "+scoreSavedInServer);
				
				saveBestScoreByID(id, scoreSavedInServer);
			}
			else if(newScoreValue> 0 && newScoreValue > best){
				GTDebug.log("Saving locally best score player has got now: "+newScoreValue);
				
				saveBestScoreByID(id, newScoreValue);
			}
			break;
			
		case ScoreOrder.ASCENDING:
			if(scoreSavedInServer > 0 && (best == 0 || (scoreSavedInServer < best && scoreSavedInServer < scoreToSend))){
				GTDebug.log("Saving locally best score is in the server side: "+scoreSavedInServer);
				
				saveBestScoreByID(id, scoreSavedInServer);
			}
			else if(newScoreValue > 0 && (best == 0 || newScoreValue < best)){
				GTDebug.log("Saving locally best score player has got now: "+newScoreValue);
				
				saveBestScoreByID(id, newScoreValue);
			}
			break;
		}
		
		//save the last score
		saveLastScoreByID (id, newScoreValue);
	}
	
	//--------------------------------------
	//--------------------------------------
	// Get && Save Scores Methods 
	//--------------------------------------
	//--------------------------------------
	public long getLastScoreByIndex(int scoreIndex){
		long res = 0;
		if(GameSettings.Instance.CurrentScores.Count > scoreIndex){
			res = getLastScoreByID(GameSettings.Instance.CurrentScores[scoreIndex].Id);
		}
		else{
			Debug.LogError("ScoresHandler - the index " + scoreIndex + "is out of range");
		}
		
		return res;
	}
	public long getLastScoreByID(string id){
		string key = GameSettings.PP_LAST_SCORE + id; //ultima_puntuacion_RANKING_ID
		string score = PlayerPrefs.GetString(key);
		long res = 0;
		long.TryParse(score, out res);
		
		return res;
	}
	public long getBestScoreByIndex(int scoreIndex){
		long res = 0;
		if(GameSettings.Instance.CurrentScores.Count > scoreIndex){
			res = getBestScoreByID(GameSettings.Instance.CurrentScores[scoreIndex].Id);
		}
		else{
			Debug.LogError("ScoresHandler - the index " + scoreIndex + "is out of range");
		}
		
		return res;
	}
	public long getBestScoreByID(string id){
		string key = GameSettings.PP_BEST_SCORE + id; //ultima_puntuacion_RANKING_ID
		string score = PlayerPrefs.GetString(key);
		long res = 0;
		long.TryParse(score, out res);
		
		return res;
	}
	
	public void saveLastScoreByIndex(int scoreIndex, long score){
		if(GameSettings.Instance.CurrentScores.Count > scoreIndex){
			saveLastScoreByID(GameSettings.Instance.CurrentScores[scoreIndex].Id, score);
		}
		else{
			GTDebug.logErrorAlways("The index " + scoreIndex + "is out of range");
		}
	}
	public void saveLastScoreByID(string id, long score){
		string key = GameSettings.PP_LAST_SCORE + id; //ultima_puntuacion_RANKING_ID
		PlayerPrefs.SetString(key, score.ToString()); 
	}
	
	public void saveBestScoreByIndex(int scoreIndex, long score){
		if(GameSettings.Instance.CurrentScores.Count > scoreIndex){
			saveBestScoreByID(GameSettings.Instance.CurrentScores[scoreIndex].Id, score);
		}
		else{
			GTDebug.logErrorAlways("The index " + scoreIndex + "is out of range");
		}
	}
	
	public void saveBestScoreByID(string id, long score){
		string key = GameSettings.PP_BEST_SCORE + id; //ultima_puntuacion_RANKING_ID
		PlayerPrefs.SetString(key, score.ToString()); 
	}
	
	//--------------------------------------
	//--------------------------------------
	// Get Methods for default ranking ids
	//--------------------------------------
	//--------------------------------------
	
	/// <summary>
	/// Get the ranking id based on the game difficulty
	/// </summary>
	/// <returns>The ranking id by difficulty.</returns>
	/// <param name="dif">Dif.</param>
	public string idRankingByDifficulty(GameDifficulty dif){
		string res = GameSettings.Instance.ID_RANKING_EASY;
		
		switch(dif){
		case GameDifficulty.EASY:
			res = GameSettings.Instance.ID_RANKING_EASY;
			break;
			
		case GameDifficulty.NORMAL:
			res = GameSettings.Instance.ID_RANKING_NORMAL;
			break;
			
		case GameDifficulty.HARD:
			res = GameSettings.Instance.ID_RANKING_HARD;
			break;
			
		case GameDifficulty.PRO:
			res = GameSettings.Instance.ID_RANKING_PRO;
			break;
			
		case GameDifficulty.GOD:
			res = GameSettings.Instance.ID_RANKING_GOD;
			break;
		}
		
		return res;
	}
	
	/// <summary>
	/// Get the ranking id based on the game world
	/// </summary>
	/// <returns>The ranking by game world.</returns>
	/// <param name="world">World.</param>
	public string idRankingByGameWorld(int world){
		string res = "";//GameSettings.Instance.ID_RANKING_WORLD_1;
		
		if(world > 0 && GameSettings.Instance.CurrentWorldLevelRankingIDs != null && GameSettings.Instance.CurrentWorldLevelRankingIDs.Count > world-1){
			res = GameSettings.Instance.CurrentWorldLevelRankingIDs[world-1];
		}
		
		return res;
	}
	
	/// <summary>
	/// Get the ranking id based on the game level
	/// </summary>
	/// <returns>The ranking by game world.</returns>
	/// <param name="world">World.</param>
	public string idRankingBySurvivalLevels(int level){
		string res = "";//GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_1;
		
		if(level > 0 && GameSettings.Instance.CurrentSurvivalLevelRankingIDs != null && GameSettings.Instance.CurrentSurvivalLevelRankingIDs.Count > level-1){
			res = GameSettings.Instance.CurrentSurvivalLevelRankingIDs[level-1];
		}
		
		return res;
	}
}
