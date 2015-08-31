/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Image))]
public class UIBaseWindow : MonoBehaviour {
	//--------------------------------------
	// Constants
	//--------------------------------------
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	[Tooltip("Use UIBaseWindowIDs class or inherited to set this value")]
	private string id;
	
	[SerializeField]
	private bool startClosed = true;
	
	[SerializeField]
	private bool closeIsSetAlpha = false;
	
	[SerializeField]
	private float closeAlphaValue = 0f;
	
	[SerializeField]
	private float openAlphaValue = 1f;
	
	[SerializeField]
	[Tooltip("True if we not want to start this window with the UIBaseManager in the loadWindows method. This is useful when we want to open or close a window in other window open or close method")]
	private bool notStartWithManager = false;
	
	[SerializeField]
	[Tooltip("Greater than 0 if we want to close this window after it was open and wait this delay time")]
	private float waitForcloseAfterOpen = 0f;
	
	[SerializeField]
	private bool ignoreTimeScaleInWaitingForCloseAfterOpen = false;
	
	[SerializeField]
	private string openAnimTrigger = "";
	
	[SerializeField]
	private string closeAnimTrigger = "";
	
	[SerializeField]
	[Tooltip("These windows are open when the current window is closed")]
	private List<UIBaseWindow> openNewWinsWhenClose;
	
	[SerializeField]
	[Tooltip("These windows are closed when the current window is open")]
	private List<UIBaseWindow> closeWinsWhenOpen;
	
	[SerializeField]
	[Tooltip("These gameobjects are shown when the current window is closed")]
	private List<GameObject> showObjsWhenClose;
	
	[SerializeField]
	[Tooltip("These gameobjects are hiden when the current window is open")]
	private List<GameObject> hideObjsWhenOpen;
	
	[SerializeField]
	private List<string> soundIdsPlayWhenOpen;
	
	[SerializeField]
	private List<AutoType> autotypeEffects;
	
	[SerializeField]
	private List<string> soundIdsStopWhenOpen;
	
	[SerializeField]
	[Tooltip("Change Input Back Button behaviour")]
	private	ChangeInputBackButtonBehaviour changeIpBB;
	
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private CanvasGroup myCanvasGroup;
	private Animator anim;
	private Image window;
	private bool isOpen = false;
	private bool firstOpen = true;
	private bool firstClose = true;
	private CanvasGroup[] canvasGroups;
	private Button[] buttons;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string Id {
		get {
			return this.id;
		}
	}
	public bool StartClosed {
		get {
			return this.startClosed;
		}
	}
	
	public CanvasGroup CanvasGroup {
		get {
			return this.myCanvasGroup;
		}
	}
	
	public bool CloseIsSetAlpha {
		get {
			return this.closeIsSetAlpha;
		}
	}
	
	public float CloseAlphaValue {
		get {
			return this.closeAlphaValue;
		}
	}
	
	public float OpenAlphaValue {
		get {
			return this.openAlphaValue;
		}
	}
	
	public bool NotStartWithManager {
		get {
			return this.notStartWithManager;
		}
	}
	
	public bool IsOpen {
		get {
			return this.isOpen;
		}
	}
	
	public ChangeInputBackButtonBehaviour ChangeIpBB {
		get {
			return this.changeIpBB;
		}
		set {
			changeIpBB = value;
		}
	}
	public List<GameObject> ShowObjsWhenClose {
		get {
			return this.showObjsWhenClose;
		}
		set {
			showObjsWhenClose = value;
		}
	}
	public List<UIBaseWindow> OpenNewWinsWhenClose {
		get {
			return this.openNewWinsWhenClose;
		}
		set{
			this.openNewWinsWhenClose = value;
		}
	}
	public List<GameObject> HideObjsWhenOpen {
		get {
			return this.hideObjsWhenOpen;
		}
		set {
			hideObjsWhenOpen = value;
		}
	}
	public List<UIBaseWindow> CloseWinsWhenOpen {
		get {
			return this.closeWinsWhenOpen;
		}
		set {
			closeWinsWhenOpen = value;
		}
	}
	
	public bool FirstOpen {
		get {
			return this.firstOpen;
		}
	}
	
