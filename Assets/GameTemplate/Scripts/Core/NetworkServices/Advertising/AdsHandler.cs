using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnionAssets.FLE;

public class AdsHandler : PersistentSingleton<AdsHandler> {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string V4VC_RESULT = "gt_v4vc_result";
	
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	private static EventDispatcherBase _dispatcher  = new EventDispatcherBase ();
	
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
	public static EventDispatcherBase dispatcher {
		get {
			return _dispatcher;
		}
	}
	
	
	public bool HasPausedGame {
		get {
			return this.hasPausedGame;
		}
	}
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	#if  (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_EDITOR)
	public bool canShowAd(AdNetwork network, int zoneIndex = 0, AdType type = AdType.BANNER){
		bool hasInternet = InternetChecker.Instance.IsconnectedToInternet;
		bool canShow = hasInternet && !GameSettings.Instance.IS_PRO_VERSION && GameSettings.Instance.adsNetworks != null && GameSettings.Instance.adsNetworks.Count > 0;
		
		GTDebug.log("Has internet ? " + hasInternet);
		
		if(canShow){
			switch(network){
			case AdNetwork.GOOGLE_ADMOB:
				canShow = GameSettings.Instance.adsNetworks.Contains(AdNetwork.GOOGLE_ADMOB) && googleAdmobInited; break;
			case AdNetwork.ADCOLONY: 
				string zoneId = AdColonySettings.Instance.GetZoneIDByIndex(zoneIndex);
				canShow = GameSettings.Instance.adsNetworks.Contains(AdNetwork.ADCOLONY) 
					&& adcolonyInited 
						&& !string.IsNullOrEmpty(zoneId) 
						&& ((type == AdType.VIDEO && AdColony.IsVideoAvailable(zoneId))
						    || (type == AdType.VIDEO_V4VC && AdColony.IsV4VCAvailable(zoneId))); 
				
				
				
				GTDebug.log("Zone ID: " + zoneId);
				
				if(type == AdType.VIDEO)
					GTDebug.log("Is video available ? " +AdColony.IsVideoAvailable(zoneId));
				else if(type == AdType.VIDEO_V4VC)
					GTDebug.log("Is video V4VC available ? " +AdColony.IsV4VCAvailable(zoneId));
				break;
			}
		}
		
		GTDebug.log("Can show ad ? " +canShow);
		
		return canShow;
	}
	
	protected void Start(){
		if(GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE)
			StartCoroutine(handleInitializationWhenUsingInAppBilling());
		
		StartCoroutine(checkInternetConnection());
	}
	
	private IEnumerator checkInternetConnection(){
		inited = (!GameSettings.Instance.adsNetworks.Contains(AdNetwork.ADCOLONY) || (GameSettings.Instance.adsNetworks.Contains(AdNetwork.ADCOLONY) && adcolonyInited))
			&& (!GameSettings.Instance.adsNetworks.Contains(AdNetwork.GOOGLE_ADMOB) || (GameSettings.Instance.adsNetworks.Contains(AdNetwork.GOOGLE_ADMOB) && googleAdmobInited));
		
		//try to init
		if(!inited && !waitingFinishInitInApps && InternetChecker.Instance.IsconnectedToInternet){
			init();
		}
		
		yield return new WaitForSeconds(5);
		StartCoroutine(checkInternetConnection());
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
		if(!GameSettings.Instance.IS_PRO_VERSION && GameSettings.Instance.adsNetworks != null && GameSettings.Instance.adsNetworks.Count > 0){
			GTDebug.log("initializing");
			
			//AdColony
			if(GameSettings.Instance.adsNetworks.Contains(AdNetwork.ADCOLONY))
				initAdColony();
			
			//Admob
			if(GameSettings.Instance.adsNetworks.Contains(AdNetwork.GOOGLE_ADMOB)){
				initGoogleAdmob();
				
				//create and show banner ad
				CreateBanner ();
			}
		}
	}
	
