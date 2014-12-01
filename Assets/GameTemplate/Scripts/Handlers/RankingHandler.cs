using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class RankingHandler : Singleton<RankingHandler> {
//	private const string LEADERBOARD_ANDROID_NAME = "Ranking"; //"Ranking";
//	private const string LEADERBOARD_ANDROID_ID = "CgkIr4_0jJcUEAIQEg";//"CgkI1Oek_4cXEAIQBg";
//	private string leaderIOSBoardId =  "CgkIr4_0jJcUEAIQEg";//"CgkI1Oek_4cXEAIQBg";

	private bool jugadorConectado = false;
	private bool mostrandoLogros = false;

	/*--------------------------------
	 * Metodos de Unity
	 -------------------------------*/
	#region Unity

	void Start() {
//		DontDestroyOnLoad(gameObject);
//
//		#if UNITY_ANDROID
//		//listen for GooglePlayConnection events
//		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
//		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
//
//		GooglePlayConnection.instance.addEventListener (GooglePlayConnection.CONNECTION_INITIALIZED, OnConnect);
//		
//		//listen for GooglePlayManager events
//		GooglePlayManager.instance.addEventListener (GooglePlayManager.PLAYER_LOADED, OnPlayerInfoLoaded);
//		GooglePlayManager.instance.addEventListener (GooglePlayManager.SCORE_SUBMITED, OnScoreSubmited);
//		
//		
//		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
//			//checking if player already connected
//			OnPlayerConnected ();
//		} 
////		else{
////			GooglePlayConnection.instance.connect (); //conectar
////		}
//		
//		
//		//should be called on your application start 
//		//best practice to call it only once. Any way other calls will be ignored by the plugin.
//		//if you want to use only Game service (Leader-boards, achievements) use GooglePlayConnection.CLIENT_GAMES
//		//if you want to use only Google Cloud service use GooglePlayConnection.CLIENT_APPSTATE
//		//if you want both: GooglePlayConnection.CLIENT_GAMES | GooglePlayConnection.CLIENT_APPSTATE
//		//and if you whant to get avaliable permissions for your app use: GooglePlayConnection.CLIENT_ALL
//		GooglePlayConnection.instance.start (GooglePlayConnection.CLIENT_GAMES );
//
//		#elif UNITY_IPHONE
//		GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_LEADER_BOARD_SCORE_LOADED, OnLeaderBoarScoreLoaded);
//		
//		GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_PLAYER_AUTHENTICATED, OnAuth);
//		GameCenterManager.dispatcher.addEventListener (GameCenterManager.GAME_CENTER_PLAYER_AUTHENTIFICATION_FAILED, OnAuthFailed);
//
//		//Initializing Game Cneter class. This action will triger authentication flow
//		GameCenterManager.init();
//		#endif
	}
//
//	//--------------------------------------
//	//  DESTROY
//	//--------------------------------------
//	void OnDestroy() {
//		#if UNITY_ANDROID
//		if(!GooglePlayConnection.IsDestroyed) {
//			GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
//			GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
//			
//		}
//		
//		if(!GooglePlayManager.IsDestroyed) {
//			GooglePlayManager.instance.removeEventListener (GooglePlayManager.PLAYER_LOADED, OnPlayerInfoLoaded);
//			GooglePlayManager.instance.removeEventListener (GooglePlayManager.SCORE_SUBMITED, OnScoreSubmited);
//		}
//		#endif
//	}
	#endregion
//
//	/*--------------------------------
//	 * Eventos Google Play Services
//	 -------------------------------*/
//	#if UNITY_ANDROID
//	private void OnConnect() {
//		GooglePlayConnection.instance.connect ();
//	}
//
//	private void OnScoreSubmited(CEvent e) {
////		GooglePlayResult result = e.data as GooglePlayResult;
////		AndroidNative.showMessage ("OnScoreSubmited", result.message);
//
//	}
//	
//	private void OnPlayerInfoLoaded(CEvent e) {
//		GooglePlayResult result = e.data as GooglePlayResult;
//		
//		if(result.isSuccess) {
//			jugadorConectado = true;
//		} 
//		else {
//			jugadorConectado = false;
//		}
//	}
//	
//	private void OnPlayerDisconnected() {
//		jugadorConectado = false;
//	}
//	
//	private void OnPlayerConnected() {
//		GooglePlayManager.instance.loadPlayer ();
//	}
//
////	private void OnLeaderBoardsLoaded(CEvent e) {
////		GooglePlayManager.instance.removeEventListener (GooglePlayManager.LEADERBOARDS_LOEADED, OnLeaderBoardsLoaded);
////		
////		GooglePlayResult result = e.data as GooglePlayResult;
////		if(result.isSuccess) {
////			if( GooglePlayManager.instance.GetLeaderBoard(LEADERBOARD_ANDROID_ID) == null) {
////				AndroidNative.showMessage("Leader boards loaded", LEADERBOARD_ANDROID_ID + " not found in leader boards list");
////				return;
////			}
////			
////			
////			AndroidNative.showMessage (LEADERBOARD_ANDROID_NAME + "  score",  GooglePlayManager.instance.GetLeaderBoard(LEADERBOARD_ANDROID_ID).GetScore(GPCollectionType.COLLECTION_PUBLIC, GPBoardTimeSpan.TIME_SPAN_ALL_TIME).ToString());
////		} 
////		else {
////			AndroidNative.showMessage ("OnLeaderBoardsLoaded error: ", result.message);
////		}
////	}
//	#elif UNITY_IPHONE
//	/*--------------------------------
//	 * Eventos Game Center
//	 -------------------------------*/
//	private void OnLeaderBoarScoreLoaded(CEvent e) {
////		LeaderBoardScoreData data = e.data as LeaderBoardScoreData;
////		IOSNative.showMessage("Leader Board " + data.leaderBoardId, "Score: " + data.leaderBoardScore);
//	}
//	
//	
//	private void OnAuth() {
////		IOSNative.showMessage("Player Authed ", "ID: " + GameCenterManager.player.playerId + "\n" + "Name: " + GameCenterManager.player.displayName);
//	}
//	
//	private void OnAuthFailed() {
//		IOSNative.showMessage("Game Cneter ", "Player auntification failed");
//		
//		//if you got this event it means that player canseled auntification flow. With probably mean that playr do not whant to use gamcenter in your game
//		
//		
//	}
//	#endif

	/*--------------------------------
	 * Metodos publicos
	 -------------------------------*/
	public void mostrarClasificacionGeneral(GameDifficulty dif = GameDifficulty.NONE){
//		GameDifficulty dif = (GameDifficulty) PlayerPrefs.GetInt (Configuracion.CLAVE_DIFICULTAD_JUEGO);

		if(Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor) return;
		
		#if UNITY_IPHONE
		if(!Configuracion.USAR_GAMECENTER)
			return;
		#elif UNITY_ANDROID
		if(!Configuration.USE_GOOGLE_PLAY_SERVICES)
			return;
		#endif


		#if UNITY_ANDROID
		if(GooglePlayConnection.state == GPConnectionState.STATE_UNCONFIGURED){
			GPSConnect.Instance.init();
			
			AndroidDialog dialog = AndroidDialog.Create(Localization.Localize(ExtraLocalizations.TITULO_POPUP_LOGIN_GPS)
			                                            , Localization.Localize(ExtraLocalizations.DESC_POPUP_LOGIN_GPS)
			                                            , Localization.Localize(ExtraLocalizations.BOTON_OK_POPUP_LOGIN_GPS)
			                                            , Localization.Localize(ExtraLocalizations.BOTON_CANCEL_POPUP_LOGIN_GPS));
			dialog.addEventListener(BaseEvent.COMPLETE, OnDialogClose);
		}
		else if(GooglePlayConnection.state == GPConnectionState.STATE_DISCONNECTED){
			AndroidDialog dialog = AndroidDialog.Create(Localization.Localize(ExtraLocalizations.TITULO_POPUP_LOGIN_GPS)
			                                            , Localization.Localize(ExtraLocalizations.DESC_POPUP_LOGIN_GPS)
			                                            , Localization.Localize(ExtraLocalizations.BOTON_OK_POPUP_LOGIN_GPS)
			                                            , Localization.Localize(ExtraLocalizations.BOTON_CANCEL_POPUP_LOGIN_GPS));
			dialog.addEventListener(BaseEvent.COMPLETE, OnDialogClose);
		}
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
			if(dif == GameDifficulty.NONE)
				GooglePlayManager.instance.showLeaderBoardsUI();
			else
				GooglePlayManager.instance.showLeaderBoardById(idRanking(dif));

		}

		#elif UNITY_IPHONE
		string id = idRanking(dif).Replace("-","_");

		if(dif == GameDifficulty.NONE)
			GameCenterManager.showLeaderBoards();
		else
			GameCenterManager.showLeaderBoard(id);
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
	public void enviarPuntuacion(int puntos, bool gpsOGC = true){
		int mejor = mejorPuntuacion ();
		int puntosEnviar = puntos >= mejor ? puntos : mejor;
		string id = idRanking (GameManager.Instance.Difficulty);



//		Debug.Log ("Enviando puntuacion: " + puntosEnviar + " a ranking: " + id);

		if(GameManager.Instance.Difficulty != GameDifficulty.NONE){

			#if UNITY_ANDROID
			if(gpsOGC){
				if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
					if(GooglePlayManager.instance.GetLeaderBoard (id) != null){
						int scoreServidor = GooglePlayManager.instance.GetLeaderBoard (id).GetCurrentPlayerScore(GPBoardTimeSpan.ALL_TIME, GPCollectionType.GLOBAL).rank;

						if(scoreServidor < puntosEnviar)
							GooglePlayManager.instance.submitScoreById (id, puntosEnviar);
						else
							guardarMejorPuntuacion(scoreServidor);
					}
					else{
						GooglePlayManager.instance.submitScoreById (id, puntosEnviar);
					}
				}


			}
			#elif UNITY_IPHONE
			if(gpsOGC && GameCenterManager.IsPlayerAuthed){
				id = id.Replace("-","_");
				int scoreServidor = GameCenterManager.GetLeaderBoard(id).GetCurrentPlayerScore(GCBoardTimeSpan.ALL_TIME, GCCollectionType.GLOBAL).rank;
				if(scoreServidor < puntosEnviar)
					GameCenterManager.reportScore(puntosEnviar, id);
				else
					guardarMejorPuntuacion(scoreServidor);
			}
			#endif
		}

//		string nivel = PlayerPrefs.GetInt (Configuracion.CLAVE_NIVEL_SELECCIONADO).ToString();

		//guardamos la mejor puntuacion
		if(puntos > mejor)
			guardarMejorPuntuacion(puntos);

		//guardamos la puntuacion ultima
		guardarUltimaPuntuacion (puntos);
	}
	public int ultimaPuntuacion(){
		string difString = ((int) PlayerPrefs.GetInt(Configuration.PP_GAME_DIFFICULTY)).ToString();
		string key = Configuration.PP_LAST_SCORE + difString; //ultima_puntuacion_0 (en facil) , _1 (normal)...


		return PlayerPrefs.GetInt(key);
	}
	public int mejorPuntuacion(){
		string difString = ((int) PlayerPrefs.GetInt(Configuration.PP_GAME_DIFFICULTY)).ToString();
		string key = Configuration.PP_BEST_SCORE + difString; //ultima_puntuacion_0 (en facil) , _1 (normal)...
		return PlayerPrefs.GetInt(key);
	}

	public void guardarUltimaPuntuacion(int puntos){
		string difString = ((int) PlayerPrefs.GetInt(Configuration.PP_GAME_DIFFICULTY)).ToString();
		string key = Configuration.PP_LAST_SCORE + difString; //ultima_puntuacion_0 (en facil) , _1 (normal)...
		PlayerPrefs.SetInt(key, puntos); 
	}

	public void guardarMejorPuntuacion(int puntos){
		string difString = ((int) PlayerPrefs.GetInt(Configuration.PP_GAME_DIFFICULTY)).ToString();
		string key = Configuration.PP_BEST_SCORE + difString; //ultima_puntuacion_0 (en facil) , _1 (normal)...
		PlayerPrefs.SetInt(key, puntos); 
	}

