using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Achievement {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string name;

	[SerializeField]
	private string description;

	[SerializeField]
	private string id;

	[SerializeField]
	private bool isIncremental = false;

	[SerializeField]
	private List<GameAction> actions;
		
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool unlocked = false;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string Name {
		get {
			return this.name;
		}
	}

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

	public List<GameAction> Actions {
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
		return string.Format ("[Achievement: id={1}, name={3}, description={0}, unlocked={2}]", description, id, unlocked, name);
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Gets the progress percentage.
	/// </summary>
	/// <returns>The progress percentage.</returns>
	public float getProgressPercentage(){
		float res = 0f;
		int activeActions = 0;

		//check total active actions
		foreach(GameAction action in actions){
			if(action.isCompleted()){
				activeActions++;
			}
		}

		res = (activeActions * 100f) / actions.Count;

		return res;
	}

	/// <summary>
	/// Gets the progress integer value.
	/// The total completed actions
	/// </summary>
	/// <returns>The progress integer value.</returns>
	public int getProgressIntegerValue(){
		int res = 0;

		//check total active actions
		foreach(GameAction action in actions){
			if(action.isCompleted()){
				res++;
			}
		}

		return res;
	}

}
