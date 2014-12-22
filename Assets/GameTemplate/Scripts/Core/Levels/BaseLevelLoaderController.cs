using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseLevelLoaderController<T> : Singleton<T> where T: MonoBehaviour {
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const char 	SEPARATOR_ATTRIBUTES 		= ',';
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string levelsFile = "Levels";
	
	[SerializeField]
	private bool loadCurrentLevelAtStart = true;
	
	[SerializeField]
	private bool loadAllLevels = false;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected List<BaseLevel> levels;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
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
	
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		if(loadCurrentLevelAtStart)
			loadCurrentLevel();
		if(loadAllLevels)
			loadAllOfLevels();
	}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
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
	public string getLevelContent(int levelId){
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
					int levelIdFromLine;
					
					//first attribute is the level id
					if(int.TryParse(level[0], out levelIdFromLine) && levelIdFromLine == levelId){
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
		return getLevelContent(PlayerPrefs.GetInt(GameSettings.PP_SELECTED_LEVEL)); // get the current level selected content
	}
	
	/// <summary>
	/// Loads the current selected level
	/// </summary>
	/// <param name="levelId">Level identifier.</param>
	public void loadCurrentLevel(){
		loadLevel(PlayerPrefs.GetInt(GameSettings.PP_SELECTED_LEVEL));
	}
	
	/// <summary>
	/// Do start the level load given
	/// </summary>
	public virtual void loadLevel(int levelId){
		if(levelId <= 0)
			Debug.LogError("Level id "+ levelId + " is invalid. Level did not load.");
	}
	
	/// <summary>
	/// Loads all of the levels.
	/// </summary>
	public virtual void loadAllOfLevels(){
		
	}
	
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
}
