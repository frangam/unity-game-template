using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIBaseController : UIBaseController<UIBaseController>{

}

public class UIBaseController<T> : Singleton<T> where T: MonoBehaviour {
	//--------------------------------------
	// Constants
	//--------------------------------------
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private List<UIBaseWindow> windows;
		
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){

	}
	public virtual void Start(){}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void open(string id){
		UIBaseWindow win = null;

		foreach(UIBaseWindow w in windows){
			if(w.Id == id){
				win = w;
				break;
			}
		}

		if(win != null){
			open(win);
		}
		else{
			Debug.LogError("Windows with id " + id + " does not exist");
		}
	}


	public virtual void open(UIBaseWindow window){
		if(windows.Contains(window)){
			window.open();
		}
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
}
