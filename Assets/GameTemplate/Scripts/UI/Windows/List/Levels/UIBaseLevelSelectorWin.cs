using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIBaseLevelSelectorWin : UIBaseListWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Text lbLevelDesc;
	
	[SerializeField]
	private Text lbMoneyReward;
	
	[SerializeField]
	private Text lbGemsReward;
	
	[SerializeField]
	private Transform pnlGemsReward;
	
	[SerializeField]
	private UIBasePlayLevelButton pbLevelButton;
	
	[SerializeField]
	private Transform populateWithLevelButtonPb;
	
	[SerializeField]
	private List<UIBasePlayLevelButton> levelButtons;
	
	[SerializeField]
	private string sceneToGoAtSelectLevel = GameSettings.SCENE_GAME;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected int 			lastUnlockedLevel;
	
	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public List<UIBasePlayLevelButton> LevelButtons {
		get {
			return this.levelButtons;
		}
	}
	
	public UIBasePlayLevelButton CurrentLevelButton{
		get{
			UIBasePlayLevelButton res = null;
			UIBaseLevel uiLevel = (UIBaseLevel) currentItemSelected;
			
			foreach(UIBasePlayLevelButton b in levelButtons){
				if(b.Level.Id.Equals(uiLevel.Level.Id)){
					res = b;
					break;
				}
			}
			
			return res;
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void loadItems ()
	{
		base.loadItems ();
		
		if(indexCurrentItem >= items.Count)
			indexCurrentItem--;
		
		loadLevelButtons();
	}
	
	public override UIBaseListItem createListItem (string data)
	{
		return new UIBaseLevel(data);
	}
	
	public override void setLabelName (UIBaseListItem item)
	{
		if(lbNameCurrentItem){
			UIBaseLevel level = (UIBaseLevel) item;
			
			if(level != null)
				lbNameCurrentItem.text = level.Id+"/"+items.Count.ToString();
		}
	}
	
	public override void initIndexCurrentItem ()
	{
		int last = PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED);
		
		if(last == 0)
			PlayerPrefs.SetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED, 1);
		
		lastUnlockedLevel = last == 0 ? 1: last; 
		indexCurrentItem = lastUnlockedLevel - 1; //-1 porque el primer nivel es el 1 y el indice debe empezar en 0;
	}
	
	public override void initIndexLastItem ()
	{
		indexLastItem = lastUnlockedLevel;
	}
	
	public override void showItem (UIBaseListItem item)
	{
		base.showItem (item);
		
		UIBaseLevel uiLevel = (UIBaseLevel) item;
		
		if(uiLevel != null && uiLevel.Level != null)
			showLevel(uiLevel.Level);
	}
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void loadLevelButtons(){
		if((levelButtons == null || (levelButtons != null && levelButtons.Count == 0)) && pbLevelButton && populateWithLevelButtonPb){
			loadLevelForButtons();
		}
	}
	
	public virtual void loadLevelForButtons(){
		
		
		if(items != null && items.Count > 0){
			levelButtons = new List<UIBasePlayLevelButton>();
			
			foreach(UIBaseListItem item in items){
				UIBaseLevel uiLevel = (UIBaseLevel) item;
				
				if(uiLevel.Level != null){
					UIBasePlayLevelButton button = pbLevelButton.Spawn(populateWithLevelButtonPb);
					//					button.transform.parent = populateWithLevelButtonPb;
					button.init(uiLevel.Level, this);
					levelButtons.Add(button);
				}
			}
		}
	}
	
	
	
	public virtual void showLevel(BaseLevel level){
		if(lbLevelDesc)
			lbLevelDesc.text = level.LocalizedDescription;
		if(lbMoneyReward)
			lbMoneyReward.text = level.RealMoneyReward.ToString();
		if(lbGemsReward)
			lbGemsReward.text = level.RealGemsReward.ToString();
		
		if(pnlGemsReward)
			pnlGemsReward.gameObject.SetActive(level.RealGemsReward > 0);
	}
	
	public virtual void playLevel(string levelId = ""){
		string _id = items[indexCurrentItem].Id; //id from the current item selected
		
		//the given id
		if(!string.IsNullOrEmpty(levelId)){
			_id = levelId;
		}
		
		//select and play level
		if(!string.IsNullOrEmpty(_id)){
			int l;
			
			if(int.TryParse(_id, out l)){
				PlayerPrefs.SetInt(GameSettings.PP_SELECTED_LEVEL, l);
				ScreenLoaderVisualIndicator.Instance.LoadScene(sceneToGoAtSelectLevel);
			}
		}
	}
	
	public virtual void showMissions(bool show){
		int pShow = show ? 1: 0;
		PlayerPrefs.SetInt(GameSettings.PP_SHOW_MISSIONS_WINDOW, pShow);
	}
}
