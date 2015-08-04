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
	private bool playSoundWhenPress = true;
	
	[SerializeField]
	private SoundTrigger trigger;
	
	[SerializeField]
	[Tooltip("Use BaseSoundIDs class or inherited to set this value")]
	private string soundId = BaseSoundIDs.UI_BUTTON_CLICK_FX;
	
	[SerializeField]
	[Tooltip("Time to wait before to do the press effect if we want to wait a while. 0 if not wait and doing press effect inmediately.")]
	private float timeToWaitBeforeDoPress = 0;
	
	[SerializeField]
	[Tooltip("Time to stop the button pressing. 0 if not stopd")]
	private float timeToStopPressing = 0;
	
	[SerializeField]
	[Tooltip("Time to wait before press the button again. 0 if not wait.")]
	private float timeToWaitBeforePressAgain = 0;
	
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected Button button;
	private float initialPressingTime = 0;
	private float lastSuccesfulPressingTime = 0;
	private float pressingTime = 0;
	private float currentTime;
	private bool pressingStoped = false;
	private bool pressing = false;
	private bool firstPress = true;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void OnEnable(){}
	public virtual void OnDisable(){}
	public virtual void Awake(){
		init();
	}
	public virtual void Start(){
		init();
	}
	public virtual void OnDestroy(){
		
	}
	public virtual void Update(){
		//stop the pressing proccess during maintaining press the button
		if(timeToStopPressing > 0 && pressing && !pressingStoped){
			currentTime = Time.time;
			
			if((currentTime - initialPressingTime) >= timeToStopPressing){
				stopPressing();
			}
		}
		
		//stop pressing when user wants to press again
		if(timeToWaitBeforePressAgain > 0 && !firstPress){
			currentTime = Time.time;
			
			if((currentTime - lastSuccesfulPressingTime) <= timeToWaitBeforePressAgain){
				pressingStoped = true;
			}
			else{
				pressingStoped = false;
			}
		}
	}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private IEnumerator doPressBeforeAWhile(){
		yield return new WaitForSeconds(timeToWaitBeforeDoPress);
		doPress();
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void press(){
		firstPress = false;
		pressing = true;
		initialPressingTime = Time.time;
		
		if(canPress()){
			lastSuccesfulPressingTime = Time.time;
			StartCoroutine(doPressBeforeAWhile());
		}
	}
	
	protected virtual void init(){
		button = GetComponent<Button>();
		pressingStoped = false;
		initialPressingTime = 0;
		lastSuccesfulPressingTime = 0;
		pressingTime = 0;
		pressing = false;
	}
	
	protected virtual void doPress(){
		if(playSoundWhenPress && !pressingStoped){
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
	
	/// <summary>
	/// Press a button
	/// </summary>
	protected virtual bool canPress(){
		return !pressingStoped;
	}
	
	public virtual void release(){
		pressing = false;
		pressingTime = Time.time;
		pressingStoped = false;
		
		if(button != null && button.animator != null)
			button.animator.ResetTrigger(button.animationTriggers.pressedTrigger);
		
	}
	
	public virtual void stopPressing(){
		pressingStoped = true;
	}
}
