using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdsHandler : PersistentSingleton<AdsHandler> {
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
	private bool isShowingFullScreenAd = false;
	
	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public bool IsShowingFullScreenAd {
		get {
			return this.isShowingFullScreenAd;
		}
	}
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	#if  (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_EDITOR)
	private bool canShowAd(AdNetwork network){
		bool canShow = !GameSettings.Instance.IS_PRO_VERSION && GameSettings.Instance.adsNetworks != null && GameSettings.Instance.adsNetworks.Count > 0;
		
		if(canShow){
			switch(network){
			case AdNetwork.GOOGLE_ADMOB:
				canShow = GameSettings.Instance.adsNetworks.Contains(AdNetwork.GOOGLE_ADMOB) && googleAdmobInited; break;
			case AdNetwork.ADCOLONY: canShow = GameSettings.Instance.adsNetworks.Contains(AdNetwork.ADCOLONY) && adcolonyInited; break;
			}
		}
		
		return canShow;
	}
	
	protected void Start(){
		//	protected override void Awake (){
		//		base.Awake ();
		
		if(GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE)
			StartCoroutine(handleInitializationWhenUsingInAppBilling());
		else
			init();
	}
	
	private IEnumerator handleInitializationWhenUsingInAppBilling(){
		yield return null;
		
		currentTime = Time.timeSinceLevelLoad;
		
		//		if(GameSettings.Instance.showTestLogs)
		//			Debug.Log("AdsHandler handleInitialization() - current time: " + currentTime);
		
		
		
		if((GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE && currentTime < IN_APP_WAIT_INITIALIZATION_TIME 
		    && (!CoreIAPManager.Instance.IsInited 
		    || (CoreIAPManager.Instance.IsInited && GameLoaderManager.Instance.InAppNeedRestoreProducts && !GameLoaderManager.Instance.InAppAllProductsRestored))))
			StartCoroutine(handleInitializationWhenUsingInAppBilling());
		
		//long time waiting to init, now we can init ads handler
		else
			init();
	}
	
	private void init(){
		if(!GameSettings.Instance.IS_PRO_VERSION && GameSettings.Instance.adsNetworks != null && GameSettings.Instance.adsNetworks.Count > 0){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("AdsHandler - initializing");
			
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
	
	private void pauseGame(bool pause = true){
		//pausing the game
		if(BaseGameScreenController.Instance.Section == GameSection.GAME)
			GameController.Instance.Manager.Paused = pause;
		else
			Time.timeScale = pause ? 0f: 1f;
		
		//mute or active sounds
		BaseSoundManager.Instance.muteOrActiveAllOncesMuteOncesActiveAndPlayOrStopAfter(true);
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
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
		isShowingFullScreenAd = true;
		IsInterstisialsAdReady = false;
		pauseGame();
		//		//pausing the game
		//		if(BaseGameScreenController.Instance.Section == GameSection.GAME)
		//			GameController.Instance.Manager.Paused = true;
	}
	
	private void OnInterstisialsClosed(){
		isShowingFullScreenAd = false;
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
		isShowingFullScreenAd = true;
		pauseGame();
		
		if(GameSettings.Instance.showTestLogs)
			Debug.Log( "AdsHandler OnVideoStarted() - Ad video playing." );
	}
	
	void OnVideoFinished( bool ad_shown ){
		pauseGame(false);
		isShowingFullScreenAd = false;
		
		if(GameSettings.Instance.showTestLogs)
			Debug.Log( "AdsHandler OnVideoFinished() Ad video finished." );
		
		
	}
	
	void OnV4VCResult( bool success, string name, int amount ){
		if (success){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log( "AdsHandler OnV4VCResult() - Awarded " + amount + " " + name );
			// e.g. "Awarded 100 Gold"
		}
		else{
			if(GameSettings.Instance.showTestLogs)
				Debug.Log( "AdsHandler OnV4VCResult() - not Awarded " + amount + " " + name );
		}
	}
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	public void showRandomGameplayInterstitialOrVideoAd(int adZoneIndex = 0){
		int videoPercentage = Mathf.Clamp(1,100,GameSettings.Instance.videoPercentageInRandomShow);
		int election = Random.Range(0, 100);
		bool canShowAdmobAd = canShowAd(AdNetwork.GOOGLE_ADMOB);
		bool canShowAdColonyAd = canShowAd(AdNetwork.ADCOLONY);
		
		if(canShowAdmobAd && canShowAdColonyAd){
			//show adcolony video
			if(election >= 0 && election < videoPercentage){
				PlayAVideo(adZoneIndex);
			}
			//admob interstitial
			else{
				showInterstitial();
			}
		}
		else if(canShowAdmobAd && !canShowAdColonyAd){
			showInterstitial();
		}
		else if(!canShowAdmobAd && canShowAdColonyAd){
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
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("AdsHandler PlayAVideo() - Play AdColony Video in this zone ID " + zoneID);
				
				// Call AdColony.ShowVideoAd with that zone to play an interstitial video.
				// Note that you should also pause your game here (audio, etc.) AdColony will not
				// pause your app for you.
				AdColony.ShowVideoAd(zoneID); 
			}
			else{
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("AdsHandler PlayAVideo() - Video Not Available in this zone ID "+zoneID);
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
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("AdsHandler DestroyBanner() - Found banner with id: " + b.id + " destroying");
				//				b.Hide();
				GoogleMobileAd.DestroyBanner(b.id);
			}
		}
	}
	
	private void HideBanner(){
		if (registerdBanners.ContainsKey(UniqueBannerID)){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("AdsHandler HideBanner() - Found banner with id: " + UniqueBannerID);
			
			GoogleMobileAdBanner banner = registerdBanners[UniqueBannerID];
			if (banner.IsLoaded){
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("AdsHandler HideBanner() - banner with id: " + UniqueBannerID + " loaded");
				
				if (banner.IsOnScreen){
					if(GameSettings.Instance.showTestLogs)
						Debug.Log("AdsHandler HideBanner() - banner with id: " + UniqueBannerID + " hiding");
					
					banner.Hide();
				}
			}
			else{
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("AdsHandler HideBanner() - banner with id: " + UniqueBannerID + " not loaded");
				
				banner.ShowOnLoad = false;
			}
		}
		else{
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("AdsHandler HideBanner() - No banner found with id: " + UniqueBannerID);
		}
	}
	
	private void RefreshBanner(){
		if (registerdBanners.ContainsKey(UniqueBannerID)){
			GoogleMobileAdBanner banner = registerdBanners[UniqueBannerID];
			if (banner.IsLoaded){
				if (banner.IsOnScreen){
					if(GameSettings.Instance.showTestLogs)
						Debug.Log("AdsHandler refresh() - banner with id: " + UniqueBannerID + " refreshing");
					
					//					if(GameSettings.Instance.showTestLogs)
					//						Debug.Log("AdsHandler - refreshing banner ad at position: ");
					
					banner.Refresh();
				}
				else{
					if(GameSettings.Instance.showTestLogs)
						Debug.Log("AdsHandler refresh() - banner with id: " + UniqueBannerID + "not refreshing");
				}
			}
			else{
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("AdsHandler refresh() - banner with id: " + UniqueBannerID + "not loaded");
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
