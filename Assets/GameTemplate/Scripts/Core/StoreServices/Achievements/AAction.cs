using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnionAssets.FLE;

/// <summary>
/// An action measures an achievement activationValue. It is a counter with some special attributes
/// (such as default value and update constraints)
/// </summary>
public class AAction : MonoBehaviour{
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	/// <summary>
	/// Observer pattern
	/// </summary>
	private static EventDispatcherBase _dispatcher  = new EventDispatcherBase ();

	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string name;

	[SerializeField]
	private AActionID id;

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
	private int progress;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string Name {
		get {
			return this.name;
		}
	}

	public AActionID Id {
		get {
			return this.id;
		}
	}

	public AchieveCondition ActivationCondition {
		get {
			return this.activation;
		}
	}

	public int Progress {
		get {
			return this.progress;
		}
		set {
			progress = value;

			//Observer Pattern
			if(isCompleted()){
				AActionResult res =  new AActionResult(id, value);
				AAction.dispatcher.dispatch(BaseAchievementsManager.ACTION_COMPLETED, res);
			}
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

	public static EventDispatcherBase dispatcher {
		get {
			return _dispatcher;
		}
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString (){
		return string.Format ("[AAction: name={0}, id={1}, activation={2}, activationValue={3}, initialValue={4}, progress={5}]", name, id, activation, activationValue, initialValue, progress);
	}


	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Checks if the achievement action is completed or not
	/// </summary>
	/// <returns><c>true</c>, if completed was ised, <c>false</c> otherwise.</returns>
	public bool isCompleted(){
		bool res = false;

		switch(activation){
		case AchieveCondition.ACTIVE_IF_EQUALS_TO:
			res = progress == activationValue;
			break;

		case AchieveCondition.ACTIVE_IF_GREATER_THAN:
			res = progress > activationValue;
			break;

		case AchieveCondition.ACTIVE_IF_LOWER_THAN:
			res = progress < activationValue;
			break;
		}

		return res;
	}

	public void reset(){
		progress = initialValue;
	}
}
