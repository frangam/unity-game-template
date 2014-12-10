using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDAchievementsExample : Singleton<HUDAchievementsExample> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Text txEnemiesKills;

	[SerializeField]
	private Text txGamesPlayed;

	[SerializeField]
	private Text txCoinsCollected;

	[SerializeField]
	private AchievementsPlayerExample player;

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Update(){
		txEnemiesKills.text = player.EnemiesKills.ToString();
		txGamesPlayed.text = player.GamesPlayed.ToString();
		txCoinsCollected.text = player.CoinsCollected.ToString();
	}
	#endregion
}
