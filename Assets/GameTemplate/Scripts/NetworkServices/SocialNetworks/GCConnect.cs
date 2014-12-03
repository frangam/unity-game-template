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
			LoadManager.Instance.GCPrepared = true;
			notifiedLoader = true;
		}
	}
	
	void OnEnable(){
		AchievementsHandler.OnAchievementsChecked += OnAchievementsChecked;		
	}
	
	void OnDisable(){
		AchievementsHandler.OnAchievementsChecked -= OnAchievementsChecked;	
	}

	public void init () {


		if(!IsInited) {


			//--
			// Logros
			//--
			//Achievement registration. If you will skipt this step GameCenterManager.achievements array will contain only achievements with reported progress 

			foreach(string id in AchievementsHandler.Instance.Ids){
				GameCenterManager.registerAchievement (id);
			}

			
			
			//Listen for the Game Center events
			GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_ACHIEVEMENTS_LOADED, OnAchievementsLoaded);
			GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_ACHIEVEMENT_PROGRESS, OnAchievementProgress);
			GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_ACHIEVEMENTS_RESET, OnAchievementsReset);

			//--
			// Puntos
			//--
			GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_LEADER_BOARD_SCORE_LOADED, OnLeaderBoarScoreLoaded);


			GameCenterManager.OnAuthFinished += OnAuthFinished;

	//		DontDestroyOnLoad (gameObject);

			//Initializing Game Cneter class. This action will triger authentication flow
			GameCenterManager.init();
			IsInited = true;
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
//			IOSNativePopUpManager.showMessage("Player Authed ", "ID: " + GameCenterManager.player.playerId + "\n" + "Alias: " + GameCenterManager.player.alias);
		} else {
//			IOSNativePopUpManager.showMessage("Game Cneter ", "Player auntification failed");
			LoadManager.Instance.GCPrepared = false;
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
