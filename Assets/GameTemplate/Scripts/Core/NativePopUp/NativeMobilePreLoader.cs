using UnityEngine;
using System.Collections;

public class NativeMobilePreLoader : NMPopUp {

	public static void ShowPreloader(string title, string message) {
		#if UNITY_ANDROID
		AndroidNativeUtility.ShowPreloader(title, message);
		#endif
		#if UNITY_IPHONE 
		IOSNativeUtility.ShowPreloader();
		#endif
		#if UNITY_WP8
		WP8NativeUtils.ShowPreloader();
		#endif
	}
	public static void HidePreloader() {
		#if UNITY_ANDROID
		AndroidNativeUtility.HidePreloader();
		#endif
		#if UNITY_IPHONE 
		IOSNativeUtility.HidePreloader();
		#endif
		#if UNITY_WP8
		WP8NativeUtils.HidePreloader();
		#endif
	}



}
