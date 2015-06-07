using UnityEngine;
using UnityEngine.UI;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;

public class BotonLogin : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private SocialNetwork red;
	
	[SerializeField]
	private Transform[] hideWhenLogin;
	[SerializeField]
	private Transform[] showWhenLogin;
	[SerializeField]
	private Transform[] hideWhenLogout;
	[SerializeField]
	private Transform[] showWhenLogout;
	
	public bool cambiarTexto = true;
	public string spriteConectar;
	public string spriteDesconectar;
	
	private Text label;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public SocialNetwork RedSocial {
		get {
			return this.red;
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	#region Unity
	public override void Awake(){
		label = GetComponentInChildren<Text> ();
		
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
			gameObject.SetActive(Application.platform == RuntimePlatform.IPhonePlayer && !GameCenterManager.IsPlayerAuthenticated);
			break;
			
		case SocialNetwork.FACEBOOK:
			
			break;
		}
		
	}
	
	void LateUpdate(){
		#if UNITY_ANDROID
		if(GooglePlayConnection.state == GPConnectionState.STATE_UNCONFIGURED || GooglePlayConnection.state == GPConnectionState.STATE_DISCONNECTED)
			label.text = Localization.Get(ExtraLocalizations.SIMPLE_LOGIN_BUTTON);
		else if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED)
			label.text = Localization.Get(ExtraLocalizations.SIMPLE_LOGOUT_BUTTON);
		#elif UNITY_IPHONE
		
		#endif
		
		
		//facebook
		if(red == SocialNetwork.FACEBOOK && FB.IsLoggedIn){
			//			gameObject.SetActive(false);
			
			if(cambiarTexto)
				GetComponentInChildren<Text>().text = Localization.Get(ExtraLocalizations.SIMPLE_LOGOUT_BUTTON);
			
			//			if(spriteDesconectar != "")
			//				GetComponent<Image>().spriteName = spriteDesconectar;
			
			GetComponent<Animator>().enabled = false;
		}
		else if(red == SocialNetwork.FACEBOOK && !FB.IsLoggedIn){
			if(cambiarTexto)
				GetComponentInChildren<Text>().text = Localization.Get(ExtraLocalizations.SIMPLE_LOGIN_BUTTON);
			
			//			if(spriteConectar != "")
			//				GetComponent<UISprite>().spriteName = spriteConectar;
			
			GetComponent<Animator>().enabled = true;
		}
	}
	#endregion
	
	
	protected override void doPress ()
	{
		base.doPress ();
		
		switch(red){
		case SocialNetwork.GOOGLE_PLAY_SERVICES:
			if(GooglePlayConnection.state == GPConnectionState.STATE_UNCONFIGURED){
				GPSConnect.Instance.init();
				GooglePlayConnection.instance.connect (); //conectar
				GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerAndroidConnected);
				
			}
			else if(GooglePlayConnection.state == GPConnectionState.STATE_DISCONNECTED){
				GooglePlayConnection.instance.connect();
				GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerAndroidConnected);
			}
			else if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
				GooglePlayConnection.instance.disconnect();
				GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerAndroidDisconnected);
				PlayerPrefs.SetInt(GameSettings.PP_LAST_OPENNING_USER_CONNECTED_TO_STORE_SERVICE, 0);
			}
			break;
			
		case SocialNetwork.GAME_CENTER:
			if(!GameCenterManager.IsPlayerAuthenticated){
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("LoginButton - Showing Authentication flow of Game Center");
				
				GameCenterManager.OnAuthFinished += OnAuthIOSFinished;
				GameCenterManager.init();
			}
			else{
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("LoginButton - Authenticated on Game Center");
			}
			break;
			
		case SocialNetwork.FACEBOOK:
			if(FB.IsLoggedIn)
				BaseSocialController.Instance.logout(red, true);
			else if(!FB.IsLoggedIn){
				
				
				//					UIHandler.Instance.abrir(GameScreen.CONNECTING_FACEBOOK); //abre la ventana de conectando
				BaseSocialController.Instance.login(red);
			}
			break;
		}
	}
	
	private void showAndHideTranforms(bool whenLogin = true){
		if(whenLogin){
			if(showWhenLogin != null && showWhenLogin.Length > 0){
				foreach(Transform t in showWhenLogin)
					t.gameObject.SetActive(true);
			}
			if(hideWhenLogin != null && hideWhenLogin.Length > 0){
				foreach(Transform t in hideWhenLogin)
					t.gameObject.SetActive(false);
			}
		}
		else{
			if(showWhenLogout != null && showWhenLogout.Length > 0){
				foreach(Transform t in showWhenLogout)
					t.gameObject.SetActive(true);
			}
			if(hideWhenLogout != null && hideWhenLogout.Length > 0){
				foreach(Transform t in hideWhenLogout)
					t.gameObject.SetActive(false);
			}
		}
	}
	
	//--------------------------------------
	// Events Methods
	//--------------------------------------
	//On iOS only can connect from the app, not disconnect. (disconnection is made from GameCenter app)
	void OnAuthIOSFinished (ISN_Result res) {
		GameCenterManager.OnAuthFinished -= OnAuthIOSFinished;
		
		if (res.IsSucceeded) {
			showAndHideTranforms();
			gameObject.SetActive(false);
		} 
	}
	private void OnPlayerAndroidDisconnected() {
		GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerAndroidDisconnected);
		
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("LoginButton - android player disconnected");
		
		showAndHideTranforms(false);
	}
	
	private void OnPlayerAndroidConnected() {
		GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerAndroidConnected);
		
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("LoginButton - android OnPlayerConnected - player connected");
		
		showAndHideTranforms();
	}
}
