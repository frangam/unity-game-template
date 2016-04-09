/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;
//using Heyzap;

public class AdsHandler : PersistentSingleton<AdsHandler> {
	//--------------------------------------
	// Constants
	//--------------------------------------
	
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	public static event System.Action OnIncentivedVideoFinished = delegate{};
    public static event System.Action OnVideoOrIntertisialFinished = delegate { };

    //--------------------------------------
    // Setting Attributes
    //--------------------------------------
	public AdSize banSize = AdSize.SmartBanner;
	public AdPosition banPos = AdPosition.Top;
	public bool test = true;
	public bool useNativeAdmob = false;
	public bool useNativeUnityAds = false;
	public bool useUnityAdsForVideoRewardOnly = true;
	public bool useAdmobVideoRewardAds = false;
	public string admobBannerAndroidID;
	public string admobInterstitialAndroidID;
	public string admobVideoRewardAndroidID;
	public string admobBannerIOSID;
	public string admobInterstitialIOSID;
	public string admobVideoRewardIOSID;
	public string unityAndroidID = "";
	public string unityiOSID = "";

//	public List<string> testDevices = new List<string> (){ "7ED25D61C9899E1848EEADFED7420538", "ed2309f7c7297dea857d6763ff95d2d7" , "f8eaba0e0e5dfa633693be9e948b5a75",
	//		"b3f4f866e8f4c67aea4889f714d78220d2ab3f4f", "42D2D9ECF99580DD6CF4F6CB797B5059" };
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private const float IN_APP_WAIT_INITIALIZATION_TIME = 15; 
	private bool IsInterstisialsAdReady = false;
	private static Dictionary<string, GoogleMobileAdBanner> _registerdBanners = null;
	private float currentTime;
	private bool googleAdmobInited = false;
	private bool adcolonyInited = false;
	private bool hasPausedGame = false;
	private bool inited = false;
	private bool waitingFinishInitInApps = false;
	private bool initedHeyzap = false;
	private bool usingHeyzapOnlyForAdmobBanner;
	private BannerView admobBannerView;
	private InterstitialAd admobInterstitialAd;
	private RewardBasedVideoAd admobRewardVideoAd;
	private bool showingFullScreenAd = false;

	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public bool HasPausedGame {
		get {
			return this.hasPausedGame;
		}
	}
	public static Dictionary<string, GoogleMobileAdBanner> registerdBanners{
		get{
			if (_registerdBanners == null){
				_registerdBanners = new Dictionary<string, GoogleMobileAdBanner>();
			}
			return _registerdBanners;
		}
	}
	public string UniqueBannerID{
		get{
			return Application.loadedLevelName + "_" + this.gameObject.name + "_"+ Time.deltaTime.ToString();
		}
	}
	public bool ShowingFullScreenAd {
		get {
			return this.showingFullScreenAd;
		}
	}
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	#if  (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_EDITOR)
	public bool canShowAd(AdType type = AdType.BANNER, string tag = "default"){
		bool hasInternet = InternetChecker.Instance.IsconnectedToInternet;
		bool canShow = hasInternet 
			&& (useNativeUnityAds || useNativeAdmob || (!useNativeAdmob && !useNativeUnityAds && !string.IsNullOrEmpty(GameSettings.Instance.heyZapID)))
			&& (type == AdType.VIDEO_V4VC //can show if the ad type is video with and a reward even player had purchase for quitting ads 
			    || (type != AdType.VIDEO_V4VC && !GameSettings.Instance.IS_PRO_VERSION));



		if(canShow){
			switch(type){
			case AdType.INTERSTITIAL: 
//				if (!useNativeAdmob && !useNativeUnityAds)
//					canShow = HZInterstitialAd.IsAvailable (tag);
//				else
					if (useNativeAdmob && (useNativeUnityAds && !useUnityAdsForVideoRewardOnly))
					canShow = admobInterstitialAd.IsLoaded () && Advertisement.IsReady ("defaultZone");
				else if (useNativeAdmob)
					canShow = admobInterstitialAd.IsLoaded ();
				else if(useNativeUnityAds && !useUnityAdsForVideoRewardOnly)
					canShow = Advertisement.IsReady ("defaultZone");
				break;
			case AdType.VIDEO:
//				if (!useNativeAdmob && !useNativeUnityAds) {
//					HZVideoAd.fetch (); 
//					canShow = HZVideoAd.IsAvailable (tag); 
//				}
//				else 
					if (useNativeAdmob && (useNativeUnityAds && !useUnityAdsForVideoRewardOnly))
					canShow = admobInterstitialAd.IsLoaded() && Advertisement.IsReady ("defaultZone");
				else if (useNativeAdmob)
					canShow = admobInterstitialAd.IsLoaded();
				else if(useNativeUnityAds && !useUnityAdsForVideoRewardOnly)
					canShow = Advertisement.IsReady ("defaultZone");
				break;
			case AdType.VIDEO_V4VC: 
//				if (!useNativeUnityAds) {
//					HZIncentivizedAd.fetch (); 
//					canShow = HZIncentivizedAd.IsAvailable (tag); 
//				}
//				else 
					if(useNativeAdmob && useAdmobVideoRewardAds){
					canShow = admobRewardVideoAd.IsLoaded ();
				}
				else if(useNativeUnityAds && useUnityAdsForVideoRewardOnly){
					canShow = Advertisement.IsReady ("rewardedVideoZone");
				}
				break;
			case AdType.RANDOM_INTERSTITIAL_VIDEO:
				canShow = canShowAdRandom ();
				break;
			}
		}

		GTDebug.log ("Can show AD TYPE: " + type + " ? " + canShow);
		
		return canShow;
	}

