using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
			UILoadingPanel.Instance.changeProgress(async.progress, false, 1f);
			yield return null;
		}
		//		if(async != null && async.progress >= 1f)
		//			async.allowSceneActivation = true;
		//		//
		//		yield return async;
		//		if(GameSettings.Instance.showTestLogs)
		//			Debug.Log("Loading complete");
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
			UILoadingPanel.Instance.changeProgress(async.progress, false, 1);
			yield return null;
		}
		//		if(async != null && async.progress >= 1)
		//			async.allowSceneActivation = true;
		
		
		//		yield return async;
		
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
	
	public bool LoadScene(GameSection section, bool showLoadIndicator = true, bool showLoadingPanel = true){
		bool valid = false;
		List<GTBuildScene> scenes = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.scenes;
		int selectedScene = 0;
		
		for(int i=0; i<scenes.Count; i++){
			if(scenes[i].Section == section){
				selectedScene = i;
				break;
			}
		}
		
		//		if(!string.IsNullOrEmpty(selectedScene)){
		valid = true;
		StartCoroutine(Load(selectedScene, showLoadIndicator, showLoadingPanel));
		//		}
		
		return valid;
	}
	
	public void finishLoad(){
		#if UNITY_IPHONE || UNITY_ANDROID
		Handheld.StopActivityIndicator();
		#endif
	}
}
