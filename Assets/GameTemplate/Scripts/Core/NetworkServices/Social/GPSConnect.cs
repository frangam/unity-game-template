using UnityEngine;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;

public class GPSConnect : PersistentSingleton<GPSConnect> {
	private static bool jugadorConectado = false;
	private bool leaderBoardsLoaded = false;
	private bool achievementsLoaded = false;
	private bool achievementsChecked = false; //it has checked if there are achievements that need to be updated in server side beacuse they were unlocked locally
	private bool notifiedLoader = false;
	private bool submittedScoreZeroAtInit = false;
	
	void Update(){
		if(BaseGameScreenController.Instance.Section == GameSection.LOAD_SCREEN && !notifiedLoader && leaderBoardsLoaded && achievementsLoaded && achievementsChecked){
			GameLoaderManager.Instance.GPSPrepared = true;
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
	
	//--------------------------------------
	//  Inicializar
	//--------------------------------------
	public void init (bool showLoginWindowGameServices = true) {
		//listen for GooglePlayConnection events
		GooglePlayConnection.ActionPlayerConnected += OnPlayerConnected;
		GooglePlayConnection.ActionPlayerDisconnected += OnPlayerDisconnected;
		GooglePlayConnection.ActionConnectionResultReceived += ActionConnectionResultReceived;
		
		//listen for GooglePlayManager events
		GooglePlayManager.ActionScoreSubmited += OnScoreSubmited;
		GooglePlayManager.ActionAchievementUpdated += OnAchivmentUpdated;
		
		
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			GTDebug.log("Player connected");
			
			//checking if player already connected
			OnPlayerConnected ();
		} 
		else{
			GTDebug.log("Player not connected. Show Login Window ? " + showLoginWindowGameServices);
			
			
			if(showLoginWindowGameServices){
				GooglePlayConnection.instance.connect (); //conectar
			}
		}
	}
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------
	private void OnDestroy() {
		if(!GooglePlayConnection.IsDestroyed) {
			GTDebug.log("Destroying events");
			GooglePlayConnection.ActionPlayerConnected -= OnPlayerConnected;
			GooglePlayConnection.ActionPlayerDisconnected -= OnPlayerDisconnected;
			GooglePlayConnection.ActionConnectionResultReceived -= ActionConnectionResultReceived;
		}
		
		if(!GooglePlayManager.IsDestroyed) {
			GooglePlayManager.ActionAchievementUpdated -= OnAchivmentUpdated;
			GooglePlayManager.ActionScoreSubmited -= OnScoreSubmited;
		}
	}
	/*--------------------------------
	 * Eventos Google Play Services
	 -------------------------------*/
	private void ActionConnectionResultReceived(GooglePlayConnectionResult result) {
		
		if(result.IsSuccess) {
			GTDebug.log("Connected! ActionConnectionResultReceived");
			
			OnConnectionEstablished();
		} else {
			GTDebug.log("Connection failed with code: " + result.code.ToString());
		}
	}
	
	//	private void ActionAvailableDeviceAccountsLoaded(List<string> accounts) {
	//		string msg = "Device contains following google accounts:" + "\n";
	//		foreach(string acc in GooglePlayManager.instance.deviceGoogleAccountList) {
	//			msg += acc + "\n";
	//		} 
	//		
	//		AndroidDialog dialog = AndroidDialog.Create("Accounts Loaded", msg, "Sign With Fitst one", "Do Nothing");
	//		dialog.ActionComplete += SighDialogComplete;
	//		
	//	}
	private void SighDialogComplete (AndroidDialogResult res) {
		if(res == AndroidDialogResult.YES) {
			GooglePlayConnection.instance.connect(GooglePlayManager.instance.deviceGoogleAccountList[0]);
		}
		
	}
	
	private void OnScoreSubmited(GooglePlayResult result) {
		//		GooglePlayResult result = e.data as GooglePlayResult;
		//		AndroidNative.showMessage ("OnScoreSubmited", result.message);
		
		
		//try to load scores if we submitted a dummy zero score previously
		if(submittedScoreZeroAtInit){
			GTDebug.log("Dummy zero score submitted");
			
			submittedScoreZeroAtInit = false;
			loadLeaderBoards();
		}
		
		GooglePlayManager.instance.LoadPlayerCenteredScores(GameSettings.Instance.CurrentUniqueRankingID, GPBoardTimeSpan.ALL_TIME, GPCollectionType.GLOBAL, 25); //25 is the maximum number of scores to fetch per page
	}
	
	private void OnPlayerInfoLoaded(CEvent e) {
		GooglePlayResult result = e.data as GooglePlayResult;
		
		if(result.isSuccess) {
			jugadorConectado = true;
			
			//			Arranque.Instancia.GpsConectado = true;
		} 
		else {
			jugadorConectado = false;
			
			if(BaseGameScreenController.Instance.Section == GameSection.LOAD_SCREEN)
				GameLoaderManager.Instance.GPSPrepared = false;
		}
	}
	
	private void OnPlayerDisconnected() {
		GTDebug.log("Player disconnected");
		
		PlayerPrefs.SetInt(GameSettings.PP_LAST_OPENNING_USER_CONNECTED_TO_STORE_SERVICE, 0);
		
		jugadorConectado = false;
	}
	
	private void OnPlayerConnected() {
		//		GTDebug.log("Player connected");
	}
	
	private void OnConnectionEstablished(){
		PlayerPrefs.SetInt(GameSettings.PP_LAST_OPENNING_USER_CONNECTED_TO_STORE_SERVICE, 1);
		
		GooglePlayManager.instance.LoadConnectedPlayers ();
		
		if(GameSettings.Instance.CurrentScores != null && GameSettings.Instance.CurrentScores.Count > 0)
			loadLeaderBoards ();
		
		if(GameSettings.Instance.CurrentAchievements != null && GameSettings.Instance.CurrentAchievements.Count > 0)
			loadAchievements ();
	}
	
	private void OnAchivmentUpdated(GP_GamesResult result) {
		
	}
	
	private void loadLeaderBoards() {
		GTDebug.log("Loading leaderboards");
		
		//listening for load event 
		GooglePlayManager.ActionLeaderboardsLoaded += OnLeaderBoardsLoaded;
		GooglePlayManager.instance.LoadLeaderBoards ();
		
	}
	
	private void loadAchievements() {
		GooglePlayManager.ActionAchievementsLoaded += OnAchivmentsLoaded;
		GooglePlayManager.instance.LoadAchievements ();
	}
	
	private void OnAchivmentsLoaded(GooglePlayResult result) {
		GooglePlayManager.ActionAchievementsLoaded -= OnAchivmentsLoaded;
		
		if(result.isSuccess){
			achievementsLoaded = true;
			BaseAchievementsManager.Instance.initialCheckingInServerSide();
		} 
	}
	
	private void OnLeaderBoardsLoaded(GooglePlayResult result) {
		GooglePlayManager.ActionLeaderboardsLoaded -= OnLeaderBoardsLoaded;
		
		GTDebug.log("Leader boards loaded result success: " +result.isSuccess + ". Result code: " +result.response);
		
		if(result.isSuccess) {
			initialLeaderboardsLoad(new Score(GameSettings.Instance.CurrentUniqueRankingID));
			initialLeaderboardsLoad(new Score(GameSettings.Instance.CurrentUniqueSurvivalRankingID));
			
			if(GameSettings.Instance.CurrentScores != null && GameSettings.Instance.CurrentScores.Count > 0){
				foreach(Score score in GameSettings.Instance.CurrentScores){
					initialLeaderboardsLoad(score);
				}
			}
		} else {
			GTDebug.log("Leader-Boards Loaded error: "+ result.message);
		}
	}
	
	private void initialLeaderboardsLoad(Score score){
		if(score == null || (score != null && string.IsNullOrEmpty(score.Id))) return;
		
		GooglePlayManager.instance.LoadPlayerCenteredScores(score.Id, GPBoardTimeSpan.ALL_TIME, GPCollectionType.GLOBAL, 25); //25 is the maximum number of scores to fetch per page
		GPLeaderBoard leaderboard = GooglePlayManager.instance.GetLeaderBoard(score.Id);
		
		if( leaderboard == null) {
			GTDebug.log("Leader boards loaded " + score.Id + " not found in leader boards list");
			return;
		}
		
		
		GPScore gpScore = leaderboard.GetCurrentPlayerScore(GPBoardTimeSpan.ALL_TIME, GPCollectionType.GLOBAL);
		long scoreValue = gpScore.score;
		
		//a tricky to get a previous score
		//<0 means not configured good
		if(scoreValue < 0){
			//				ScoresHandler.Instance.showRanking(id);
			//			GTDebug.log("Submitting dummy zero score to store because score value from store is "+scoreValue);
			
			//			submittedScoreZeroAtInit = true;
			//			ScoresHandler.Instance.sendScoreToServerByID(score.Id, 0);
			
			GTDebug.log("Leader board id " + score.Id + " current score: " +gpScore.score);
		}
		else{
			GTDebug.log("Leader board id " + score.Id + " current player "+gpScore.playerId + " rank: " +gpScore.rank + " score value: " + gpScore.score);
			
			ScoresHandler.Instance.loadBestScoreFromStore(score.Id, scoreValue);
		}
	}
	
	
	
	
	//--------------------------------
	// Eventos Gestor Logros
	//--------------------------------
	void OnAchievementsChecked (){
		achievementsChecked = true;
	}
}
