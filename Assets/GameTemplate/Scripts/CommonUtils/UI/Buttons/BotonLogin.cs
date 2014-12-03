using UnityEngine;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;

public class BotonLogin : MonoBehaviour {
	/*--------------------------------
	 * Atributos de configuracion
	 -------------------------------*/
	[SerializeField]
	private SocialNetwork red;

	public bool cambiarTexto = true;
	public string spriteConectar;
	public string spriteDesconectar;

	private UILabel label;
	
	/*--------------------------------
	 * Getters/Setters
	 -------------------------------*/
	public SocialNetwork RedSocial {
		get {
			return this.red;
		}
	}
	
	/*--------------------------------
	 * Metodos Unity
	 -------------------------------*/
	#region Unity
	void Awake(){
		label = GetComponentInChildren<UILabel> ();

		switch(red){
		case SocialNetwork.GOOGLE_PLAY_SERVICES:
			gameObject.SetActive(Application.platform == RuntimePlatform.Android);

//			#if UNITY_ANDROID
//			if(GooglePlayConnection.state == GPConnectionState.STATE_UNCONFIGURED || GooglePlayConnection.state == GPConnectionState.STATE_DISCONNECTED)
//				label.text = "login";
//			else if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED)
//				label.text = "exit";
//			#endif
			break;

		case SocialNetwork.GAME_CENTER:
			gameObject.SetActive(Application.platform == RuntimePlatform.IPhonePlayer);
			break;

		case SocialNetwork.FACEBOOK:

			break;
		}
		
	}

	void LateUpdate(){
		#if UNITY_ANDROID
		if(GooglePlayConnection.state == GPConnectionState.STATE_UNCONFIGURED || GooglePlayConnection.state == GPConnectionState.STATE_DISCONNECTED)
			label.text = "login"; //Localization.Localize(LocalizacionesExtra.BOTON_LOGIN);
		else if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED)
			label.text = "exit";//Localization.Localize(LocalizacionesExtra.BOTON_LOGOUT);
		#endif


		


		//facebook
		if(red == SocialNetwork.FACEBOOK && FB.IsLoggedIn){
//			gameObject.SetActive(false);

			if(cambiarTexto)
				GetComponentInChildren<UILabel>().text = Localization.Get(ExtraLocalizations.SIMPLE_LOGOUT_BUTTON);

			if(spriteDesconectar != "")
				GetComponent<UISprite>().spriteName = spriteDesconectar;

			GetComponent<Animator>().enabled = false;
		}
		else if(red == SocialNetwork.FACEBOOK && !FB.IsLoggedIn){
			if(cambiarTexto)
				GetComponentInChildren<UILabel>().text = Localization.Get(ExtraLocalizations.SIMPLE_LOGIN_BUTTON);

			if(spriteConectar != "")
				GetComponent<UISprite>().spriteName = spriteConectar;

			GetComponent<Animator>().enabled = true;
		}
	}
	#endregion
	
	/*--------------------------------
	 * Metodos NGUI
	 -------------------------------*/
	#region NGUI
	void OnPress(bool inicioToque){
		//touch up
		if(!inicioToque){
			switch(red){
			case SocialNetwork.GOOGLE_PLAY_SERVICES:
				if(GooglePlayConnection.state == GPConnectionState.STATE_UNCONFIGURED){
					GPSConnect.Instance.init();
					GooglePlayConnection.instance.connect (); //conectar
				}
				else if(GooglePlayConnection.state == GPConnectionState.STATE_DISCONNECTED){
					GooglePlayConnection.instance.connect();
//					label.text = "exit";
				}
				else if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
					GooglePlayConnection.instance.disconnect();
//					label.text = "login";
				}
				break;
				
			case SocialNetwork.GAME_CENTER:

				break;

			case SocialNetwork.FACEBOOK:
				if(FB.IsLoggedIn)
					GestorSocial.Instance.logout(red, true);
				else if(!FB.IsLoggedIn){
					

					UIHandler.Instance.abrir(GameScreen.CONNECTING_FACEBOOK); //abre la ventana de conectando
					GestorSocial.Instance.login(red);
				}
				break;
			}
		}
	}
	#endregion



}
