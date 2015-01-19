using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Image window;
	
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
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void open(){
		//		window.gameObject.SetActive(true);
	}
	public virtual void close(){
		//		window.gameObject.SetActive(false);
	}
	
	public virtual void init(bool pStartClosed){
		startClosed = pStartClosed;
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
}