    public bool canShowAdRandom()
    {
        bool hasInternet = InternetChecker.Instance.IsconnectedToInternet;
        bool hasPro = GameSettings.Instance.IS_PRO_VERSION;
        bool canShow = hasInternet 
			&& ((useNativeUnityAds && Advertisement.IsReady ("defaultZone")) 
				|| (useNativeAdmob && admobInterstitialAd != null && admobInterstitialAd.IsLoaded()) 
				|| (!useNativeAdmob && !useNativeUnityAds && !string.IsNullOrEmpty(GameSettings.Instance.heyZapID))) 
			&& !hasPro;

        return canShow;
    }
    protected void Start(){
		StartCoroutine(handleInitializationWhenUsingInAppBilling());
	}
	
	
	private IEnumerator handleInitializationWhenUsingInAppBilling(){
		waitingFinishInitInApps = true;
		yield return null;
		
		currentTime = Time.timeSinceLevelLoad;
		
		//		GTDebug.log("Current time: " + currentTime);
		
		if(GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE 
		   && (currentTime < IN_APP_WAIT_INITIALIZATION_TIME 
		    && (!CoreIAPManager.Instance.IsInited 
		    || (CoreIAPManager.Instance.IsInited 
		    && BaseGameScreenController.Instance.Section == GameSection.LOAD_SCREEN 
		    && GameLoaderManager.Instance.InAppNeedRestoreProducts && !GameLoaderManager.Instance.InAppAllProductsRestored
		    )
		    )
		    )
		   )
			StartCoroutine(handleInitializationWhenUsingInAppBilling());
		
		//long time waiting to init, now we can init ads handler
		else
			init();
	}

//	private void initHeyzap(){
//		GTDebug.log ("Initing Heyzap from AdsHandler...");
//
//		// Your Publisher ID is: 1d3b3bbd9f2a398c60822451a696ffea
//		HeyzapAds.Start(GameSettings.Instance.heyZapID, HeyzapAds.FLAG_NO_OPTIONS);
//
//		//--------------------------------------
//		//  HEYZAP PLUGIN EVENT LISTNERS
//		//--------------------------------------
//		//------
//		//BANNER ADS
//		//------
//		HZBannerAd.SetDisplayListener(delegate(string adState, string adTag){
//			if (adState == "loaded") {
//				// Do something when the banner ad is loaded
//
//				//ANALYTICS
//				GTAnalyticsHandler.Instance.logEvent(GAEventCategories.BANNER_AD, GAEventActions.SHOWN);
//			}
//			if (adState == "error") {
//				// Do something when the banner ad fails to load (they can fail when refreshing after successfully loading)
//
//				//ANALYTICS
//				GTAnalyticsHandler.Instance.logEvent(GAEventCategories.BANNER_AD, GAEventActions.FAILED);
//			}
//			if (adState == "click") {
//				// Do something when the banner ad is clicked, like pause your game
//
//				//ANALYTICS
//				GTAnalyticsHandler.Instance.logEvent(GAEventCategories.BANNER_AD, GAEventActions.PRESSED);
//			}
//		});
//
//
//		//------
//		//Interstitial Ads
//		//------
//		HZInterstitialAd.SetDisplayListener(delegate(string adState, string adTag){
//			if ( adState.Equals("show") ) {
//				// Do something when the ad shows, like pause your game
//				pauseGame();
//			}
//			if ( adState.Equals("hide") ) {
//				// Do something after the ad hides itself
//				pauseGame(false);
//				OnVideoOrIntertisialFinished();
//			}
//			if ( adState.Equals("click") ) {
//				// Do something when an ad is clicked on
//
//				//ANALYTICS
//				GTAnalyticsHandler.Instance.logEvent(GAEventCategories.INTERSTITIAL_AD, GAEventActions.PRESSED);
//			}
//			if ( adState.Equals("failed") ) {
//				// Do something when an ad fails to show
//				pauseGame(false);
//
//				//ANALYTICS
//				GTAnalyticsHandler.Instance.logEvent(GAEventCategories.INTERSTITIAL_AD, GAEventActions.FAILED);
//			}
//			if ( adState.Equals("available") ) {
//				// Do something when an ad has successfully been fetched
//			}
//			if ( adState.Equals("fetch_failed") ) {
//				// Do something when an ad did not fetch
//			}
//			if ( adState.Equals("audio_starting") ) {
//				// The ad being shown will use audio. Mute any background music
//			}
//			if ( adState.Equals("audio_finished") ) {
//				// The ad being shown has finished using audio.
//				// You can resume any background music.
//			}
//		});
//
//
//		//------
//		//Reward Videos
//		//------
//		HZIncentivizedAd.SetDisplayListener(delegate(string adState, string adTag){
//			if (adState.Equals ("show")) {
//				// Do something when the ad shows, like pause your game
//				pauseGame();
//
//				//ANALYTICS
//				GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_REWARD_AD, GAEventActions.SHOWN);
//			}
//			if (adState.Equals ("hide")) {
//				// Do something after the ad hides itself
//				//				pauseGame(false);
//			}
//			if (adState.Equals ("click")) {
//				// Do something when an ad is clicked on
//
//				//ANALYTICS
//				GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_REWARD_AD, GAEventActions.PRESSED);
//			}
//			if (adState.Equals ("failed")) {
//				// Do something when an ad fails to show
//
//				//ANALYTICS
//				GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_REWARD_AD, GAEventActions.FAILED);
//			}
//			if (adState.Equals ("available")) {
//				// Do something when an ad has successfully been fetched
//			}
//			if (adState.Equals ("fetch_failed")) {
//				// Do something when an ad did not fetch
//			}
//			if (adState.Equals ("incentivized_result_complete")) {
//				// The user has watched the entire video and should be given a reward.
//				pauseGame(false);
//				OnIncentivedVideoFinished();
//
//				//ANALYTICS
//				GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_REWARD_AD, GAEventActions.COMPLETED);
//			}
//			if (adState.Equals ("incentivized_result_incomplete")) {
//				// The user did not watch the entire video and should not be given a reward.
//				pauseGame(false);
//
//				//ANALYTICS
//				GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_REWARD_AD, GAEventActions.INCOMPLETED);
//			}
//		});
//
//
//		//------
//		// Videos
//		//------
//		HZVideoAd.SetDisplayListener( delegate(string adState, string adTag){
//			if (adState.Equals ("show")) {
//				// Do something when the ad shows, like pause your game
//				pauseGame();
//			}
//			if (adState.Equals ("hide")) {
//				// Do something after the ad hides itself
//				pauseGame(false);
//				OnVideoOrIntertisialFinished();
//			}
//			if (adState.Equals ("click")) {
//				// Do something when an ad is clicked on
//
//				//ANALYTICS
//				GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_AD, GAEventActions.PRESSED);
//			}
//			if (adState.Equals ("failed")) {
//				// Do something when an ad fails to show
//
//				//ANALYTICS
//				GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_AD, GAEventActions.FAILED);
//			}
//			if (adState.Equals ("available")) {
//				// Do something when an ad has successfully been fetched
//			}
//			if (adState.Equals ("fetch_failed")) {
//				// Do something when an ad did not fetch
//			}
//		});
//
////		if (!useNativeAdmob)
////			CreateBanner();
//
//
//		initedHeyzap = true;
//
//		//For Test purposes
//		if(test)
//			HeyzapAds.showMediationTestSuite();
//	}
	
