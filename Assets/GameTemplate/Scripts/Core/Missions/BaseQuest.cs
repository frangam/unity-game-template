using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BaseQuest {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const char 	SEPARATOR_ATTRIBUTES 		= ',';
	public const char 	SEPARATOR_ACTIONS_IDS 		= '.';
	public const char 	SEPARATOR_LOCALIZED_DESC	= '|';
	public const int 	NO_LEVEL 					= -1;
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string name;
	
	[SerializeField]
	private string description;
	
	[SerializeField]
	private string id;
	
	[SerializeField]
	protected int level;
	
	[SerializeField]
	public List<GameAction> actions;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool completed = false;
	
	//--------------------------------------
	// Protected Attributes
	//--------------------------------------
	protected string idPlayerPrefs;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string Name {
		get {
			return this.name;
		}
	}
	
	public string Description {
		get {
			return this.description;
		}
		set{
			this.description = value;
		}
	}
	
	public string Id {
		get {
			return this.id;
		}
		set{
			this.id = value;
		}
	}
	
	public int Level {
		get {
			return this.level;
		}
		set{
			this.level = value;
		}
	}
	
	public List<GameAction> Actions {
		get {
			return this.actions;
		}
		set{
			this.actions = value;
		}
	}
	
	public bool Completed {
		get {
			return this.completed;
		}
		set {
			completed = value;
		}
	}
	
	public string IdPlayerPrefs {
		get {
			return this.idPlayerPrefs;
		}
	}
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	public BaseQuest(){
		id = "";
		actions = new List<GameAction>();
		description = "";
	}
	
	public BaseQuest(BaseQuest aQuest){
		id = aQuest.Id;
		actions = aQuest.Actions;
		description = aQuest.Description;
	}
	
	public BaseQuest(string pID, List<GameAction> pActions, int pLevel, string locDescriptionKey = ""){
		id = pID;
		actions = pActions;
		
		if(!string.IsNullOrEmpty(locDescriptionKey))
			description = Localization.Get(locDescriptionKey);
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseQuest`1"/> class.
	/// 
	/// Attributes:
	/// ID, Actions, Level, Localized description
	/// 
	/// Actions: a1.a2.a3.a4.
	/// Localized description: EN, ES, FR
	/// 
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	/// <param name="pAllActions">P all actions.</param>
	public BaseQuest(string attributes, List<GameAction> pAllActions){
		string[] atts = attributes.Split(SEPARATOR_ATTRIBUTES);
		
		//Quest ID
		if(atts.Length > 0){
			id = atts[0];
			init();
		}
		else{
			Debug.LogError("It was not found ID attribute");
		}
		
		//Game Actions
		if(atts.Length > 1){
			string[] actionIds = atts[1].Split(SEPARATOR_ACTIONS_IDS);
			this.actions = new List<GameAction>();
			List<string> idsAux = new List<string>();
			foreach(string aId in actionIds){
				if(!idsAux.Contains(aId)){
					idsAux.Add(aId);
					
					foreach(GameAction ga in pAllActions){
						if(ga.Id == aId){
							this.actions.Add(ga);
							break;
						}
					}
				}
			}
		}
		else{
			Debug.LogError("It was not found actions ids attribute");
		}
		
		//Level
		if(atts.Length > 2){
			int iL;
			
			if(int.TryParse(atts[2], out iL)){
				level = iL;
			}
			else{
				level = NO_LEVEL;
			}
		}
		else{
			level = NO_LEVEL;
		}
		
		//Description
		if(atts.Length > 3){
			string[] desc = atts[3].Split(SEPARATOR_LOCALIZED_DESC);
			string locKey = desc[0]; //localization key for a localized description
			
			if(!string.IsNullOrEmpty(locKey))
				description = Localization.Get(locKey);
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString (){
		return string.Format ("[BaseQuest: id={0}, level={1} actions={2} name={3}, description={4}, completed={5}]", id, level, actionsToString(),name, description, completed);
	}
	
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual string actionsToString(){
		string actionsStr = "";
		
		for(int i=0; i<actions.Count; i++){
			actionsStr += actions[i];
			
			if(i<actions.Count-1){
				actionsStr += SEPARATOR_ACTIONS_IDS;
			}
		}
		
		return actionsStr;
	}
	
	public virtual bool loadedCorrectly(){
		return (id != null && actions.Count > 0 && level > 0);
	}
	
	public bool completedPreviously(){
		return PlayerPrefs.GetInt(idPlayerPrefs) == 1;
	}
	
	public virtual void init(){
		idPlayerPrefs = "pp_quest_unlocked_" + Id;
	}
	
	/// <summary>
	/// Gets the progress percentage.
	/// </summary>
	/// <returns>The progress percentage.</returns>
	public float getProgressPercentage(){
		float res = 0f;
		int activeActions = 0;
		
		//check total active actions
		foreach(GameAction action in actions){
			if(action.isCompleted()){
				activeActions++;
			}
		}
		
		res = (activeActions * 100f) / actions.Count;
		
		return res;
	}
	
	/// <summary>
	/// Gets the progress integer value.
	/// The total completed actions
	/// </summary>
	/// <returns>The progress integer value.</returns>
	public int getProgressIntegerValue(){
		int res = 0;
		
		//check total active actions
		foreach(GameAction action in actions){
			if(action.isCompleted()){
				res++;
			}
		}
		
		return res;
	}
}
