using UnityEngine;
using System.Collections;

public class ScreenLoaderVisualIndicator : Singleton<ScreenLoaderVisualIndicator> {
	
	private IEnumerator Load(string escena, bool showLoadIndicator = true,  bool showLoadingPanel = true){
		if(showLoadingPanel)
			UILoadingPanel.Instance.show();
		
		if(showLoadIndicator){
			#if UNITY_IPHONE
			Handheld.SetActivityIndicatorStyle(UnityEngine.iOS.ActivityIndicatorStyle.WhiteLarge);
			Handheld.StartActivityIndicator();
			#elif UNITY_ANDROID
			Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
			Handheld.StartActivityIndicator();
			#endif
		}
		
		//		yield return null;
		//		Application.LoadLevel (escena);
		//		finCarga ();
		
		AsyncOperation async = Application.LoadLevelAsync(escena);
		if(async == null)
			async.allowSceneActivation = false;
		while (!async.isDone) {
			//			Debug.Log("%: " + async.progress);
			UILoadingPanel.Instance.changeProgress(async.progress);
			yield return null;
		}
		if(async != null && async.progress >= 0.9f)
			async.allowSceneActivation = true;
		//
		//		yield return async;
		//		Debug.Log("Loading complete");
	}
	
	private IEnumerator Load(int escena, bool showLoadIndicator = true,  bool showLoadingPanel = true){
		if(showLoadingPanel)
			UILoadingPanel.Instance.show();
		
		if(showLoadIndicator){
			#if UNITY_IPHONE
			Handheld.SetActivityIndicatorStyle(UnityEngine.iOS.ActivityIndicatorStyle.WhiteLarge);
			Handheld.StartActivityIndicator();
			#elif UNITY_ANDROID
			Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
			Handheld.StartActivityIndicator();
			#endif
		}
		
		//		yield return null;
		//		finCarga ();
		//		Application.LoadLevel (escena);
		AsyncOperation async = Application.LoadLevelAsync(escena);
		if(async == null)
			async.allowSceneActivation = false;
		while (!async.isDone) {
			//			Debug.Log("%: " + async.progress);
			UILoadingPanel.Instance.changeProgress(async.progress);
			yield return null;
		}
		if(async != null && async.progress >= 0.9f)
			async.allowSceneActivation = true;
		
		
		//		yield return null;
		//		Debug.Log("Loading complete");
	}
	
	private IEnumerator ShowLoadIndicatorOnly(){
		#if UNITY_IPHONE
		Handheld.SetActivityIndicatorStyle(UnityEngine.iOS.ActivityIndicatorStyle.WhiteLarge);
		Handheld.StartActivityIndicator();
		#elif UNITY_ANDROID
		Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
		Handheld.StartActivityIndicator();
		#endif
		
		yield return null;
		//		finCarga ();
	}
	
	public void ShowLoadIndicator(){
		StartCoroutine(ShowLoadIndicatorOnly());
	}
	
	public void LoadScene(string scene, bool showLoadIndicator = true, bool showLoadingPanel = true){
		StartCoroutine(Load(scene, showLoadIndicator, showLoadingPanel));
	}
	
	public void LoadScene(int scene, bool showLoadIndicator = true, bool showLoadingPanel = true){
		StartCoroutine(Load(scene, showLoadIndicator, showLoadingPanel));
	}
	
	public void finishLoad(){
		#if UNITY_IPHONE || UNITY_ANDROID
		Handheld.StopActivityIndicator();
		#endif
	}
}