	private void init(){
		GTDebug.log ("Initializing Ads..");

		if(useNativeUnityAds){
	#if UNITY_ANDROID
			Advertisement.Initialize (unityAndroidID);
	#elif UNITY_IPHONE
			Advertisement.Initialize (unityiOSID);
	#endif
		}

		if (useNativeAdmob) {
//			GoogleMobileAd.Init ();

			initNativeAdmob ();
		}

//		if(!useNativeUnityAds || !useNativeAdmob){
//			initHeyzap ();
//		}
	}

	private void initNativeAdmob(bool onlyInterstitialAndVideo = false){
		// Create an ad request.
		AdRequest request = new AdRequest.Builder()
			.AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
			.AddTestDevice("7ED25D61C9899E1848EEADFED7420538")  // Nexus 10 Fran
			.AddTestDevice("ed2309f7c7297dea857d6763ff95d2d7")  //  iPhone5 Fran
			.AddTestDevice("f8eaba0e0e5dfa633693be9e948b5a75")  // iPhone 4S Fran
			.AddTestDevice("b3f4f866e8f4c67aea4889f714d78220d2ab3f4f")  // WP8 Prestigio
			.AddTestDevice("42D2D9ECF99580DD6CF4F6CB797B5059")  // Moto G Fran
			.Build();

		if(!onlyInterstitialAndVideo)
			initAdmobBanner (request);
		
		initAdmobInterstitialAd (request);

		if(useAdmobVideoRewardAds)
			initAdmobRewardedVideo (request);
	}

