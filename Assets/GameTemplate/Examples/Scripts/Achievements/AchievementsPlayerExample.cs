using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
			List<GameActionID> actionsIds = new List<GameActionID>() {GameActionID.GAME_ACTION_2, GameActionID.GAME_ACTION_3, GameActionID.GAME_ACTION_7};
			GameActionResult res =  new GameActionResult(actionsIds, value);
			BaseAchievementsManager.dispatcher.dispatch(BaseAchievementsManager.GAME_PROPERTY_CHANGED, res);
		}
	}

	public int GamesPlayed {
		get {
			return this.gamesPlayed;
		}
		set {
			gamesPlayed = value;

			//Observer Pattern
			List<GameActionID> actionsIds = new List<GameActionID>() {GameActionID.GAME_ACTION_1, GameActionID.GAME_ACTION_4, GameActionID.GAME_ACTION_5};
			GameActionResult res =  new GameActionResult(actionsIds, value);
			BaseAchievementsManager.dispatcher.dispatch(BaseAchievementsManager.GAME_PROPERTY_CHANGED, res);
		}
	}

	public int CoinsCollected {
		get {
			return this.coinsCollected;
		}
		set {
			coinsCollected = value;

			//Observer Pattern
			List<GameActionID> actionsIds = new List<GameActionID>() {GameActionID.GAME_ACTION_6};
			GameActionResult res =  new GameActionResult(actionsIds, value);
			BaseAchievementsManager.dispatcher.dispatch(BaseAchievementsManager.GAME_PROPERTY_CHANGED, res);
		}
	}

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
