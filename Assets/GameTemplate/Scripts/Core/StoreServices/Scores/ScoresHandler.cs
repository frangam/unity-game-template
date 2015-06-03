using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class ScoresHandler : PersistentSingleton<ScoresHandler> {
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	//	private void OnPlayerScoreLoaded (ISN_PlayerScoreLoadedResult result) {
	//		if(result.IsSucceeded) {
	//			GCScore score = result.loadedScore;
	//			
	//			if(GameSettings.Instance.showTestLogs)
	//				Debug.Log("Leaderboard " + score.leaderboardId + "Score: " + score.score + "\n" + "Rank:" + score.rank);
	//			
	//			//			IOSNativePopUpManager.showMessage("Leaderboard " + score.leaderboardId, "Score: " + score.score + "\n" + "Rank:" + score.rank);
	//			
	//			//			Debug.Log("double score representation: " + score.GetDoubleScore());
	//			
	//			if(GameSettings.Instance.showTestLogs)
	//				Debug.Log("long score representation: " + score.GetLongScore());
	//			
	//			string id = GameSettings.Instance.ID_UNIQUE_RANKING;
	//			long scoreLong = score.GetLongScore();
	//			loadBestScore(id, scoreLong);
	//		}
	//	}
	//
	//	private void OnLeaderBoardsLoaded(GooglePlayResult result) {
	//#if UNITY_ANDROID
	//		GooglePlayManager.ActionLeaderboardsLoaded -= OnLeaderBoardsLoaded;
	//		if(result.isSuccess) {
	//			string id = GameSettings.Instance.ID_UNIQUE_RANKING;
	//
	//			if(GooglePlayManager.instance.GetLeaderBoard(id) != null){
	//				GPLeaderBoard leaderboard = GooglePlayManager.instance.GetLeaderBoard(id);
	//				long score = leaderboard.GetCurrentPlayerScore(GPBoardTimeSpan.ALL_TIME, GPCollectionType.GLOBAL).score;
	//				loadBestScore(id, score);
	//			}
	//
	//		} else {
	//			if(GameSettings.Instance.showTestLogs)
	//				Debug.Log("ScoresHandler - Leader-Boards Loaded error: "+ result.message);
	////			AndroidMessage.Create("Leader-Boards Loaded error: ", result.message);
	//		}
	//#endif
	//	}
	
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	//	public void Start(){
	//	
	//#if UNITY_ANDROID
	//		GooglePlayManager.ActionLeaderboardsLoaded += OnLeaderBoardsLoaded;
	//		GooglePlayManager.instance.loadLeaderBoards ();
	//#elif UNITY_IPHONE
	//		GameCenterManager.OnPlayerScoreLoaded += OnPlayerScoreLoaded;
	//#endif
	//	}
	
	public void loadBestScore(string leaderboardID, long score){
		if(!string.IsNullOrEmpty(leaderboardID)){
			if(GameSettings.Instance.showTestLogs){
				Debug.Log(leaderboardID + "  long score saved on the Store: "+  score.ToString());
			}
			
			long scoreFromStore = score; //the score saved on the online store
			long savedLocally = getBestScore (leaderboardID); //the score saved on the device (locally)
			
			if(GameSettings.Instance.showTestLogs){
				Debug.Log(leaderboardID + " int score saved on the Store: "+  scoreFromStore.ToString());
				Debug.Log(leaderboardID + "  score saved on the device: "+  savedLocally.ToString());
			}
			
			//update the score saved on the device by the online score is greater
			if(scoreFromStore > savedLocally){
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("ScoresHandler - saving locally score from store because is greater than the previous saved on the device. From Store: "+scoreFromStore+", SavedLocally: "+savedLocally);
				saveScoreOnlyLocally(leaderboardID, scoreFromStore);
			}
			//send to server the greatest score that is the saved score locally
			else if(scoreFromStore < savedLocally){
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("ScoresHandler - sending to server the best score saved locally: " +savedLocally +". From Store: " + scoreFromStore);
				
				ScoresHandler.Instance.sendScoreToServer(leaderboardID, savedLocally);
			}
			
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
			dialog.addEventListener(BaseEvent.COMPLETE, OnDialogClose);
		}
		else if(GooglePlayConnection.state == GPConnectionState.STATE_DISCONNECTED){
			AndroidDialog dialog = AndroidDialog.Create(Localization.Localize(ExtraLocalizations.POPUP_TITLE_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.POPUP_DESC_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.OK_BUTTON_GPS_LOGIN_POPUP)
			                                            , Localization.Localize(ExtraLocalizations.CANCEL_BUTTON_GPS_LOGIN_POPUP));
			dialog.addEventListener(BaseEvent.COMPLETE, OnDialogClose);
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
		string id = rankingID.Replace("-","_"); //replace because in iOS it is not supported ids with "-"
		
		//Show all rankings
		if(string.IsNullOrEmpty(rankingID))
			GameCenterManager.ShowLeaderboards();
		//show specific ranking
		else
			GameCenterManager.ShowLeaderboard(id);
		#endif
	}
	private void OnDialogClose(CEvent e) {
		//removing listner
		(e.dispatcher as AndroidDialog).removeEventListener(BaseEvent.COMPLETE, OnDialogClose);
		
		//parsing result
		switch((AndroidDialogResult)e.data) {
		case AndroidDialogResult.YES:
			GooglePlayConnection.instance.connect();
			GooglePlayManager.instance.showAchievementsUI();
			break;
		case AndroidDialogResult.NO:
			break;
			
		}
	}
	public void saveScoreOnlyLocally(string rankingID, long score){
		long best = getBestScore (rankingID);
		long scoreToSend = score >= best ? score : best;
		
		//--------------------------------------
		// Save locally on PlayerPrefs
		//--------------------------------------
		//save the best score
		if(score > best)
			saveBestScore(rankingID, score);
		
		//save the last score
		saveLastScore (rankingID, score);
	}
	
	public void sendScoreToServer(string rankingID, long score){
		long best = getBestScore (rankingID);
		long scoreToSend = score >= best ? score : best;
		long serverScore = 0;
		string id = rankingID;		
		
		
		#if UNITY_ANDROID
		if(GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES){
			if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
				if(GameSettings.Instance.showTestLogs)
					Debug.Log ("Sending score to the server: " + scoreToSend + " a ranking: " + id);
				
				if(GooglePlayManager.instance.GetLeaderBoard (id) != null){
					serverScore = GooglePlayManager.instance.GetLeaderBoard (id).GetCurrentPlayerScore(GPBoardTimeSpan.ALL_TIME, GPCollectionType.GLOBAL).score;
					
					if(GameSettings.Instance.showTestLogs)
						Debug.Log("ScoresHandler - Server score: "+serverScore + ". Score to send to the server: "+scoreToSend);
					
					if(serverScore < scoreToSend){
						GooglePlayManager.instance.SubmitScoreById (id, scoreToSend);
					}
				}
				else
					GooglePlayManager.instance.SubmitScoreById (id, scoreToSend);
			}
			
			
		}
		#elif UNITY_IPHONE
		if(GameSettings.Instance.USE_GAMECENTER && GameCenterManager.IsPlayerAuthenticated){
			id = id.Replace("-","_"); //replace because in iOS it is not supported ids with "-"
			
			if(GameSettings.Instance.showTestLogs)
				Debug.Log ("Sending score to the server: " + scoreToSend + " a ranking: " + id);
			
			if(GameCenterManager.GetLeaderboard(id) != null){
				serverScore = GameCenterManager.GetLeaderboard(id).GetCurrentPlayerScore(GCBoardTimeSpan.ALL_TIME, GCCollectionType.GLOBAL).GetLongScore();
				
				if(serverScore < scoreToSend)
					GameCenterManager.ReportScore(scoreToSend, id);
			}
			else{
				GameCenterManager.ReportScore(scoreToSend, id);
			}
		}
		#endif
		
		
		//--------------------------------------
		// Save locally on PlayerPrefs
		//--------------------------------------
		//save the best score
		if(serverScore > best && serverScore > scoreToSend){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("ScoresHandler - saving locally best score is in the server side: "+serverScore);
			
			saveBestScore(rankingID, serverScore);
		}
		else if(score > best){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("ScoresHandler - saving locally best score player has got now: "+score);
			
			saveBestScore(rankingID, score);
		}
		
		//save the last score
		saveLastScore (rankingID, score);
	}
	
	//--------------------------------------
	//--------------------------------------
	// Get && Save Scores Methods 
	//--------------------------------------
	//--------------------------------------
	public long getLastScore(string id){
		string key = GameSettings.PP_LAST_SCORE + id; //ultima_puntuacion_RANKING_ID
		string score = PlayerPrefs.GetString(key);
		long res = 0;
		long.TryParse(score, out res);
		
		return res;
	}
	public long getBestScore(string id){
		string key = GameSettings.PP_BEST_SCORE + id; //ultima_puntuacion_RANKING_ID
		string score = PlayerPrefs.GetString(key);
		long res = 0;
		long.TryParse(score, out res);
		
		return res;
	}
	
	public void saveLastScore(string id, long score){
		string key = GameSettings.PP_LAST_SCORE + id; //ultima_puntuacion_RANKING_ID
		PlayerPrefs.SetString(key, score.ToString()); 
	}
	
	public void saveBestScore(string id, long score){
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
		
		if(world > 0 && GameSettings.Instance.worldLevelRankingIDS != null && GameSettings.Instance.worldLevelRankingIDS.Count > world-1){
			res = GameSettings.Instance.worldLevelRankingIDS[world-1];
		}
		
		//		switch(world){
		//		case 1: res = GameSettings.Instance.ID_RANKING_WORLD_1; break;
		//		case 2: res = GameSettings.Instance.ID_RANKING_WORLD_2; break;
		//		case 3: res = GameSettings.Instance.ID_RANKING_WORLD_3; break;
		//		case 4: res = GameSettings.Instance.ID_RANKING_WORLD_4; break;
		//		case 5: res = GameSettings.Instance.ID_RANKING_WORLD_5; break;
		//		case 6: res = GameSettings.Instance.ID_RANKING_WORLD_6; break;
		//		case 7: res = GameSettings.Instance.ID_RANKING_WORLD_7; break;
		//		case 8: res = GameSettings.Instance.ID_RANKING_WORLD_8; break;
		//		case 9: res = GameSettings.Instance.ID_RANKING_WORLD_9; break;
		//		case 10: res = GameSettings.Instance.ID_RANKING_WORLD_10; break;
		//		}
		
		return res;
	}
	
	/// <summary>
	/// Get the ranking id based on the game level
	/// </summary>
	/// <returns>The ranking by game world.</returns>
	/// <param name="world">World.</param>
	public string idRankingBySurvivalLevels(int level){
		string res = "";//GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_1;
		
		if(level > 0 && GameSettings.Instance.survivalLevelRankingIDS != null && GameSettings.Instance.survivalLevelRankingIDS.Count > level-1){
			res = GameSettings.Instance.worldLevelRankingIDS[level-1];
		}
		
		//		switch(level){
		//		case 1: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_1; break;
		//		case 2: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_2; break;
		//		case 3: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_3; break;
		//		case 4: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_4; break;
		//		case 5: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_5; break;
		//		case 6: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_6; break;
		//		case 7: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_7; break;
		//		case 8: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_8; break;
		//		case 9: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_9; break;
		//		case 10: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_10; break;
		//		}
		
		return res;
	}
}