	private void initAdmobBanner(AdRequest request){
		string adUnitId = "";
	#if UNITY_ANDROID
		adUnitId = admobBannerAndroidID; 
	#elif UNITY_IPHONE
		adUnitId = admobBannerIOSID;
	#endif

		admobBannerView = new BannerView(adUnitId, banSize, banPos);

		// Called when an ad request has successfully loaded.
		admobBannerView.OnAdLoaded += OnAdMobBannerLoaded;
		//		// Called when an ad request failed to load.
		//		bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when an ad is clicked.
		admobBannerView.OnAdOpening += OnAdmobBannerOpened;
		//		// Called when the user returned from the app after an ad click.
		//		bannerView.OnAdClosed += HandleOnAdClosed;
		//		// Called when the ad click caused the user to leave the application.
		//		bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;



		// Load the banner with the request.
		admobBannerView.LoadAd(request);
	}

	private void initAdmobInterstitialAd(AdRequest request){
		string adUnitId = "unexpected_platform";
	#if UNITY_ANDROID
		adUnitId = admobInterstitialAndroidID; 
	#elif UNITY_IPHONE
		adUnitId = admobInterstitialIOSID;
	#endif

		// Initialize an InterstitialAd.
		admobInterstitialAd = new InterstitialAd(adUnitId);

		// Load the interstitial with the request.
		admobInterstitialAd.LoadAd(request);

		admobInterstitialAd.OnAdOpening += OnAdmobInterstitialOpened;
		admobInterstitialAd.OnAdClosed += OnAdmobInterstitialClosed;
		admobInterstitialAd.OnAdFailedToLoad += OnAdmobInterstitialFailedToLoad;

	}

