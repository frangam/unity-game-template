using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ChangeInputBackButtonBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool active = false;
	
	[SerializeField]
	private InputBackButton.Action action;
	
	[SerializeField]
	[Tooltip("Leave empty if do navigate with Action. If we go to an specific scene represented by another game section fill it")]
	private string specificScreenToGO;
	
	[SerializeField]
	private UIBaseManager uiManagerToDoAction;
	
	[SerializeField]
	private List<UIBaseWindow> openWindows;
	
	[SerializeField]
	private List<UIBaseWindow> closeWindows;
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public bool Active {
		get {
			return this.active;
		}
		set {
			active = value;
		}
	}
	
	public InputBackButton.Action Action {
		get {
			return this.action;
		}
		set {
			action = value;
		}
	}
	
	public string SpecificScreenToGO {
		get {
			return this.specificScreenToGO;
		}
		set {
			specificScreenToGO = value;
		}
	}
	
	public List<UIBaseWindow> OpenWindows {
		get {
			return this.openWindows;
		}
		set {
			openWindows = value;
		}
	}
	
	public List<UIBaseWindow> CloseWindows {
		get {
			return this.closeWindows;
		}
		set {
			closeWindows = value;
		}
	}
	
	public UIBaseManager UiManagerToDoAction {
		get {
			return this.uiManagerToDoAction;
		}
		set {
			uiManagerToDoAction = value;
		}
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void change(){
		if(active){
			InputBackButton.Instance.UiManagerToDoAnAction = uiManagerToDoAction;
			InputBackButton.Instance.OpenWindows = openWindows;
			InputBackButton.Instance.CloseWindows = closeWindows;
			InputBackButton.Instance.SpecificScreenToGO = specificScreenToGO;
			InputBackButton.Instance.CurrentAction = action;
		}
	}
}
