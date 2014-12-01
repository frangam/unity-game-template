using UnityEngine;
using System.Collections;
using System;
using ChartboostSDK;

public class GestorChartboost : Singleton<GestorChartboost> {

	[SerializeField]
	private string androidAppID = "";

	[SerializeField]
	private string androidSignature = "";

	[SerializeField]
	private string iosAppID = "";

	[SerializeField]
	private string iosSignature = "";

	[SerializeField]
	private string amazonAppID = "";
	
	[SerializeField]
	private string amazonSignature = "";
	
	[SerializeField]
	private bool buildForAmazon = false;


	#if UNITY_ANDROID || UNITY_IPHONE
//	
//	public void Update() {
//		#if UNITY_ANDROID
//		// Handle the Android back button (only if impressions are set to not use activities)
//		if (Input.GetKeyUp(KeyCode.Escape)) {
//			// Check if Chartboost wants to respond to it
//			if (CBBinding.onBackPressed()) {
//				// If so, return and ignore it
//				return;
//			} 
//		}
//		#endif
//	}
	
	void OnEnable() {
//		// Initialize the Chartboost plugin
//		#if UNITY_ANDROID
//		if(buildForAmazon)
//			CBBinding.init(amazonAppID, amazonSignature);
//		else
//			// Replace these with your own Android app ID and signature from the Chartboost web portal
//			CBBinding.init(androidAppID, androidSignature);
//		#elif UNITY_IPHONE
//		// Replace these with your own iOS app ID and signature from the Chartboost web portal
//		CBBinding.init(iosAppID, iosSignature);
//		#endif

		Chartboost.didFailToLoadMoreApps += didFailToLoadMoreApps;
		Chartboost.didDismissMoreApps += didDismissMoreApps;
		Chartboost.didCloseMoreApps += didCloseMoreApps;
		Chartboost.didClickMoreApps += didClickMoreApps;
		Chartboost.didCacheMoreApps += didCacheMoreApps;
		Chartboost.shouldDisplayMoreApps += shouldDisplayMoreApps;
		Chartboost.didDisplayMoreApps += didDisplayMoreApps;

		#if UNITY_IPHONE
		Chartboost.didCompleteAppStoreSheetFlow += didCompleteAppStoreSheetFlow;
		#endif
		
		cachearMoreApps ();
	}
	
//	void OnApplicationPause(bool paused) {
//		#if UNITY_ANDROID
//		// Manage Chartboost plugin lifecycle
//		CBBinding.pause(paused);
//		#endif
//	}
	
	void OnDisable() {
//		// Shut down the Chartboost plugin
//		#if UNITY_ANDROID
//		CBBinding.destroy();
//		#endif


		Chartboost.didDismissMoreApps -= didDismissMoreApps;
		Chartboost.didCloseMoreApps -= didCloseMoreApps;
		Chartboost.didClickMoreApps -= didClickMoreApps;
		Chartboost.didCacheMoreApps -= didCacheMoreApps;
		Chartboost.shouldDisplayMoreApps -= shouldDisplayMoreApps;
		Chartboost.didDisplayMoreApps -= didDisplayMoreApps;
		#if UNITY_IPHONE
		Chartboost.didCompleteAppStoreSheetFlow -= didCompleteAppStoreSheetFlow;
		#endif
	}



	public void mostrarPantallazo(){
		Chartboost.showInterstitial(CBLocation.Default);
	}
	public void cacherarPantallazo(){
		Chartboost.cacheInterstitial(CBLocation.Default);
	}
	public void mostrarMoreApps(){
		Chartboost.showMoreApps(CBLocation.Default);

		if(!estaCacheadoMoreApps())
			cachearMoreApps();
	}
	public void cachearMoreApps(){
		Chartboost.cacheInterstitial(CBLocation.Default);
	}
	public bool estaCacheadoMoreApps(){
		return Chartboost.hasMoreApps (CBLocation.Default);
	}
	public bool estaCacheadoPantallazo(){
		return Chartboost.hasInterstitial (CBLocation.Default);
	}


	//EVENTOS CHARTBOOST
	void didFailToLoadMoreApps(CBLocation location, CBImpressionError error) {
//		Debug.Log(string.Format("didFailToLoadMoreApps: {0} at location: {1}", error, location));
	}
	
	void didDismissMoreApps(CBLocation location) {
//		Debug.Log(string.Format("didDismissMoreApps at location: {0}", location));
	}
	
	void didCloseMoreApps(CBLocation location) {
//		Debug.Log(string.Format("didCloseMoreApps at location: {0}", location));
	}
	
	void didClickMoreApps(CBLocation location) {
//		Debug.Log(string.Format("didClickMoreApps at location: {0}", location));
	}
	
	void didCacheMoreApps(CBLocation location) {
//		Debug.Log(string.Format("didCacheMoreApps at location: {0}", location));
	}
	
	bool shouldDisplayMoreApps(CBLocation location) {
//		Debug.Log(string.Format("shouldDisplayMoreApps at location: {0}", location));
		return true;
	}
	
	void didDisplayMoreApps(CBLocation location){
//		Debug.Log("didDisplayMoreApps: " + location);
	}
	#if UNITY_IPHONE
	void didCompleteAppStoreSheetFlow() {
		Debug.Log("didCompleteAppStoreSheetFlow");
	}
	#endif

	// UNITY_ANDROID || UNITY_IPHONE
	#endif
}
