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
	public const string GAME_PROPERTY_CHANGED = "aa_game_property_changed";
	public const string ACTION_COMPLETED = "aa_action_completed";

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


	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		AAction.dispatcher.addEventListener(GAME_PROPERTY_CHANGED, OnGamePropertyChanged); 
		AAction.dispatcher.addEventListener(ACTION_COMPLETED, OnActionChanged); 
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

	/// <summary>
	/// Report the progress to the server side
	/// </summary>
	/// <param name="achievement">Achievement.</param>
	private void reportToServer(Achievement achievement){
		string aID = achievement.Id;

		//si no se habia desbloqueado previamente, lo anotamos en la memoria
		if(PlayerPrefs.GetInt(aID) != 1)
			PlayerPrefs.SetInt(aID, 1);
		
		#if UNITY_ANDROID
		//si el logro no se ha conseguido
		GPAchievement gpAchievement = GooglePlayManager.instance.GetAchievement(aID);
		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED && gpAchievement != null && gpAchievement.state != GPAchievementState.STATE_UNLOCKED){
			if(achievement.IsIncremental){
				GooglePlayManager.instance.IncrementAchievementById(aID, achievement.getProgressIntegerValue());
			}
			else{
				GooglePlayManager.instance.UnlockAchievementById(aID);
			}
		}
		#elif UNITY_IPHONE
		if(GameCenterManager.IsPlayerAuthed && GameCenterManager.getAchievementProgress(aID) < 100){ //Menor al 100%
			GameCenterManager.submitAchievement(achievement.getProgressPercentage(), aID); //Completamos con el 100% del progreso
		}
		#elif WP8

		#endif
	}

	/// <summary>
	/// Unlock all oh the achievements were unlocked locally
	/// An this checks if it is needed sending to the server
	/// </summary>
	public void intialCheckingInServerSide(){
		bool lockedInServer = false;

		if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
			foreach(Achievement a in achievementsList){
				if(PlayerPrefs.GetInt(a.Id) == 1){
					
					#if UNITY_ANDROID
					GPAchievement gpAchievement = GooglePlayManager.instance.GetAchievement(a.Id);
					lockedInServer = gpAchievement != null && gpAchievement.state != GPAchievementState.STATE_UNLOCKED;
					#elif UNITY_IPHONE
					lockedInServer = GameCenterManager.IsPlayerAuthed && GameCenterManager.getAchievementProgress(id) < 100;
					#elif WP8
					#endif

					//report to the server to unlock
					if(lockedInServer){
						reportToServer(a);
					}
				}
			}		
		}		
	}

	/// <summary>
	/// Checks all of the achievements have the given action
	/// </summary>
	/// <returns>The achievements.</returns>
	/// <param name="tags">Tags.</param>
	/// <param name="action">Action.</param>
	private void checkAchievements(List<string> tags, AAction action){
		foreach(Achievement a in achievementsList){
			if(a.Actions.Contains(action) //has given action
			   && !a.Unlocked){	//locked
				//if all actions are active unlock achievement
				if(a.getProgressIntegerValue() == a.Actions.Count){
					a.Unlocked = true;
				}
			}
		}
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




	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	/// <summary>
	/// When an game property value changes raise this event
	/// </summary>
	/// <param name="e">E.</param>
	public void OnGamePropertyChanged(CEvent e){
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

	/// <summary>
	/// When an action value changes raise this event
	/// </summary>
	/// <param name="e">E.</param>
	public void OnActionChanged(CEvent e){
		AActionResult result = e.data as AActionResult;


	}
}
