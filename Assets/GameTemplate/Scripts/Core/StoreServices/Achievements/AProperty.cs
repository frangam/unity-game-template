using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A property measures an achievement progress. It is a counter with some special attributes
/// (such as default value and update constraints)
/// </summary>
[System.Serializable]
public class AProperty {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string name;

	[SerializeField]
	private AchieveCondition activation;

	[SerializeField]
	private int activationValue;

	[SerializeField]
	private int initialValue;

	[SerializeField]
	private List<string> tags;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int value;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string Name {
		get {
			return this.name;
		}
	}

	public AchieveCondition Activation {
		get {
			return this.activation;
		}
	}

	public int ActivationValue {
		get {
			return this.activationValue;
		}
	}

	public int InitialValue {
		get {
			return this.initialValue;
		}
	}

	public List<string> Tags {
		get {
			return this.tags;
		}
	}

	public int Value {
		get {
			return this.value;
		}
		set {
			value = value;
		}
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Checks if the achievement property is active or not
	/// </summary>
	/// <returns><c>true</c>, if active was ised, <c>false</c> otherwise.</returns>
	public bool isActive(){
		bool res = false;

		switch(activation){
		case AchieveCondition.ACTIVE_IF_EQUALS_TO:
			res = value == activationValue;
			break;

		case AchieveCondition.ACTIVE_IF_GREATER_THAN:
			res = value > activationValue;
			break;

		case AchieveCondition.ACTIVE_IF_LESS_THAN:
			res = value < activationValue;
			break;
		}

		return res;
	}

	public void reset(){
		value = initialValue;
	}
}