	private void initAdmobRewardedVideo(AdRequest request){
		string adUnitId = "unexpected_platform";
	#if UNITY_ANDROID
		adUnitId = admobVideoRewardAndroidID; 
	#elif UNITY_IPHONE
		adUnitId = admobVideoRewardIOSID;
	#endif
		
		admobRewardVideoAd = RewardBasedVideoAd.Instance;
		admobRewardVideoAd.LoadAd (request, adUnitId);

		admobRewardVideoAd.OnAdClosed += OnAdmobRewardVideoClosed;
		admobRewardVideoAd.OnAdOpening += OnAdmobRewardVideoOpening;
		admobRewardVideoAd.OnAdRewarded += OnAdmobRewardVideoRewarded;
		admobRewardVideoAd.OnAdFailedToLoad += OnAdmobRewardVideoFailedToLoad;
	}
	
	
	private void pauseGame(bool pause = true, bool alwaysMute = true){
		//alway mute sound and music when show an ad
		if(alwaysMute)
			BaseSoundManager.Instance.muteOrActiveAllOncesMuteOncesActiveAndPlayOrStopAfter();
		
		//pausing / unpausing the game
		//if we have paused or if we have not paused and game was not paused
		if(BaseGameScreenController.Instance.Section == GameSection.GAME 
		   && (hasPausedGame || (!hasPausedGame && !GameController.Instance.Manager.Paused))){
			if(pause)
				hasPausedGame = pause;
			
			if(!alwaysMute)
				BaseSoundManager.Instance.muteOrActiveAllOncesMuteOncesActiveAndPlayOrStopAfter();
			
			GameController.Instance.Manager.Paused = pause;
			
			if(!pause)
				hasPausedGame = pause;
		}
		else if(BaseGameScreenController.Instance.Section != GameSection.GAME
				&& (hasPausedGame || (!hasPausedGame && Time.timeScale != 0f))){
			if(pause)
				hasPausedGame = pause;
			
			if(!alwaysMute)
				BaseSoundManager.Instance.muteOrActiveAllOncesMuteOncesActiveAndPlayOrStopAfter();
			
			Time.timeScale = pause ? 0f: 1f;
			
			if(!pause)
				hasPausedGame = pause;
		}
		
		
		if(!pause)
			hasPausedGame = false;

        //TODO nei,trhow the finish event on test
        #if UNITY_EDITOR
                OnIncentivedVideoFinished();
        #endif

    }
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	public void OnAdMobBannerLoaded(object sender, EventArgs args){
		if (admobBannerView != null) {
			admobBannerView.Show ();
		}
	}
	public void OnAdmobBannerOpened(object sender, EventArgs args){
		GTDebug.log("OnAdLoaded event received.");
		// Handle the ad loaded event.
	}

	public void OnAdmobBannerFailedToLoad(object sender, AdFailedToLoadEventArgs args){
		GTDebug.logErrorAlways("Banner Failed to load: " + args.Message);
		// Handle the ad failed to load event.
	}
	public void OnAdmobInterstitialOpened(object sender, EventArgs args){
		GTDebug.log("On Interstitial Opened event received.");
		pauseGame();
		showingFullScreenAd = true;

		//ANALYTICS
		GTAnalyticsHandler.Instance.logEvent(GAEventCategories.INTERSTITIAL_AD, GAEventActions.OPENED);
	}

	public void OnAdmobInterstitialClosed(object sender, EventArgs args){
		GTDebug.log("On Interstitial Closed event received.");
		pauseGame(false);
		showingFullScreenAd = false;

		//ANALYTICS
		GTAnalyticsHandler.Instance.logEvent(GAEventCategories.INTERSTITIAL_AD, GAEventActions.CLOSED);

		initNativeAdmob (true);
	}

