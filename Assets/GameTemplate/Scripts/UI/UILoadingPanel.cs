using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
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
		panel = GetComponent<CanvasGroup>();
		hide();
	}
	#endregion

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void show(){
		if(panel)
			panel.alpha = alfa;	
	}
	
	public void hide(){
		if(panel)
			panel.alpha = 0;
	}
}
