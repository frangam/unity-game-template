using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class UIAchievementNotification : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
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
		txAchievementTitle.text = achievement.Name;
		anim.SetTrigger(triggerShowAnimation);
	}
}
