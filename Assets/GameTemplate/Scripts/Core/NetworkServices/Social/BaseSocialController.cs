/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
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
	private Image screenshotPanel;
	
	private string hashtag;
	private string iconURL;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool isLoggedInTw = false;
	private bool isLoggedInFB = false;
	private bool posting = false;
	private bool doScreenshot = false;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public Image ScreenshotPanel {
		get {
			return this.screenshotPanel;
		}
	}
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		hashtag = GameSettings.Instance.HASHTAG;
		iconURL = GameSettings.Instance.LOGO_APP_LINK;
		
		if(screenshotPanel)
			screenshotPanel.gameObject.SetActive (false);
		
		if(GameSettings.Instance.USE_FACEBOOK){
			SPFacebook.instance.OnAuthCompleteAction +=	 OnAuthFB;
			SPFacebook.instance.OnUserDataRequestCompleteAction += OnUserDataLoaded;
			SPFacebook.instance.OnFocusChangedAction += OnFocusChanged;
			SPFacebook.instance.OnFriendsDataRequestCompleteAction += OnFriendsDataLoaded;
			SPFacebook.instance.OnPostingCompleteAction += OnPostFB;
		}
		
		//---
		//TWITTER
		//---
		#region Twitter
		
		#if UNITY_ANDROID || UNITY_EDITOR
		if(GameSettings.Instance.USE_TWITTER){
			AndroidTwitterManager.instance.OnPostingCompleteAction += OnPost;
			AndroidTwitterManager.instance.OnAuthCompleteAction += OnAuthTwitter;
		}
		#elif UNITY_IPHONE
		#endif
		
		#endregion
		
		
		//---
		//FB
		//---
		#region 
		if(GameSettings.Instance.USE_FACEBOOK){
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
	private void OnPostFB(FBPostResult res) {
		
		if(res.IsSucceeded) {
			
		} else {
			
		}
	}
	
	private void OnAuthFB(FBResult result) {
		if(BaseGameScreenController.Instance.Section == GameSection.LOAD_SCREEN)
			GameLoaderManager.Instance.FbInited = true;
		
		isLoggedInFB = SPFacebook.instance.IsLoggedIn;
		
		if(SPFacebook.instance.IsLoggedIn) {
			GTDebug.log("Success to log in FB");
			SPFacebook.instance.LoadUserData();
			LoadFriends();
		} else {
			GTDebug.log("Failed to log in FB");
		}		
	}
	
	
	private void OnAuthTwitter(TWResult result) {
		isLoggedInTw = result.IsSucceeded;
		
		if(result.IsSucceeded) {
			if(posting){
				postear(SocialNetwork.TWITTER, doScreenshot);
				posting = false;
			}
		}
		
		
	}
	
	
	private void OnPost(TWResult result) {
		if(screenshotPanel)
			screenshotPanel.gameObject.SetActive (false);
		
		if(result.IsSucceeded) {
			GTDebug.log("Congrats. You just posted something to Twitter!");
		} else {
			GTDebug.log("Oops! Posting failed. Something went wrong.");
		}
	}
	
	
	private void OnFocusChanged(bool focus) {                            
		Time.timeScale = focus ? 1f : 0f;                                                                  
	}
	
	private void OnUserDataLoaded(FBResult result) {
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
	
	private void OnFriendsDataLoaded(FBResult res) {
		
		
		
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
	// Public Methods
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
	
	
	
	public void post(SocialNetwork network, bool shareLevelCompleted = false, bool screenshot = false){
		posting = true;
		doScreenshot = screenshot;
		
		#if UNITY_ANDROID || UNITY_IPHONE || UNITY_EDITOR
		login (network); //logeamos si fuese necesario
		
		if(posting){
			postear(network, shareLevelCompleted, doScreenshot);
			posting = false;
		}
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
	private void postear(SocialNetwork network, bool shareLevelCompleted = false, bool screenshot = false){
		string linkAPP = "";
		#if UNITY_ANDROID
		linkAPP = !string.IsNullOrEmpty(GameSettings.Instance.CurrentAndroidAppShortLink) ? GameSettings.Instance.CurrentAndroidAppShortLink 
			: GameSettings.Instance.CurrentAndroidAppLink;
		#elif UNITY_IPHONE
		linkAPP = !string.IsNullOrEmpty(GameSettings.Instance.CurrentIOSAppShortLink) ? GameSettings.Instance.CurrentIOSAppShortLink 
			: GameSettings.Instance.CurrentIOSAppLink;
		#endif
		
		string twMessage = shareLevelCompleted ? Localization.Localize(ExtraLocalizations.SHARE_LEVEL_COMPLETED)+" "+PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_PLAYED)
			: Localization.Localize(ExtraLocalizations.SOCIAL_POST_BEST_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+" "+Localization.Localize(ExtraLocalizations.SOCIAL_POST_CURRENT_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+hashtag+" "+linkAPP;
		string fbMessage = shareLevelCompleted ? Localization.Localize(ExtraLocalizations.SHARE_LEVEL_COMPLETED)+" "+PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_PLAYED)
			: Localization.Localize(ExtraLocalizations.SOCIAL_POST_BEST_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+" "+Localization.Localize(ExtraLocalizations.SOCIAL_POST_CURRENT_SCORE)+" "+PlayerPrefs.GetInt(GameSettings.PP_BEST_SCORE)+" "+linkAPP;
		
		if(screenshot){
			string screenShotMessage = "";
			
			switch(network){
			case SocialNetwork.FACEBOOK: screenShotMessage = fbMessage; break;
			case SocialNetwork.TWITTER: screenShotMessage =  twMessage; break;
			}
			
			StartCoroutine(postCaptura(network, screenShotMessage));
		}
		else{
			
			
			#if UNITY_ANDROID
			if(network == SocialNetwork.TWITTER){
				AndroidTwitterManager.instance.Post(twMessage);
			}
			else if(network == SocialNetwork.FACEBOOK){
				
				SPFacebook.instance.Post (
					link: linkAPP,
					linkName: GameSettings.Instance.CurrentGameName,
					linkCaption: "Ranking",
					linkDescription: fbMessage,
					picture: iconURL
					);
			}
			
			
			#elif UNITY_IPHONE
			if(network == SocialNetwork.TWITTER)
				IOSSocialManager.instance.TwitterPost(twMessage);
			else if(network == SocialNetwork.FACEBOOK)
				IOSSocialManager.instance.FacebookPost(fbMessage);
			#endif
		}
	}
	
	private IEnumerator postCaptura(SocialNetwork network, string message) {
		
		yield return new WaitForEndOfFrame();
		
		//---
		// Create a texture the size of the screen, RGB24 format
		//---
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D( width, height, TextureFormat.RGB24, false );
		tex.ReadPixels( new Rect(0, 0, width, height), 0, 0 ); // Read screen contents into the texture
		tex.Apply();
		
		
		//---
		//Posting message with the screenshot
		//---
		#if UNITY_ANDROID
		string linkAPP = !string.IsNullOrEmpty(GameSettings.Instance.CurrentAndroidAppShortLink) ? GameSettings.Instance.CurrentAndroidAppShortLink 
			: GameSettings.Instance.CurrentAndroidAppLink;
		
		if(network == SocialNetwork.FACEBOOK){
			SPFacebook.instance.PostImage(message, tex);
		}
		else if(network == SocialNetwork.TWITTER){
			AndroidTwitterManager.instance.Post(message, tex);
		}
		
		#elif UNITY_IPHONE
		string linkAPP = !string.IsNullOrEmpty(GameSettings.Instance.CurrentIOSAppShortLink) ? GameSettings.Instance.CurrentIOSAppShortLink 
			: GameSettings.Instance.CurrentIOSAppLink;
		
		if(network == SocialNetwork.FACEBOOK){
			IOSSocialManager.Instance.FacebookPost(message, linkAPP, tex);
		}
		else if(network == SocialNetwork.TWITTER){
			IOSSocialManager.Instance.TwitterPost(message, linkAPP, tex);
		}
		#endif
		
		Destroy(tex);
	}
	
	
	
}
