using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Base game screen controller handles the logic of an specific Screen
/// </summary>
public class BaseGameScreenController : Singleton<BaseGameScreenController> {
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	[SerializeField]
	private GameSection currentSection = GameSection.MAIN_MENU;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public GameSection Section {
		get {
			return this.currentSection;
		}
	}
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		
	}
	
	public virtual void Start () {
		//set Timemanager fixed timestep
		BaseGameScreenController.Instance.resetFixedTimeStepOfTimeManager();
		
		
		#if  (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		//refresh banner in every Screen loaded
		AdsHandler.Instance.refrescarBanner();
		#endif
		//stop visual loader indicator
		ScreenLoaderVisualIndicator.Instance.finishLoad ();
		
		//reset time scale to 1 en all sections except Game Scene
		if(currentSection != GameSection.GAME){
			Time.timeScale = 1f;
			
		}
		
		//Generic logic for these sections
		switch(currentSection){
		case GameSection.MAIN_MENU:
			BaseSoundManager.Instance.stop (BaseSoundIDs.GAME_MUSIC);
			BaseSoundManager.Instance.play (BaseSoundIDs.MENU_MUSIC);
			
			break;
			
		case GameSection.GAME:
			BaseSoundManager.Instance.stop (BaseSoundIDs.MENU_MUSIC);
			BaseSoundManager.Instance.play (BaseSoundIDs.GAME_MUSIC);
			break;
		}
	}
	
	
	public virtual void OnApplicationPause(bool paused){
		#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)
		if(currentSection != GameSection.GAME){
			Time.timeScale = paused ? 0f : 1f;
		}
		else if(paused && !GameController.Instance.Manager.Finished){
			GTDebug.log("Pausing game in Game Scene");
			
			GameController.Instance.Manager.Paused = paused;
		}
		
		
		if(!paused){ //resume
			ScreenLoaderVisualIndicator.Instance.finishLoad();
		}
		#endif
	}
	
	#endregion
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void resetFixedTimeStepOfTimeManager(){
		if(!PlayerPrefs.HasKey(GameSettings.PP_DEFAULT_FIXED_TIMESTEP))
			PlayerPrefs.SetFloat(GameSettings.PP_DEFAULT_FIXED_TIMESTEP, Time.fixedDeltaTime);
		else{
			float defaultValue = PlayerPrefs.GetFloat(GameSettings.PP_DEFAULT_FIXED_TIMESTEP);
			
			if(defaultValue != Time.fixedDeltaTime)
				Time.fixedDeltaTime = defaultValue;
		}
	}
}
