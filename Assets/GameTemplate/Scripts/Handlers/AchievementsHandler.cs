using UnityEngine;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;

public class AchievementsHandler : Singleton<AchievementsHandler> {
	public delegate void achievementsChecked();
	public static event achievementsChecked OnAchievementsChecked;


	public enum TipoLogro{
		PARTIDAS_JUGADAS 							= 1,
		COMPRAS_REALIZADAS 							= 2,


	}

	//--------------------------------------
	// Setting Attributes
	//--------------------------------------


	/*--------------------------------
	 * Achievements ids
	 -------------------------------*/

	public const string PARTIDA_1 							= "CgkI_bDBmfcREAIQBA";	//Jugar 1 partida


	private List<string> ids;

	public List<string> Ids {
		get {
			return this.ids;
		}
	}
	
	/*--------------------------------
	 * Metodos de Unity
	 -------------------------------*/
	#region Unity
	void Awake() {
		ids = new List<string> ();
		ids.Add (PARTIDA_1);

	}
	#endregion

	public void mostrarLogros(){
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
		else if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
			GooglePlayManager.instance.showAchievementsUI();
		}
		#elif UNITY_IPHONE
		if(Configuracion.USAR_GAMECENTER && GameCenterManager.IsPlayerAuthed)
			GameCenterManager.showAchievements();
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


	public void comprobarLogros(int puntos, TipoLogro tipo){
		if(puntos <= 0 && (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)) return;


		switch(tipo){
			//--
			//logros de num partidas jugadas
			//--
			case TipoLogro.PARTIDAS_JUGADAS:
				comprobarLogrosTotalPartidasJugadas(puntos);
			break;
		}
	}

	private void comprobarLogrosTotalPartidasJugadas(int puntos){
		if(PlayerPrefs.GetInt(PARTIDA_1) != 1 && puntos >= ConfigAchievements.PROGRESO_GET_1 && puntos < ConfigAchievements.PROGRESO_GET_150_PUNTOS){
			desbloquearLogro( PARTIDA_1);
		}
	}

	public bool desbloquearLogro(string idLogro){
		//si no se habia desbloqueado previamente, lo anotamos en la memoria
		if(PlayerPrefs.GetInt(idLogro) != 1)
			PlayerPrefs.SetInt(idLogro, 1);

		#if UNITY_ANDROID
		//si el logro no se ha conseguido
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED && GooglePlayManager.instance.GetAchievement(idLogro).state != GPAchievementState.STATE_UNLOCKED){
			GooglePlayManager.instance.reportAchievementById(idLogro);
			return true;
		}
		#elif UNITY_IPHONE
		if(GameCenterManager.IsPlayerAuthed && GameCenterManager.getAchievementProgress(idLogro) < 100){ //Menor al 100%
			GameCenterManager.submitAchievement(100, idLogro); //Completamos con el 100% del progreso
			return true;
		}
		#endif

		return false;
	}

	/// <summary>
	/// Desbloquea logros que se consiguieron y no estan desbloqueados en el servidor
	/// </summary>
	public void comprobacionInicial(){
		#if UNITY_ANDROID
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
			foreach(string id in ids){
				if(PlayerPrefs.GetInt(id) == 1){
					GPAchievement logro = GooglePlayManager.instance.GetAchievement(id);

					if(logro.state != GPAchievementState.STATE_UNLOCKED){
						desbloquearLogro(id);
					}
				}
			}		
		}

		#elif UNITY_IPHONE
		if(GameCenterManager.IsPlayerAuthed){
			foreach(string id in ids){
				if(PlayerPrefs.GetInt(id) == 1){
					if(GameCenterManager.getAchievementProgress(id) < 100){
						desbloquearLogro(id);
					}
				}
			}
		}
		#endif
	}
}
