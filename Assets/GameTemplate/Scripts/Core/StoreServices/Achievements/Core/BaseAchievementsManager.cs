/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
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
	}
	#endregion
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override bool loadGameActionsContentFromGameSettingsAsset ()
	{
		bool loaded = GameSettings.Instance.CurrentAchievementsActions != null && GameSettings.Instance.CurrentAchievementsActions.Count > 0;
		
		if(!loaded){
			Debug.LogError("Could not load any Action");
		}
		else{
			actions = GameSettings.Instance.CurrentAchievementsActions;
		}
		
		return loaded;
	}
	
	public override bool loadQuestsContentFromGameSettingsAsset ()
	{
		bool loaded = GameSettings.Instance.CurrentAchievements != null && GameSettings.Instance.CurrentAchievements.Count > 0;
		
		if(!loaded){
			Debug.LogWarning("Could not load any Quest");
		}
		else{
			foreach(Achievement a in GameSettings.Instance.CurrentAchievements){
				BaseQuest quest = new Achievement(a);
				Quests = new List<BaseQuest>();
				Quests.Add(quest);
			}
		}
		
		return loaded;
	}
	
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
		aID = achievement.IdForIOS;

		if(GameCenterManager.IsPlayerAuthenticated && GameCenterManager.GetAchievementProgress(aID) < 100){ //Menor al 100%
			aID = aID.Replace("-", "_");
			GameCenterManager.SubmitAchievement(achievement.getProgressPercentage(), aID); //Completamos con el 100% del progreso
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
				if(GameCenterManager.IsPlayerAuthenticated){
					string id = a.IdForIOS;
					lockedInServer = GameCenterManager.GetAchievementProgress(id) < 100;
				}
				#elif WP8
				#endif
				
				//report to the server to unlock
				if(lockedInServer){
					reportToServer(a);
				}
			}
		}	
		
		dispatcher.dispatch(ACHIEVEMENTS_INITIAL_CHEKING);
	}
	
	public void showAchievementsFromServer(){
		#if UNITY_ANDROID
		if(GooglePlayConnection.state == GPConnectionState.STATE_UNCONFIGURED){
			GPSConnect.Instance.init();
			
			AndroidDialog dialog = AndroidDialog.Create(Localization.Localize(ExtraLocalizations.POPUP_TITLE_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.POPUP_DESC_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.OK_BUTTON_GPS_LOGIN_POPUP)
			                                            , Localization.Localize(ExtraLocalizations.CANCEL_BUTTON_GPS_LOGIN_POPUP));
			dialog.ActionComplete += OnAndroidDialogClose;
		}
		else if(GooglePlayConnection.state == GPConnectionState.STATE_DISCONNECTED){
			AndroidDialog dialog = AndroidDialog.Create(Localization.Localize(ExtraLocalizations.POPUP_TITLE_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.POPUP_DESC_GPS_LOGIN)
			                                            , Localization.Localize(ExtraLocalizations.OK_BUTTON_GPS_LOGIN_POPUP)
			                                            , Localization.Localize(ExtraLocalizations.CANCEL_BUTTON_GPS_LOGIN_POPUP));
			dialog.ActionComplete += OnAndroidDialogClose;
		}
		else if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED){
			GooglePlayManager.instance.ShowAchievementsUI();
		}
		#elif UNITY_IPHONE
		if(GameSettings.Instance.USE_GAMECENTER && GameCenterManager.IsPlayerAuthenticated)
			GameCenterManager.ShowAchievements();
		else if(GameSettings.Instance.USE_GAMECENTER && !GameCenterManager.IsPlayerAuthenticated){
			IOSDialog dialog = IOSDialog.Create(Localization.Localize(ExtraLocalizations.POPUP_TITLE_GC_LOGIN)
			                                    ,Localization.Localize(ExtraLocalizations.POPUP_DESC_GC_LOGIN)
			                                    , Localization.Localize(ExtraLocalizations.OK_BUTTON_GC_LOGIN_POPUP)
			                                    , Localization.Localize(ExtraLocalizations.CANCEL_BUTTON_GC_LOGIN_POPUP));
			dialog.OnComplete += onDialogIOSClose;
		}
		#endif
	}
	
	private void onDialogIOSClose(IOSDialogResult result) {
		
		//parsing result
		switch(result) {
		case IOSDialogResult.YES:
			GameCenterManager.OnAuthFinished += OnAuthIOSFinished;
			GameCenterManager.init();
			break;
		case IOSDialogResult.NO:
			break;
			
		}
	}
	void OnAuthIOSFinished (SA.Common.Models.Result res) {
		GameCenterManager.OnAuthFinished -= OnAuthIOSFinished;
		if (res.IsSucceeded) {
			GameCenterManager.ShowAchievements();
		} 
	}
	private void OnAndroidDialogClose(AndroidDialogResult res) {

		//parsing result
		switch(res) {
		case AndroidDialogResult.YES:
			GooglePlayConnection.Instance.Connect();
			GooglePlayManager.Instance.ShowAchievementsUI();
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
}
