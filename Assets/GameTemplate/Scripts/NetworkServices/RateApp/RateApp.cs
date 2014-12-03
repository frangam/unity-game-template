using UnityEngine;
using System;
using System.Collections;
using UnionAssets.FLE;


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
		//we ask for rate after player plays its first game
        if (PlayerPrefs.GetInt(Configuration.PP_FIRST_PLAY) == 1){
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
		IOSRateUsPopUp rate = IOSRateUsPopUp.Create(Localization.Localize(ExtraLocalizations.RATE_POPUP_TITLE)
		                                            ,Localization.Get(ExtraLocalizations.MENSAJE_POPUP_RATE)
		                                            ,Localization.Get(ExtraLocalizations.BOTON_VOTAR_POPUP_RATE)
		                                            ,Localization.Get(ExtraLocalizations.BOTON_RECORDAR_POPUP_RATE)
		                                            ,Localization.Get(ExtraLocalizations.BOTON_RECHAZAR_POPUP_RATE));
		rate.addEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
		
#elif UNITY_ANDROID
		string link = Configuration.BUILD_FOR_AMAZON ? Configuration.LINK_AMAZON_APP : Configuration.LINK_ANDROID_APP;
		AndroidRateUsPopUp rate = AndroidRateUsPopUp.Create(Localization.Get(ExtraLocalizations.RATE_POPUP_TITLE)
		                                                    ,Localization.Get(ExtraLocalizations.RATE_POPUP_MESSAGE)
		                                                    ,link,Localization.Get(ExtraLocalizations.RATE_POPUP_VOTE_BUTTON)
		                                                    ,Localization.Get(ExtraLocalizations.RATE_POPUP_REMEMBER_BUTTON)
		                                                    ,Localization.Get(ExtraLocalizations.RATE_POPUP_REFUSE_BUTTON));
		rate.addEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
#elif UNITY_WP8
		WP8RateUsPopUp rate = WP8RateUsPopUp.Create(Localization.Get(ExtraLocalizations.RATE_POPUP_TITLE)
                                                    , Localization.Get(ExtraLocalizations.MENSAJE_POPUP_RATE));
        rate.addEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
#endif
    }


    //--------------------------------------
    //  EVENTS 
    //--------------------------------------
    private void onRatePopUpClose(CEvent e){
#if UNITY_IPHONE
		(e.dispatcher as IOSRateUsPopUp).removeEventListener(BaseEvent.COMPLETE, onRatePopUpClose);

#elif UNITY_ANDROID
		(e.dispatcher as AndroidRateUsPopUp).removeEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
#elif UNITY_WP8
        (e.dispatcher as WP8RateUsPopUp).removeEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
#endif
		//the result
        string result = e.data.ToString();

        //depending on the result
        if (result == "RATED" || result == "DECLINED"){
            PlayerPrefs.SetInt(PP_RATE_DONE, 1);
        }
        else if (result == "REMIND"){
            PlayerPrefs.SetString(PP_RATE_LATER, System.DateTime.Now.ToBinary().ToString());
        }
    }
}
