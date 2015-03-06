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
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	#if  (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_EDITOR)
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
		
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("AdsHandler handleInitialization() - current time: " + currentTime);
		
		if(currentTime < IN_APP_WAIT_INITIALIZATION_TIME && !CoreIAPManager.Instance.IsInited)
			StartCoroutine(handleInitializationWhenUsingInAppBilling());
		
		//long time waiting to init, now we can init ads handler
		else
			init();
	}
	
	private void init(){
		if(!GameSettings.Instance.IS_PRO_VERSION){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("AdsHandler - initializing");
			
			//Required
			GoogleMobileAd.Init();
			
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
			
			//create a show banner ad
			CreateBanner ();
			//			mostrarBanner ();
		}
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
		IsInterstisialsAdReady = false;
		
		//pausing the game
		if(BaseGameScreenController.Instance.Section == GameSection.GAME)
			GameController.Instance.Manager.Paused = true;
	}
	
	private void OnInterstisialsClosed(){
		//un-pausing the game
		if (BaseGameScreenController.Instance.Section == GameSection.GAME)
			GameController.Instance.Manager.Paused = false;
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
	
	
	
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	public void quitAds(){
		PlayerPrefs.SetInt(GameSettings.PP_PURCHASED_QUIT_ADS, 1);
		GameSettings.Instance.IS_PRO_VERSION = true;
		DestroyAllBanners();
	}
	
	public void ocultarBanner(){
		if (!GameSettings.Instance.IS_PRO_VERSION)
			HideBanner();
	}
	
	
	
	public void mostrarPantallazo(){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(!GameSettings.Instance.IS_PRO_VERSION){
			//loadin ad:
			GoogleMobileAd.LoadInterstitialAd ();
		}
		#endif
	}
	
	public void refrescarBanner(){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(!GameSettings.Instance.IS_PRO_VERSION)
			RefreshBanner();
		#endif
	}
	
	private void CreateBanner(){
		if(!GameSettings.Instance.IS_PRO_VERSION){
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
