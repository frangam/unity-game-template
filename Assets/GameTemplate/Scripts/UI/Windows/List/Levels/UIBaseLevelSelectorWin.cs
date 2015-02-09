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
	private string sceneToGoAtSelectLevel = GameSettings.SCENE_GAME;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int 			lastUnlockedLevel;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void loadItems ()
	{
		base.loadItems ();

		if(indexCurrentItem >= items.Count)
			indexCurrentItem--;
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
	
	public virtual void playLevel(){
		int l;
		
		if(int.TryParse(items[indexCurrentItem].Id, out l)){
			PlayerPrefs.SetInt(GameSettings.PP_SELECTED_LEVEL, l);
				ScreenLoaderVisualIndicator.Instance.LoadScene(sceneToGoAtSelectLevel);
		}
	}

	public virtual void showMissions(bool show){
		int pShow = show ? 1: 0;
			PlayerPrefs.SetInt(GameSettings.PP_SHOW_MISSIONS_WINDOW, pShow);
	}
}
