using UnityEngine;
using System.Collections;

public class LevelLoaderControllerExample : BaseLevelLoaderController{
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private int levelToLoad = 1;
	
	[SerializeField]
	private int levelPackToLoad = 1;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake (){
		base.Awake ();
		
		if(!LoadCurrentLevelAtStart)
			loadLevel(levelToLoad, levelPackToLoad);
		
	}
	
	public override void loadLevel (int levelId, int levelPackId){
		string currentLevelContent = getLevelContent(levelId, levelPackId);
		
		if(!string.IsNullOrEmpty(currentLevelContent)){
			LevelExample currentLevel = new LevelExample(currentLevelContent);
			((UILevelLoaderExample)UIController.Instance.Manager).setLevelView(currentLevel);
		}
	}
}
