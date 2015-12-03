/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
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
		if(GameSettings.Instance.USE_TWITTER){
			AndroidTwitterManager.instance.OnAuthCompleteAction += OnAuthTwitter;
		}
		#endregion
		
		
		
		//		
		#endif

		
		//---
		//FB
		//---
		#region
		
		if(GameSettings.Instance.USE_FACEBOOK){
			SPFacebook.instance.OnAuthCompleteAction +=	 OnAuthFB;
			SPFacebook.instance.OnUserDataRequestCompleteAction += OnUserDataLoaded;
			SPFacebook.instance.OnFocusChangedAction += OnFocusChanged;
			SPFacebook.instance.OnFriendsDataRequestCompleteAction += OnFriendsDataLoaded;
			
			SPFacebook.instance.Init();
		}
		#endregion
	}
	
	void OnDestroy(){
		#if UNITY_ANDROID 
		
		//---
		//TWITTER
		//---
		#region Twitter
		if(GameSettings.Instance.USE_TWITTER){
			AndroidTwitterManager.instance.OnAuthCompleteAction -= OnAuthTwitter;
		}
		#endregion
		
		
		
		//		
		#endif
		
		
		//---
		//FB
		//---
		#region
		
		if(GameSettings.Instance.USE_FACEBOOK){
			SPFacebook.instance.OnAuthCompleteAction -=	 OnAuthFB;
			SPFacebook.instance.OnUserDataRequestCompleteAction -= OnUserDataLoaded;
			SPFacebook.instance.OnFocusChangedAction -= OnFocusChanged;
			SPFacebook.instance.OnFriendsDataRequestCompleteAction -= OnFriendsDataLoaded;
		}
		#endregion
	}

	void Start(){
		// Check for a Facebook logged in user
		if (GameSettings.Instance.USE_FACEBOOK && FB.IsLoggedIn) {
			
			//// Check if we're logged in to Parse
			//if (ParseUser.CurrentUser == null) {
			//    // If not, log in with Parse
			//    GestorParse.instance.login();
			//} else {
			// Show any user cached profile info

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
	// EVENTS
	// --------------------------------------	

	private void OnAuthFB(FB_APIResult result) {
		if(BaseGameScreenController.Instance.Section == GameSection.LOAD_SCREEN)
			GameLoaderManager.Instance.FbInited = true;

		if(SPFacebook.instance.IsLoggedIn) {
			GTDebug.log("Success to log in FB");
			SPFacebook.instance.LoadUserData();
			LoadFriends();
		} else {
			GTDebug.log("Failed to log in FB");
		}		
	}
	
	
	private void OnAuthTwitter(TWResult result) {
		if(BaseGameScreenController.Instance.Section == GameSection.LOAD_SCREEN)
			GameLoaderManager.Instance.TwInited = true;
		
		if(result.IsSucceeded) {

		}
	}

	
	
	private void OnFocusChanged(bool focus) {                            
		Time.timeScale = focus ? 1f : 0f;                                                                  
	}
	
	private void OnUserDataLoaded(FB_APIResult result) {
		if (result.Error == null)  { 
			GTDebug.log("FB user data loaded");
			//			IsUserInfoLoaded = true;
			
			//user data available, we can get info using
			//SPFacebook.instance.userInfo getter
			//and we can also use userInfo methods, for exmple download user avatar image
			//			SPFacebook.instance.userInfo.LoadProfileImage(FacebookProfileImageSize.square);
			SPFacebook.instance.userInfo.LoadProfileImage(FacebookProfileImageSize.large);
			
		} else {
			GTDebug.log("Opps, user FB data load failed, something was wrong");
		}
	}
	
	private void OnFriendsDataLoaded(FB_APIResult res) {
		
		
		
		if(res.Error == null) {
			GTDebug.log("FB friends data loaded");
			//friednds data available, we can get it using
			//SPFacebook.instance.friendsList getter
			//and we can also use FacebookUserInfo methods, for exmple download user avatar image
			
			//			foreach(FacebookUserInfo friend in SPFacebook.instance.friendsList) {
			//				friend.LoadProfileImage(FacebookProfileImageSize.square);
			//			}
			foreach(FacebookUserInfo friend in SPFacebook.instance.friendsList) {
				friend.LoadProfileImage(FacebookProfileImageSize.large);
			}
			
			//			IsFrindsInfoLoaded = true;
		} else {
			GTDebug.log("Opps, FB friends data load failed, something was wrong");
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
