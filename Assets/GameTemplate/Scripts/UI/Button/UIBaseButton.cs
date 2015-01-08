using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[AddComponentMenu("Game Template/UI")]
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
	
	[SerializeField]
	[Tooltip("Time to stop the button pressing. 0 if not stopd")]
	private float timeToStopPressing = 0;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Button button;
	private float initialPressingTime = 0;
	private float pressingTime = 0;
	private float currentTime;
	private bool pressingStoped = false;
	private bool pressing = false;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		button = GetComponent<Button>();
		pressingStoped = false;
		initialPressingTime = 0;
		pressingTime = 0;
		pressing = false;
	}
	public virtual void Update(){
		if(timeToStopPressing > 0 && pressing && !pressingStoped){
			currentTime = Time.time;
			
			if((currentTime - initialPressingTime) >= timeToStopPressing){
				stopPressing();
			}
		}
	}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void press(){
		if(!pressingStoped){
			pressing = true;
			initialPressingTime = Time.time;
			
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
	
	public virtual void release(){
		pressing = false;
		pressingTime = Time.time;
		pressingStoped = false;
	}
	
	public virtual void stopPressing(){
		pressingStoped = true;
	}
}