//	private string nombreRanking(){
//		GameDifficulty dif = (GameDifficulty) PlayerPrefs.GetInt (Configuracion.CLAVE_DIFICULTAD_JUEGO);
//		string res = Configuracion.RANKING_EASY;
//
//		switch(dif){
//		case GameDifficulty.EASY:
//			res = Configuracion.RANKING_EASY;
//			break;
//
//		case GameDifficulty.NORMAL:
//			res = Configuracion.RANKING_NORMAL;
//			break;
//
//		case GameDifficulty.HARD:
//			res = Configuracion.RANKING_HARD;
//			break;
//
//		case GameDifficulty.PRO:
//			res = Configuracion.RANKING_PRO;
//			break;
//
//		case GameDifficulty.GOD:
//			res = Configuracion.RANKING_GOD;
//			break;
//		}
//
//		return res;
//	}

	private string idRanking(GameDifficulty dif){
		string res = Configuration.ID_RANKING_FACIL;
		
		switch(dif){
		case GameDifficulty.EASY:
			res = Configuration.ID_RANKING_FACIL;
			break;
			
		case GameDifficulty.NORMAL:
			res = Configuration.ID_RANKING_NORMAL;
			break;
			
		case GameDifficulty.HARD:
			res = Configuration.ID_RANKING_DIFICIL;
			break;
			
		case GameDifficulty.PRO:
			res = Configuration.ID_RANKING_PRO;
			break;
			
		case GameDifficulty.GOD:
			res = Configuration.ID_RANKING_GOD;
			break;
		}
		
		return res;
	}
}
