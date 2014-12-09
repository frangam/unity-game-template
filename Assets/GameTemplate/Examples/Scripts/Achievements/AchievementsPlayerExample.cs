using UnityEngine;
using System.Collections;

public class AchievementsPlayerExample : MonoBehaviour {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int enemiesKills 	= 0;
	private int gamesPlayed 	= 0;
	private int coinsCollected 	= 0;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public int EnemiesKills{
		get{return enemiesKills;}
		set{
			enemiesKills = value;

			//Observer Pattern
			AActionResult res =  new AActionResult(AActionID.AACTION_1, value);
			AAction.dispatcher.dispatch(BaseAchievementsManager.ACTION_CHANGED, res);
		}
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void killEnemy(){
		EnemiesKills++;
	}
}