	public void OnAdmobInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args){
		GTDebug.logErrorAlways("Interstitial Failed to load: " + args.Message);
		pauseGame(false,false);
		showingFullScreenAd = false;

		//ANALYTICS
		GTAnalyticsHandler.Instance.logEvent(GAEventCategories.INTERSTITIAL_AD, GAEventActions.FAILED);

		initNativeAdmob (true);
	}

	public void OnAdmobRewardVideoClosed(object sender, EventArgs args){
		//ANALYTICS
		GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_REWARD_AD, GAEventActions.CLOSED);

		GTDebug.log("On Reward Video closed event received.");
		pauseGame(false);
		showingFullScreenAd = false;

		initNativeAdmob (true);
	}
	public void OnAdmobRewardVideoOpening(object sender, EventArgs args){
		//ANALYTICS
		GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_REWARD_AD, GAEventActions.OPENED);

		GTDebug.log("On Reward video Opened event received.");
		pauseGame();
		showingFullScreenAd = true;


	}
	public void OnAdmobRewardVideoRewarded(object sender, EventArgs args){
		GTDebug.log ("Video reward completed. User rewarded");
		OnIncentivedVideoFinished();

		//ANALYTICS
		GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_REWARD_AD, GAEventActions.COMPLETED);
	}

	public void OnAdmobRewardVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args){
		//ANALYTICS
		GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_REWARD_AD, GAEventActions.FAILED);

		GTDebug.logErrorAlways("Reward Video Failed to load: " + args.Message);
		pauseGame(false,false);
		showingFullScreenAd = false;

		initNativeAdmob (true);
	}
		

	public void testOnInterstitialOpen(){
        //		OnInterstisialsOpen();
        #if UNITY_EDITOR
		StartCoroutine(pauseWithDelay());
        #endif
    }
	public void testOnInterstitialClose(){
        //		OnInterstisialsClosed();
        #if UNITY_EDITOR
		StartCoroutine(pauseWithDelay(false));
        #endif
    }
    public void testOnVideoStarted(){
        //		OnVideoStarted();
        #if UNITY_EDITOR
		StartCoroutine(pauseWithDelay());
        #endif
    }
    public void testOnVideoFinished(){
        //		OnVideoFinished(true);
        #if UNITY_EDITOR
		StartCoroutine(pauseWithDelay(false));
        #endif
    }


	private IEnumerator pauseWithDelay(bool pause = true){
		Time.timeScale = pause ? 0 : 1;
		yield return new WaitForSeconds (1);
		pauseGame(pause);
	}


    //--------------------------------------
    //  PUBLIC METHODS
    //--------------------------------------
    public void showRandomGameplayInterstitialOrVideoAd(int adZoneIndex = 0){
		int videoPercentage = Mathf.Clamp(GameSettings.Instance.videoPercentageInRandomShow, 0, 100);
		int election = UnityEngine.Random.Range(0, 100);
		bool canshowInterstitial = canShowAd(AdType.INTERSTITIAL);
		bool canShowVideo = canShowAd(AdType.VIDEO);
		
		GTDebug.log("Can show interstitial ads ? " +canshowInterstitial + ", can show video ads ? "+ canShowVideo);
		GTDebug.log("Random percentage for video electo: "+ videoPercentage + ". Election %: " + election);
		
		if(canshowInterstitial && canShowVideo){
			//show video
			if(election >= 0 && election < videoPercentage){
				GTDebug.log("Playing video");
				//				PlayAVideo(adZoneIndex);

//				if (!useNativeAdmob)
//					HZVideoAd.Show ();
//				else
					showNativeAdmobInterstitial ();
			}
			//show interstitial
			else{
				GTDebug.log("Showing Interstitial");
				showInterstitial();
			}
		}
		else if(canshowInterstitial && !canShowVideo){
			GTDebug.log("Showing Interstitial");
			showInterstitial();

		}
		else if(!canshowInterstitial && canShowVideo){
			GTDebug.log("Playing video");
			//			PlayAVideo(adZoneIndex);

//			if(!useNativeAdmob)
//				HZVideoAd.Show();
//			else
				showNativeAdmobInterstitial ();
		}

		if (RuntimePlatformUtils.IsEditor ())
			testOnInterstitialClose ();
	}
	
	public void quitAds(){
		PlayerPrefs.SetInt(GameSettings.PP_PURCHASED_QUIT_ADS, 1);
		GameSettings.Instance.IS_PRO_VERSION = true;

		destroyBanner ();
	}
	
	public void destroyBanner(){
//		if (!useNativeAdmob)
//			HZBannerAd.Destroy ();
//		else
			destroyNativeAdmobBanner ();
	}
	
	public void hideBanner(){
//		if (!useNativeAdmob)
//			HZBannerAd.Hide ();
//		else
			HideNativeAdmobBanner ();
	}
	
	
	
	public void showInterstitial(){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(canShowAd(AdType.INTERSTITIAL)){
			showingFullScreenAd = true;
//			if(!useNativeAdmob)
//				HZInterstitialAd.show();
//			else
				showNativeAdmobInterstitial ();
		}
		#endif
	}
	
