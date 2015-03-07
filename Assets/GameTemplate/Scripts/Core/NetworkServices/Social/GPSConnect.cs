using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class GPSConnect : Singleton<GPSConnect> {
	private static bool jugadorConectado = false;
	private bool leaderBoardsLoaded = false;
	private bool achievementsLoaded = false;
	private bool achievementsChecked = false; //it has checked if there are achievements that need to be updated in server side beacuse they were unlocked locally
	private bool notifiedLoader = false;
	
	
	void Update(){
		if(!notifiedLoader && leaderBoardsLoaded && achievementsLoaded && achievementsChecked){
			GameLoaderManager.Instance.GPSPrepared = true;
			notifiedLoader = true;
		}
	}
	
	void OnEnable(){
		BaseAchievementsManager.dispatcher.addEventListener(BaseAchievementsManager.ACHIEVEMENTS_INITIAL_CHEKING, OnAchievementsChecked);			
	}
	
	void OnDisable(){
		BaseAchievementsManager.dispatcher.removeEventListener(BaseAchievementsManager.ACHIEVEMENTS_INITIAL_CHEKING, OnAchievementsChecked);	
	}
	
	//--------------------------------------
	//  Inicializar
	//--------------------------------------
	public void init (bool showLoginWindowGameServices = true) {
		//		//listen for GooglePlayConnection events
		//		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
		//		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
		//
		//
		//		//listen for GooglePlayManager events
		//		GooglePlayManager.instance.addEventListener (GooglePlayManager.ACHIEVEMENT_UPDATED, OnAchivmentUpdated);
		//		GooglePlayManager.instance.addEventListener (GooglePlayManager.SCORE_SUBMITED, OnScoreSubmited);
		//		
		//		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
		//			//checking if player already connected
		//			OnPlayerConnected ();
		//		} 
		//		else{
		//			GooglePlayConnection.instance.connect (); //conectar
		//		}
		
		
		
		//listen for GooglePlayConnection events
		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
		
		
		//		GooglePlayConnection.ActionConnectionResultReceived += ActionConnectionResultReceived;
		
		
		
		//listen for GooglePlayManager events
		GooglePlayManager.instance.addEventListener (GooglePlayManager.ACHIEVEMENT_UPDATED, OnAchivmentUpdated);
		GooglePlayManager.instance.addEventListener (GooglePlayManager.SCORE_SUBMITED, OnScoreSubmited);
		//		GooglePlayManager.instance.addEventListener (GooglePlayManager.SCORE_REQUEST_RECEIVED, OnScoreUpdated);
		
		
		
		//		GooglePlayManager.instance.addEventListener (GooglePlayManager.SEND_GIFT_RESULT_RECEIVED, OnGiftResult);
		//		GooglePlayManager.instance.addEventListener (GooglePlayManager.PENDING_GAME_REQUESTS_DETECTED, OnPendingGiftsDetected);
		//		GooglePlayManager.instance.addEventListener (GooglePlayManager.GAME_REQUESTS_ACCEPTED, OnGameRequestAccepted);
		
		//		GooglePlayManager.ActionOAuthTockenLoaded += ActionOAuthTockenLoaded;
		//		GooglePlayManager.ActionAvaliableDeviceAccountsLoaded += ActionAvaliableDeviceAccountsLoaded;
		//		
		//		GooglePlayManager.instance.addEventListener (GooglePlayManager.ACHIEVEMENTS_LOADED, OnAchievmnetsLoadedInfoListner);
		
		
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("GPSConnect - connected");
			
			//checking if player already connected
			OnPlayerConnected ();
		} 
		else{
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("GPSConnect - not connected. Show Login Window ? " + showLoginWindowGameServices);
			
			
			if(showLoginWindowGameServices){
				GooglePlayConnection.instance.connect (); //conectar
			}
		}
	}
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------
	//	void OnDestroy() {
	//		if(!GooglePlayConnection.IsDestroyed) {
	//			GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
	//			GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
	//			
	//		}
	//		
	//		if(!GooglePlayManager.IsDestroyed) {
	//			GooglePlayManager.instance.removeEventListener (GooglePlayManager.ACHIEVEMENT_UPDATED, OnAchivmentUpdated);
	////			GooglePlayManager.instance.removeEventListener (GooglePlayManager.PLAYER_LOADED, OnPlayerInfoLoaded);
	//			GooglePlayManager.instance.removeEventListener (GooglePlayManager.SCORE_SUBMITED, OnScoreSubmited);
	//		}
	//	}
	private void OnDestroy() {
		if(!GooglePlayConnection.IsDestroyed) {
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("GPSConnect - destroying events");
			
			GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
			GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
			
			
			//			GooglePlayConnection.ActionConnectionResultReceived -= ActionConnectionResultReceived;
		}
		
		if(!GooglePlayManager.IsDestroyed) {
			GooglePlayManager.instance.removeEventListener (GooglePlayManager.ACHIEVEMENT_UPDATED, OnAchivmentUpdated);
			GooglePlayManager.instance.removeEventListener (GooglePlayManager.SCORE_SUBMITED, OnScoreSubmited);
			
			//			GooglePlayManager.instance.removeEventListener (GooglePlayManager.SEND_GIFT_RESULT_RECEIVED, OnGiftResult);
			//			GooglePlayManager.instance.removeEventListener (GooglePlayManager.PENDING_GAME_REQUESTS_DETECTED, OnPendingGiftsDetected);
			//			GooglePlayManager.instance.removeEventListener (GooglePlayManager.GAME_REQUESTS_ACCEPTED, OnGameRequestAccepted);
			
			//			GooglePlayManager.ActionAvaliableDeviceAccountsLoaded -= ActionAvaliableDeviceAccountsLoaded;
			//			GooglePlayManager.ActionOAuthTockenLoaded -= ActionOAuthTockenLoaded;
			
			
			
			//			GooglePlayManager.instance.removeEventListener (GooglePlayManager.ACHIEVEMENTS_LOADED, OnAchievmnetsLoadedInfoListner);
		}
	}
	/*--------------------------------
	 * Eventos Google Play Services
	 -------------------------------*/
	
	private void OnScoreSubmited(CEvent e) {
		//		GooglePlayResult result = e.data as GooglePlayResult;
		//		AndroidNative.showMessage ("OnScoreSubmited", result.message);
		
	}
	
	private void OnPlayerInfoLoaded(CEvent e) {
		GooglePlayResult result = e.data as GooglePlayResult;
		
		if(result.isSuccess) {
			jugadorConectado = true;
			
			//			Arranque.Instancia.GpsConectado = true;
		} 
		else {
			jugadorConectado = false;
			GameLoaderManager.Instance.GPSPrepared = false;
		}
	}
	
	private void OnPlayerDisconnected() {
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("GPSConnect - player connected");
		
		PlayerPrefs.SetInt(GameSettings.PP_LAST_OPENNING_USER_CONNECTED_TO_STORE_SERVICE, 0);
		
		jugadorConectado = false;
	}
	
	private void OnPlayerConnected() {
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("GPSConnect - player connected");
		
		PlayerPrefs.SetInt(GameSettings.PP_LAST_OPENNING_USER_CONNECTED_TO_STORE_SERVICE, 1);
		
		GooglePlayManager.instance.LoadConnectedPlayers ();
		
		loadLeaderBoards ();
		loadAchievements ();
	}
	
	private void OnAchivmentUpdated(CEvent e) {
		//		GooglePlayResult result = e.data as GooglePlayResult;
		//		AndroidNative.showMessage ("OnAchivmentUpdated ", "Id: " + result.achievementId + "\n status: " + result.message);
	}
	
	private void loadLeaderBoards() {
		
		//listening for load event 
		GooglePlayManager.instance.addEventListener (GooglePlayManager.LEADERBOARDS_LOADED, OnLeaderBoardsLoaded);
		GooglePlayManager.instance.LoadLeaderBoards ();
	}
	
	private void loadAchievements() {
		GooglePlayManager.instance.addEventListener (GooglePlayManager.ACHIEVEMENTS_LOADED, OnAchivmentsLoaded);
		GooglePlayManager.instance.LoadAchievements ();
	}
	
	
	private void OnLeaderBoardsLoaded(CEvent e) {
		GooglePlayManager.instance.removeEventListener (GooglePlayManager.LEADERBOARDS_LOADED, OnLeaderBoardsLoaded);
	}
	
	private void OnAchivmentsLoaded(CEvent e) {
		GooglePlayManager.instance.removeEventListener (GooglePlayManager.ACHIEVEMENTS_LOADED, OnAchivmentsLoaded);
		GooglePlayResult result = e.data as GooglePlayResult;
		
		if(result.isSuccess){
			achievementsLoaded = true;
			BaseAchievementsManager.Instance.initialCheckingInServerSide();
		} 
	}
	
	//--------------------------------
	// Eventos Gestor Logros
	//--------------------------------
	void OnAchievementsChecked (){
		achievementsChecked = true;
	}
}
