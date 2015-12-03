//#define SA_DEBUG_MODE
using UnityEngine;
using System;
using System.Collections;
#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif



public class IOSNativeUtility : ISN_Singleton<IOSNativeUtility> {


	#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
	[DllImport ("__Internal")]
	private static extern void _ISN_RedirectToAppStoreRatingPage(string appId);

	[DllImport ("__Internal")]
	private static extern void _ISN_ShowPreloader();
	
	[DllImport ("__Internal")]
	private static extern void _ISN_HidePreloader();


	[DllImport ("__Internal")]
	private static extern void _ISN_SetApplicationBagesNumber(int count);


	[DllImport ("__Internal")]
	private static extern void _ISN_GetLocale();

	#endif
	public static event Action<ISN_Locale> OnLocaleLoaded = delegate {};


	void Awake() {
		DontDestroyOnLoad (gameObject);
	}


	public void GetLocale() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
		_ISN_GetLocale();
		#endif
	}

	public static void RedirectToAppStoreRatingPage() {
		RedirectToAppStoreRatingPage(IOSNativeSettings.Instance.AppleId);
	}

	public static void RedirectToAppStoreRatingPage(string appleId) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_ISN_RedirectToAppStoreRatingPage(appleId);
		#endif
	}

	public static void SetApplicationBagesNumber(int count) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_ISN_SetApplicationBagesNumber(count);
		#endif
	}



	public static void ShowPreloader() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_ISN_ShowPreloader();
		#endif
	}
	
	public static void HidePreloader() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_ISN_HidePreloader();
		#endif
	}


	//--------------------------------------
	//  Handlers
	//--------------------------------------


	private void OnLocaleLoadedHandler(string data)  {
		string[] dataArray 		= data.Split(IOSNative.DATA_SPLITTER);
		string countryCode 		= dataArray[0];
		string contryName 		= dataArray[1];
		string languageCode 	= dataArray[2]; 
		string languageName  	= dataArray[3];

		ISN_Locale locale = new ISN_Locale (countryCode, contryName, languageCode, languageName);
		OnLocaleLoaded (locale);

		

	}


}