	private void initGoogleAdmob(){
		AdmobIDsPack pack =  GoogleAdmobSettings.Instance.CurrentIDsPack;
		if(pack != null){
			//Required
			GoogleMobileAd.Init();
			
			//set ids
			GoogleMobileAd.SetBannersUnitID(pack.android_BannerID, pack.iOS_BannerID, pack.wp_BannerID);
			GoogleMobileAd.SetInterstisialsUnitID(pack.android_InterstitialID, pack.iOS_InterstitialID, pack.wp_InterstitialID);
			
			
			//		//Optional, add data for better ad targeting
			//		GoogleMobileAd.SetGender(GoogleGenger.Male);
			//		GoogleMobileAd.AddKeyword("game");
			//		GoogleMobileAd.SetBirthday(1989, AndroidMonth.MARCH, 18);
			//		GoogleMobileAd.TagForChildDirectedTreatment(false);
			
			
			
			
			//More eventts ot explore under GoogleMobileAdEvents class
			GoogleMobileAd.addEventListener(GoogleMobileAdEvents.ON_INTERSTITIAL_AD_LOADED, OnInterstisialsLoaded);
			GoogleMobileAd.addEventListener(GoogleMobileAdEvents.ON_INTERSTITIAL_AD_OPENED, OnInterstisialsOpen);
			GoogleMobileAd.controller.addEventListener(GoogleMobileAdEvents.ON_INTERSTITIAL_AD_CLOSED, OnInterstisialsClosed);
			
			//		//listening for InApp Event
			//		//You will only receive in-app purchase (IAP) ads if you specifically configure an IAP ad campaign in the AdMob front end.
			//		GoogleMobileAd.addEventListener(GoogleMobileAdEvents.ON_AD_IN_APP_REQUEST, OnInAppRequest);
			
			googleAdmobInited = true;
		}
	}
	
