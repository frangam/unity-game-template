using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Game Template/UI")]
[RequireComponent(typeof(Button))]
public class UIBaseButton : MonoBehaviour {
	//Button Event When sound will play
	public enum SoundTrigger{
		TOUCH_UP,
		TOUCH_DOWN,
	}

	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private SoundTrigger trigger;
	
	[SerializeField]
	[Tooltip("Use BaseSoundIDs class or inherited to set this value")]
	private string soundId = BaseSoundIDs.UI_BUTTON_CLICK_FX;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Button button;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		button = GetComponent<Button>();
	}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void press(){
		//touch up
		if(trigger == SoundTrigger.TOUCH_UP){
			BaseSoundManager.Instance.play(soundId);
		}
		//touch down
		else if(trigger == SoundTrigger.TOUCH_DOWN){
			BaseSoundManager.Instance.play(soundId);
		}
	}
}
