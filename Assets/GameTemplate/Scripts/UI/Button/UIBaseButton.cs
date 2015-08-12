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
	private bool playSoundWhenPress = true;
	
	[SerializeField]
	private bool disableButtonWhenCantPress = false;
	
	[SerializeField]
	private bool changeLbBtnNameWhenCantPress = false;
	
	[SerializeField]
	private float rateForCheckCanPress = 5;
	
	[SerializeField]
	private Animator externalAnim;
	
	[SerializeField]
	private bool hasShowTriggerExtAnim = false;
	
	//	[SerializeField]
	//	private bool hasShowAnimTrigger = false;
	
	[SerializeField]
	private string showExtAnimTrigger = "Show";
	
	[SerializeField]
	private string disableExtAnimTrigger = "Disable";
	
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
	
	[SerializeField]
	private Text lbButtonName;
	
	[SerializeField]
	private string locAvailableAd = "lb_available_ad";
	
	[SerializeField]
	private string locNoAvailableAd = "lb_no_available_ad";
	
	
	
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
	private bool disabled = false;
	private bool shown = false;
	
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
		StartCoroutine(doCheckAvailability());
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
	private IEnumerator doCheckAvailability(){
		checkAvailability();
		yield return new WaitForSeconds(rateForCheckCanPress);
		StartCoroutine(doCheckAvailability());
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	protected virtual void init(){
		button = GetComponent<Button>();
		pressingStoped = false;
		initialPressingTime = 0;
		lastSuccesfulPressingTime = 0;
		pressingTime = 0;
		pressing = false;
		disabled = false;
		shown = false;
		
		if(changeLbBtnNameWhenCantPress && !lbButtonName){
			GTDebug.logErrorAlways("Not found label for button name");
		}
		
		checkAvailability();
	}
	
	public void press(){
		firstPress = false;
		pressing = true;
		initialPressingTime = Time.time;
		
		if(canPress()){
			lastSuccesfulPressingTime = Time.time;
			StartCoroutine(doPressBeforeAWhile());
		}
	}
	
	public virtual void checkAvailability(){
		bool canP = canPress();
		
		if(button){
			button.interactable = canP;
			
			if(disableButtonWhenCantPress){
				if(canP && (disabled || !shown) && hasShowTriggerExtAnim && externalAnim && !string.IsNullOrEmpty(showExtAnimTrigger)){		
					externalAnim.SetTrigger(showExtAnimTrigger);
					disabled = false;
					shown = true;
				}
				else if(!canP && !disabled && hasShowTriggerExtAnim && externalAnim && !string.IsNullOrEmpty(disableExtAnimTrigger)){
					externalAnim.SetTrigger(disableExtAnimTrigger);
					disabled = true;
					shown = false;
				}
				
				
				
				switch(button.transition){
				case Selectable.Transition.ColorTint: button.targetGraphic.color = canP ? button.colors.normalColor : button.colors.disabledColor; break;
				case Selectable.Transition.Animation: 
					if(canP && (disabled || !shown) && button.animator){		
						button.animator.SetTrigger(button.animationTriggers.normalTrigger);
						
						if(!externalAnim){
							disabled = false;
							shown = true;
						}
					}else if(!canP && !disabled && button.animator){
						button.animator.SetTrigger(button.animationTriggers.disabledTrigger);
						
						if(!externalAnim){
							disabled = true;
							shown = false;
						}
					}
					break;
				}
			}
		}
		
		
		if(lbButtonName && changeLbBtnNameWhenCantPress){
			lbButtonName.text = canP ? Localization.Get(locAvailableAd) : Localization.Get(locNoAvailableAd);
		}
		else if(lbButtonName && !changeLbBtnNameWhenCantPress){
			lbButtonName.text = Localization.Get(locAvailableAd);
		}
	}
	
	
	protected virtual IEnumerator doPressBeforeAWhile(){
		yield return new WaitForSeconds(timeToWaitBeforeDoPress);
		doPress();
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
