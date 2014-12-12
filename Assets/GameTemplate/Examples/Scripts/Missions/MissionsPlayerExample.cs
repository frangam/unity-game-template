using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionsPlayerExample : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	public int selectedLevel = 1;

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
			List<string> actionsIds = new List<string>() 
			{GameActionID.GAME_ACTION_1, GameActionID.GAME_ACTION_2};

			GameActionResult res =  new GameActionResult(actionsIds, value);
			BaseQuestManager.dispatcher.dispatch(BaseQuestManager.GAME_PROPERTY_CHANGED, res);
		}
	}

	public int GamesPlayed {
		get {
			return this.gamesPlayed;
		}
		set {
			gamesPlayed = value;


		}
	}

	public int CoinsCollected {
		get {
			return this.coinsCollected;
		}
		set {
			coinsCollected = value;


		}
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Start(){
		BaseQuestManager.Instance.init(selectedLevel);
	}
	#endregion

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void killEnemies(int enemies = 1){
		EnemiesKills += enemies;
	}

	public void playGames(int games = 1){
		GamesPlayed += games;
	}

	public void collectCoins(int coins = 1){
		CoinsCollected += coins;
	}
}
