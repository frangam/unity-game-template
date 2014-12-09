using UnityEngine;
using System.Collections;

public class ScreenLoaderIndicator : Singleton<ScreenLoaderIndicator> {

	public IEnumerator Load(string escena){
		#if UNITY_IPHONE
		Handheld.SetActivityIndicatorStyle(iOSActivityIndicatorStyle.WhiteLarge);
		Handheld.StartActivityIndicator();
		#elif UNITY_ANDROID
			Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
		Handheld.StartActivityIndicator();
		#endif

		yield return null;
		Application.LoadLevel (escena);
//		finCarga ();
	}

	public IEnumerator Load(int escena){
		#if UNITY_IPHONE
		Handheld.SetActivityIndicatorStyle(iOSActivityIndicatorStyle.WhiteLarge);
		Handheld.StartActivityIndicator();
		#elif UNITY_ANDROID
		Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
		Handheld.StartActivityIndicator();
		#endif

		yield return null;
//		finCarga ();
		Application.LoadLevel (escena);
	}

	public IEnumerator Load(){
		#if UNITY_IPHONE
		Handheld.SetActivityIndicatorStyle(iOSActivityIndicatorStyle.WhiteLarge);
		Handheld.StartActivityIndicator();
		#elif UNITY_ANDROID
		Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
		Handheld.StartActivityIndicator();
		#endif

		yield return new WaitForSeconds(0);
//		finCarga ();
	}

	public void finCarga(){
		#if UNITY_IPHONE || UNITY_ANDROID
		Handheld.StopActivityIndicator();
		#endif
	}
}
