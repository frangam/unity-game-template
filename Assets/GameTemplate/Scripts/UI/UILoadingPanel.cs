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
	
	public void changeProgress(float pProgress){
		if(slider)
			slider.value = pProgress/0.9f;
		
		if(lbProgress)
			lbProgress.text = ((int)(pProgress*100)).ToString()+"%";
	}
}
