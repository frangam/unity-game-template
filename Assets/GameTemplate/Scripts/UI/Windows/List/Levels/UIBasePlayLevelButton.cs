using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBasePlayLevelButton : UIBaseButton {
	//--------------------------------------
	// Settings Attributes
	//--------------------------------------
	[SerializeField]
	private UIBaseLevelSelectorWin window;
	
	[SerializeField]
	private Text lbLevelID;
	
	[SerializeField]
	private Image spLock;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private BaseLevel level;
	private bool unlocked = true;
	
	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public BaseLevel Level {
		get {
			return this.level;
		}
	}
	
	public bool Unlocked {
		get {
			return this.unlocked;
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		
		if(window == null)
			window = GetComponentInParent<UIBaseLevelSelectorWin>();
	}
	
	protected override bool canPress ()
	{
		return unlocked;
	}
	
	protected override void doPress ()
	{
		base.doPress ();
		
		if(window != null){
			if(level != null)
				window.playLevel(level);
			else
				window.playLevel();
		}
		else
			Debug.LogError("Not found UIBaseLevelSelectorWin");
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(BaseLevel level, UIBaseLevelSelectorWin _window){
		if(level != null){
			this.level = level;
			
			if(lbLevelID)
				lbLevelID.text = level.Id;
			
			if(spLock){
				int lastUnlockedLevel = PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED+level.LevelPackId);
				int id;
				
				
				
				if(int.TryParse(level.Id, out id)){
					if(level.Id.Equals("101")){
						Debug.Log(level);
						Debug.Log("last unlocked: " +lastUnlockedLevel);
					}
					
					unlocked = id <= lastUnlockedLevel;
					button.interactable = unlocked;
					spLock.gameObject.SetActive(!unlocked);
				}
			}
			
			
			
			this.window = _window;
		}
	}
}
