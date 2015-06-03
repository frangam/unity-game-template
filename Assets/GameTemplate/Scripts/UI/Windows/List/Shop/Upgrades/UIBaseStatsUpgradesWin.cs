using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIBaseStatsUpgradesWin : UIBaseShopListWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private UIConfirmDialogPayWithGemsUpgradeWin payWithGemsWin;
	
	[SerializeField]
	private List<UpgradeStatButton> upgradeButtons;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public List<UpgradeStatButton> UpgradeButtons {
		get {
			return this.upgradeButtons;
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		
		if(upgradeButtons == null || (upgradeButtons != null && upgradeButtons.Count == 0)){
			UpgradeStatButton[] b = GetComponentsInChildren<UpgradeStatButton>() as UpgradeStatButton[];
			upgradeButtons = new List<UpgradeStatButton>(b);
		}
		else
			Debug.LogError("Not found any UpgradeStatButton");
	}
	
	
	public override void initIndexCurrentItem ()
	{
		string itemSelected = PlayerPrefs.GetString(GameSettings.PP_UNIQUE_LIST_CURRENT_ITEM_SELECTED_ID); 
		int res;
		
		if(int.TryParse(itemSelected, out res)){
			indexCurrentItem = res;
		}
		else
			base.initIndexCurrentItem();
	}
	
	
	public override UIBaseListItem createListItem (string data)
	{
		return new UIBaseUpgradableStatListItem(PP_KEY, data);
	}
	
	public override void open ()
	{
		base.open ();
		
		if(upgradeButtons != null)
			init ();
	}
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void init(){
		foreach(UpgradeStatButton b in upgradeButtons)
			b.init();
	}
	
	public virtual void upgrade(UpgradableStat stat){
		UIBaseUpgradableStatListItem uItem = (UIBaseUpgradableStatListItem) currentItemSelected;
		
		if(uItem != null)
			uItem.upgrade(stat);
	}
	
	public virtual void openConfirmPayWithGems(int moneyPrice, UpgradeStatButton button){
		UIBaseUpgradableStatListItem uItem = (UIBaseUpgradableStatListItem) currentItemSelected;
		
		if(payWithGemsWin){
			payWithGemsWin.init(button);
			payWithGemsWin.init(moneyPrice, GameMoneyManager.Instance.hasEnoughMoney(moneyPrice), GameMoneyManager.Instance.hasEnoughGems(moneyPrice, true)); //first init the window
			UIController.Instance.Manager.open(payWithGemsWin);
		}
	}
	
	public virtual void openAddMoreCredit(){
		if(addMoreGemsWin)
			UIController.Instance.Manager.open(addMoreGemsWin);
	}
}
