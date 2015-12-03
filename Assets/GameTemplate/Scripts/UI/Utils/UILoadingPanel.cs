/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UILoadingPanel : Singleton<UILoadingPanel> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private float alfa = 0.748f;
	
	[SerializeField]
	private Slider slider;
	
	[SerializeField]
	private Text lbProgress;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private CanvasGroup panel;
	private float currentProgress;
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		panel = GetComponentInChildren<CanvasGroup>();
		
		if(panel == null)
			Debug.LogWarning("Not found CanvasGroup component in children");
		
		hide();
	}
	#endregion
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void show(){
		if(panel){
			panel.alpha = alfa;
			panel.gameObject.SetActive(true);
		}
	}
	
	public void hide(){
		if(panel){
			panel.alpha = 0;
			panel.gameObject.SetActive(false);
		}
	}
	
	public void changeProgress(float pProgress, bool addToCurrentProgress = true,float divideSliderValueFactor = 1){
		//add or set current progress
		if(addToCurrentProgress){
			currentProgress += pProgress;
		}
		else if(!addToCurrentProgress && currentProgress > 0){
			currentProgress += pProgress;
		}
		else if(!addToCurrentProgress){
			currentProgress = pProgress; 
		}
		
		
		if(slider)
			slider.value = currentProgress/divideSliderValueFactor;
		
		if(lbProgress)
			lbProgress.text = ((int)(currentProgress*100)).ToString()+"%";
		
		
	}
}
