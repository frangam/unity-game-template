/***************************************************************************
Project:     Trivial Cofrade
Copyright (c) Altasy
Author:       Angel Nuñez (neinei89@gmail.com)
Modified: 	Fran Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LoggerManager : PersistentSingleton<LoggerManager>
{

    //--------------------------------------
    // events & delegates
    //--------------------------------------
   

    public static event System.Action OnLoggedSuccessful = delegate { };
    public static event System.Action OnLogout = delegate { };
    public static event System.Action OnLoggedFail = delegate { };
    public static event System.Action OnCreateAccountExistNick = delegate { };
    public static event System.Action OnCreateAccountSuccessful = delegate { };
    public static event System.Action OnRestoreLastSessionSuccesFul = delegate { };
    public static event System.Action OnRestoreLastSessionFail = delegate { };
	public static event System.Action OnLogginWithFacebook = delegate { };

    //--------------------------------------
    // Private Attributes
    //--------------------------------------
    private string errorMsg = "";
    private string statusMsg = "";
    private string fbUserName = null;
    private string fbUserId = null;
    private bool isLogged = false;


    //--------------------------------------
    // Setting Attributes
    //--------------------------------------
    [Header("Time to check internet conecction, for login the user")]
    [SerializeField]
    private float time = 60f;//1 minute
    [Header("Facebook Setitngs")]
    [SerializeField]
    private bool useFacebookLogin = false;
    [SerializeField]
    private string FB_APP_ID = "1423985527901101";
    [SerializeField]
    private string[] READ_PERMISSIONS = new string[] { "read_stream", "read_friendlists" };

    //--------------------------------------
    // Getters/Setters
    //--------------------------------------
    public bool IsLogged
    {
        get
        {
            return isLogged;
        }

    }

	public bool UseFacebookLogin {
		get {
			return this.useFacebookLogin;
		}
	}

	public bool IsLoggedWithFacebook{
		get{
						return false; // userProfile != null && userProfile.profile != null && FacebookBinding.IsSessionOpen();
		}
	}
	public string FacebookAppID{
		get{ return FB_APP_ID; }
	}


//    //--------------------------------------
//    // Unity Methods
//    //--------------------------------------
//    #region Unity
//    protected override void Awake()
//    {
//        base.Awake();
//
//        if (useFacebookLogin)
//            FacebookBinding.Init(FB_APP_ID);
//    }
//
//    void Start()
//    {
//
//        if (GamedoniaBackend.INSTANCE == null)
//        {
//
//            statusMsg = "Missing Api Key/Secret. Check the README.txt for more info.";
//        }
//
//
//        checkToRegisterOrLoginUser();
//
//        InvokeRepeating("checkToRegisterOrLoginUser", 300, time);
//      
//    }
//    #endregion
//
//    //--------------------------------------
//    // Public Methods
//    //--------------------------------------
//	/// <summary>
//	/// Checks when it was possible if can login to Gamedonia
//	/// or register user on Gamedonia
//	/// 
//	/// This is useful when we has a user nickname saved in PlayerPrefs
//	/// 
//	/// Priority: Facebook > Email
//	/// </summary>
//    public void checkToRegisterOrLoginUser(){
//		if (InternetChecker.Instance.IsconnectedToInternet && !IsLogged){
//			//login with Facebook
//			if (!Application.isEditor && useFacebookLogin && PlayerPrefs.HasKey (TSettings.PP_FACEBOOK_ID)) {
//				GTDebug.log ("Logging with Facebook");
//				loginWithFace ();
//			} 
//			//login with email
//			else if (PlayerPrefs.HasKey (TSettings.PP_USER_NICK) && isSavedUserEmailCredentialsInPlayerPref ()) {
//				GTDebug.log ("Restoring last session");
//				restoreLastSession ();//intenta recuperar la sesion antigua,en caso de que la sesion este caducada inicia sesion con los datos guardados en el player pref
//			} 
//			//register user
//			else if (PlayerPrefs.HasKey (TSettings.PP_USER_NICK) && PlayerPrefs.HasKey (TSettings.PP_PLAYER_LOCAL_NAME)) {
//				GTDebug.log ("Creating a new user");
//				string playerNick = PlayerPrefs.GetString (TSettings.PP_USER_NICK);//get the user nick
//				string playerName = PlayerPrefs.GetString (TSettings.PP_PLAYER_LOCAL_NAME);//get the user name
//				createUser (playerNick, playerName);//create a new user
//			} else {
//				OnLoggedFail ();
//			}
//		} else {
//			OnLoggedFail ();
//		}
//    }
//    public void LoginWithPPdata()
//    {
//        string ppPass = PlayerPrefs.GetString(TSettings.PP_USER_LOGIN_PASS);
//        string ppEmail = PlayerPrefs.GetString(TSettings.PP_USER_LOGIN_EMAIL);
//
//        Login(ppEmail, ppPass);
//    }
//    public void Login(string email_, string pass_)
//    {
//		if (InternetChecker.Instance.IsconnectedToInternet)
//			GamedoniaUsers.LoginUserWithEmail (email_.ToLower (), pass_, OnLogin);
//		else
//			OnLoggedFail ();
//    }
//    public bool isSavedUserEmailCredentialsInPlayerPref()
//    {
//
//        return PlayerPrefs.HasKey(TSettings.PP_USER_LOGIN_PASS) && PlayerPrefs.HasKey(TSettings.PP_USER_LOGIN_EMAIL);
//    }
//
//    public void loginWithFace()
//    {
//        if (!Application.isEditor)
//        {
//            statusMsg = "Initiating Facebook session...";
//			GTDebug.log (statusMsg);
//            FacebookBinding.OpenSessionWithReadPermissions(READ_PERMISSIONS, OnFacebookOpenSession);
//        }
//        else
//        {
//            errorMsg = "Facebook won't work on Unity Editor, try it on a device.";
//			GTDebug.logError (errorMsg);
//        }
//    }
//
//    public void GetUSerInfo()
//    {
//		GTDebug.log ("Getting user info and cancelling check to register or login user");
//		CancelInvoke("checkToRegisterOrLoginUser");
//        GamedoniaUsers.GetMe(OnGetMe);
//    }
//	public void createUser(string nick, string name)
//    {
//        int userCount = 0;
//        GamedoniaData.Search("appinfo", "{}", delegate (bool success, IList list) {
//            if (success)
//            {
//				GTDebug.log("Creating user (nickname: "+nick+")...");
//
//                Dictionary<string, object> info = (Dictionary<string, object>)list[0];//get the first
//                userCount = Int32.Parse(info["totalusers"].ToString())+1;
//                createCredentialAndLogin(userCount, nick, name);
//            }
//            else
//            {
//				OnLoggedFail ();
//            }
//        });
//
//
//
//    }
//    public void logout()
//    {
//        GamedoniaUsers.LogoutUser(delegate (bool success) {
//            if (success)
//            {
//                OnLogout();
//            }
//            else
//            {
//                errorMsg = GamedoniaBackend.getLastError().ToString();
//                GTDebug.log(errorMsg);
//            }
//        });
//    }
//
//    public void restoreLastSession()
//    {
//        GamedoniaUsers.LoginUserWithSessionToken(delegate (bool success) {
//            if (success)
//            {
//                GetUSerInfo();
//                GTDebug.log("<color=green> LoogerManager !</color> :" + "login using gc Login TOken");
//                OnRestoreLastSessionSuccesFul();
//
//                CancelInvoke("checkToRegisterOrLoginUser");
//            }
//            else
//            {
//                OnRestoreLastSessionFail();
//                GTDebug.log("<color=red> LoogerManager !</color> : Fail to restore the last session");
//                LoginWithPPdata();
//            }
//        });
//    }
//
//
//    //--------------------------------------
//    // Private Methods
//    //--------------------------------------
//
//	private void OnUpdatedUserAccount(bool success){
//		if (success) {
//			GTDebug.log("Updated user account:" + "login Ok");
//			OnLoggedSuccessful ();
//		} else {
//			errorMsg = GamedoniaBackend.getLastError().ToString();
//			GTDebug.logError("LogerManager error on updating user account: " + errorMsg);
//		}
//	}
//
//    private void OnLogin(bool success)
//    {
//
//        statusMsg = "";
//        if (success)
//        {
//			GTDebug.log("<color=red> LoogerManager !</color> :" + "login Ok");
//            isLogged = true;
//            GetUSerInfo();
//        }
//        else
//        {
//            OnLoggedFail();
//            isLogged = false;
//            errorMsg = GamedoniaBackend.getLastError().ToString();
//			GTDebug.logError("<color=red> LoogerManager error on login</color> :" + errorMsg);
//        }
//
//    }
//
//    private void OnGetMe(bool success, GDUserProfile userProfile)
//    {
//
//        if (success)
//        {
//			
//            this.userProfile = userProfile;
//            GTDebug.log("LogerManager success On GetMe: user profile load correctly");
//            isLogged = true;
//
//			if (PlayerPrefs.HasKey (TSettings.PP_USER_NICK) && this.userProfile != null && this.userProfile.profile != null && this.userProfile.profile.ContainsKey ("nickname")
//				&& !this.userProfile.profile ["nickname"].ToString().Equals (PlayerPrefs.GetString (TSettings.PP_USER_NICK)))
//				PlayerPrefs.SetString (TSettings.PP_USER_NICK, this.userProfile.profile["nickname"].ToString());
//
//
//			bool canUpdateUserAccount = checkIfUpdateUserDataWhenLogingWithFacebook ();
//
//			if (!canUpdateUserAccount) {
//				getCoinsAndGemsFromServer ();
//
//				if (!userProfile.profile.ContainsKey ("multiplayer")) {
//					userProfile.profile.Add ("multiplayer", true);
//					GamedoniaUsers.UpdateUser (userProfile.profile, OnUpdatedUserAccount);
//				}
//				else
//					OnLoggedSuccessful ();
//			}
//        }
//        else
//        {
//            this.userProfile = null;
//
//            LoginWithPPdata();
//            errorMsg = GamedoniaBackend.getLastError().ToString();
//			GTDebug.log("<color=red> LoogerManager On GetMe error</color> :" + errorMsg);
//			OnLoggedFail();
//        }
//
//    }
//
//	private bool checkIfUpdateUserDataWhenLogingWithFacebook(){
//		bool canUpdateUserAccount = (PlayerPrefs.HasKey (TSettings.PP_FACEBOOK_ID) || !PlayerPrefs.HasKey (TSettings.PP_FACEBOOK_ID)) 
//			&& userProfile != null && userProfile.profile != null && !userProfile.profile.ContainsKey("multiplayer");
//		
//		GTDebug.log ("checking if update user data when loging with facebook ? "+canUpdateUserAccount);
//		GTDebug.log ("Logged with facebook ? "+IsLoggedWithFacebook);
//		GTDebug.log ("saved facebook user id ? "+PlayerPrefs.HasKey (TSettings.PP_FACEBOOK_ID));
//		GTDebug.log ("user profile ? "+ (userProfile != null));
//		GTDebug.log ("userProfile.profile ? "+ (userProfile.profile != null));
//		GTDebug.log ("Has multiplayer flag ? " + userProfile.profile.ContainsKey ("multiplayer"));
//
//		if(userProfile.profile.ContainsKey("name"))
//			GTDebug.log ("Facebook user name at Gamedonia: "+ userProfile.profile["name"]);
//	
//		GTDebug.log ("Facebook user name from Facebook internet: "+ fbUserName);
//
//		if(IsLoggedWithFacebook && !PlayerPrefs.HasKey (TSettings.PP_FACEBOOK_ID)){
//			GTDebug.log ("Login with facebook");
//			loginWithFace ();
//		}
//		else if (canUpdateUserAccount) {
//			string nickname = "";
//
//
//			//update facebook name
//			if (!string.IsNullOrEmpty (fbUserName)
//				&& (!userProfile.profile.ContainsKey("name") || (userProfile.profile.ContainsKey("name") && !userProfile.profile["name"].Equals(fbUserName)))) {
//				if(!userProfile.profile.ContainsKey("name"))
//					userProfile.profile.Add ("name", fbUserName);
//				else
//					userProfile.profile["name"] = fbUserName;
//				
//				GTDebug.log ("Updating Gamedonia user with facebook name: " + fbUserName);
//			}
//
//			GTDebug.log ("Current Profile: has name? "+ userProfile.profile.ContainsKey ("name"));
//			GTDebug.log ("Current Profile: has nickname? "+ userProfile.profile.ContainsKey ("nickname"));
//			GTDebug.log ("Current Profile: has registerDate? "+ userProfile.profile.ContainsKey ("registerDate"));
//
//			//create a nickname
//			if (!userProfile.profile.ContainsKey("nickname")) {
//				if (!PlayerPrefs.HasKey (TSettings.PP_USER_NICK)) {
//					nickname = fbUserName.Replace (" ", "");
//					userProfile.profile.Add ("nickname", nickname.ToLower ());
//				} else {
//					nickname = PlayerPrefs.GetString (TSettings.PP_USER_NICK);
//					userProfile.profile.Add ("nickname", nickname);
//				}
//			}
//
//			//registerDate
//			if (!userProfile.profile.ContainsKey ("registerDate")) {
//				userProfile.profile.Add ("registerDate", DateTime.Now);
//			}
//
//			//coins
//			if(userProfile.profile.ContainsKey(GameMoneyManager.MONEY_COINS)){
//				long coinsOnServer;
//				string coinsOnServString = userProfile.profile [GameMoneyManager.MONEY_COINS].ToString ();
//
//				GTDebug.log ("Trying to get coins from server");
//
//				if(long.TryParse(coinsOnServString, out coinsOnServer)){
//					long actualCoinsOnDevice = GameMoneyManager.Instance.getTotalMoney();
//
//					GTDebug.log ("coins on device: " + actualCoinsOnDevice+", coins on server: "+coinsOnServer.ToString());
//
//					if(coinsOnServer > 0 && coinsOnServer < actualCoinsOnDevice)
//						userProfile.profile[GameMoneyManager.MONEY_COINS] = actualCoinsOnDevice;
//				}
//
//			}
//
//			//gems
//			if(userProfile.profile.ContainsKey(GameMoneyManager.MONEY_GEMS)){
//				long gemsOnServer;
//
//				GTDebug.log ("Trying to get gems from server");
//
//				if(long.TryParse(userProfile.profile[GameMoneyManager.MONEY_GEMS].ToString(), out gemsOnServer)){
//					long actualGemsOnDevice = GameMoneyManager.Instance.getTotalGems();
//
//					if(gemsOnServer < actualGemsOnDevice)
//						userProfile.profile[GameMoneyManager.MONEY_GEMS] = actualGemsOnDevice;
//				}			
//			}
//
//			//score
//			if(userProfile.profile.ContainsKey("score")){
//				long scoreOnServer;
//
//				GTDebug.log ("Trying to get score from server");
//
//				if(long.TryParse(userProfile.profile["score"].ToString(), out scoreOnServer)){
//					long actualScoreOnDevice = ScoresHandler.Instance.getBestScoreByIndex (0);
//
//					if(scoreOnServer < actualScoreOnDevice)
//						userProfile.profile["score"] = actualScoreOnDevice;
//				}			
//			}
//
//
//			canUpdateUserAccount = userProfile.profile.Count > 0;
//			GTDebug.log ("Can uptade user with facebook credentials ? " +canUpdateUserAccount);
//
//			//flag for users has multiplayer version
//			if (!userProfile.profile.ContainsKey ("multiplayer")) {
//				userProfile.profile.Add ("multiplayer", true);
//			}
//
//			if (userProfile.profile.Count > 0) {
//				GTDebug.log ("Updating Gamedonia user with facebook credentials. Total params: " + userProfile.profile.Count + ". Name: " + fbUserName + ", nick: " + nickname);
//				GamedoniaUsers.UpdateUser (userProfile.profile, OnUpdatedUserAccount);
//			} 
//		}
//
//		return canUpdateUserAccount;
//	}
//
//	private void getCoinsAndGemsFromServer(){
//		if (!PlayerPrefs.HasKey (TSettings.PP_FIRST_GETTING_COINS_FROM_SERVER) && PlayerPrefs.GetInt (TSettings.PP_FIRST_GETTING_COINS_FROM_SERVER) == 0) {
//		
//			//coins
//			if (userProfile.profile.ContainsKey (GameMoneyManager.MONEY_COINS)) {
//				long coinsOnServer;
//				string coinsOnServString = userProfile.profile [GameMoneyManager.MONEY_COINS].ToString ();
//
//				GTDebug.log ("Trying to get coins from server");
//
//				if (long.TryParse (coinsOnServString, out coinsOnServer)) {
//					long actualCoinsOnDevice = GameMoneyManager.Instance.getTotalMoney ();
//
//					GTDebug.log ("coins on device: " + actualCoinsOnDevice + ", coins on server: " + coinsOnServer.ToString ());
//
//					if (coinsOnServer > 0 && coinsOnServer > actualCoinsOnDevice) {
//						GameMoneyManager.Instance.addMoney (coinsOnServer);
//						PlayerPrefs.SetInt (TSettings.PP_FIRST_GETTING_COINS_FROM_SERVER, 1);
//					}
//				}
//
//			}
//
//			//gems
//			if (userProfile.profile.ContainsKey (GameMoneyManager.MONEY_GEMS)) {
//				long gemsOnServer;
//
//				GTDebug.log ("Trying to get gems from server");
//
//				if (long.TryParse (userProfile.profile [GameMoneyManager.MONEY_GEMS].ToString (), out gemsOnServer)) {
//					long actualGemsOnDevice = GameMoneyManager.Instance.getTotalGems ();
//
//					if (gemsOnServer > actualGemsOnDevice)
//						GameMoneyManager.Instance.addMoney (gemsOnServer);
//				}			
//			}
//
//		}
//	}
//
//    private string getRandomPasswrod()
//    {
//
//        string lettres = "0123456789abcdefghijklmnopqrstuvwxABCDEFGHIJKLMNOPQRSTUVWXYZ";
//        string res = "";
//
//        for (int i = 0; i < 15; i++)
//        {
//            int a = UnityEngine.Random.Range(0, lettres.Length);
//            res += lettres[a];
//        }
//
//        return res;
//    }
//
//	private void createCredentialAndLogin(int usersCounts, string nickname, string name="")
//    {
//		if (!string.IsNullOrEmpty (nickname)) {
//			int random = UnityEngine.Random.Range (0, 256) * UnityEngine.Random.Range (0, 256);
//			string email = nickname.ToString () + usersCounts.ToString () + random.ToString () + TSettings.EMAIL_DOMAIN;
//			string password = getRandomPasswrod ();
//
//			GTDebug.log ("Creating credentials");
//			GTDebug.log ("Creating credentials- Nickname: " + nickname + ", Name: " + name + ", Email: " + email);
//			GTDebug.log ("User count: " + usersCounts);
//
//			Credentials credentials = new Credentials ();
//			credentials.email = email.ToLower ();
//			credentials.password = password;
//			GDUser user = new GDUser ();
//			user.credentials = credentials;
//			user.profile.Add ("nickname", nickname);
//
//			if (!string.IsNullOrEmpty (name))
//				user.profile.Add ("name", name);
//
//			if (UseFacebookLogin) {
//				if (!string.IsNullOrEmpty (fbUserId))
//					user.profile.Add ("fb_id", fbUserId);
//			}
//				
//
//			user.profile.Add ("score", (long)0);
//			user.profile.Add (GameMoneyManager.MONEY_COINS, GameSettings.Instance.INITIAL_MONEY);
//			user.profile.Add (GameMoneyManager.MONEY_GEMS, GameSettings.Instance.INITIAL_GEMS);
//			user.profile.Add ("registerDate", DateTime.Now);
//
//
//			saveUSerDataInPlyerPref (email, password);//store data in player prefs
//			GamedoniaUsers.CreateUser (user, OnCreateUser);
//		}
//    }
//    private void saveUSerDataInPlyerPref(string email, string pass)
//    {
//
//        PlayerPrefs.SetString(TSettings.PP_USER_LOGIN_EMAIL, email);
//        PlayerPrefs.SetString(TSettings.PP_USER_LOGIN_PASS, pass);
//
//    }
//
//    private string getEmail()
//    {
//        return PlayerPrefs.GetString(TSettings.PP_USER_LOGIN_EMAIL);
//    }
//    private string getPass()
//    {
//        return PlayerPrefs.GetString(TSettings.PP_USER_LOGIN_PASS);
//    }
//    private void OnCreateUser(bool success)
//    {
//
//        if (success)
//        {
//            OnCreateAccountSuccessful();
//			GTDebug.log("<color=green> LoogerManager !</color> :" + "User created succesful!!!!!");
//            GamedoniaUsers.LoginUserWithEmail(getEmail().ToLower(), getPass(), OnLogin);
//        }
//        else
//        {
//            errorMsg = GamedoniaBackend.getLastError().ToString();
//            //remove pp data
//            PlayerPrefs.DeleteKey(TSettings.PP_USER_LOGIN_EMAIL);
//            PlayerPrefs.DeleteKey(TSettings.PP_USER_LOGIN_PASS);
//			GTDebug.log("<color=red> LoogerManager error on Creating user. </color> :" + errorMsg);
//        }
//    }
//
//    private void OnFacebookOpenSession(bool success, bool userCancelled, string message)
//    {
//
//        if (success)
//        {
//			OnLogginWithFacebook ();
//            statusMsg = "Recovering Facebook profile...";
//            FacebookBinding.RequestWithGraphPath("/me", null, "GET", OnFacebookMe);
//
//			GTDebug.log ("Facebook Session Opened. Message: "+statusMsg);
//        }
//        else
//        {
//            errorMsg = "Unable to open session with Facebook";
//			GTDebug.logError ("Facebook Open Session Failed. Error: "+errorMsg);
//        }
//    }
//
//    private void OnFacebookMe(IDictionary data)
//    {
//
//        statusMsg = "Initiating Gamedonia session...";
//        fbUserId = data["id"] as string;
//        fbUserName = data["name"] as string;
//		string accesToken = FacebookBinding.GetAccessToken ();
//
//		GTDebug.log("Facebook AccessToken: " + accesToken + " fbuid: " + fbUserId);
//
//        Dictionary<string, object> facebookCredentials = new Dictionary<string, object>();
//        facebookCredentials.Add("fb_uid", fbUserId);
//		facebookCredentials.Add("fb_access_token", accesToken);
//
//
//		GTDebug.log ("Saving in PlayerPrefs Facebook user id, name and token...");
//		PlayerPrefs.SetString (TSettings.PP_PLAYER_LOCAL_NAME, fbUserName);
//		PlayerPrefs.SetString (TSettings.PP_FACEBOOK_ID, fbUserId);
//		PlayerPrefs.SetString (TSettings.PP_FACEBOOK_ACCES_TOKEN, accesToken);
//			
//
//		//link current user gamedonia account with this facebook account
//		if (PlayerPrefs.HasKey (TSettings.PP_USER_NICK) && isSavedUserEmailCredentialsInPlayerPref () && !PlayerPrefs.HasKey (TSettings.PP_FACEBOOK_ID)) {
//			GTDebug.log ("Updating Gamedonia User... Linking with Facebook account. FB user name: " + fbUserName);
//			linkAccountWithFacebook (fbUserId, accesToken);
//		} else {
//			if (!PlayerPrefs.HasKey (TSettings.PP_USER_NICK))
//				PlayerPrefs.SetString (TSettings.PP_USER_NICK, fbUserName.ToLower ().Replace (" ", ""));
//
//			GTDebug.log ("Authenticating to gamedonia with Facebook credentials...");
//			GamedoniaUsers.Authenticate (GamedoniaBackend.CredentialsType.FACEBOOK, facebookCredentials, OnFacebookLogin);
//		}
//    }
//
//	private void OnFacebookLogin(bool success){
//		if (success){
//			GetUSerInfo ();
//			GTDebug.log ("Facebook Logged.. Getting user info...");
//		}
//		else{
//			errorMsg = GamedoniaBackend.getLastError().ToString();
//			GTDebug.logError ("Facebook Login Failed. Error: "+errorMsg);
//		}
//
//	}
//
//	private void linkAccountWithFacebook(string userID, string accesToken){
//		Credentials linkedcredentials = new Credentials();
//		linkedcredentials.fb_uid = userID;
//		linkedcredentials.fb_access_token = accesToken; 
//
//		GamedoniaUsers.LinkUser(linkedcredentials, delegate (bool success, GDUserProfile profiles){
//			if (success){
//				GTDebug.log("Success linking account with facebook");
//
//
//				Dictionary<string, object> facebookCredentials = new Dictionary<string, object>();
//				facebookCredentials.Add("fb_uid", fbUserId);
//				facebookCredentials.Add("fb_access_token", accesToken);
//
//				GTDebug.log ("Authenticating to gamedonia with Facebook credentials...");
//				GamedoniaUsers.Authenticate (GamedoniaBackend.CredentialsType.FACEBOOK, facebookCredentials, OnFacebookLogin);
//			}
//			else {
//				GTDebug.logError("Error when linking account with facebook");
//			}
//		});
//	}
}
