/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class UIAchievementNotification : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	[Tooltip("If true, only valid for test purposes")]
	private bool onlyForTest = true;

	[SerializeField]
	private string triggerShowAnimation = "show";

	[SerializeField]
	private Image imAchievementIco;
	
	[SerializeField]
	private Text txAchievementTitle;


	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Animator anim;

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		anim = GetComponent<Animator>();
	}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------

	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void showUnlockedAchievement(Achievement achievement){
		if((onlyForTest && RuntimePlatformUtils.IsEditor()) 
		   || (!onlyForTest)){
			txAchievementTitle.text = achievement.Name;
			anim.SetTrigger(triggerShowAnimation);
		}
	}
}
