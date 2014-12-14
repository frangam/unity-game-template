using UnityEngine;
using System.Collections;

public class ScreenLoaderVisualIndicator : Singleton<ScreenLoaderVisualIndicator> {

	public IEnumerator Load(string escena, bool showLoadingPanel = true){
		if(showLoadingPanel)
			UILoadingPanel.Instance.show();

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

	public IEnumerator Load(int escena, bool showLoadingPanel = true){
		if(showLoadingPanel)
			UILoadingPanel.Instance.show();

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

	public void finishLoad(){
		#if UNITY_IPHONE || UNITY_ANDROID
		Handheld.StopActivityIndicator();
		#endif
	}
}
