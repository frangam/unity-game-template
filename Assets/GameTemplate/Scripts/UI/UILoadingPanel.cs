using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UILoadingPanel : Singleton<UILoadingPanel> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private float alfa = 0.748f;
	
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
			Debug.LogError("Not found CanvasGroup component in children");
		
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
}
