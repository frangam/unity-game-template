using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDMissionsExample : Singleton<HUDAchievementsExample> {
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
	private MissionsPlayerExample player;

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
