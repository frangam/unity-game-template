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
public class BaseAchievementsManager : BaseQuestManager<Achievement> {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string ACHIEVEMENT_UNLOCKED = "aa_achievement_unlocked";


	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public override void Awake(){
		base.Awake();

		intialCheckingInServerSide();

		//listeners
		dispatcher.addEventListener(ACTION_COMPLETED, OnActionCompleted); 
	}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------


	/// <summary>
	/// Report the progress to the server side
	/// </summary>
	/// <param name="achievement">Achievement.</param>
	private void reportToServer(Achievement achievement){
		string aID = achievement.Id;

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

		foreach(Achievement a in Quests){
			if(PlayerPrefs.GetInt(a.Id) == 1){
				a.Completed = true;
				
				#if UNITY_ANDROID
				if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
					GPAchievement gpAchievement = GooglePlayManager.instance.GetAchievement(a.Id);
					lockedInServer = gpAchievement != null && gpAchievement.state != GPAchievementState.STATE_UNLOCKED;
				}
				#elif UNITY_IPHONE
				if(GameCenterManager.IsPlayerAuthed){
					lockedInServer = GameCenterManager.getAchievementProgress(id) < 100;
				}
				#elif WP8
				#endif

				//report to the server to unlock
				if(lockedInServer){
					reportToServer(a);
				}
			}
		}			
	}

//	/// <summary>
//	/// Checks all of the achievements have the given action
//	/// </summary>
//	/// <returns>The achievements.</returns>
//	/// <param name="tags">Tags.</param>
//	/// <param name="action">Action.</param>
//	protected virtual List<Achievement> checkAchievements(List<string> tags, GameAction action){
//		List<Achievement> res = new List<Achievement>();
//
//		foreach(Achievement achievement in quests){
//			if(achievement.Actions.Contains(action) //has given action
//			   && !achievement.Completed){	//locked
//				//if all actions are active unlock achievement
//				if(achievement.getProgressIntegerValue() == achievement.Actions.Count){
//					achievement.Completed = true; //uptade unlocked flag to true
//					res.Add(achievement); //add it to the result
//					reportToServer(achievement); //report achiement progress to the server side
//					dispatcher.dispatch(ACHIEVEMENT_UNLOCKED, achievement); //dispatch the corresponding achievement
//				}
//			}
//		}
//
//		return res;
//	}






	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	public override void OnQuestCompleted (CEvent e){
		Achievement achievement = e.data as Achievement;
		
		if(achievement != null){
			//TODO Delete Log
			Debug.Log("Achievement " + achievement + " is completed");
			reportToServer(achievement); //report achiement progress to the server side
			dispatcher.dispatch(ACHIEVEMENT_UNLOCKED, achievement);
		}
	}
}
