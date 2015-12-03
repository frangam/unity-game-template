/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
public class UIBaseButton : MonoBehaviour {
	//Button Event When sound will play
	public enum SoundTrigger{
		TOUCH_UP,
		TOUCH_DOWN,
	}
	
	//--------------------------------------
	// Constants
	//--------------------------------------
	private const string PP_BUTTON_CLICKS_COUNTER = "pp_button_clicks_counter_";
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	[Tooltip("To identify this button. Mainly, it is used for save clicks counter for this button")]
	private string id;
	
	[SerializeField]
	private bool playSoundWhenPress = true;
	
	[SerializeField]
	private bool playSoundWithoutWaitingPressDelay = true;
	
	[SerializeField]
	private bool disableButtonWhenCantPress = false;
	
	[SerializeField]
	private bool changeLbBtnNameWhenCantPress = false;
	
	[SerializeField]
	[Tooltip("By default, for Analitics purposes to know how may times this button has been clicked")]
	private bool saveClicksCounter = false;
	
	[SerializeField]
	[Range(-1,int.MaxValue)]
	[Tooltip("< 0: Init clicks counter with saved value")]
	private int initialValueClicksCounterOnAwake = -1;
	
	[SerializeField]
	[Tooltip("Previous Sections we are coming from to reset clicks counter to zero of this button")]
	private List<GameSection> prevSectionsToInitClicksCounterToZero;
	
	[SerializeField]
	[Tooltip("Reset clicks counter of these buttons when the current instance button is pressed")]
	private UIBaseButton[] btnsResetClicksCounterWhenClickThis;
	
	[SerializeField]
	[Range(-100,100)]
	protected int resetValueForResetButtons = -2;
	
	[SerializeField]
	private float rateForCheckCanPress = 5;
	
	[SerializeField]
	[Tooltip("If it has an external animator located in other GameObject")]
	private Animator externalAnim;
	
	[SerializeField]
	private bool hasShowTriggerExtAnim = false;
	
	//	[SerializeField]
	//	private bool hasShowAnimTrigger = false;
	
	[SerializeField]
	private string showExtAnimTrigger = "Show";
	
	[SerializeField]
	private string disableExtAnimTrigger = "Disable";
	
	//	[SerializeField]
	//	private SoundTrigger trigger;
	
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
	private bool hold = false;
	
	[SerializeField]
	private Text lbButtonName;
	
	[SerializeField]
	private string locCanPress = "lb_can_press";
	
	[SerializeField]
	private string locCannotPress = "lb_cannot_press";
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected Button 	button;
	private float 		initialPressingTime = 0;
	private float 		lastSuccesfulPressingTime = 0;
	private float 		pressingTime = 0;
	private float 		currentTime;
	private bool 		pressingStoped = false;
	private bool 		pressing = false;
	private bool 		firstPress = true;
	private bool 		disabled = false;
	private bool 		shown = false;
	private bool 		pointerDown;
	private float 		timePointerDown;
	private int			clicksCounter = 0;
	private string		clicksCounterPPKey; //PlayerPrefs key for clicks counter
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	#region getters/setters
	public string Id {
		get {
			return this.id;
		}
	}
	
	public Button Button {
		get {
			return this.button;
		}
	}
	
	public float InitialPressingTime {
		get {
			return this.initialPressingTime;
		}
	}
	
	public float PressingTime {
		get {
			return this.pressingTime;
		}
	}
	
	public bool PressingStoped {
		get {
			return this.pressingStoped;
		}
	}
	
	public bool Pressing {
		get {
			return this.pressing;
		}
	}
	
	public bool FirstPress {
		get {
			return this.firstPress;
		}
	}
	
	public bool Disabled {
		get {
			return this.disabled;
		}
	}
	
	public bool Shown {
		get {
			return this.shown;
		}
	}
	
	public bool PointerDown {
		get {
			return this.pointerDown;
		}
	}
	
	public float TimePointerDown {
		get {
			return this.timePointerDown;
		}
	}
	
	public int ClicksCounter {
		get {
			return this.clicksCounter;
		}
	}
	
	public string ClicksCounterPPKey {
		get {
			return this.clicksCounterPPKey;
		}
	}
	
	
	#endregion
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void OnEnable(){
		StartCoroutine(doCheckAvailability());
	}
	public virtual void OnDisable(){}
	public virtual void Awake(){
		init();
	}
	public virtual void Start(){
		
	}
	public virtual void OnDestroy(){
		
	}
	public virtual void Update(){
		//if we want to hold a button pressed
		//we need to press every frame because the EventTrigger API does not make this for us
		if(hold && pointerDown){
			timePointerDown += Time.deltaTime;
			press();
		}
		
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
		if(disableButtonWhenCantPress){
			checkAvailabilityForPressing();
			yield return new WaitForSeconds(rateForCheckCanPress);
			StartCoroutine(doCheckAvailability());
		}
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
			GTDebug.logErrorAlways(" btn "+ name + " Not found label for button name");
		}
		
