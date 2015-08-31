/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBasePurchasableLevelWin : UIBaseShopListWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------	
	[SerializeField]
	private Text 			lbLevelDesc;
	
	[SerializeField]
	private Text 			lbMoneyReward;
	
	[SerializeField]
	private Text 			lbGemsReward;
	
	[SerializeField]
	private Transform 		pnlGemsReward;
	
	[SerializeField]
	private string 			sceneToGoAtSelectLevel = GameSettings.SCENE_GAME;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected int 			lastUnlockedLevel;
	
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
		return new UIBasePurchasableLevel(PP_KEY, data);
	}
	
	public override void setLabelName (UIBaseListItem item)
	{
		if(lbNameCurrentItem){
			UIBasePurchasableLevel level = (UIBasePurchasableLevel) item;
			
			if(level != null)
				lbNameCurrentItem.text = level.Id;
		}
	}
	
	public override void initIndexCurrentItem ()
	{
		int last = lastSelectedLevel();
		int lastPack = lastSelectedLevelPack();
		
		if(last == 0)
			PlayerPrefs.SetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED+lastPack.ToString(), 1);
		
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
		
		UIBasePurchasableLevel uiLevel = (UIBasePurchasableLevel) item;
		
		if(uiLevel != null && uiLevel.Level != null)
			showLevel(uiLevel.Level);
	}
	
	
	public override void itemPurchased (UIBasePurchasableListItem item, bool payWithMoney = true)
	{
		base.itemPurchased (item);
		
		PlayerPrefs.SetInt(item.PlayerPrefsKey, 1);
	}
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual int lastSelectedLevel(){
		return PlayerPrefs.GetInt(GameSettings.PP_SELECTED_LEVEL); 
	}
	public virtual int lastSelectedLevelPack(){
		return PlayerPrefs.GetInt(GameSettings.PP_SELECTED_LEVEL_PACK); 
	}
	
	public virtual void showLevel(BasePurchasableLevel level){
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
