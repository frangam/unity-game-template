using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHealthBar : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Slider slider;
	[SerializeField]
	private bool hideWhenCharDead = true;
//	[SerializeField]
//	private Texture		texBackground;	//texture for bacground bar
//	[SerializeField]
//	private Texture		texForeground;	//texture for life foreground bar

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private float 		maxValue = 100;
	private float 		initialValue = 100;
	private float 		_value = 100;
	private Collider	collider;
	private GameObject	livingCharacter;
	private bool 		inited = false;


	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public float Value {
		get {
			return this._value;
		}
		set {
			_value = value;
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
	}

	void Update(){
		if(inited){
			slider.value = _value/maxValue;

			if(hideWhenCharDead && slider.value <= 0)
				gameObject.SetActive(false);
		}


	}

//	void OnGUI(){
//		draw ();
//	}
	#endregion

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(float _initialValue, float _maxValue, GameObject go = null){
		initialValue = _initialValue;
		maxValue = _maxValue;
		_value = initialValue;
		livingCharacter = go;
		inited = true;
	}
	public void changeValue(float value){
		_value = value;
	}
//
//	//--------------------------------------
//	// Private Methods
//	//--------------------------------------
//	private void draw(){
//		if(livingCharacter == null) return;
//
//		//Life bar
//		Rect rcBg = new Rect(0,0, 60,14); //rect for the background
//		Rect rcProgress = new Rect(0,0,((this._value)/this.initialValue)*56,10); //rect for the progress
//		Rect label = new Rect(0,0,60,30);
//
//		float offset = livingCharacter != null && livingCharacter.collider != null ? livingCharacter.collider.bounds.size.y : 0;
//
//		//screen point to locate in 3D space the progress bar on the top of the livingCharacter
//		Vector3 point = Camera.mainCamera.WorldToScreenPoint(new Vector3(
//			livingCharacter.transform.position.x,
//			livingCharacter.transform.position.y + offset ,
//			livingCharacter.transform.position.z 
//			));
//		
//		rcBg.y = Screen.height-point.y;
//		rcBg.x = point.x-30;
//		rcProgress.x = rcBg.x+2;
//		rcProgress.y = rcBg.y+2;
//		label.x = rcBg.x+5;
//		label.y = rcBg.y-12;
//		
//		//draw background and foreground textures and label with the progress value
//		GUI.DrawTexture(rcBg, texBackground);
//		GUI.DrawTexture(rcProgress, texForeground, ScaleMode.ScaleAndCrop);
//		GUI.Label(label, this._value.ToString());
//	}
}
