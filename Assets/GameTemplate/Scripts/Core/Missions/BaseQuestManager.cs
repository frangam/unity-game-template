using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnionAssets.FLE;

public class BaseQuestManager : BaseQuestManager<BaseQuestManager, BaseQuest>{
	
}

public class BaseQuestManager<S, T> : Singleton<S> where S : MonoBehaviour {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string GAME_PROPERTY_CHANGED = "gt_game_property_changed";
	public const string GAME_PROPERTY_RESETED = "gt_game_property_reseted";
	public const string ACTION_COMPLETED = "gt_action_completed";
	public const string QUEST_COMPLETED = "gt_quest_completed";
	public const string ALL_QUESTS_COMPLETED = "gt_all_quests_completed";
	
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	/// <summary>
	/// Observer pattern
	/// </summary>
	private static EventDispatcherBase _dispatcher  = new EventDispatcherBase ();
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private List<GameMode> validIn = new List<GameMode>(){GameMode.CAMPAIGN};
	
	[SerializeField]
	private string questsFile = "Quests";
	
	[SerializeField]
	private string actionsFile = "Actions";
	
	[SerializeField]
	private bool saveOnPlayerPrefs = true;
	
	[SerializeField]
	private bool updateCompletedFlagOfPreviousCompletedQuests = true;
	
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected List<GameAction> actions;
	private int levelSelected;
	private List<BaseQuest> quests;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public static EventDispatcherBase dispatcher {
		get {
			return _dispatcher;
		}
	}
	
