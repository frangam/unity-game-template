/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple script that lets you localize a UIWidget.
/// </summary>

//[ExecuteInEditMode]
[RequireComponent(typeof(Text))]
[AddComponentMenu("Game Template/UI/Localize")]
public class UILocalize : MonoBehaviour
{
	
	private Font latinFont;
	private Text lbl;
	
	/// <summary>
	/// Localization key.
	/// </summary>
	
	public string key;
	
	/// <summary>
	/// Manually change the value of whatever the localization component is attached to.
	/// </summary>
	
	public string value
	{
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				if(lbl == null)
					lbl = GetComponent<Text>();
				
				if (lbl != null){
					lbl.text = value;
				}
			}
		}
	}
	
	bool mStarted = false;
	
	/// <summary>
	/// Localize the widget on enable, but only if it has been started already.
	/// </summary>
	
	void Awake(){
		if(lbl == null)
			lbl = GetComponent<Text>();
		
		getInitialFont();
	}
	void OnEnable ()
	{
		Localization.OnLocalize += OnLocalize;
		
		//#if UNITY_EDITOR
		//		if (!Application.isPlaying) return;
		//#endif
		if (mStarted) 
			OnLocalize();
	}
	
	void OnDisable (){
		Localization.OnLocalize -= OnLocalize;
	}
	
	void OnDestroy(){
		Localization.OnLocalize -= OnLocalize;
	}
	
	/// <summary>
	/// Localize the widget on start.
	/// </summary>
	
	void Start ()
	{
		//#if UNITY_EDITOR
		//		if (!Application.isPlaying) return;
		//#endif
		
		mStarted = true;
		OnLocalize();
	}
	
	/// <summary>
	/// This function is called by the Localization manager via a broadcast SendMessage.
	/// </summary>
	
	void OnLocalize ()
	{
		//		if(isActiveAndEnabled){
		if(!mStarted)
			getInitialFont();
		
		if(!Languages.Instance.IsCurrentLangLatin && lbl != null && Languages.Instance.chineseFont != null){
			lbl.font = Languages.Instance.chineseFont;
		}
		else if(lbl != null && latinFont!= null){
			lbl.font = latinFont;
		}
		
		// If no localization key has been specified, use the label's text as the key
		if (string.IsNullOrEmpty(key) && lbl != null){
			key = lbl.text;
		}
		
		// If we still don't have a key, leave the value as blank
		if (!string.IsNullOrEmpty(key)){
			value = Localization.Get(key);
		}
		
		Languages.Instance.labelLocalized();
		//		}
	}
	
	void getInitialFont(){
		//save latin font is set by default a latin font
		if (lbl == null)
			lbl = GetComponent<Text>();
		if (lbl != null)
			latinFont = lbl.font;
	}
}
