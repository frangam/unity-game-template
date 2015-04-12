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
	
	void Update(){
		if(!notifiedLoader && leaderBoardsLoaded && achievementsLoaded && achievementsChecked){
			GameLoaderManager.Instance.GCPrepared = true;
			notifiedLoader = true;
		}
	}
	
	void OnEnable(){
		BaseAchievementsManager.dispatcher.addEventListener(BaseAchievementsManager.ACHIEVEMENTS_INITIAL_CHEKING, OnAchievementsChecked);		
	}
	
	void OnDisable(){
		BaseAchievementsManager.dispatcher.removeEventListener(BaseAchievementsManager.ACHIEVEMENTS_INITIAL_CHEKING, OnAchievementsChecked);	
	}
	
	public void init (bool showLoginWindowGameServices = true) {
		if(!IsInited) {
			
			
			//--
			// Logros
			//--
			//Achievement registration. If you will skipt this step GameCenterManager.achievements array will contain only achievements with reported progress 
			
			//			foreach(string id in baseachi.Instance.Ids){
			//				GameCenterManager.registerAchievement (id);
			//			}
			
			
			
			//Listen for the Game Center events
			GameCenterManager.Dispatcher.addEventListener (GameCenterManager.GAME_CENTER_ACHIEVEMENTS_LOADED, OnAchievementsLoaded);
			GameCenterManager.Dispatcher.addEventListener (GameCenterManager.GAME_CENTER_ACHIEVEMENT_PROGRESS, OnAchievementProgress);
			GameCenterManager.Dispatcher.addEventListener (GameCenterManager.GAME_CENTER_ACHIEVEMENTS_RESET, OnAchievementsReset);
			
			//--
			// Puntos
			//--
			GameCenterManager.Dispatcher.addEventListener (GameCenterManager.GAME_CENTER_LEADERBOARD_SCORE_LOADED, OnLeaderBoarScoreLoaded);
			
			//actions use example
			GameCenterManager.OnPlayerScoreLoaded += OnPlayerScoreLoaded;
			GameCenterManager.OnAuthFinished += OnAuthFinished;
			
			//		DontDestroyOnLoad (gameObject);
			
			//Initializing Game Cneter class. This action will triger authentication flow
			if(showLoginWindowGameServices){
				GameCenterManager.init();
				IsInited = true;
			}
		}
	}
	
	/*--------------------------------
	 * Eventos Game Center
	 -------------------------------*/
	private void OnPlayerScoreLoaded (ISN_PlayerScoreLoadedResult result) {
		if(result.IsSucceeded) {
			//			GCScore score = result.loadedScore;
			//
			//			if(GameSettings.Instance.showTestLogs)
			//				Debug.Log("Leaderboard " + score.leaderboardId + "Score: " + score.score + "\n" + "Rank:" + score.rank);
			//
			////			IOSNativePopUpManager.showMessage("Leaderboard " + score.leaderboardId, "Score: " + score.score + "\n" + "Rank:" + score.rank);
			//			
			////			Debug.Log("double score representation: " + score.GetDoubleScore());
			//
			//			if(GameSettings.Instance.showTestLogs)
			//				Debug.Log("long score representation: " + score.GetLongScore());
			//
			//			string id = GameSettings.Instance.ID_UNIQUE_RANKING;
			//			loadBestScore(id);
			//
			//			if(score.leaderboardId.Equals(leaderBoardId2)) {
			//				Debug.Log("Updating leaderboard 2 score");
			//				long LB2BestScores = score.GetLongScore();
			//				
			//			}
		}
	}
	private void OnLeaderBoarScoreLoaded(CEvent e) {
		ISN_PlayerScoreLoadedResult result = e.data as ISN_PlayerScoreLoadedResult;
		
		if(result.IsSucceeded) {
			GCScore score = result.loadedScore;
			
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("Leaderboard " + score.leaderboardId+ "Score: " + score.score + "\n" + "Rank:" + score.rank);
			
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("long score representation: " + score.GetLongScore());
			
			string id = GameSettings.Instance.ID_UNIQUE_RANKING;
			long scoreLong = score.GetLongScore();
			ScoresHandler.Instance.loadBestScore(id, scoreLong);
		}
	}
	
	void OnAuthFinished (ISN_Result res) {
		
		
		if (res.IsSucceeded) {
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("GCConnect - player connected");
			
			PlayerPrefs.SetInt(GameSettings.PP_LAST_OPENNING_USER_CONNECTED_TO_STORE_SERVICE, 1);
			
			//			IOSNativePopUpManager.showMessage("Player Authed ", "ID: " + GameCenterManager.player.playerId + "\n" + "Alias: " + GameCenterManager.player.alias);
		} else {
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("GCConnect - player connected");
			
			PlayerPrefs.SetInt(GameSettings.PP_LAST_OPENNING_USER_CONNECTED_TO_STORE_SERVICE, 0);
			
			//			IOSNativePopUpManager.showMessage("Game Cneter ", "Player auntification failed");
			//			GameLoaderManager.Instance.GCPrepared = false;
		}
	}
	
	
	private void OnAchievementsLoaded() {
		//
		//
		//		Debug.Log ("Achievemnts was loaded from IOS Game Center");
		//		
		//		foreach(AchievementTemplate tpl in GameCenterManager.achievements) {
		//			Debug.Log (tpl.id + ":  " + tpl.progress);
		//		}
		
		BaseAchievementsManager.Instance.initialCheckingInServerSide();
		
	}
	
	private void OnAchievementsReset() {
		//		Debug.Log ("All  Achievemnts was reseted");
	}
	
	private void OnAchievementProgress(CEvent e) {
		//		Debug.Log ("OnAchievementProgress");
		//		
		//		AchievementTemplate tpl = e.data as AchievementTemplate;
		//		Debug.Log (tpl.id + ":  " + tpl.progress.ToString());
	}
	
	//--------------------------------
	// Eventos Gestor Logros
	//--------------------------------
	void OnAchievementsChecked (){
		achievementsChecked = true;
	}
}
