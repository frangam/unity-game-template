using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class GCConnect : Singleton<GCConnect> {
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
			
			
			GameCenterManager.OnAuthFinished += OnAuthFinished;
			
			//		DontDestroyOnLoad (gameObject);
			
			//Initializing Game Cneter class. This action will triger authentication flow
			if(showLoginWindowGameServices){
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("GCConnect - showing authentication flow of Game Center");
				
				GameCenterManager.init();
				IsInited = true;
			}
			else{
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("GCConnect - NOT showing authentication flow of Game Center");
			}
		}
	}
	
	/*--------------------------------
	 * Eventos Game Center
	 -------------------------------*/
	private void OnLeaderBoarScoreLoaded(CEvent e) {
		//		LeaderBoardScoreData data = e.data as LeaderBoardScoreData;
		//		IOSNative.showMessage("Leader Board " + data.leaderBoardId, "Score: " + data.leaderBoardScore);
	}
	
	void OnAuthFinished (ISN_Result res) {
		
		
		if (res.IsSucceeded) {
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("GCConnect - player connected");
			
			PlayerPrefs.SetInt(GameSettings.PP_LAST_OPENNING_USER_CONNECTED_TO_STORE_SERVICE, 1);
			
			//			IOSNativePopUpManager.showMessage("Player Authed ", "ID: " + GameCenterManager.player.playerId + "\n" + "Alias: " + GameCenterManager.player.alias);
		} else {
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("GCConnect - player NOT connected");
			
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