	public List<BaseQuest> Quests {
		get {
			return this.quests;
		}
	}
	
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		if(validIn != null && validIn.Count > 0){
			bool active = validIn.Contains(GameController.Instance.Manager.GameMode);
			gameObject.SetActive(active);
			
			if(active){
				//do some stuff if it is active
			}
		}
		else{
			//do some stuff if it is active
		}
	}
	public virtual void OnDestroy(){
		dispatcher.removeEventListener(GAME_PROPERTY_CHANGED, OnGamePropertyChanged); 
		dispatcher.removeEventListener(GAME_PROPERTY_RESETED, OnGamePropertyReseted); 
		dispatcher.removeEventListener(ACTION_COMPLETED, OnActionCompleted); 
		dispatcher.removeEventListener(QUEST_COMPLETED, OnQuestCompleted); 
		dispatcher.removeEventListener(ALL_QUESTS_COMPLETED, OnAllQuestsCompleted); 
	}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void initialUpdateOfPreviousCompletedQuests(){
		if(quests != null && quests.Count > 0){
			//update to completed all quest were completed previously
			foreach(BaseQuest q in quests){
				if(q.completedPreviously()){
					q.Completed = true;
				}
			}
		}
		else{
			Debug.LogWarning("First it is required to load quests");
		}
	}
	
	private void initialVerification(){
		if(quests != null){
			foreach(BaseQuest quest in quests){
				if(quest.Actions == null || (quest.Actions != null && quest.Actions.Count == 0))
					Debug.LogWarning("BaseQuest " + quest + " has not any actions attached");
				else{
					foreach(GameAction action in quest.Actions){
						if(action == null)
							Debug.LogWarning("BaseQuest " + quest + " has some empty actions attached");
					}
				}
			}
		}
		else{
			Debug.LogWarning("There are not any quests to handle");
		}
	}
	
	private bool hasTag(GameAction action, List<string> tags){
		bool res = false;
		
		foreach(string tag in tags){
			if(action.Tags != null){
				res = action.Tags.Contains(tag);
				
				if(res)
					break;
			}
		}
		
		return res;
	}
	
	private void doSetValue(GameAction action, int value, bool ignoreActivationConstraint = false){
		int finalValue = value;
		
		if(!ignoreActivationConstraint){
			int actionProgress = action.Progress;
			
			switch(action.ActivationCondition){
			case AchieveCondition.ACTIVE_IF_GREATER_THAN:
				finalValue = value > actionProgress ? value : actionProgress;
				break;
				
			case AchieveCondition.ACTIVE_IF_LOWER_THAN:
				finalValue = value < actionProgress ? value : actionProgress;
				break;
			}
		}
		
		action.Progress = finalValue;
	}
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void init(int pSelectedLevel){
		if((typeof(T) != typeof(Achievement) && pSelectedLevel > 0)
		   || (typeof(T) == typeof(Achievement))){
			levelSelected = pSelectedLevel;
			if(loadGameActionsContentFromTextFile() && loadQuestsContentFromTextFile()) { //load quest from text file resource
				//update completed flag of all of the previous completed quests
				if(updateCompletedFlagOfPreviousCompletedQuests){
					initialUpdateOfPreviousCompletedQuests();
				}
				
				//show quests and actions for test purposes
				if(GameSettings.Instance.showTestLogs)
					showActionsAndQuests();
				
				initialVerification();
				
				//listeners
				dispatcher.addEventListener(GAME_PROPERTY_CHANGED, OnGamePropertyChanged); 
				dispatcher.addEventListener(GAME_PROPERTY_RESETED, OnGamePropertyReseted); 
				dispatcher.addEventListener(ACTION_COMPLETED, OnActionCompleted); 
				dispatcher.addEventListener(QUEST_COMPLETED, OnQuestCompleted); 
				dispatcher.addEventListener(ALL_QUESTS_COMPLETED, OnAllQuestsCompleted); 
				
				
			}
		}
		else{
			Debug.LogWarning("Any level selected");
		}
	}
	
	public bool loadGameActionsContentFromTextFile(){
		bool loaded = false, located = false;
		
		//Try to load actions file
		TextAsset text = Resources.Load(actionsFile) as TextAsset;
		if(text == null){
			Debug.LogError("You must provide a correct actions filename");
			located = false;
		}
		else{
			located = true;
		}
		
		if(located){
			string fileContents = text.text;
			string[] lines = fileContents.Split('\n');
			actions = new List<GameAction>();
			
			foreach(string line in lines){
				if(line != null && line != "" && line.Length > 0){
					GameAction action = new GameAction(line);
					
					if(action != null && action.loadedCorrectly()){
						actions.Add(action);
					}
				}
			}
			
			loaded = actions != null && actions.Count > 0;
			
			if(!loaded){
				Debug.LogError("Could not load any Action");
			}
		}
		
		return located && loaded;
	}
	
	public bool loadQuestsContentFromTextFile(){
		bool loaded = false, located = false;
		string content = "";
		
		if(actions != null && actions.Count > 0){
			//Try to load quests file
			TextAsset text = Resources.Load(questsFile) as TextAsset;
			if(text == null){
				Debug.LogError("You must provide a correct quests filename");
				located = false;
			}
			else{
				located = true;
			}
			
			if(located){
				
				string fileContents = text.text;
				string[] lines = fileContents.Split('\n');
				quests = new List<BaseQuest>();
				
				foreach(string line in lines){
					if(line != null && line != "" && line.Length > 0){
						BaseQuest quest = QuestFactory.create(typeof(T),line, actions);
						
						//we add only BaseQuests its level is the selected
						//or if BaseQuest is an Achievement
						if(quest != null && quest.loadedCorrectly() 
						   && ((typeof(T) != typeof(Achievement) && quest.Level == levelSelected)
						    || (typeof(T) == typeof(Achievement)))){
							quests.Add(quest);
						}
					}
				}
				
				loaded = quests != null && quests.Count > 0;
				
				if(!loaded){
					Debug.LogWarning("Could not load any Quest");
				}
			}
		}
		
		return located && loaded;
	}
	
	public void addValue(GameAction action, int value, bool ignoreActivationConstraint = false){
		doSetValue(action, action.Progress + value, ignoreActivationConstraint);
	}
	
	public void setValue(GameAction action, int value, bool ignoreActivationConstraint = false){
		doSetValue(action, value, ignoreActivationConstraint);
	}
	
	/// <summary>
	/// Resets all actions tagged with tags indicated
	/// </summary>
	/// <param name="tags">Tags.</param>
	public void resetProperties(List<string> tags = null){
		if(actions != null){
			foreach(GameAction a in actions){
				if(a.Tags == null || hasTag(a, tags)){
					a.reset();
				}
			}
		}
	}
	
	public virtual void checkGoalsProgresses(List<string> tags, GameAction action){
		//		List<BaseQuest> res = new List<BaseQuest>();
		
		foreach(BaseQuest quest in quests){
			if(quest.Actions.Contains(action) //has given action
			   && !quest.Completed){	//locked
				//if all actions are active unlock achievement
				if(quest.getProgressIntegerValue() == quest.Actions.Count){
					quest.Completed = true; //uptade unlocked flag to true
					//					res.Add(quest); //add it to the result
					dispatcher.dispatch(QUEST_COMPLETED, quest); //dispatch event quest completation
				}
			}
		}
		
		
		//		return res;
	}
	
	public virtual void showActionsAndQuests(){
		string s = "Actions:\n";
		foreach(GameAction a in actions){
			s+=a.ToString() + "\n";
		}
		
		s+="\nQuests:\n";
		foreach(BaseQuest q in quests){
			s+=q.ToString() + "\n";
		}
		
		Debug.Log(s);
	}
	
	public virtual void saveCompletedQuestsInPlayerPrefs(BaseQuest quest){
		string aID = quest.IdPlayerPrefs; //get the playerprefs id to save completed state of the quest
		
		//si no se habia desbloqueado previamente, lo anotamos en la memoria
		if(PlayerPrefs.GetInt(aID) != 1)
			PlayerPrefs.SetInt(aID, 1);
	}
	
	
	
	
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	/// <summary>
	/// When an game property value changes raise this event
	/// </summary>
	/// <param name="e">E.</param>
	public void OnGamePropertyChanged(CEvent e){
		GameActionResult result = e.data as GameActionResult;
		
		if(result.IsSucceeded){
			foreach(string id in result.ActionsIds){
				foreach(GameAction action in actions){
					if(action.Id == id){
						if(!action.isCompleted()){
							//modify action progress
							switch(result.ActionOperation){
							case GameActionOperation.ADD:
								addValue(action, result.GamePropertyValue);
								break;
								
							case GameActionOperation.SET:
								setValue(action, result.GamePropertyValue);
								break;
							}
						}
						
						break;
					}
				}
			}
		}
	}
	
	/// <summary>
	/// When an game property value is reseted raise this event
	/// </summary>
	/// <param name="e">E.</param>
	public void OnGamePropertyReseted(CEvent e){
		GameActionResult result = e.data as GameActionResult;
		
		if(result.IsSucceeded){
			foreach(string id in result.ActionsIds){
				foreach(GameAction action in actions){
					if(action.Id == id){
						action.reset();					
						break;
					}
				}
			}
		}
	}
	
	/// <summary>
	/// When an action value changes raise this event
	/// </summary>
	/// <param name="e">E.</param>
	public void OnActionCompleted(CEvent e){
		GameActionResult result = e.data as GameActionResult;
		//		List<BaseQuest> completedQuests = null;
		
		if(result.IsSucceeded){
			foreach(GameAction action in actions){
				if(action.Id == result.CurrentActionId){
					checkGoalsProgresses(null, action);
					break;
				}
			}
			
			//---
			//All quests completed
			int completed = 0;
			foreach(BaseQuest q in Quests){
				if(q.Completed){
					completed++;
				}
			}
			
			if(completed == Quests.Count){
				dispatcher.dispatch(ALL_QUESTS_COMPLETED);
			}
			//---
		}
	}
	
	public virtual void OnQuestCompleted(CEvent e){
		BaseQuest quest = e.data as BaseQuest;
		
		if(quest != null){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log(quest + " is completed");
			
			if(saveOnPlayerPrefs){
				if(GameSettings.Instance.showTestLogs)
					Debug.Log(quest + " is saving on PlayerPrefs");
				
				saveCompletedQuestsInPlayerPrefs(quest);
			}
		}
	}
	
	public virtual void OnAllQuestsCompleted(CEvent e){
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("All of the quests are completed");
	}
}
