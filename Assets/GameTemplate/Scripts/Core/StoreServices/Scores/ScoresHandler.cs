using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class ScoresHandler : PersistentSingleton<ScoresHandler> {
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	
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
				GooglePlayManager.instance.ShowLeaderBoard(rankingID);
			
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
	public void saveScoreOnlyLocally(string rankingID, int score){
		int best = getBestScore (rankingID);
		int scoreToSend = score >= best ? score : best;
		
		//--------------------------------------
		// Save locally on PlayerPrefs
		//--------------------------------------
		//save the best score
		if(score > best)
			saveBestScore(rankingID, score);
		
		//save the last score
		saveLastScore (rankingID, score);
	}
	
	public void sendScoreToServer(string rankingID, int score){
		int best = getBestScore (rankingID);
		int scoreToSend = score >= best ? score : best;
		string id = rankingID;
		
		
		
		
		
		#if UNITY_ANDROID
		if(GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES){
			if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
				if(GameSettings.Instance.showTestLogs)
					Debug.Log ("Sending score to the server: " + scoreToSend + " a ranking: " + id);
				
				if(GooglePlayManager.instance.GetLeaderBoard (id) != null){
					int scoreServidor = GooglePlayManager.instance.GetLeaderBoard (id).GetCurrentPlayerScore(GPBoardTimeSpan.ALL_TIME, GPCollectionType.GLOBAL).rank;
					
					if(scoreServidor < scoreToSend)
						GooglePlayManager.instance.SubmitScoreById (id, scoreToSend);
					else
						saveBestScore(id, scoreServidor);
				}
				else{
					GooglePlayManager.instance.SubmitScoreById (id, scoreToSend);
				}
			}
			
			
		}
		#elif UNITY_IPHONE
		if(GameSettings.Instance.USE_GAMECENTER && GameCenterManager.IsPlayerAuthenticated){
			id = id.Replace("-","_"); //replace because in iOS it is not supported ids with "-"
			
			if(GameSettings.Instance.showTestLogs)
				Debug.Log ("Sending score to the server: " + scoreToSend + " a ranking: " + id);
			
			if(GameCenterManager.GetLeaderboard(id) != null){
				int scoreServidor = GameCenterManager.GetLeaderboard(id).GetCurrentPlayerScore(GCBoardTimeSpan.ALL_TIME, GCCollectionType.GLOBAL).rank;
				
				
				
				if(scoreServidor < scoreToSend)
					GameCenterManager.ReportScore(scoreToSend, id);
				else
					saveBestScore(id, scoreServidor);
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
		if(score > best)
			saveBestScore(rankingID, score);
		
		//save the last score
		saveLastScore (rankingID, score);
	}
	
	//--------------------------------------
	//--------------------------------------
	// Get && Save Scores Methods 
	//--------------------------------------
	//--------------------------------------
	public int getLastScore(string id){
		string key = GameSettings.PP_LAST_SCORE + id; //ultima_puntuacion_RANKING_ID
		
		
		return PlayerPrefs.GetInt(key);
	}
	public int getBestScore(string id){
		string key = GameSettings.PP_BEST_SCORE + id; //ultima_puntuacion_RANKING_ID
		return PlayerPrefs.GetInt(key);
	}
	
	public void saveLastScore(string id, int puntos){
		string key = GameSettings.PP_LAST_SCORE + id; //ultima_puntuacion_RANKING_ID
		PlayerPrefs.SetInt(key, puntos); 
	}
	
	public void saveBestScore(string id, int puntos){
		string key = GameSettings.PP_BEST_SCORE + id; //ultima_puntuacion_RANKING_ID
		PlayerPrefs.SetInt(key, puntos); 
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
