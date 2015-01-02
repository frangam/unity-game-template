using UnityEngine;
using System.Collections;

public class LevelLoaderControllerExample : BaseLevelLoaderController{
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private int levelToLoad = 1;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake (){
		base.Awake ();

		if(!LoadCurrentLevelAtStart)
			loadLevel(levelToLoad);

	}

	public override void loadLevel (int levelId){
		string currentLevelContent = getLevelContent(levelId);

		if(!string.IsNullOrEmpty(currentLevelContent)){
			LevelExample currentLevel = new LevelExample(currentLevelContent);
			((UILevelLoaderExample)UIController.Instance.Manager).setLevelView(currentLevel);
		}
	}
}
