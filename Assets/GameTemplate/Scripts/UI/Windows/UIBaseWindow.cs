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
	private Image window;

	[SerializeField]
	private bool startClosed = true;
		
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string Id {
		get {
			return this.id;
		}
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		if(window){
			window.gameObject.SetActive(!startClosed);
		}
		else{
			Debug.LogWarning("No window attached");
		}
	}
	public virtual void Start(){}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void open(){
		window.gameObject.SetActive(true);
	}
	public virtual void close(){
		window.gameObject.SetActive(false);
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
}
