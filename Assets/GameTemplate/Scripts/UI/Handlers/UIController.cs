using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Base/UIController")]
public class UIController : Singleton<UIController>{
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private UIBaseManager manager;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public UIBaseManager Manager {
		get {
			return this.manager;
		}
	}
	
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		if(manager == null){
			manager = gameObject.getFirstComponentInChildren<UIBaseManager>() as UIBaseManager;
			
			if(manager == null){
				Debug.LogError("Could not found the UIBaseManager in children or not attached");
			}
		}
	}
	#endregion
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------




}
