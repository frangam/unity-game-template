/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnionAssets.FLE;

public class UICharacterControlSensitivity : MonoBehaviour {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string ON_CHANGED_VALUE = "character_control_sensitivity_on_changed_value";
	
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	private static EventDispatcherBase _dispatcher  = new EventDispatcherBase ();
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Slider slider;
	
	[SerializeField]
	private Text lbValue;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private float 		minValue = 0.25f;
	private float 		maxValue = 10f;
	private float 		initialValue = 100;
	private float 		_value = 100;
	private GameObject	character;
	private bool 		inited = false;
	
	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public static EventDispatcherBase dispatcher {
		get {
			return _dispatcher;
		}
	}
	
	public float Value {
		get {
			return slider.value;
		}
		set {
			slider.value = value;
		}
	}
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		//		_value = initialValue;
		if(slider == null){
			slider = GetComponent<Slider>();
			
			if(slider == null)
				Debug.LogError("Not found slider component");
		}
		
		//initialization
		if(slider != null){
			init();
		}
	}
	
	//	void Update(){
	//		if(inited)
	//			slider.value = _value/maxValue;
	//	}
	#endregion
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(){
		initialValue = PlayerPrefs.GetFloat(GameSettings.PP_CHARACTER_CONTROL_SENSITIVITY);
		maxValue = GameSettings.Instance.MAX_CHAR_CONTROL_SENSITIVITY;
		minValue = GameSettings.Instance.MIN_CHAR_CONTROL_SENSITIVITY;
		
		slider.value = initialValue/maxValue;
		lbValue.text = initialValue.ToString("#.##");
		inited = true;
	}
	
	public void changeValue(){
		float valueResult = Mathf.Clamp(slider.value*maxValue, minValue, maxValue);
		PlayerPrefs.SetFloat(GameSettings.PP_CHARACTER_CONTROL_SENSITIVITY, valueResult); //save it
		lbValue.text = valueResult.ToString("#.##");
		dispatcher.dispatch(ON_CHANGED_VALUE, valueResult);
	}
}