//	public void refrescarBanner(){
//		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
//		destroyBanner();
//		CreateBanner();
//		#endif
//	}


	
	public void playVideoV4VC(){ 
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(canShowAd(AdType.VIDEO_V4VC)){
			showingFullScreenAd = true;
//			if(!useNativeUnityAds && !useNativeAdmob)
//				HZIncentivizedAd.show();
//			else{
				if(useNativeAdmob && useAdmobVideoRewardAds)
					showNativeAdmobRewardVideo();
				else if(useNativeUnityAds && useUnityAdsForVideoRewardOnly)
					showNativeUnityRewardVideo();
//			}
		}
		#endif
	}
	
	public void PlayAVideo(){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(canShowAd(AdType.VIDEO)){
			showingFullScreenAd = true;
//			if(!useNativeAdmob)
//				HZVideoAd.show();
//			else
				showNativeAdmobInterstitial ();
		}
		#endif
	}


	
//	private void CreateBanner(){
////		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
//		GTDebug.log("can show ad banner ?" + canShowAd(AdType.BANNER));
//		if(canShowAd(AdType.BANNER)){
//			string pos = HZBannerShowOptions.POSITION_TOP;
//			switch(anchor){
//			case TextAnchor.LowerCenter:
//			case TextAnchor.LowerLeft:
//			case TextAnchor.LowerRight:
//				pos = HZBannerShowOptions.POSITION_BOTTOM;
//				break;
//			}
//			HZBannerShowOptions opt = new HZBannerShowOptions();
//			opt.Position = pos;
//			
//			HZBannerAd.ShowWithOptions(opt);
//		}
////		#endif
//	}

	//--------------------------------------
	//  Admob
	//--------------------------------------

	public void showNativeAdmobInterstitial(){
		if(!GameSettings.Instance.IS_PRO_VERSION && admobInterstitialAd != null){
			showingFullScreenAd = true;
			admobInterstitialAd.Show ();

			if (RuntimePlatformUtils.IsEditor ())
				testOnInterstitialClose ();
		}
	}

	public void showNativeAdmobRewardVideo(){
		if(!GameSettings.Instance.IS_PRO_VERSION && admobRewardVideoAd != null){
			showingFullScreenAd = true;
			admobRewardVideoAd.Show ();

			if (RuntimePlatformUtils.IsEditor ())
				testOnVideoFinished ();
		}
	}


//	public void refreshNativeAdmobBanner(){
//	#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
//	if(!GameSettings.Instance.IS_PRO_VERSION)
//	RefreshBanner();
//	#endif
//	}

//	private void CreateNativeAdmobBanner(){
//	#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
//	if(!GameSettings.Instance.IS_PRO_VERSION){
//	GTDebug.log ("Creating Native Admob Banner with id "+UniqueBannerID);
//	GoogleMobileAdBanner banner;
//
//	if (registerdBanners.ContainsKey(UniqueBannerID)){
//	banner = registerdBanners[UniqueBannerID];
//	}
//	else{
//	banner = GoogleMobileAd.CreateAdBanner(anchor, size);
//	registerdBanners.Add(UniqueBannerID, banner);
//	GTDebug.log ("Created new Native Admob Banner ...");
//	}
//
//
//	if(banner.IsLoaded && !banner.IsOnScreen) {
//	GTDebug.log ("Showing new Native Admob Banner ...");
//	banner.Show();
//	}
//	else{
//	GTDebug.log ("Waiting for showing Native Admob Banner when loaded. Loaded ? "+banner.IsLoaded+", is on screen ? "+banner.IsOnScreen);
//	//listening for banner to load example using C# actions:
//	banner.OnLoadedAction += OnNativeAdmobBannerLoadedAction;
//
//	//By setting this flsg to fals we will prevent banner to show when it's loaded
//	//e will listner for OnLoadedAction event and show it by our selfs instead
//	banner.ShowOnLoad = false;
//
//	}
//	}
//	#endif
//	}

