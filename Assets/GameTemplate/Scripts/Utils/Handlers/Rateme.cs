using UnityEngine;
using System;
using System.Collections;
using UnionAssets.FLE;


public class Rateme : Singleton<Rateme>
{
    private double tiempoRecordarEnHoras = 3;

    private const string CLAVE_RATE = "rate_hecho";
    private const string CLAVE_RATE_MAS_TARDE = "rate_recordar";


    // Use this for initialization
    void Start()
    {
        //pedimos el voto despues de que juegue una primera vez, este flag se activa cuando realiza su primera puntuacion
        if (PlayerPrefs.GetInt(Configuration.PP_FIRST_PLAY) == 1)
        {
            if (!PlayerPrefs.HasKey(CLAVE_RATE) || PlayerPrefs.GetInt(CLAVE_RATE) == 0)
            {
                if (!PlayerPrefs.HasKey(CLAVE_RATE_MAS_TARDE))
                {
                    rate();
                }
                else
                {
                    DateTime tiempoActual = DateTime.Now;

                    //obtenemos el tiempo desde que se pulso en recordar mas tarde
                    long temp = Convert.ToInt64(PlayerPrefs.GetString(CLAVE_RATE_MAS_TARDE));

                    //convertimos a DateTime el tiempo de vida long
                    DateTime tiempoDesdeRecordarMasTarde = DateTime.FromBinary(temp);
                    //		Debug.Log("tiempo desde que pulso en recordar mas tarde: " + tiempoDesdeRecordarMasTarde);
                    //		Debug.Log("tiempo actual: " + tiempoActual);

                    //obtenemos el tiempo que ha pasado desde que se pulso en recordar mas tarde
                    TimeSpan tiempoTranscurrido = tiempoActual.Subtract(tiempoDesdeRecordarMasTarde);

                    //si se pulso en recordar mas tarde y ya ha pasado el tiempo que espera para recordar de nuevo
                    if (tiempoTranscurrido.Hours >= tiempoRecordarEnHoras)
                    {
                        rate();
                    }
                }
            }
        }
        else
        {
            PlayerPrefs.SetInt(Configuration.PP_FIRST_PLAY, 1);
        }
    }

    private void rate()
    {
#if UNITY_IPHONE
		IOSRateUsPopUp rate = IOSRateUsPopUp.Create(Localization.Localize(ExtraLocalizations.RATE_POPUP_TITLE)
		                                            ,Localization.Get(ExtraLocalizations.MENSAJE_POPUP_RATE)
		                                            ,Localization.Get(ExtraLocalizations.BOTON_VOTAR_POPUP_RATE)
		                                            ,Localization.Get(ExtraLocalizations.BOTON_RECORDAR_POPUP_RATE)
		                                            ,Localization.Get(ExtraLocalizations.BOTON_RECHAZAR_POPUP_RATE));
		rate.addEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
		
#elif UNITY_ANDROID
		string link = Configuration.buildForAmazon ? Configuration.LINK_AMAZON_APP : Configuration.LINK_ANDROID_APP;
		AndroidRateUsPopUp rate = AndroidRateUsPopUp.Create(Localization.Get(ExtraLocalizations.RATE_POPUP_TITLE)
		                                                    ,Localization.Get(ExtraLocalizations.MENSAJE_POPUP_RATE)
		                                                    ,link,Localization.Get(ExtraLocalizations.BOTON_VOTAR_POPUP_RATE)
		                                                    ,Localization.Get(ExtraLocalizations.BOTON_RECORDAR_POPUP_RATE)
		                                                    ,Localization.Get(ExtraLocalizations.BOTON_RECHAZAR_POPUP_RATE));
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
    private void onRatePopUpClose(CEvent e)
    {
#if UNITY_IPHONE
		(e.dispatcher as IOSRateUsPopUp).removeEventListener(BaseEvent.COMPLETE, onRatePopUpClose);

#elif UNITY_ANDROID
		(e.dispatcher as AndroidRateUsPopUp).removeEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
#elif UNITY_WP8
        (e.dispatcher as WP8RateUsPopUp).removeEventListener(BaseEvent.COMPLETE, onRatePopUpClose);
#endif

        string result = e.data.ToString();

        //Segun el resultado
        if (result == "RATED" || result == "DECLINED")
        {
            PlayerPrefs.SetInt(CLAVE_RATE, 1);
        }
        else if (result == "REMIND")
        {
            PlayerPrefs.SetString(CLAVE_RATE_MAS_TARDE, System.DateTime.Now.ToBinary().ToString());
        }
    }
}
