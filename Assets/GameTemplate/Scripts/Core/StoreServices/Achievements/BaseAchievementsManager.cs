using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnionAssets.FLE;

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
public class BaseAchievementsManager : Singleton<BaseAchievementsManager> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	public const string ACTION_CHANGED = "aa_action_changed";

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
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		AAction.dispatcher.addEventListener(ACTION_CHANGED, OnActionChanged); 
	}
	#endregion

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

	private void doSetValue(AAction action, int value, bool ignoreActivationConstraint = false){
		int finalValue = value;

		if(!ignoreActivationConstraint){
			int actionProgress = action.Progress;

			switch(action.ActivationCondition){
			case AchieveCondition.ACTIVE_IF_GREATER_THAN:
				finalValue = value > actionProgress ? value : actionProgress;
				break;

			case AchieveCondition.ACTIVE_IF_LOWER_THAN:
				finalValue = value < actionProgress ? value : actionProgress;
				break;
			}
		}

		action.Progress = finalValue;
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void addValue(AAction action, int value, bool ignoreActivationConstraint = false){
		doSetValue(action, action.Progress + value, ignoreActivationConstraint);
	}

	public void setValue(AAction action, int value, bool ignoreActivationConstraint = false){
		doSetValue(action, value, ignoreActivationConstraint);
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
					if((tags != null || hasTag(action, tags)) && action.isCompleted()){
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


	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	/// <summary>
	/// When an action value changes raise this event
	/// </summary>
	/// <param name="e">E.</param>
	public void OnActionChanged(CEvent e){
		AActionResult result = e.data as AActionResult;

		if(result.IsSucceeded){
			foreach(AAction action in actionsList){
				if(action.Id == result.ActionId){
					//modify action progress
					switch(result.ActionOperation){
					case AActionOperation.ADD:
						addValue(action, result.GamePropertyValue);
						break;

					case AActionOperation.SET:
						setValue(action, result.GamePropertyValue);
						break;
					}
			
					break;
				}
			}
		}
	}
}
