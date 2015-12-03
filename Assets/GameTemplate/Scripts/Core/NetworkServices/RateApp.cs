/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System;
using System.Collections;


public class RateApp : Singleton<RateApp>{
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private double timeRememberInHours = 3;
	
	//Playerprefs keys
	private const string PP_RATE_DONE = "pr_rate_done";
	private const string PP_RATE_LATER = "pr_rate_later";
	
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Start(){
		//we ask for rate after player plays its first game opening
		if (PlayerPrefs.GetInt(GameSettings.PP_TOTAL_GAME_OPENINGS) > 2){
			if (!PlayerPrefs.HasKey(PP_RATE_DONE) || PlayerPrefs.GetInt(PP_RATE_DONE) == 0){
				if (!PlayerPrefs.HasKey(PP_RATE_LATER)){
					rate();
				}
				else{
					DateTime tiempoActual = DateTime.Now;
					
					//obtenemos el tiempo desde que se pulso en recordar mas tarde
					long temp = Convert.ToInt64(PlayerPrefs.GetString(PP_RATE_LATER));
					
					//convertimos a DateTime el tiempo de vida long
					DateTime tiempoDesdeRecordarMasTarde = DateTime.FromBinary(temp);
					//		Debug.Log("tiempo desde que pulso en recordar mas tarde: " + tiempoDesdeRecordarMasTarde);
					//		Debug.Log("tiempo actual: " + tiempoActual);
					
					//obtenemos el tiempo que ha pasado desde que se pulso en recordar mas tarde
					TimeSpan tiempoTranscurrido = tiempoActual.Subtract(tiempoDesdeRecordarMasTarde);
					
					//si se pulso en recordar mas tarde y ya ha pasado el tiempo que espera para recordar de nuevo
					if (tiempoTranscurrido.Hours >= timeRememberInHours){
						rate();
					}
				}
			}
		}
	}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void rate(){
		#if UNITY_IPHONE
		IOSRateUsPopUp rate = IOSRateUsPopUp.Create(Localization.Get(ExtraLocalizations.RATE_POPUP_TITLE)
		                                            ,Localization.Get(ExtraLocalizations.RATE_POPUP_MESSAGE)
		                                            ,Localization.Get(ExtraLocalizations.RATE_POPUP_VOTE_BUTTON)
		                                            ,Localization.Get(ExtraLocalizations.RATE_POPUP_REMEMBER_BUTTON)
		                                            ,Localization.Get(ExtraLocalizations.RATE_POPUP_REFUSE_BUTTON));
		rate.OnComplete += onRateIOSPopUpClose;
		
		#elif UNITY_ANDROID
		string link = GameSettings.Instance.BUILD_FOR_AMAZON ? GameSettings.Instance.CurrentAmazonAppLink : GameSettings.Instance.CurrentAndroidAppLink;
		AndroidRateUsPopUp rate = AndroidRateUsPopUp.Create(Localization.Get(ExtraLocalizations.RATE_POPUP_TITLE)
		                                                    ,Localization.Get(ExtraLocalizations.RATE_POPUP_MESSAGE)
		                                                    ,link,Localization.Get(ExtraLocalizations.RATE_POPUP_VOTE_BUTTON)
		                                                    ,Localization.Get(ExtraLocalizations.RATE_POPUP_REMEMBER_BUTTON)
		                                                    ,Localization.Get(ExtraLocalizations.RATE_POPUP_REFUSE_BUTTON));
		rate.ActionComplete += onRatePopUpClose;
		#elif UNITY_WP8
		WP8RateUsPopUp rate = WP8RateUsPopUp.Create(Localization.Get(ExtraLocalizations.RATE_POPUP_TITLE)
		                                            , Localization.Get(ExtraLocalizations.RATE_POPUP_MESSAGE));
		rate.ActionComplete += onRatePopUpClose;
		#endif
	}
	
	public void rateUsNow(){
		#if UNITY_IPHONE
		IOSNativeUtility.RedirectToAppStoreRatingPage();
		#elif UNITY_ANDROID
		string url = GameSettings.Instance.BUILD_FOR_AMAZON ? GameSettings.Instance.CurrentAmazonAppLink : GameSettings.Instance.CurrentAndroidAppLink;
		AN_PoupsProxy.OpenAppRatePage(url);
		#elif UNITY_WP8
		WP8PopUps.PopUp.ShowRateWindow();
		#endif
	}
	
	
	//--------------------------------------
	//  EVENTS 
	//--------------------------------------
	private void onRatePopUpClose(AndroidDialogResult result){
		switch(result) {
		case AndroidDialogResult.RATED:
			PlayerPrefs.SetInt(PP_RATE_DONE, 1);
			break;
		case AndroidDialogResult.REMIND:
		case AndroidDialogResult.DECLINED:
			PlayerPrefs.SetString(PP_RATE_LATER, System.DateTime.Now.ToBinary().ToString());
			break;
		}
	}

	private void onRateIOSPopUpClose(IOSDialogResult result){

		switch(result) {
		case IOSDialogResult.RATED:
			PlayerPrefs.SetInt(PP_RATE_DONE, 1);
			break;
		case IOSDialogResult.REMIND:
		case IOSDialogResult.DECLINED:
			PlayerPrefs.SetString(PP_RATE_LATER, System.DateTime.Now.ToBinary().ToString());
			break;
		}
	}
	
}
