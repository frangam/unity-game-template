using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Achievement : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string description;

	[SerializeField]
	private string id;

	[SerializeField]
	private bool isIncremental = false;

	[SerializeField]
	private List<AAction> actions;
		
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool unlocked = false;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string Description {
		get {
			return this.description;
		}
	}

	public string Id {
		get {
			return this.id;
		}
	}

	public bool IsIncremental {
		get {
			return this.isIncremental;
		}
	}

	public List<AAction> Actions {
		get {
			return this.actions;
		}
	}

	public bool Unlocked {
		get {
			return this.unlocked;
		}
		set {
			unlocked = value;
		}
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString (){
		return string.Format ("[Achievement: id={1}, description={0}, unlocked={2}]", description, id, unlocked);
	}
	

	//--------------------------------------
	// Private Methods
	//--------------------------------------

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public float getProgress(){
		float res = 0f;


		return res;
	}

}
