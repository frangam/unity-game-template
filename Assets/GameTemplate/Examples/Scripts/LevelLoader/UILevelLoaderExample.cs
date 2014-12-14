using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UILevelLoaderExample : UIBaseController<UILevelLoaderExample> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Text lbLevel;

	[SerializeField]
	private Text lbPlayerLife;

	[SerializeField]
	private Text lbGameLifes;

	[SerializeField]
	private Text lbSpawnPositions;

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void setLevelView(LevelExample level){
		if(lbLevel)
			lbLevel.text = "Level: " + level.Id.ToString();

		if(lbPlayerLife)
			lbPlayerLife.text = "Player life: "+ level.PlayerLife.ToString();

		if(lbGameLifes)
			lbGameLifes.text = "Game lifes: " + level.GameLifes.ToString();

		if(lbSpawnPositions){
			lbSpawnPositions.text = level.ToString();
		}
	}
}