	public bool FirstClose {
		get {
			return this.firstClose;
		}
	}
	
	public Animator Anim {
		get {
			return this.anim;
		}
	}
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		canvasGroups = GetComponentsInChildren<CanvasGroup>();
		myCanvasGroup = GetComponent<CanvasGroup>();
		buttons = GetComponentsInChildren<Button>();
		anim = GetComponent<Animator>();
		window = GetComponent<Image>();
		
		if(!window){
			GTDebug.logWarningAlways("No window attached");
		}
		
		if(closeIsSetAlpha && !myCanvasGroup){
			GTDebug.logErrorAlways("Not found canvas group and cloaseIsSetAlpha is set");
		}
	}
	public virtual void Start(){}
	public virtual void Update(){}
	public virtual void OnDestroy(){}
	public virtual void OnEnable(){}
	public virtual void OnDisable(){}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void activeInterectablesChildren(bool active = true){
		if(canvasGroups != null && canvasGroups.Length > 0){
			foreach(CanvasGroup g in canvasGroups){
				g.interactable = active;
				g.blocksRaycasts = active;
			}
		}
		
		if(buttons != null && buttons.Length > 0){
			foreach(Button b in buttons){
				b.interactable = active;
			}
		}
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void open(){
		activeInterectablesChildren(); //active button children
		
		if(firstOpen)
			firstOpen = false;
		
		if(changeIpBB != null)
			changeIpBB.change();
		
		if(soundIdsStopWhenOpen != null && soundIdsStopWhenOpen.Count > 0)
			foreach(string id in soundIdsStopWhenOpen)
				BaseSoundManager.Instance.stop(id);
		
		if(soundIdsPlayWhenOpen != null && soundIdsPlayWhenOpen.Count > 0)
			foreach(string id in soundIdsPlayWhenOpen)
				BaseSoundManager.Instance.play(id);
		
		if(autotypeEffects != null && autotypeEffects.Count > 0)
			foreach(AutoType at in autotypeEffects)
				at.initType();
		
		isOpen = true;
		
		if(Anim != null && !string.IsNullOrEmpty(openAnimTrigger))
			Anim.SetTrigger(openAnimTrigger);
		
		
		//do something extra actions
		doSomeThingWhenOpen();
		
		//call to close it after a delay time
		if(waitForcloseAfterOpen > 0)
			UIController.Instance.Manager.waitForClose(this, waitForcloseAfterOpen, ignoreTimeScaleInWaitingForCloseAfterOpen);
	}
	
	/// <summary>
	/// Forces close this window
	/// </summary>
	public virtual void forceClose(){
		close();
		
		if(Anim != null && !string.IsNullOrEmpty(openAnimTrigger))
			Anim.ResetTrigger(openAnimTrigger);
		
	}
	
	public virtual void close(){
		activeInterectablesChildren(false);
		
		if(firstClose)
			firstClose = false;
		
		isOpen = false;
		
		if(Anim != null && !string.IsNullOrEmpty(closeAnimTrigger))
			Anim.SetTrigger(closeAnimTrigger);
		
		//do something extra actions
		doSomeThingWhenClose();
	}
	
	/// <summary>
	/// Do something extra actions when open.
	/// </summary>
	public virtual void doSomeThingWhenOpen(){}
	/// <summary>
	/// Do something extra actions when close.
	/// </summary>
	public virtual void doSomeThingWhenClose(){}
	
	public virtual void initBestTime(bool pStartClosed){
		startClosed = pStartClosed;
	}
	
	public virtual bool FinishedOpenAnim(){
		bool res = false;
		
		if(!Anim || string.IsNullOrEmpty(openAnimTrigger) 
		   || (Anim != null && !string.IsNullOrEmpty(openAnimTrigger) && anim.GetCurrentAnimatorStateInfo(0).IsName("Open"))) 
			res = true;
		
		return res;
	}
	
	public virtual bool FinishedClosedAnim(){
		bool res = false;
		
		if(!Anim || string.IsNullOrEmpty(closeAnimTrigger) 
		   || (Anim != null && !string.IsNullOrEmpty(closeAnimTrigger) && anim.GetCurrentAnimatorStateInfo(0).IsName("Closed"))) 
			res = true;
		
		return res;
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
}
