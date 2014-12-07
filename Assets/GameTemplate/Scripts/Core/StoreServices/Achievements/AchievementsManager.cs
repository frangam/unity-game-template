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
	private List<AProperty> propertiesList;

	[SerializeField]
	private List<Achievement> achievementsList;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Dictionary<string, AProperty> properties;
	private Dictionary<string, Achievement> achievements;

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private bool hasTag(AProperty property, List<string> tags){
		bool res = false;

		foreach(string tag in tags){
			if(property.Tags != null){
				res = property.Tags.Contains(tag);

				if(res)
					break;
			}
		}

		return res;
	}

	private void setValue(AProperty property, int value, bool ignoreActivationConstraint = false){
		doSetValue(property.Name, value, ignoreActivationConstraint);
	}

	private void doSetValue(string propertyName, int value, bool ignoreActivationConstraint = false){
		//TODO Checkpropertyexists

		int finalValue = value;

		if(!ignoreActivationConstraint){
			int propValue = properties[propertyName].Value;

			switch(properties[propertyName].Activation){
			case AchieveCondition.ACTIVE_IF_GREATER_THAN:
				finalValue = value > propValue ? value : propValue;
				break;

			case AchieveCondition.ACTIVE_IF_LESS_THAN:
				finalValue = value < propValue ? value : propValue;
				break;
			}
		}

		properties[propertyName].Value = finalValue;
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public int getValue(string propertyName){
		return properties[propertyName].Value;
	}

	public void addValue(List<AProperty> _properties, int value, bool ignoreActivationConstraint = false){
		foreach(AProperty p in _properties){
			doSetValue(p.Name, getValue(p.Name) + value, ignoreActivationConstraint);
		}
	}

	/// <summary>
	/// Resets all properties tagged with tags indicated
	/// </summary>
	/// <param name="tags">Tags.</param>
	public void resetProperties(List<string> tags){
		foreach(AProperty p in propertiesList){
			if(p.Tags == null || hasTag(p, tags)){
				p.reset();
			}
		}
	}

	public List<Achievement> checkAchievements(List<string> tags){
		List<Achievement> achievements = new List<Achievement>();

		foreach(Achievement a in achievements){
			//locked
			if(!a.Unlocked){
				int activeProperties = 0;

				//check total active properties
				foreach(AProperty p in a.Properties){
					if((tags != null || hasTag(p, tags)) && p.isActive()){
						activeProperties++;
					}
				}

				//if all achievement properties are active unlock achievement
				if(activeProperties == a.Properties.Count){
					a.Unlocked = true;
					achievements.Add(a);
				}
			}
		}

		return achievements;
	}
}
