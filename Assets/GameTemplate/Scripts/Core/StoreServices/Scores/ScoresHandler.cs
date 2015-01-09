using UnityEngine;
using UnionAssets.FLE;
using System.Collections;

public class ScoresHandler : PersistentSingleton<ScoresHandler> {
	private bool jugadorConectado = false;
	private bool mostrandoLogros = false;
	
	/*--------------------------------
	 * Metodos publicos
	 -------------------------------*/
	public void showRanking(GameDifficulty dif = GameDifficulty.NONE){
		if(Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor) return;
		
		#if UNITY_IPHONE
		if(!GameSettings.USE_GAMECENTER)
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
	public void saveScoreOnlyLocally(int puntos){
		int mejor = getBestScore ();
		int puntosEnviar = puntos >= mejor ? puntos : mejor;
		
		//guardamos la mejor puntuacion
		if(puntos > mejor)
			saveBestScore(puntos);
		
		//guardamos la puntuacion ultima
		saveLastScore (puntos);
	}
	
	public void sendScoreToServer(int puntos, bool gpsOGC = true){
		int mejor = getBestScore ();
		int puntosEnviar = puntos >= mejor ? puntos : mejor;
		string id = idRanking (GameController.Instance.Manager.Difficulty);
		
		//		Debug.Log ("Enviando puntuacion: " + puntosEnviar + " a ranking: " + id);
		
		if(GameController.Instance.Manager.Difficulty != GameDifficulty.NONE){
			
			#if UNITY_ANDROID
			if(gpsOGC){
				if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
					if(GooglePlayManager.instance.GetLeaderBoard (id) != null){
						int scoreServidor = GooglePlayManager.instance.GetLeaderBoard (id).GetCurrentPlayerScore(GPBoardTimeSpan.ALL_TIME, GPCollectionType.GLOBAL).rank;
						
						if(scoreServidor < puntosEnviar)
							GooglePlayManager.instance.submitScoreById (id, puntosEnviar);
						else
							saveBestScore(scoreServidor);
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
					saveBestScore(scoreServidor);
			}
			#endif
		}
		
		//		string nivel = PlayerPrefs.GetInt (Configuracion.CLAVE_NIVEL_SELECCIONADO).ToString();
		
		//guardamos la mejor puntuacion
		if(puntos > mejor)
			saveBestScore(puntos);
		
		//guardamos la puntuacion ultima
		saveLastScore (puntos);
	}
	public int getLastScore(){
		string difString = ((int) PlayerPrefs.GetInt(GameSettings.PP_GAME_DIFFICULTY)).ToString();
		string key = GameSettings.PP_LAST_SCORE + difString; //ultima_puntuacion_0 (en facil) , _1 (normal)...
		
		
		return PlayerPrefs.GetInt(key);
	}
	public int getBestScore(){
		string difString = ((int) PlayerPrefs.GetInt(GameSettings.PP_GAME_DIFFICULTY)).ToString();
		string key = GameSettings.PP_BEST_SCORE + difString; //ultima_puntuacion_0 (en facil) , _1 (normal)...
		return PlayerPrefs.GetInt(key);
	}
	
	public void saveLastScore(int puntos){
		string difString = ((int) PlayerPrefs.GetInt(GameSettings.PP_GAME_DIFFICULTY)).ToString();
		string key = GameSettings.PP_LAST_SCORE + difString; //ultima_puntuacion_0 (en facil) , _1 (normal)...
		PlayerPrefs.SetInt(key, puntos); 
	}
	
	public void saveBestScore(int puntos){
		string difString = ((int) PlayerPrefs.GetInt(GameSettings.PP_GAME_DIFFICULTY)).ToString();
		string key = GameSettings.PP_BEST_SCORE + difString; //ultima_puntuacion_0 (en facil) , _1 (normal)...
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
	
	private string idRankingByGameWorld(int world){
		string res = GameSettings.Instance.ID_RANKING_WORLD_1;
		
		switch(world){
		case 1: res = GameSettings.Instance.ID_RANKING_WORLD_1; break;
		case 2: res = GameSettings.Instance.ID_RANKING_WORLD_2; break;
		case 3: res = GameSettings.Instance.ID_RANKING_WORLD_3; break;
		case 4: res = GameSettings.Instance.ID_RANKING_WORLD_4; break;
		case 5: res = GameSettings.Instance.ID_RANKING_WORLD_5; break;
		case 6: res = GameSettings.Instance.ID_RANKING_WORLD_6; break;
		case 7: res = GameSettings.Instance.ID_RANKING_WORLD_7; break;
		case 8: res = GameSettings.Instance.ID_RANKING_WORLD_8; break;
		case 9: res = GameSettings.Instance.ID_RANKING_WORLD_9; break;
		case 10: res = GameSettings.Instance.ID_RANKING_WORLD_10; break;
		}
		
		return res;
	}
	
	private string idRankingBySurvivalLevels(int level){
		string res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_1;
		
		switch(level){
		case 1: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_1; break;
		case 2: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_2; break;
		case 3: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_3; break;
		case 4: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_4; break;
		case 5: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_5; break;
		case 6: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_6; break;
		case 7: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_7; break;
		case 8: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_8; break;
		case 9: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_9; break;
		case 10: res = GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_10; break;
		}
		
		return res;
	}
}
