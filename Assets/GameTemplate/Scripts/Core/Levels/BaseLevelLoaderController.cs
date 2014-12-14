using UnityEngine;
using System.Collections;

public class BaseLevelLoaderController : Singleton<BaseLevelLoaderController> {
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
	private string levelFile = "Levels";
		
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		
	}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Gets all of the content levels file, that is, all of the text in levels file
	/// </summary>
	/// <returns>The content level file.</returns>
	public string getContentLevelFile(){
		bool located = false;
		string content = "";

		//Try to load levels file
		TextAsset text = Resources.Load(levelFile) as TextAsset;

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

	/// <summary>
	/// Gets the content (all attributes) of the current level selected
	/// </summary>
	/// <returns>The current level content.</returns>
	public string getCurrentLevelContent(){
		string content = getContentLevelFile();
		string[] lines = null;
		string result = "";

		if(!string.IsNullOrEmpty(content)){
			lines = content.Split('\n'); //get each line of the content

			if(lines == null || lines.Length == 0){
				Debug.LogWarning("Levels content is empty or is invalid");
			}
			else{
				int currentLevel = PlayerPrefs.GetInt(GameSettings.PP_SELECTED_LEVEL); // get the current level selected

				foreach(string line in lines){
					string[] level = line.Split(SEPARATOR_ATTRIBUTES);
					int levelId;

					//first attribute is the level id
					if(int.TryParse(level[0], out levelId) && levelId == currentLevel){
						result = line;
						break;
					}
				}
			}
		}

		return result;
	}

	/// <summary>
	/// Do start the level load
	/// </summary>
	public virtual void loadLevel(){

	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
}
