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
	[Tooltip("True if we not want to start this window with the UIBaseManager in the loadWindows method. This is useful when we want to open or close a window in other window open or close method")]
	private bool notStartWithManager = false;
	
	[SerializeField]
	[Tooltip("Greater than 0 if we want to close this window after it was open and wait this delay time")]
	private float waitForcloseAfterOpen = 0f;
	
	[SerializeField]
	private string openAnimTrigger = "";
	
	[SerializeField]
	private string closeAnimTrigger = "";
	
	[SerializeField]
	[Tooltip("These windows are open when the current window is closed")]
	private List<UIBaseWindow> openNewWinsWhenClose;
	
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
	private Animator anim;
	private Image window;
	private bool isOpen = false;
	private bool firstOpen = true;
	private bool firstClose = true;
	
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
	
	public List<UIBaseWindow> OpenNewWinsWhenClose {
		get {
			return this.openNewWinsWhenClose;
		}
		set{
			this.openNewWinsWhenClose = value;
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
		anim = GetComponent<Animator>();
		window = GetComponent<Image>();
		
		if(!window){
			Debug.LogWarning("No window attached");
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
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void open(){
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
		
		//call to close it after a delay time
		if(waitForcloseAfterOpen > 0)
			UIController.Instance.Manager.waitForClose(this, waitForcloseAfterOpen);
	}
	
	/// <summary>
	/// Forces close this window
	/// </summary>
	public virtual void forceClose(){
		close();
		
		if(Anim != null && !string.IsNullOrEmpty(openAnimTrigger))
			Anim.ResetTrigger(openAnimTrigger);
		if(Anim != null && !string.IsNullOrEmpty(closeAnimTrigger))
			Anim.SetTrigger(closeAnimTrigger);
	}
	
	public virtual void close(){
		if(firstClose)
			firstClose = false;
		
		isOpen = false;
	}
	
	public virtual void init(bool pStartClosed){
		startClosed = pStartClosed;
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
}
