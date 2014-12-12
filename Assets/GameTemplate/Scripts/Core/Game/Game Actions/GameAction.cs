using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnionAssets.FLE;

/// <summary>
/// An action measures an achievement activationValue. It is a counter with some special attributes
/// (such as default value and update constraints)
/// </summary>

public class GameAction {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const char SEPARATOR_ATTRIBUTES 		= ',';
	public const char SEPARATOR_INTERVAL 		= '-';
	public const char SEPARATOR_TAGS 			= '.';

	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string name;

	[SerializeField]
	private string id;

	[SerializeField]
	private AchieveCondition activation;

	[SerializeField]
	private int activationValue;

	[SerializeField]
	[Tooltip("Valid if activation is ACTIVE_IF_BETWEEN")]
	private IntervalObject<int> activationInterval;

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

	public string Id {
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
				GameActionResult res =  new GameActionResult(id, value);
				BaseAchievementsManager.dispatcher.dispatch(BaseAchievementsManager.ACTION_COMPLETED, res);
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

	//--------------------------------------
	// Constructors
	//--------------------------------------
	
	/// <summary>
	/// Initializes a new instance of the <see cref="GameAction"/> class.
	/// 
	/// Attributes:
	/// ID, AchieveCondition, activationValue or activationInterval, initialValue, Tags
	/// 
	/// AchieveCondition: L (Lower than); E (Equals to); G (Greater than); B (Between)
	/// activationInterval: 10-15 (from: 10, to: 15).
	/// Tags: tag1.tag2.tag3
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public GameAction(string attributes){
		string[] atts = attributes.Split(SEPARATOR_ATTRIBUTES);
		int aV1, aV2, aIV;

		//ID
		if(atts.Length > 0){
			id = atts[0];
		}
		else{
			Debug.LogError("It was not found ID attribute");
		}

		//AchieveCondition
		if(atts.Length > 1){
			switch(atts[1]){
			case "L":
				activation = AchieveCondition.ACTIVE_IF_LOWER_THAN;
				break;
			case "E":
				activation = AchieveCondition.ACTIVE_IF_EQUALS_TO;
				break;
			case "G":
				activation = AchieveCondition.ACTIVE_IF_GREATER_THAN;
				break;
			case "B":
				activation = AchieveCondition.ACTIVE_IF_BETWEEN;

				//activation value IntervalObject
				if(atts.Length > 2){
					string[] interval = atts[2].Split(SEPARATOR_INTERVAL);
					if(int.TryParse(interval[0], out aV1) && int.TryParse(interval[1], out aV2)){
						activationInterval = new IntervalObject<int>(aV1, aV2);
					}
					else{
						Debug.LogError("It was not found Activation Interval value attribute");
					}
				}
				else{
					Debug.LogError("It was not found Activation Interval value attribute");
				}
				break;
			}
		}
		else{
			Debug.LogError("It was not found Activation condition attribute");
		}

		//activation value with condition != BETWEEN
		if(activation != AchieveCondition.ACTIVE_IF_BETWEEN){
			if(atts.Length > 2){
				if(int.TryParse(atts[2], out aV1)){
					activationValue = aV1;
				}
			}
			else{
				Debug.LogError("It was not found Activation value attribute");
			}
		}

		//initial value
		if(atts.Length > 3){
			if(int.TryParse(atts[3], out aIV)){
				initialValue = aIV;
			}
		}
		else{
			Debug.LogError("It was not found Initial value attribute");
		}

		//tags (optional)
		if(atts.Length > 4){
			string[] pTags = atts[4].Split(SEPARATOR_TAGS);
			tags = new List<string>();
			if(pTags != null && pTags.Length > 0){
				foreach(string tag in pTags){
					if(tag != null && tag != "" && tag.Length > 0){
						tags.Add(tag);
					}
				}
			}
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
	public bool loadedCorrectly(){
		return (id != null && activation != null 
			        && ((activation != AchieveCondition.ACTIVE_IF_BETWEEN && activationValue != null) 
			    || (activation == AchieveCondition.ACTIVE_IF_BETWEEN && activationInterval != null))
			        && initialValue != null);
	}

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

		case AchieveCondition.ACTIVE_IF_BETWEEN:
			res = activationInterval.contiene(progress);
			break;
		}

		return res;
	}

	public void reset(){
		progress = initialValue;
	}
}
