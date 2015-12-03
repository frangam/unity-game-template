/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Heyzap;

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
    public GADBannerSize size = GADBannerSize.SMART_BANNER;
	public TextAnchor anchor = TextAnchor.LowerCenter;
	public bool test = true;
	
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
	
	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public bool HasPausedGame {
		get {
			return this.hasPausedGame;
		}
	}
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	#if  (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_EDITOR)
	public bool canShowAd(AdType type = AdType.BANNER, string tag = "default"){
		bool hasInternet = InternetChecker.Instance.IsconnectedToInternet;
		bool canShow = hasInternet && !string.IsNullOrEmpty(GameSettings.Instance.heyZapID)
			&& (type == AdType.VIDEO_V4VC //can show if the ad type is video with and a reward even player had purchase for quitting ads 
			    || (type != AdType.VIDEO_V4VC && !GameSettings.Instance.IS_PRO_VERSION));
		
		if(canShow){
			switch(type){
			case AdType.INTERSTITIAL: canShow = HZInterstitialAd.isAvailable(tag); break;
			case AdType.VIDEO: HZVideoAd.fetch(); canShow = HZVideoAd.isAvailable(tag); break;
			case AdType.VIDEO_V4VC: HZIncentivizedAd.fetch(); canShow = HZIncentivizedAd.isAvailable(tag); break;
			}
		}
		
		return canShow;
	}

    public bool canShowAdRandom()
    {
        bool hasInternet = InternetChecker.Instance.IsconnectedToInternet;
        bool hasPro = GameSettings.Instance.IS_PRO_VERSION;
        bool canShow = hasInternet && !string.IsNullOrEmpty(GameSettings.Instance.heyZapID) && !hasPro;

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
	
	private void init(){
		// Your Publisher ID is: 1d3b3bbd9f2a398c60822451a696ffea
		HeyzapAds.start(GameSettings.Instance.heyZapID, HeyzapAds.FLAG_NO_OPTIONS);
		
		//--------------------------------------
		//  HEYZAP PLUGIN EVENT LISTNERS
		//--------------------------------------
		//------
		//BANNER ADS
		//------
		HZBannerAd.setDisplayListener(delegate(string adState, string adTag){
			if (adState == "loaded") {
				// Do something when the banner ad is loaded
			}
			if (adState == "error") {
				// Do something when the banner ad fails to load (they can fail when refreshing after successfully loading)
			}
			if (adState == "click") {
				// Do something when the banner ad is clicked, like pause your game
			}
		});
		
		
		//------
		//Interstitial Ads
		//------
		HZInterstitialAd.setDisplayListener(delegate(string adState, string adTag){
			if ( adState.Equals("show") ) {
				// Do something when the ad shows, like pause your game
				pauseGame();
			}
			if ( adState.Equals("hide") ) {
				// Do something after the ad hides itself
				pauseGame(false);
                OnVideoOrIntertisialFinished();
            }
			if ( adState.Equals("click") ) {
				// Do something when an ad is clicked on
			}
			if ( adState.Equals("failed") ) {
				// Do something when an ad fails to show
				pauseGame(false);
			}
			if ( adState.Equals("available") ) {
				// Do something when an ad has successfully been fetched
			}
			if ( adState.Equals("fetch_failed") ) {
				// Do something when an ad did not fetch
			}
			if ( adState.Equals("audio_starting") ) {
				// The ad being shown will use audio. Mute any background music
			}
			if ( adState.Equals("audio_finished") ) {
				// The ad being shown has finished using audio.
				// You can resume any background music.
			}
		});
		
		
		//------
		//Reward Videos
		//------
		HZIncentivizedAd.setDisplayListener(delegate(string adState, string adTag){
			if (adState.Equals ("show")) {
				// Do something when the ad shows, like pause your game
				pauseGame();
			}
			if (adState.Equals ("hide")) {
				// Do something after the ad hides itself
				//				pauseGame(false);
			}
			if (adState.Equals ("click")) {
				// Do something when an ad is clicked on
			}
			if (adState.Equals ("failed")) {
				// Do something when an ad fails to show
			}
			if (adState.Equals ("available")) {
				// Do something when an ad has successfully been fetched
			}
			if (adState.Equals ("fetch_failed")) {
				// Do something when an ad did not fetch
			}
			if (adState.Equals ("incentivized_result_complete")) {
				// The user has watched the entire video and should be given a reward.
				pauseGame(false);
				OnIncentivedVideoFinished();
			}
			if (adState.Equals ("incentivized_result_incomplete")) {
				// The user did not watch the entire video and should not be given a reward.
				pauseGame(false);
			}
		});
		
		
		//------
		//Reward Videos
		//------
		HZVideoAd.setDisplayListener( delegate(string adState, string adTag){
			if (adState.Equals ("show")) {
				// Do something when the ad shows, like pause your game
				pauseGame();
			}
			if (adState.Equals ("hide")) {
				// Do something after the ad hides itself
				pauseGame(false);
                OnVideoOrIntertisialFinished();
            }
			if (adState.Equals ("click")) {
				// Do something when an ad is clicked on
			}
			if (adState.Equals ("failed")) {
				// Do something when an ad fails to show
			}
			if (adState.Equals ("available")) {
				// Do something when an ad has successfully been fetched
			}
			if (adState.Equals ("fetch_failed")) {
				// Do something when an ad did not fetch
			}
		});
		
		
		CreateBanner();
		
		//For Test purposes
		//		HeyzapAds.showMediationTestSuite();
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
	public void testOnInterstitialOpen(){
        //		OnInterstisialsOpen();
        #if UNITY_EDITOR
            pauseGame();
        #endif
    }
	public void testOnInterstitialClose(){
        //		OnInterstisialsClosed();
        #if UNITY_EDITOR
                pauseGame(false);
        #endif
    }
    public void testOnVideoStarted(){
        //		OnVideoStarted();
        #if UNITY_EDITOR
                pauseGame();
        #endif
    }
    public void testOnVideoFinished(){
        //		OnVideoFinished(true);
        #if UNITY_EDITOR
            pauseGame(false);
        #endif
    }



    //--------------------------------------
    //  PUBLIC METHODS
    //--------------------------------------
    public void showRandomGameplayInterstitialOrVideoAd(int adZoneIndex = 0){
		int videoPercentage = Mathf.Clamp(GameSettings.Instance.videoPercentageInRandomShow, 0, 100);
		int election = Random.Range(0, 100);
		bool canshowInterstitial = canShowAd(AdType.INTERSTITIAL);
		bool canShowVideo = canShowAd(AdType.VIDEO);
		
		GTDebug.log("Can show interstitial ads ? " +canshowInterstitial + ", can show video ads ? "+ canShowVideo);
		GTDebug.log("Random percentage for video electo: "+ videoPercentage + ". Election %: " + election);
		
		if(canshowInterstitial && canShowVideo){
			//show video
			if(election >= 0 && election < videoPercentage){
				GTDebug.log("Playing video");
				//				PlayAVideo(adZoneIndex);
				HZVideoAd.show();
			}
			//show interstitial
			else{
				GTDebug.log("Showing Interstitial");
				//				showInterstitial();
				HZInterstitialAd.show();
			}
		}
		else if(canshowInterstitial && !canShowVideo){
			GTDebug.log("Showing Interstitial");
			//			showInterstitial();
			HZInterstitialAd.show();
		}
		else if(!canshowInterstitial && canShowVideo){
			GTDebug.log("Playing video");
			//			PlayAVideo(adZoneIndex);
			HZVideoAd.show();
		}
	}
	
	public void quitAds(){
		PlayerPrefs.SetInt(GameSettings.PP_PURCHASED_QUIT_ADS, 1);
		GameSettings.Instance.IS_PRO_VERSION = true;
		HZBannerAd.Destroy ();
	}
	
	public void destroyBanner(){
		HZBannerAd.Destroy();
	}
	
	public void hideBanner(){
		HZBannerAd.Hide();
	}
	
	
	
	public void showInterstitial(){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(canShowAd(AdType.INTERSTITIAL))
			HZInterstitialAd.show();
		#endif
	}
	
	public void refrescarBanner(){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		destroyBanner();
		CreateBanner();
		#endif
	}
	
	
	public void playVideoV4VC(){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(canShowAd(AdType.VIDEO_V4VC))
			HZIncentivizedAd.show();
		#endif
	}
	
	public void PlayAVideo(){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(canShowAd(AdType.VIDEO))
			HZVideoAd.show();
		#endif
	}
	
	private void CreateBanner(){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(canShowAd(AdType.BANNER)){
			string pos = HZBannerAd.POSITION_TOP;
			switch(anchor){
			case TextAnchor.LowerCenter:
			case TextAnchor.LowerLeft:
			case TextAnchor.LowerRight:
				pos = HZBannerAd.POSITION_BOTTOM;
				break;
			}
			HZBannerShowOptions opt = new HZBannerShowOptions();
			opt.Position = pos;
			
			HZBannerAd.ShowWithOptions(opt);
		}
		#endif
	}
	#endif
}
