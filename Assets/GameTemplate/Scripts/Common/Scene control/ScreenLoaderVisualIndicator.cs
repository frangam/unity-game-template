/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
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
		//set the current section as the previous one, because we are going to load a new one
		GameSettings.previousGameSection = BaseGameScreenController.Instance.Section;
		
		StartCoroutine(Load(scene, showLoadIndicator, showLoadingPanel));
	}
	
	public void LoadScene(int scene, bool showLoadIndicator = true, bool showLoadingPanel = true){
		//set the current section as the previous one, because we are going to load a new one
		GameSettings.previousGameSection = BaseGameScreenController.Instance.Section;
		
		StartCoroutine(Load(scene, showLoadIndicator, showLoadingPanel));
	}
	
	public bool LoadScene(GameSection section, bool showLoadIndicator = true, bool showLoadingPanel = true){
		List<GTBuildScene> scenes = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.scenes;
		bool valid = scenes != null && scenes.Count > 0;
		
		//set the current section as the previous one, because we are going to load a new one
		//		GameSettings.previousGameSection = BaseGameScreenController.Instance.Section;
		
		if(valid){
			string selectedScene = "";
			int sceneIndex = 0;
			for(int i=0; i<scenes.Count; i++){
				if(scenes[i].Section == section){
					selectedScene = scenes[i].name;
					sceneIndex = i;
					break;
				}
			}
			valid = !string.IsNullOrEmpty(selectedScene);
			
			if(valid){
				StartCoroutine(Load(sceneIndex, showLoadIndicator, showLoadingPanel));
			}
			else{
				GTDebug.logErrorAlways("Not found scene for section "+section+" in this current build "+GameSettings.Instance.currentGameMultiversion.ToString());
			}
		}
		else{
			GTDebug.logErrorAlways("Not found any scene in this current build "+GameSettings.Instance.currentGameMultiversion.ToString());
		}
		
		return valid;
	}
	
	public void finishLoad(){
		#if UNITY_IPHONE || UNITY_ANDROID
		Handheld.StopActivityIndicator();
		#endif
	}
}