	private void initAdColony(){
		AdColonyIDsPack pack = AdColonySettings.Instance.CurrentIDsPack;
		if(pack != null){
			AdColony.OnVideoStarted = OnVideoStarted;
			AdColony.OnVideoFinished = OnVideoFinished;
			AdColony.OnV4VCResult = OnV4VCResult;
			//     	AdColony.OnAdAvailabilityChange = OnAdAvailabilityChange;
			
			string[] zoneIDs = null;
			string appID = "";
			switch(Application.platform){
			case RuntimePlatform.Android: appID = pack.android_appID; zoneIDs = pack.android_adZoneIDs.ToArray(); break;
			case RuntimePlatform.IPhonePlayer: appID = pack.iOS_appID; zoneIDs = pack.iOS_adZoneIDs.ToArray(); break;
			}
			
			AdColony.Configure( "1.0", appID, zoneIDs );
			
			adcolonyInited = true;
		}
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
		else if(hasPausedGame || (!hasPausedGame && Time.timeScale != 0f)){
			if(pause)
				hasPausedGame = pause;
			
			if(!alwaysMute)
				BaseSoundManager.Instance.muteOrActiveAllOncesMuteOncesActiveAndPlayOrStopAfter();
			
			Time.timeScale = pause ? 0f: 1f;
			
			if(!pause)
				hasPausedGame = pause;
		}
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	public void testOnInterstitialOpen(){
		OnInterstisialsOpen();
	}
	public void testOnInterstitialClose(){
		OnInterstisialsClosed();
	}
	public void testOnVideoStarted(){
		OnVideoStarted();
	}
	public void testOnInterstitialFinished(){
		OnVideoFinished(true);
	}
	
	
	private void OnBannerLoadedAction (GoogleMobileAdBanner banner) {
		banner.OnLoadedAction -= OnBannerLoadedAction;
		banner.Show();
	}
	
	private void OnInterstisialsLoaded() {
		//ad loaded, strting ad
		IsInterstisialsAdReady = true;
		GoogleMobileAd.ShowInterstitialAd();
	}
	
	private void OnInterstisialsOpen() {
		IsInterstisialsAdReady = false;
		pauseGame();
		//		//pausing the game
		//		if(BaseGameScreenController.Instance.Section == GameSection.GAME)
		//			GameController.Instance.Manager.Paused = true;
	}
	
	private void OnInterstisialsClosed(){
		pauseGame(false);
		
		//		//un-pausing the game
		//		if (BaseGameScreenController.Instance.Section == GameSection.GAME)
		//			GameController.Instance.Manager.Paused = false;
	}
	
	//	private void OnInAppRequest(CEvent e) {
	//		//getting product id
	//		string productId = (string) e.data;
	//		Debug.Log ("In App Request for product Id: " + productId + " received");
	//		
	//		
	//		//Then you should perfrom purchase  for this product id, using this or another game billing plugin
	//		//Once the purchase is complete, you should call RecordInAppResolution with one of the constants defined in GADInAppResolution:
	//		
	//		GoogleMobileAd.RecordInAppResolution(GADInAppResolution.RESOLUTION_SUCCESS);
	//		
	//	}
	
	void OnVideoStarted(){
		hasPausedGame = true;
		pauseGame();
		
		GTDebug.log( "Ad video playing." );
	}
	
	void OnVideoFinished( bool ad_shown ){
		pauseGame(false);
		hasPausedGame = false;
		
		GTDebug.log( "Ad video finished." );
		
		
	}
	
	void OnV4VCResult( bool success, string name, int amount ){
		if (success){
			//dispatch event
			_dispatcher.dispatch(V4VC_RESULT, new V4VCResult(name, amount));
			
			GTDebug.log( "Awarded " + amount + " " + name );
			// e.g. "Awarded 100 Gold"
			
			
			if(name.Equals("Bills")){
				BaseSoundManager.Instance.play(BaseSoundIDs.CLAIM_BILLS_FX);
				GameMoneyManager.Instance.addMoney(amount);
			}
			
			
		}
		else{
			GTDebug.log( "not Awarded " + amount + " " + name );
		}
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	public void showRandomGameplayInterstitialOrVideoAd(int adZoneIndex = 0){
		int videoPercentage = Mathf.Clamp(GameSettings.Instance.videoPercentageInRandomShow, 1, 100);
		int election = Random.Range(0, 100);
		bool canShowAdmobAd = canShowAd(AdNetwork.GOOGLE_ADMOB);
		bool canShowAdColonyAd = canShowAd(AdNetwork.ADCOLONY);
		
		GTDebug.log("Can show admob ads ? " +canShowAdmobAd + ", can show adcolony ads ? "+ canShowAdColonyAd);
		GTDebug.log("Random percentage for video electo: "+ videoPercentage+election + ". Election %: " + election);
		
		if(canShowAdmobAd && canShowAdColonyAd){
			//show adcolony video
			if(election >= 0 && election < videoPercentage){
				GTDebug.log("Playing video");
				PlayAVideo(adZoneIndex);
			}
			//admob interstitial
			else{
				GTDebug.log("Showing Interstitial");
				showInterstitial();
			}
		}
		else if(canShowAdmobAd && !canShowAdColonyAd){
			GTDebug.log("Showing Interstitial");
			showInterstitial();
		}
		else if(!canShowAdmobAd && canShowAdColonyAd){
			GTDebug.log("Playing video");
			PlayAVideo(adZoneIndex);
		}
	}
	
	public void quitAds(){
		PlayerPrefs.SetInt(GameSettings.PP_PURCHASED_QUIT_ADS, 1);
		GameSettings.Instance.IS_PRO_VERSION = true;
		DestroyAllBanners();
	}
	
	public void hideBanner(){
		if (canShowAd(AdNetwork.GOOGLE_ADMOB))
			HideBanner();
	}
	
	
	
	public void showInterstitial(){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(canShowAd(AdNetwork.GOOGLE_ADMOB)){
			//loadin ad:
			GoogleMobileAd.LoadInterstitialAd ();
		}
		#endif
	}
	
	public void refrescarBanner(AdNetwork network = AdNetwork.GOOGLE_ADMOB){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(canShowAd(AdNetwork.GOOGLE_ADMOB))
			RefreshBanner();
		#endif
	}
	
	public void playVideoV4VC(int zoneIndex = 0, bool prePopup = false, bool postPopup = false){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		playVideoV4VC(AdColonySettings.Instance.GetZoneIDByIndex(zoneIndex),prePopup,postPopup);
		#endif
	}
	public void playVideoV4VC(string zoneID, bool prePopup = false, bool postPopup = false){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		// Check to see if a video for V4VC is available in the zone.
		if(AdColony.IsV4VCAvailable(zoneID)){
			GTDebug.log("Play AdColony V4VC Ad");
			// The AdColony class exposes two methods for showing V4VC Ads.
			// ---------------------------------------
			// The first `ShowV4VC`, plays a V4VC Ad and, optionally, displays
			// a popup when the video is finished.
			// ---------------------------------------
			// The second is `OfferV4VC`, which popups a confirmation before
			// playing the ad and, optionally, displays popup when the video 
			// is finished.
			
			// Call one of the V4VC Video methods:
			// Note that you should also pause your game here (audio, etc.) AdColony will not
			// pause your app for you.
			if(prePopup)
				AdColony.OfferV4VC(postPopup, zoneID);
			else
				AdColony.ShowV4VC(postPopup, zoneID);
		}
		else{
			GTDebug.log("V4VC Ad Not Available in this zone id "+zoneID);
		}
		
		#endif
	}
	
	/// <summary>
	/// Play A video related with list index
	/// </summary>
	/// <param name="zoneIndex">Zone index.</param>
	public void PlayAVideo( int zoneIndex = 0){
		PlayAVideo(AdColonySettings.Instance.GetZoneIDByIndex(zoneIndex));
	}
	
	// When a video is available, you may choose to play it in any fashion you like.
	// Generally you will play them automatically during breaks in your game,
	// or in response to a user action like clicking a button.
	// Below is a method that could be called, or attached to a GUI action.
	public void PlayAVideo( string zoneID )
	{
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(canShowAd(AdNetwork.ADCOLONY)){
			// Check to see if a video is available in the zone.
			if(AdColony.IsVideoAvailable(zoneID)){
				GTDebug.log("Play AdColony Video in this zone ID " + zoneID);
				
				// Call AdColony.ShowVideoAd with that zone to play an interstitial video.
				// Note that you should also pause your game here (audio, etc.) AdColony will not
				// pause your app for you.
				AdColony.ShowVideoAd(zoneID); 
			}
			else{
				GTDebug.log("Video Not Available in this zone ID "+zoneID);
			}
		}
		#endif
	}
	
	private void CreateBanner(){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(canShowAd(AdNetwork.GOOGLE_ADMOB)){
			GoogleMobileAdBanner banner;
			
			if (registerdBanners.ContainsKey(UniqueBannerID)){
				banner = registerdBanners[UniqueBannerID];
			}
			else{
				banner = GoogleMobileAd.CreateAdBanner(anchor, size);
				registerdBanners.Add(UniqueBannerID, banner);
			}
			
			if (banner.IsLoaded && !banner.IsOnScreen){
				//listening for banner to load example using C# actions:
				banner.OnLoadedAction += OnBannerLoadedAction;
				
				//By setting this flsg to fals we will prevent banner to show when it's loaded
				//e will listner for OnLoadedAction event and show it by our selfs instead
				banner.ShowOnLoad = false;
			}
		}
		#endif
	}
	
	private void DestroyAllBanners(){
		if(registerdBanners != null && registerdBanners.Count > 0){
			
			foreach(GoogleMobileAdBanner b in registerdBanners.Values){
				GTDebug.log("Found banner with id: " + b.id + " destroying");
				//				b.Hide();
				GoogleMobileAd.DestroyBanner(b.id);
			}
		}
	}
	
	private void HideBanner(){
		if (registerdBanners.ContainsKey(UniqueBannerID)){
			GTDebug.log("Found banner with id: " + UniqueBannerID);
			
			GoogleMobileAdBanner banner = registerdBanners[UniqueBannerID];
			if (banner.IsLoaded){
				GTDebug.log("Banner with id: " + UniqueBannerID + " loaded");
				
				if (banner.IsOnScreen){
					GTDebug.log("Banner with id: " + UniqueBannerID + " hiding");
					
					banner.Hide();
				}
			}
			else{
				GTDebug.log("Banner with id: " + UniqueBannerID + " not loaded");
				
				banner.ShowOnLoad = false;
			}
		}
		else{
			GTDebug.log("No banner found with id: " + UniqueBannerID);
		}
	}
	
	private void RefreshBanner(){
		if (registerdBanners.ContainsKey(UniqueBannerID)){
			GoogleMobileAdBanner banner = registerdBanners[UniqueBannerID];
			if (banner.IsLoaded){
				if (banner.IsOnScreen){
					GTDebug.log("Banner with id: " + UniqueBannerID + " refreshing");
					
					//					GTDebug.log("Refreshing banner ad at position: ");
					
					banner.Refresh();
				}
				else{
					GTDebug.log("Banner with id: " + UniqueBannerID + "not refreshing");
				}
			}
			else{
				GTDebug.log("Banner with id: " + UniqueBannerID + "not loaded");
			}
			//else
			//{
			//    banner.ShowOnLoad = true;
			//}
		}
	}
	
	// --------------------------------------
	// GET / SET
	// --------------------------------------
	public static Dictionary<string, GoogleMobileAdBanner> registerdBanners{
		get{
			if (_registerdBanners == null){
				_registerdBanners = new Dictionary<string, GoogleMobileAdBanner>();
			}
			return _registerdBanners;
		}
	}
	
	public string sceneBannerId{
		get{
			return Application.loadedLevelName + "_" + this.gameObject.name;
		}
	}
	
	public string UniqueBannerID{
		get{
			return this.gameObject.name;
		}
	}
	#endif
}
