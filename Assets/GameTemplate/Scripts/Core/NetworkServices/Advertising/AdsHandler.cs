using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AdsHandler : Singleton<AdsHandler> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	public GADBannerSize size = GADBannerSize.SMART_BANNER;
	public TextAnchor anchor = TextAnchor.UpperCenter;
	public bool test = true;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private GoogleMobileAdBanner banner;
	private bool IsInterstisialsAdReady = false;
    private static Dictionary<string, GoogleMobileAdBanner> _registerdBanners = null;

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	#if  (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_EDITOR)
	void Awake() {
		if(!GameSettings.IS_PRO_VERSION || !GameSettings.purchasedForQuitAds){
		
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
			crearBanner ();
			mostrarBanner ();
		}
	}

	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	private void OnInterstisialsLoaded() {
        //ad loaded, strting ad
		IsInterstisialsAdReady = true;
        GoogleMobileAd.ShowInterstitialAd();
	}
	
	private void OnInterstisialsOpen() {
		IsInterstisialsAdReady = false;
       
        //pausing the game
		if(BaseGameScreenController.Instance.Section == GameSection.GAME)
			BaseGameController.Instance.Paused = true;
	}

    private void OnInterstisialsClosed(){
        //un-pausing the game
		if (BaseGameScreenController.Instance.Section == GameSection.GAME)
			BaseGameController.Instance.Paused = false;
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

	private void crearBanner(){
		if(!GameSettings.IS_PRO_VERSION || !GameSettings.purchasedForQuitAds)
			banner = GoogleMobileAd.CreateAdBanner(anchor, size);
	}
	

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	public void mostrarBanner(){
		if (!GameSettings.IS_PRO_VERSION || !GameSettings.purchasedForQuitAds)
            ShowBanner();
	}

	public void ocultarBanner(){
		if (!GameSettings.IS_PRO_VERSION || !GameSettings.purchasedForQuitAds)
            HideBanner();
	}



	public void mostrarPantallazo(){
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(!GameSettings.IS_PRO_VERSION || !GameSettings.purchasedForQuitAds){
            //loadin ad:
			GoogleMobileAd.LoadInterstitialAd ();
		}
#endif
                                   }

	public void refrescarBanner(){
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		if(!GameSettings.IS_PRO_VERSION || !GameSettings.purchasedForQuitAds)
			RefreshBanner();
#endif
                                 }

    public void ShowBanner(){
        GoogleMobileAdBanner banner;
        
		if (registerdBanners.ContainsKey(sceneBannerId)){
            banner = registerdBanners[sceneBannerId];
        }
        else{
            banner = GoogleMobileAd.CreateAdBanner(anchor, size);
            registerdBanners.Add(sceneBannerId, banner);
        }

        if (banner.IsLoaded && !banner.IsOnScreen){
            banner.Show();
        }
    }

    public void HideBanner(){
        if (registerdBanners.ContainsKey(sceneBannerId)){
            GoogleMobileAdBanner banner = registerdBanners[sceneBannerId];
            if (banner.IsLoaded){
                if (banner.IsOnScreen){
                    banner.Hide();
                }
            }
            else{
                banner.ShowOnLoad = false;
            }
        }
    }

    public void RefreshBanner(){
        if (registerdBanners.ContainsKey(sceneBannerId)){
            GoogleMobileAdBanner banner = registerdBanners[sceneBannerId];
            if (banner.IsLoaded){
                if (banner.IsOnScreen){
                    banner.Refresh();
                }
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
#endif
}
