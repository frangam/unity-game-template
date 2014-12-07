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
	private List<AProperty> properties;
		
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

	public List<AProperty> Properties {
		get {
			return this.properties;
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

}
