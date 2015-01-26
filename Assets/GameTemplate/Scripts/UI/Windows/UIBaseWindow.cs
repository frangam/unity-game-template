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
	[Tooltip("This window attribute is open when the current window is closed")]
	private UIBaseWindow openNewWinWhenClose;
	
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
	private Image window;
	private bool isOpen = false;
	
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
	
	public UIBaseWindow OpenNewWinWhenClose {
		get {
			return this.openNewWinWhenClose;
		}
		set{
			this.openNewWinWhenClose = value;
		}
	}
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		window = GetComponent<Image>();
		
		if(!window){
			Debug.LogWarning("No window attached");
		}
	}
	public virtual void Start(){}
	public virtual void Update(){}
	public virtual void OnDestroy(){}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void open(){
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
	}
	
	/// <summary>
	/// Forces close this window
	/// </summary>
	public virtual void forceClose(){
		close();
	}
	
	public virtual void close(){
		isOpen = false;
	}
	
	public virtual void init(bool pStartClosed){
		startClosed = pStartClosed;
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
}
