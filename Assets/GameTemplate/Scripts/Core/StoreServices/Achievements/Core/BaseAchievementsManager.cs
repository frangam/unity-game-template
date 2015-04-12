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
public class BaseAchievementsManager : BaseQuestManager<BaseAchievementsManager,Achievement> {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string ACHIEVEMENTS_INITIAL_CHEKING = "aa_achievement_initial_checking";
	public const string ACHIEVEMENT_UNLOCKED = "aa_achievement_unlocked";
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool initialCheckOnServer = false;
	
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public override void Awake(){
		base.Awake();
		
		if(Quests == null || (Quests != null && Quests.Count == 0))
			init(PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED));
		
		if(initialCheckOnServer)
			initialCheckingInServerSide();
		
		//listeners
		dispatcher.addEventListener(ACTION_COMPLETED, OnActionCompleted); 
		//		dispatcher.addEventListener(ACTION_PROGRESS_CHANGED, OnActionCompleted); 
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
				int completedProgress = achievement.getProgressIntegerValue();
				
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("BaseAchievementsManager - reporting to server achievement "+achievement.Id+" progress: " + completedProgress);
				
				GooglePlayManager.instance.IncrementAchievementById(aID, 1);
			}
			else{
				GooglePlayManager.instance.UnlockAchievementById(aID);
			}
		}
		#elif UNITY_IPHONE
		if(GameCenterManager.IsPlayerAuthed && GameCenterManager.getAchievementProgress(aID) < 100){ //Menor al 100%
			float completedProgress = achievement.getProgressPercentage();
			
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("BaseAchievementsManager - reporting to server achievement progress: " + completedProgress);
			
			aID = aID.Replace("-", "_");
			GameCenterManager.submitAchievement(completedProgress, aID); //Completamos con el 100% del progreso
		}
		#elif WP8
		
		#endif
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Unlock all oh the achievements were unlocked locally
	/// An this checks if it is needed sending to the server
	/// </summary>
	public void initialCheckingInServerSide(){
		bool lockedInServer = false;
		
		if(GameSettings.Instance.showTestLogs){
			Debug.Log("Doing initial checking in server");
			Debug.Log("Achievements: "+Quests);
		}
		
		if(Quests == null || (Quests != null && Quests.Count == 0)){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("Initing Achievements Manager");
			init(PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED));
		}
		
		if(Quests != null && Quests.Count > 0){
			
			foreach(Achievement a in Quests){
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("Achievement: "+a);
				//if it was completed previously
				//we check if in the servers has updated it
				if(a.completedPreviously()){				
					#if UNITY_ANDROID
					if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
						GPAchievement gpAchievement = GooglePlayManager.instance.GetAchievement(a.Id);
						lockedInServer = gpAchievement != null && gpAchievement.state != GPAchievementState.STATE_UNLOCKED;
					}
					#elif UNITY_IPHONE
					if(GameCenterManager.IsPlayerAuthed){
						string id = a.Id.Replace("-","_");
						lockedInServer = GameCenterManager.getAchievementProgress(id) < 100;
					}
					#elif WP8
					#endif
					
					//report to the server to unlock
					if(lockedInServer){
						reportToServer(a);
					}
				}
				else if(a.IsIncremental){
					
					#if UNITY_ANDROID
					if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
						GPAchievement gpAchievement = GooglePlayManager.instance.GetAchievement(a.Id);
						lockedInServer = gpAchievement != null && gpAchievement.state != GPAchievementState.STATE_UNLOCKED && gpAchievement.type == GPAchievementType.TYPE_INCREMENTAL;
						
						if(lockedInServer){
							int progress = gpAchievement.currentSteps;
							int completedProgress = a.getProgressIntegerValue();
							if(completedProgress > 0 && completedProgress > progress){
								reportToServer(a);
								
								if(GameSettings.Instance.showTestLogs)
									Debug.Log("BaseAchievementsManager - reporting to server achievement progress: " + completedProgress);
							}
						}
					}
					#elif UNITY_IPHONE
					if(GameCenterManager.IsPlayerAuthed){
						string id = a.Id.Replace("-","_");
						float progress = GameCenterManager.getAchievementProgress(id);
						lockedInServer = progress < 100;
						
						//report to the server to unlock
						if(lockedInServer && progress < a.getProgressPercentage())
							reportToServer(a);
					}
					#elif WP8
					#endif
					
					
				}
			}	
			
			dispatcher.dispatch(ACHIEVEMENTS_INITIAL_CHEKING);
		}
		else{
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("BaseAchievementsManager - Could not complete initial checking because there are not any achievement");
		}
	}
	
	public void showAchievementsFromServer(){
		#if UNITY_ANDROID
		if(GooglePlayConnection.state == GPConnectionState.STATE_UNCONFIGURED){
			GPSConnect.Instance.init();
			
			AndroidDialog dialog = AndroidDialog.Create(Localization.Localize(ExtraLocalizations.POPUP_TITLE_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.POPUP_DESC_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.OK_BUTTON_GPS_LOGIN_POPUP)
			                                            , Localization.Localize(ExtraLocalizations.CANCEL_BUTTON_GPS_LOGIN_POPUP));
			dialog.addEventListener(BaseEvent.COMPLETE, OnDialogClose);
		}
		else if(GooglePlayConnection.state == GPConnectionState.STATE_DISCONNECTED){
			AndroidDialog dialog = AndroidDialog.Create(Localization.Localize(ExtraLocalizations.POPUP_TITLE_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.POPUP_DESC_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.OK_BUTTON_GPS_LOGIN_POPUP)
			                                            , Localization.Localize(ExtraLocalizations.CANCEL_BUTTON_GPS_LOGIN_POPUP));
			dialog.addEventListener(BaseEvent.COMPLETE, OnDialogClose);
		}
		else if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
			GooglePlayManager.instance.ShowAchievementsUI();
		}
		#elif UNITY_IPHONE
		if(GameSettings.Instance.USE_GAMECENTER && GameCenterManager.IsPlayerAuthed)
			GameCenterManager.showAchievements();
		#endif
	}
	
	private void OnDialogClose(CEvent e) {
		//removing listner
		(e.dispatcher as AndroidDialog).removeEventListener(BaseEvent.COMPLETE, OnDialogClose);
		
		//parsing result
		switch((AndroidDialogResult)e.data) {
		case AndroidDialogResult.YES:
			GooglePlayConnection.instance.connect();
			GooglePlayManager.instance.showAchievementsUI();
			break;
		case AndroidDialogResult.NO:
			break;
			
		}
	}
	
	
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	public override void OnQuestCompleted (CEvent e){
		base.OnQuestCompleted(e);
		
		Achievement achievement = e.data as Achievement;
		
		if(achievement != null){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("Achievement " + achievement + " is completed");
			reportToServer(achievement); //report achiement progress to the server side
			dispatcher.dispatch(ACHIEVEMENT_UNLOCKED, achievement);
		}
	}
	public override void OnActionProgressChanged (CEvent e)
	{
		base.OnActionProgressChanged (e);
		
		GameActionResult result = e.data as GameActionResult;
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("GameAction " +result.CurrentActionId + " progress changed event. Rult succeded? "+result.IsSucceeded);
		
		if(result.IsSucceeded){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("GameAction " +result.CurrentActionId + " progress is changed");
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("BaseAchievementsManager - reporting to server only achievements has this action "+result.CurrentActionId);
			
			foreach(Achievement a in Quests){
				if(a != null){
					foreach(GameAction ac in a.Actions){
						if(ac.Id.Equals(result.CurrentActionId)){
							if(GameSettings.Instance.showTestLogs)
								Debug.Log("BaseAchievementsManager - reporting to server achievement progress for achievement "+a.Id);
							reportToServer(a);
							break;
						}
					}
				}
			}
		}
	}
}
