using UnityEngine;
using UnionAssets.FLE;
using System.Collections;


public class SocialInitializer : Singleton<SocialInitializer> {


	void Awake () {

		#if UNITY_ANDROID 
		
		//---
		//TWITTER
		//---
		#region Twitter
		if(GameSettings.USE_TWITTER){
			AndroidTwitterManager.instance.addEventListener(TwitterEvents.TWITTER_INITED,  OnInitTwitter);
			AndroidTwitterManager.instance.addEventListener(TwitterEvents.AUTHENTICATION_SUCCEEDED,  OnAuthTwitter);
			
			AndroidTwitterManager.instance.addEventListener(TwitterEvents.POST_SUCCEEDED,  OnPost);
			AndroidTwitterManager.instance.addEventListener(TwitterEvents.POST_FAILED,  OnPostFailed);
			
			AndroidTwitterManager.instance.addEventListener(TwitterEvents.USER_DATA_LOADED,  OnUserDataLoaded);
			AndroidTwitterManager.instance.addEventListener(TwitterEvents.USER_DATA_FAILED_TO_LOAD,  OnUserDataLoadFailed);
			
//			AndroidTwitterManager.instance.Init(GameSettings.API_KEY, GameSettings.API_SECRET);
		}
		#endregion
		


//		
		#endif






		//---
		//FB
		//---
		#region

		if(GameSettings.USE_FACEBOOK){

			SPFacebook.instance.addEventListener(FacebookEvents.FACEBOOK_INITED, 			 OnInitFB);
			SPFacebook.instance.addEventListener(FacebookEvents.AUTHENTICATION_SUCCEEDED,  	 OnAuthFB);
			
			SPFacebook.instance.addEventListener(FacebookEvents.USER_DATA_LOADED,  			OnUserDataLoaded);
			SPFacebook.instance.addEventListener(FacebookEvents.USER_DATA_FAILED_TO_LOAD,   OnUserDataLoadFailed);
			
			SPFacebook.instance.addEventListener(FacebookEvents.FRIENDS_DATA_LOADED,  			OnFriendsDataLoaded);
			SPFacebook.instance.addEventListener(FacebookEvents.FRIENDS_FAILED_TO_LOAD,   		OnFriendDataLoadFailed);
			
	//		SPFacebook.instance.addEventListener(FacebookEvents.POST_FAILED,  			OnPostFailed);
	//		SPFacebook.instance.addEventListener(FacebookEvents.POST_SUCCEEDED,   		OnPost);
			
			SPFacebook.instance.addEventListener(FacebookEvents.GAME_FOCUS_CHANGED,   OnFocusChanged);
			
			SPFacebook.instance.Init();
		}
		#endregion
	}


	void Start(){
		// Check for a Facebook logged in user
		if (GameSettings.USE_FACEBOOK && FB.IsLoggedIn) {
			
            //// Check if we're logged in to Parse
            //if (ParseUser.CurrentUser == null) {
            //    // If not, log in with Parse
            //    GestorParse.instance.login();
            //} else {
				// Show any user cached profile info
				OnAuthFB();
            //}
		}
	}

	public void LoadFriends(int limit = 0) {
//		if(limit == 0)
//			SPFacebook.instance.LoadFriendsInfo ();
//		else if(limit > 0){
			SPFacebook.instance.LoadFrientdsInfo(limit);
//		}

	}

	// --------------------------------------
	// Eventos 
	// --------------------------------------	

	private void OnUserDataLoadFailed() {
		//			AndroidNative.showMessage("Error", "Opps, user data load failed, something was wrong");
	}
	
	private void OnUserDataLoaded() {
		SPFacebook.instance.userInfo.LoadProfileImage(FacebookProfileImageSize.large);
		//			AndroidNative.showMessage("Success", "user loaded");
	}
	
	private void OnFriendDataLoadFailed() {
		//			AndroidNative.showMessage("Error", "Opps, friends data load failed, something was wrong");
	}
	
	private void OnFriendsDataLoaded() {
		foreach(FacebookUserInfo friend in SPFacebook.instance.friendsList) {
			friend.LoadProfileImage(FacebookProfileImageSize.large);
		}
		//			AndroidNative.showMessage("Success", "friends loaded");
	}
	
	private void OnPost() {
		//			if(panelCaptura)
		//				panelCaptura.gameObject.SetActive (false);
		
		//			AndroidNative.showMessage("Success", "Congrats, you just postet something to twitter");
	}
	
	private void OnPostFailed() {
		//			if(panelCaptura)
		//				panelCaptura.gameObject.SetActive (false);
		
		//			AndroidNative.showMessage("Error", "Opps, post failed, something was wrong");
	}
	
	private void OnInitTwitter() {
		GameLoaderManager.Instance.TwInited = true;

		if(AndroidTwitterManager.instance.IsAuthed) {
			OnAuthTwitter();
		}
	}
	
	private void OnInitFB() {
		GameLoaderManager.Instance.FbInited = true;

		if(SPFacebook.instance.IsLoggedIn) {
			OnAuthFB();
		}
	}
	
	private void OnAuthTwitter() {
		GameLoaderManager.Instance.TwInited = true;
		
		//			AndroidNative.showMessage ("Logeado", "Está logeado en Twiiter");
	}
	private void OnAuthFB() {
		GameLoaderManager.Instance.FbInited = true;
		
		Debug.Log ("FB autenticado");
		
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
	
	// --------------------------------------
	// Eventos IOS
	// --------------------------------------	
	#if UNITY_IPHONE

	
	private void OnPostSuccses() {
		//			IOSNative.showMessage("Positng example", "Posy Succses!");
	}

	#endif
}
