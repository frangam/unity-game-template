using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Achievement approach:
/// 
/// ** Identify any interesting game metric (kills, deaths, mistakes, matches, etc).
/// ** Every metric becomes a property, guided by an update constraint. 
///    The constraint controls whether the property should be changed when a new value arrives.
/// ** The constraints are: "update only if new value is greater than current value"; 
///    "update only if new value is less than current value"; and "update no matter what the current value is".
/// ** Every property has an activation rule - for instance "kills is active if its value is greater than 10".
/// ** Check activated properties periodically. If all related properties of an achievement are active, then the achievement is unlocked.
/// </summary>
public class AchievementsManager : Singleton<AchievementsManager> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private List<AAction> actionsList;

	[SerializeField]
	private List<Achievement> achievementsList;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Dictionary<string, AAction> actions;
	private Dictionary<string, Achievement> achievements;

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private bool hasTag(AAction action, List<string> tags){
		bool res = false;

		foreach(string tag in tags){
			if(action.Tags != null){
				res = action.Tags.Contains(tag);

				if(res)
					break;
			}
		}

		return res;
	}

	private void setValue(AAction action, int value, bool ignoreActivationConstraint = false){
		doSetValue(action.Name, value, ignoreActivationConstraint);
	}

	private void doSetValue(string actionName, int value, bool ignoreActivationConstraint = false){
		//TODO Checkpropertyexists

		int finalValue = value;

		if(!ignoreActivationConstraint){
			int actionValue = actions[actionName].Value;

			switch(actions[actionName].Activation){
			case AchieveCondition.ACTIVE_IF_GREATER_THAN:
				finalValue = value > actionValue ? value : actionValue;
				break;

			case AchieveCondition.ACTIVE_IF_LESS_THAN:
				finalValue = value < actionValue ? value : actionValue;
				break;
			}
		}

		actions[actionName].Value = finalValue;
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public int getValue(string actionName){
		return actions[actionName].Value;
	}

	public void addValue(List<AAction> _actions, int value, bool ignoreActivationConstraint = false){
		foreach(AAction p in _actions){
			doSetValue(p.Name, getValue(p.Name) + value, ignoreActivationConstraint);
		}
	}

	/// <summary>
	/// Resets all actions tagged with tags indicated
	/// </summary>
	/// <param name="tags">Tags.</param>
	public void resetProperties(List<string> tags){
		foreach(AAction a in actionsList){
			if(a.Tags == null || hasTag(a, tags)){
				a.reset();
			}
		}
	}

	public List<Achievement> checkAchievements(List<string> tags){
		List<Achievement> achievements = new List<Achievement>();

		foreach(Achievement a in achievements){
			//locked
			if(!a.Unlocked){
				int activeActions = 0;

				//check total active actions
				foreach(AAction action in a.Actions){
					if((tags != null || hasTag(action, tags)) && action.isActive()){
						activeActions++;
					}
				}

				//if all achievement actions are active unlock achievement
				if(activeActions == a.Actions.Count){
					a.Unlocked = true;
					achievements.Add(a);
				}
			}
		}

		return achievements;
	}
}