//	private void DestroyAllNativeAdmobBanners(){
//	#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
//	if(registerdBanners != null && registerdBanners.Count > 0){
//
//	foreach(GoogleMobileAdBanner b in registerdBanners.Values){
//	GTDebug.log("AdsHandler DestroyBanner() - Found banner with id: " + b.id + " destroying");
//	//				b.Hide();
//	GoogleMobileAd.DestroyBanner(b.id);
//	}
//	}
//	#endif
//	}

	private void destroyNativeAdmobBanner(){
		if (admobBannerView != null) {
			admobBannerView.Destroy ();
		}
	}

	private void HideNativeAdmobBanner(){
//	#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
//	if (!GameSettings.Instance.IS_PRO_VERSION && registerdBanners.ContainsKey(UniqueBannerID)){
//	if(GameSettings.Instance.showTestLogs)
//	Debug.Log("AdsHandler HideBanner() - Found banner with id: " + UniqueBannerID);
//
//	GoogleMobileAdBanner banner = registerdBanners[UniqueBannerID];
//	if (banner.IsLoaded){
//	GTDebug.log("AdsHandler HideBanner() - banner with id: " + UniqueBannerID + " loaded");
//
//	if (banner.IsOnScreen){
//	GTDebug.log("AdsHandler HideBanner() - banner with id: " + UniqueBannerID + " hiding");
//
//	banner.Hide();
//	}
//	}
//	else{
//	GTDebug.log("AdsHandler HideBanner() - banner with id: " + UniqueBannerID + " not loaded");
//
//	banner.ShowOnLoad = false;
//	}
//	}
//	else{
//	GTDebug.log("AdsHandler HideBanner() - No banner found with id: " + UniqueBannerID);
//	}
//	#endif

		if (admobBannerView != null) {
			admobBannerView.Hide ();
		}
	}

//	private void RefreshBanner(){
//	#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
//	if (registerdBanners.ContainsKey(UniqueBannerID)){
//	GoogleMobileAdBanner banner = registerdBanners[UniqueBannerID];
//	if (banner.IsLoaded){
//	if (banner.IsOnScreen){
//	GTDebug.log("AdsHandler refresh() - banner with id: " + UniqueBannerID + " refreshing");
//
//	//					if(GameSettings.Instance.showTestLogs)
//	//						Debug.Log("AdsHandler - refreshing banner ad at position: ");
//
//	banner.Refresh();
//	}
//	else{
//	GTDebug.log("AdsHandler refresh() - banner with id: " + UniqueBannerID + "not refreshing");
//	}
//	}
//	else{
//	GTDebug.log("AdsHandler refresh() - banner with id: " + UniqueBannerID + "not loaded");
//	}
//	//else
//	//{
//	//    banner.ShowOnLoad = true;
//	//}
//	}
//	#endif
//	}



	//--------------------------------------
	//  Unity Ads
	//--------------------------------------

	public void showNativeUnityRewardVideo(){
		ShowOptions options = new ShowOptions();
		options.resultCallback = OnUnityRewardVideoResult;

		Advertisement.Show ("rewardedVideoZone",options);

		pauseGame();

//		//ANALYTICS
//		GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_REWARD_AD, GAEventActions.SHOWN);
	}

	private void OnUnityRewardVideoResult (ShowResult result){
		pauseGame(false);

		switch (result)
		{
		case ShowResult.Finished:
			GTDebug.log ("Video Reward completed. User rewarded");
			OnIncentivedVideoFinished();

			//ANALYTICS
			GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_REWARD_AD, GAEventActions.COMPLETED);
			break;
		case ShowResult.Skipped:
			GTDebug.logWarningAlways ("Video Reward was skipped.");
			//ANALYTICS
			GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_REWARD_AD, GAEventActions.SKIPPED);
			break;
		case ShowResult.Failed:
			GTDebug.logErrorAlways ("Video Reward failed to show.");
			//ANALYTICS
			GTAnalyticsHandler.Instance.logEvent(GAEventCategories.VIDEO_REWARD_AD, GAEventActions.FAILED);
			break;
		}
	}

	public void showNativeUnityVideo(){
		Advertisement.Show ("defaultZone");
	}




	#endif
}
