using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnionAssets.FLE;

public class BaseLevelLoaderController : Singleton<BaseLevelLoaderController>{
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	private static EventDispatcherBase _dispatcher  = new EventDispatcherBase ();
	
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string LEVEL_LOADED = "gt_game_level_loaded";
	public const string ALL_LEVELS_LOADED = "gt_all_game_levels_loaded";
	public const char 	SEPARATOR_ATTRIBUTES 		= ',';
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private List<GameMode> validIn = new List<GameMode>(){GameMode.CAMPAIGN};
	
	[SerializeField]
	private string levelsFile = "Levels";
	
	[SerializeField]
	private bool loadCurrentLevelAtStart = true;
	
	[SerializeField]
	private bool loadAllLevels = false;
	
	[SerializeField]
	private bool loadTestLevel = false;
	[SerializeField]
	private int levelToLoadTEST = 1;
	[SerializeField]
	private int levelPackToLoadTEST = 1;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected List<BaseLevel> levels;
	protected BaseLevel currentLevel;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public static EventDispatcherBase dispatcher {
		get {
			return _dispatcher;
		}
	}
	
	public bool LoadCurrentLevelAtStart {
		get {
			return this.loadCurrentLevelAtStart;
		}
	}
	
	public List<BaseLevel> Levels {
		get {
			return this.levels;
		}
		set{
			levels = value;
		}
	}
	
	public BaseLevel CurrentLevel {
		get {
			return this.currentLevel;
		}
	}
	
	public bool LoadTestLevel {
		get {
			return this.loadTestLevel;
		}
	}
	
	public int LevelToLoadTEST {
		get {
			return this.levelToLoadTEST;
		}
	}
	
	public int LevelPackToLoadTEST {
		get {
			return this.levelToLoadTEST;
		}
	}
	
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		if(validIn != null && validIn.Count > 0){
			bool active = validIn.Contains(GameController.Instance.Manager.GameMode);
			gameObject.SetActive(active);
			
			if(active){
				initialize();
			}
		}
		else{
			initialize();
		}
	}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void initialize(){
		if(loadTestLevel)
			loadLevel(levelToLoadTEST, levelPackToLoadTEST);
		else if(loadCurrentLevelAtStart)
			loadCurrentLevel();
		
		if(loadAllLevels)
			loadAllOfLevels();
	}
	
	/// <summary>
	/// Gets all of the content levels file, that is, all of the text in levels file
	/// </summary>
	/// <returns>The content level file.</returns>
	private string getContentLevelFile(){
		bool located = false;
		string content = "";
		
		//Try to load levels file
		TextAsset text = Resources.Load(levelsFile) as TextAsset;
		
		if(text == null){
			Debug.LogError("You must provide a correct Levels filename");
		}
		else{
			located = true;
		}
		
		if(located){
			content = text.text;
			
			if(string.IsNullOrEmpty(content)){
				Debug.LogWarning("Levels filename is empty or is invalid");
			}
		}
		
		return content;
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public string getLevelContent(int levelId, int levelPackId){
		string content = getContentLevelFile();
		string[] lines = null;
		string result = "";
		
		if(!string.IsNullOrEmpty(content)){
			lines = content.Split('\n'); //get each line of the content
			
			if(lines == null || lines.Length == 0){
				Debug.LogWarning("Levels content is empty or is invalid");
			}
			else{				
				foreach(string line in lines){
					string[] level = line.Split(SEPARATOR_ATTRIBUTES);
					int levelIdFromLine, levelPackIdFromLine;
					
					//first attribute is the level id
					if(int.TryParse(level[0], out levelIdFromLine) && int.TryParse(level[1], out levelPackIdFromLine) && levelIdFromLine == levelId && levelPackIdFromLine == levelPackId){
						result = line;
						break;
					}
				}
			}
		}
		
		return result;
	}
	
	/// <summary>
	/// Gets the content of all levels.
	/// </summary>
	/// <returns>The content of all levels.</returns>
	public string[] getContentOfAllLevels(){
		string content = getContentLevelFile();
		string[] lines = null;
		
		if(!string.IsNullOrEmpty(content)){
			lines = content.Split('\n'); //get each line of the content
			
			if(lines == null || lines.Length == 0){
				Debug.LogWarning("Levels content is empty or is invalid");
			}
		}
		
		return lines;
	}
	
	/// <summary>
	/// Gets the content (all attributes) of the current level selected
	/// </summary>
	/// <returns>The current level content.</returns>
	public string getCurrentLevelContent(){
		return getLevelContent(PlayerPrefs.GetInt(GameSettings.PP_SELECTED_LEVEL), PlayerPrefs.GetInt(GameSettings.PP_SELECTED_LEVEL_PACK)); // get the current level selected content
	}
	
	/// <summary>
	/// Loads the current selected level
	/// </summary>
	/// <param name="levelId">Level identifier.</param>
	public virtual void loadCurrentLevel(){
		if(GameController.Instance.Manager.GameMode == GameMode.SURVIVAL)
			loadLevel(PlayerPrefs.GetInt(GameSettings.PP_SELECTED_SURVIVAL_LEVEL), PlayerPrefs.GetInt(GameSettings.PP_SELECTED_LEVEL_PACK));
		else
			loadLevel(PlayerPrefs.GetInt(GameSettings.PP_SELECTED_LEVEL), PlayerPrefs.GetInt(GameSettings.PP_SELECTED_LEVEL_PACK));
	}
	
	/// <summary>
	/// Do start the level load given
	/// </summary>
	public virtual void loadLevel(int levelId, int levelPackId){
		if(levelId <= 0)
			Debug.LogError("Level id "+ levelId + " is invalid. Level did not load.");
		if(levelPackId <= 0)
			Debug.LogError("Level pack id "+ levelPackId + " is invalid. Level pack did not load.");
		
		if(currentLevel != null)
			dispatcher.dispatch(LEVEL_LOADED, currentLevel);
	}
	
	/// <summary>
	/// Loads all of the levels.
	/// </summary>
	public virtual void loadAllOfLevels(){
		if(levels != null && levels.Count > 0)
			dispatcher.dispatch(ALL_LEVELS_LOADED);
	}
	
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
}