		if(disableButtonWhenCantPress)
			checkAvailabilityForPressing();
		
		//clicks counter
		if(saveClicksCounter && !string.IsNullOrEmpty(id)){
			clicksCounterPPKey = PP_BUTTON_CLICKS_COUNTER+id;
			
			//init to zero the clicks count if previous section is in the list
			if(prevSectionsToInitClicksCounterToZero != null && prevSectionsToInitClicksCounterToZero.Count > 0
			   && prevSectionsToInitClicksCounterToZero.Contains(GameSettings.previousGameSection)){
				resetClicksCounter();
			}
			else{
				if(initialValueClicksCounterOnAwake >= 0){
					clicksCounter = initialValueClicksCounterOnAwake;
					PlayerPrefs.SetInt(clicksCounterPPKey, clicksCounter);
				}
				else{
					clicksCounter = PlayerPrefs.GetInt(clicksCounterPPKey);
				}
			}
		}
		else if(saveClicksCounter && string.IsNullOrEmpty(id)){
			GTDebug.logErrorAlways("You must indicate an identifier for this Button ("+gameObject.name+") for saving clicks counter");
		}
		
		//Inits the reset value for reset buttons if we are redifining this method in a child class
		initResetValueForResetButtons();
	}
	
	public void OnPointerDown(){
		pointerDown = true;
	}
	public void OnPointerUp(){
		pointerDown = false;
		timePointerDown = 0f;
	}
	
	public void press(){
		firstPress = false;
		pressing = true;
		initialPressingTime = Time.time;
		
		if(canPress()){
			//----
			//reset clicks counter for these buttons
			//we can initialize in a child class the value "resetValueForResetButtons" 
			//if we need a different initialization mode than the default (setting the value in the inspector)
			//----
			if(btnsResetClicksCounterWhenClickThis != null && btnsResetClicksCounterWhenClickThis.Length > 0){
				foreach(UIBaseButton b in btnsResetClicksCounterWhenClickThis){
					//a button gameobject could have several UIBaseButton components
					UIBaseButton[] btns = b.GetComponents<UIBaseButton>();
					
					//so we only are interesting only they have saveClicksCounter flag set to true
					foreach(UIBaseButton bAux in btns){
						if(bAux.saveClicksCounter)
							bAux.resetClicksCounter(resetValueForResetButtons); 
					}
				}
			}
			
			//----
			//Play sound after press delay
			//----
			if(playSoundWhenPress && playSoundWithoutWaitingPressDelay && !pressingStoped){
				BaseSoundManager.Instance.play(soundId);
			}
			lastSuccesfulPressingTime = Time.time;
			StartCoroutine(doPressBeforeAWhile());
			
			//----
			//save clicks counter
			//----
			if(saveClicksCounter && !string.IsNullOrEmpty(id)){
				if(string.IsNullOrEmpty(clicksCounterPPKey))
					clicksCounterPPKey = PP_BUTTON_CLICKS_COUNTER+id;
				
				clicksCounter++;
				PlayerPrefs.SetInt(clicksCounterPPKey, clicksCounter);
			}
		}
	}
	
	/// <summary>
	/// Inits the reset value for reset buttons.
	/// </summary>
	public virtual void initResetValueForResetButtons(){
		
	}
	
	public void resetClicksCounter(int resetValue=-1){
		if(!string.IsNullOrEmpty(id)){
			if(string.IsNullOrEmpty(clicksCounterPPKey))
				clicksCounterPPKey = PP_BUTTON_CLICKS_COUNTER+id;
			
			clicksCounter = resetValue;
			PlayerPrefs.SetInt(clicksCounterPPKey, clicksCounter);
		}
		else{
			GTDebug.logErrorAlways("Trying to reset clicks counter of this button ["+gameObject.name+"], but it has not an ID");
		}
	}
	
	
	/// <summary>
	/// Checks the availability for pressing this button
	/// </summary>
	public virtual void checkAvailabilityForPressing(){
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
			lbButtonName.text = canP ? Localization.Get(locCanPress) : Localization.Get(locCannotPress);
		}
		else if(lbButtonName && !changeLbBtnNameWhenCantPress){
			lbButtonName.text = Localization.Get(locCanPress);
		}
	}
	
	
	protected virtual IEnumerator doPressBeforeAWhile(){
		yield return new WaitForSeconds(timeToWaitBeforeDoPress);
		doPress();
	}
	
	protected virtual void doPress(){
		if(playSoundWhenPress && !playSoundWithoutWaitingPressDelay && !pressingStoped){
			BaseSoundManager.Instance.play(soundId);
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
