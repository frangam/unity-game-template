/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnionAssets.FLE;
using System.Collections;

public class UIAchievementsController : Singleton<UIAchievementsController> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private UIAchievementNotification imAchievementNotif;

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		BaseAchievementsManager.dispatcher.addEventListener(BaseAchievementsManager.ACHIEVEMENT_UNLOCKED, OnAchievementUnlocked);
	}
	#endregion


	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void showUnlockedAchievement(Achievement achievement){
		if(imAchievementNotif){
			imAchievementNotif.showUnlockedAchievement(achievement);
		}
		else{
			Debug.LogError("There is no attached an achievement notification ui component");
		}
	}


	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	public void OnAchievementUnlocked(CEvent e){
		Achievement achievement = e.data as Achievement;

		if(achievement != null){
			showUnlockedAchievement(achievement);
		}
	}
}
