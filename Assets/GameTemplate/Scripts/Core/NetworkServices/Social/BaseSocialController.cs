using UnityEngine;
using UnityEngine.UI;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;
using System;
using Facebook;
using Facebook.MiniJSON;
using System.Linq;

public class BaseSocialController : Singleton<BaseSocialController> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Image panelCaptura;
	
	private string hashtag;
	private string urlIcono;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool estaLogeadoEnTwitter = false;
	private bool estaLogeadoEnFB = false;
	private bool posteando = false;
	private bool hacerCaptura = false;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public Image PanelCaptura {
		get {
			return this.panelCaptura;
		}
	}
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		hashtag = GameSettings.Instance.HASHTAG;
		urlIcono = GameSettings.Instance.LOGO_APP_LINK;
		
		if(panelCaptura)
			panelCaptura.gameObject.SetActive (false);
		
		if(GameSettings.Instance.USE_FACEBOOK){
			SPFacebook.instance.addEventListener(FacebookEvents.AUTHENTICATION_SUCCEEDED,  	 OnAuthFB);
			SPFacebook.instance.addEventListener(FacebookEvents.USER_DATA_LOADED,  			OnUserDataLoaded);
			SPFacebook.instance.addEventListener(FacebookEvents.AUTHENTICATION_FAILED,  	 OnAuthFailedFB);
			//			SPFacebook.instance.addEventListener(FacebookUserInfo.PROFILE_IMAGE_LOADED,   OnProfileImageLoaded);
			SPFacebook.instance.addEventListener(FacebookEvents.GAME_FOCUS_CHANGED,   OnFocusChanged);
			SPFacebook.instance.addEventListener(FacebookEvents.FRIENDS_DATA_LOADED,  			OnFriendsDataLoaded);
		}
		
		//---
		//TWITTER
		//---
		#region Twitter
		
		#if UNITY_ANDROID || UNITY_EDITOR
		if(GameSettings.Instance.USE_TWITTER){
			AndroidTwitterManager.instance.addEventListener(TwitterEvents.POST_SUCCEEDED,  OnPost);
			AndroidTwitterManager.instance.addEventListener(TwitterEvents.POST_FAILED,  OnPostFailed);
			AndroidTwitterManager.instance.addEventListener(TwitterEvents.AUTHENTICATION_SUCCEEDED,  OnAuthTwitter);
		}
		#elif UNITY_IPHONE
		#endif
		
		#endregion
		
		
		//---
		//FB
		//---
		#region 
		if(GameSettings.Instance.USE_FACEBOOK){
			SPFacebook.instance.addEventListener(FacebookEvents.POST_FAILED,  			OnPostFailed);
			SPFacebook.instance.addEventListener(FacebookEvents.POST_SUCCEEDED,   		OnPost);
			
			if(!SPFacebook.instance.IsInited){
				SPFacebook.instance.Init();
			}
		}
		
		#endregion
	}
	//
	public virtual void Start(){
		// Check for a Facebook logged in user
		if (GameSettings.Instance.USE_FACEBOOK && FB.IsLoggedIn) {
			
			//// Check if we're logged in to Parse
			//if (ParseUser.CurrentUser == null) {
			//    // If not, log in with Parse
			//    GestorParse.instance.login();
			//} else {
			// Show any user cached profile info
			//OnAuthFB();
			//}
		}
	}
	#endregion
	
	
	
	// --------------------------------------
	// EVENTS
	// --------------------------------------	
	private void OnInitFB() {
		GameLoaderManager.Instance.FbInited = true;
		
		if(SPFacebook.instance.IsLoggedIn) {
			OnAuthFB();
		}
	}
	
	private void OnAuthFB() {
		estaLogeadoEnFB = true;
		Debug.Log ("FB authenticated");
		
		// Login callback
		if(FB.IsLoggedIn) {
			SPFacebook.instance.LoadUserData();
			LoadFriends();
			
			//if(ParseUser.CurrentUser == null)
			//    GestorParse.instance.login(); //logear en Parse
		} else {
			//			UIHandler.Instance.abrir(GameScreen.FACEBOOK_FAILED_CONNECTION);
			//			Debug.Log ("FBLoginCallback: User canceled login");
		}
		
	}
	
	private void OnAuthFailedFB() {
		estaLogeadoEnFB = false;
		//		Debug.Log ("FB fallo autenticacion");
		
		//		UIHandler.Instance.abrir(GameScreen.FACEBOOK_FAILED_CONNECTION);
		
	}
	
	private void OnAuthTwitter() {
		estaLogeadoEnTwitter = true;
		
		if(posteando){
			postear(SocialNetwork.TWITTER, hacerCaptura);
			posteando = false;
		}
	}
	
	
	private void OnPost() {
		if(panelCaptura)
			panelCaptura.gameObject.SetActive (false);
		
		//		AndroidNative.showMessage(Localization.Localize(ExtraLocalizations.TITULO_POPUP_POST_EXITO_ENVIADO), Localization.Localize(ExtraLocalizations.POST_DESCRIPCION_ENVIADO));
	}
	
	private void OnPostFailed() {
		if(panelCaptura)
			panelCaptura.gameObject.SetActive (false);
		
		//		AndroidNative.showMessage("Error", "Opps, post failed, something was wrong");
	}
	
	private void OnPostSuccses() {
		//		IOSNative.showMessage("Positng example", "Posy Succses!");
	}
	
	private void OnFocusChanged(CEvent e) {
		bool focus = (bool) e.data;
		
		if (!focus)  {                                                                                        
			// pause the game - we will need to hide                                             
			Time.timeScale = 0f;                                                                  
		} else  {                                                                                        
			// start the game back up - we're getting focus again                                
			Time.timeScale = 1f;                                                                  
		}   
	}
	
	private void OnUserDataLoaded() {
		SPFacebook.instance.userInfo.LoadProfileImage(FacebookProfileImageSize.large);
		//			AndroidNative.showMessage("Success", "user loaded");
	}
	
	private void OnUserDataLoadFailed() {
		//			AndroidNative.showMessage("Error", "Opps, user data load failed, something was wrong");
	}
	
	private void OnFriendsDataLoaded() {
		//		Debug.Log ("cargando imagenes amigos");
		foreach(FacebookUserInfo friend in SPFacebook.instance.friendsList) {
			friend.LoadProfileImage(FacebookProfileImageSize.large);
		}
		
		//			AndroidNative.showMessage("Success", "friends loaded");
	}
	
	private void OnFriendDataLoadFailed() {
		//			AndroidNative.showMessage("Error", "Opps, friends data load failed, something was wrong");
	}
	
	
	private void OnProfileImageLoaded(CEvent e) {
		//		Debug.Log ("fotos amigos cargadas");
		//		StartCoroutine(GestorParse.listaUsuarios ());
		//		StartCoroutine (guardarAmigos ());
	}
	
	//---
	//scores Api events
	//---
	private void OnPlayerScoreRequestComplete(CEvent e) {
		FB_APIResult result = e.data as FB_APIResult;
		
		if(result.IsSucceeded) {
			string msg = "Player has scores in " + SPFacebook.instance.userScores.Count + " apps" + "\n";
			msg += "Current Player Score = " + SPFacebook.instance.GetCurrentPlayerIntScoreByAppId(FB.AppId);
			
			
		} else {
			//			SA_StatusBar.text = result.responce;
		}
		
		
	}
	
	private void OnAppScoreRequestComplete(CEvent e) {
		FB_APIResult result = e.data as FB_APIResult;
		
		if(result.IsSucceeded) {
			string msg = "Loaded " + SPFacebook.instance.appScores.Count + " scores results" + "\n";
			msg += "Current Player Score = " + SPFacebook.instance.GetScoreByUserId(FB.UserId);
			
			
		} else {
			//			SA_StatusBar.text = result.responce;
		}
		
	}
	
	private void OnSubmitScoreRequestComplete(CEvent e) {
		
		FB_APIResult result = e.data as FB_APIResult;
		if(result.IsSucceeded) {
			string msg = "Score successfully submited" + "\n";
			msg += "Current Player Score = " + SPFacebook.instance.GetScoreByUserId(FB.UserId);
			
			
		} else {
			//			SA_StatusBar.text = result.responce;
		}
		
		
	}
	
	private void OnDeleteScoreRequestComplete(CEvent e) {
		FB_APIResult result = e.data as FB_APIResult;
		if(result.IsSucceeded) {
			string msg = "Score successfully deleted" + "\n";
			msg += "Current Player Score = " + SPFacebook.instance.GetScoreByUserId(FB.UserId);
			
			
		} else {
			//			SA_StatusBar.text = result.responce;
		}
		
		
	}
	
	
	// --------------------------------------
	// Metodos publicos
	// --------------------------------------
	public void login(SocialNetwork red){
		#if UNITY_ANDROID || UNITY_EDITOR
		if(red == SocialNetwork.TWITTER && !AndroidTwitterManager.instance.IsAuthed){
			AndroidTwitterManager.instance.AuthenticateUser();
		}
		#endif
		
		
		if(red == SocialNetwork.FACEBOOK && !SPFacebook.instance.IsLoggedIn){
			SPFacebook.instance.Login();
		}
		
	}
	
	public void logout(SocialNetwork red, bool usandoParse = false){
		#if UNITY_ANDROID
		if(red == SocialNetwork.TWITTER && AndroidTwitterManager.instance.IsAuthed){
			AndroidTwitterManager.instance.LogOut();
		}
		#endif
		
		
		if(red == SocialNetwork.FACEBOOK && SPFacebook.instance.IsLoggedIn){
			SPFacebook.instance.Logout();
			
			
			//if(usandoParse && ParseUser.CurrentUser != null){
			//    ParseUser.LogOut();
			//}
		}
	}
	
	
	//	private IEnumerator getAmigosEnParse(){
	//		if(FB.IsLoggedIn && ParseUser.CurrentUser != null){
	//			//query donde el jugador es el player
	//				var queryPlayer = ParseObject.GetQuery("Friend")
	//				.WhereEqualTo("player", ParseUser.CurrentUser);
	//
	//			//query donde el jugador es el amigo
	//			var queryAmigo = ParseObject.GetQuery("Friend")
	//				.WhereEqualTo("amigo", ParseUser.CurrentUser);
	//
	//			ParseQuery<ParseObject> queryAmigos = queryPlayer.Or(queryAmigo);
	//
	//			//scores de los amigos
	//			var queryScores = ParseObject.GetQuery("LevelScore")
	//				.WhereEqualTo("player", queryAmigos);
	//
	//			
	//			var queryTask = queryScores.FindAsync();
	//			
	//			while (!queryTask.IsCompleted) yield return null;
	//			
	//			
	//			if (queryTask.IsFaulted || queryTask.IsCanceled) {
	//				// There was an error submitting score in to Parse
	//				foreach(var e in queryTask.Exception.InnerExceptions) {
	//					ParseException parseException = (ParseException) e;
	//					Debug.Log("ParseScore: error message " + parseException.Message);
	//					Debug.Log("ParseScore: error code: " + parseException.Code);
	//				}
	//			} else {
	//				// Lista de los amigos recibidos de parse con sus records
	//				IEnumerable<ParseObject> amigos = queryTask.Result;
	//				List<FriendScore> scoresAmigos = new List<FriendScore>();
	//				List<FriendScore> amigosNoEnParse = new List<FriendScore>(); //lista de amigos de FB que no estan en parse
	//
	//
	//				ParseObject oAmigo = null;
	//
	//				foreach(ParseObject p in amigos){
	//					string id = p.Get<string>("player");
	//					ParseObject player = p.Get<ParseObject>("player");
	//					int nivel = p.Get<int>("level");
	//					int score = p.Get<int>("score");
	//					FriendScore fs = new FriendScore(id);
	//					fs.addScore(nivel, score);
	//
	//					Debug.Log("player: "+id+", level: "+nivel+", score: "+score);
	//					  
	//					
	//				}
	//
	//				//TODO TEST
	//				foreach(FriendScore f in scoresAmigos)
	//					Debug.Log("amigo: "+f);
	//
	//
	//				//comprobar que amigos de FB no estan en Parse
	//				foreach(FacebookUserInfo friend in SPFacebook.instance.friendsList){
	//					bool estaEnParse = false;
	//
	//					foreach(FriendScore s in scoresAmigos){
	//						estaEnParse = s.Id == friend.id;
	//
	//						if(estaEnParse)
	//							break;
	//					}
	//
	//					//insertamos en la lista el amigo que no esta en parse
	//					if(!estaEnParse){
	//						amigosNoEnParse.Add(new FriendScore(friend.id));
	//					}
	//				}
	//
	//
	//				//guardamos cada amigo
	//				foreach(FriendScore f in amigosNoEnParse)
	//					StartCoroutine(guardarAmigos(f));
	//
	//			}
	//			
	//			
	//			
	//		}
	//	}
	
	//	private IEnumerator guardarAmigos(FriendScore friendScore){
	//		if (FB.IsLoggedIn) {
	//			ParseObject friend = new ParseObject("Friend");
	//			friend["player"] = ParseUser.CurrentUser;
	//
	//
	//			var guardarTask = friend.SaveAsync();
	//			
	//			//			Debug.Log("expiracion accestoken: "+FB.AccessTokenExpiresAt);
	//			
	//			while (!guardarTask.IsCompleted) yield return null;
	//			// Login completed, check results
	//			if (guardarTask.IsFaulted || guardarTask.IsCanceled) {
	//				// There was an error logging in to Parse
	//				foreach(var e in loginTask.Exception.InnerExceptions) {
	//					ParseException parseException = (ParseException) e;
	//					Debug.Log("ParseLogin: error message " + parseException.Message);
	//					Debug.Log("ParseLogin: error code: " + parseException.Code);
	//				}
	//			} else {
	//				// Log in to Parse successful
	//				Debug.Log("ParseLogin: log in to Parse successful");
	//				
	//				LoadFriends();
	//				
	//				
	//				//				// Get user info
	//				//				FB.API("/me", HttpMethod.GET, FBAPICallback);
	//				//				// Display current profile info
	//				//				UpdateProfile();
	//			}
	//		}
	//	}
	
	
	
	public void post(SocialNetwork red, bool shareLevelCompleted = false, bool captura = false){
		#if UNITY_ANDROID || UNITY_EDITOR
		login (red); //logeamos si fuese necesario
		posteando = true;
		hacerCaptura = captura;
		
		
		if(posteando){
			postear(red, shareLevelCompleted, hacerCaptura);
			posteando = false;
		}
		#elif UNITY_IPHONE
		posteando = true;
		hacerCaptura = captura;
		postear(red, shareLevelCompleted, hacerCaptura);
		posteando = false;
		#endif
	}
	
	
	public void LoadFriends(int limit = 0) {
		//		if(limit == 0)
		//			SPFacebook.instance.LoadFrientdsInfo ();
		//		else if(limit > 0){
		SPFacebook.instance.LoadFrientdsInfo(limit);
		//		}
		//		SA_StatusBar.text = "Loading friends..";
	}
	
	public void AppRequest() {
		SPFacebook.instance.AppRequest(Localization.Get(ExtraLocalizations.FACEBOOK_INVITATION));
	}
	
	
	public void LoadScore() {
		SPFacebook.instance.LoadPlayerScores();
	}
	
	public void LoadAppScores() {
		SPFacebook.instance.LoadAppScores();
	}
	
	public void SubmitScore() {
		//		SPFacebook.instance.SubmitScore(ScoresHandler.Instance.getBestScore(GameController.Instance.Manager.getRankingID()));
	}
	
	
	public void DeletePlayerScores() {
		SPFacebook.instance.DeletePlayerScores();
	}
	
	
	
	
	
	// --------------------------------------
	// Metodos privados
	// --------------------------------------
	private void postear(SocialNetwork red, bool shareLevelCompleted = false, bool captura = false){
		string linkAPP = GameSettings.Instance.CurrentAndroidAppShortLink;
		#if UNITY_ANDROID
		linkAPP = GameSettings.Instance.CurrentAndroidAppShortLink;
		#elif UNITY_IPHONE
		linkAPP = GameSettings.Instance.CurrentIOSAppShortLink;
		#endif
		
		string mensajeTwitter = shareLevelCompleted ? Localization.Localize(ExtraLocalizations.SHARE_LEVEL_COMPLETED)+" "+PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_PLAYED)
			: Localization.Localize(ExtraLocalizations.SOCIAL_POST_BEST_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+" "+Localization.Localize(ExtraLocalizations.SOCIAL_POST_CURRENT_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+hashtag+" "+linkAPP;
		string mensajeFB = shareLevelCompleted ? Localization.Localize(ExtraLocalizations.SHARE_LEVEL_COMPLETED)+" "+PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_PLAYED)
			: Localization.Localize(ExtraLocalizations.SOCIAL_POST_BEST_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+" "+Localization.Localize(ExtraLocalizations.SOCIAL_POST_CURRENT_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+" "+linkAPP;
		
		if(captura){
			string mensajeCaptura = "";
			
			switch(red){
			case SocialNetwork.FACEBOOK: mensajeCaptura = mensajeFB; break;
			case SocialNetwork.TWITTER: mensajeCaptura =  mensajeTwitter; break;
			}
			
			StartCoroutine(postCaptura(red, mensajeCaptura));
		}
		else{
			
			
			#if UNITY_ANDROID
			if(red == SocialNetwork.TWITTER){
				AndroidTwitterManager.instance.Post(mensajeTwitter);
			}
			else if(red == SocialNetwork.FACEBOOK){
				
				SPFacebook.instance.Post (
					link: linkAPP,
					linkName: GameSettings.Instance.CurrentGameName + " (iPhone/iPad & Android & WP8)",
					linkCaption: "Ranking",
					linkDescription: mensajeFB,
					picture: urlIcono
					);
			}
			
			
			#elif UNITY_IPHONE
			if(red == SocialNetwork.TWITTER)
				IOSSocialManager.instance.TwitterPost(mensajeTwitter);
			else if(red == SocialNetwork.FACEBOOK)
				IOSSocialManager.instance.FacebookPost(mensajeFB);
			#endif
		}
	}
	
	private IEnumerator postCaptura(SocialNetwork red, string mensaje) {
		
		yield return new WaitForEndOfFrame();
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		// Read screen contents into the texture
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 );
		tex.Apply();
		
		//---
		//Publicacion del mensaje con imagen
		
		#if UNITY_ANDROID
		string linkAPP = GameSettings.Instance.CurrentAndroidAppShortLink;
		
		if(red == SocialNetwork.FACEBOOK){
			//			string mensaje = Localization.Localize(ExtraLocalizations.SOCIAL_POST_BEST_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+" "+Localization.Localize(ExtraLocalizations.SOCIAL_POST_CURRENT_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+" "+linkAPP;
			SPFacebook.instance.PostImage(mensaje, tex);
		}
		else if(red == SocialNetwork.TWITTER){
			//			string mensaje = Localization.Localize(ExtraLocalizations.SOCIAL_POST_BEST_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+" "+Localization.Localize(ExtraLocalizations.SOCIAL_POST_CURRENT_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+hashtag+" "+linkAPP;
			AndroidTwitterManager.instance.Post(mensaje, tex);
		}
		
		#elif UNITY_IPHONE
		linkAPP = GameSettings.Instance.CurrentIOSAppShortLink;
		
		if(red == SocialNetwork.FACEBOOK){
			//			string mensaje = Localization.Localize(ExtraLocalizations.SOCIAL_POST_BEST_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+" "+Localization.Localize(ExtraLocalizations.SOCIAL_POST_CURRENT_SCORE)+" "+ScoresHandler.Instance.mejorPuntuacion() +" "+linkAPP;
			IOSSocialManager.instance.FacebookPost(mensaje, tex);
		}
		else if(red == SocialNetwork.TWITTER){
			//			string mensaje = Localization.Localize(ExtraLocalizations.SOCIAL_POST_BEST_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+" "+Localization.Localize(ExtraLocalizations.SOCIAL_POST_CURRENT_SCORE)+" "+ScoresHandler.Instance.mejorPuntuacion()+hashtag+" "++linkAPP;
			IOSSocialManager.instance.TwitterPost(mensaje, tex);
		}
		#endif
		
		Destroy(tex);
	}
	
	
	
}
