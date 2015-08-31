/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using UnionAssets.FLE;

[System.Serializable]
public class UIBaseInAppItem : UIBaseListItem {
	public enum InAppItemType{
		COMSUMABLE,
		NON_CONSUMABLE,
		AUTO_RENEW_SUBSCRIPTION,
		FREE_SUBSCRIPTION,
		NON_RENEWABLE_SUSCRIPTION
	}
	
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string QUIT_ADS_ITEM 	= "QUIT_ADS";
	public const string MONEY_ITEM 		= "MONEY";
	public const string GEM_ITEM 		= "GEM";
	public const string PP_IN_APP_ITEM_PURCHASED = "pp_in_app_item_purchased_";
	public const string REWARD_APPLYED = "gt_inapp_item_reward_applyed";
	
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	private static EventDispatcherBase _dispatcher  = new EventDispatcherBase ();
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private float 	realMoneyPrice;
	
	[SerializeField]
	private int 	quantity;
	
	[SerializeField]
	private string 	itemType;
	
	[SerializeField]
	private InAppItemType iaType = InAppItemType.COMSUMABLE;
	
	[SerializeField]
	private string  spriteName;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private string ppKey;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public static EventDispatcherBase dispatcher {
		get {
			return _dispatcher;
		}
	}
	
	public float RealMoneyPrice {
		get {
			return this.realMoneyPrice;
		}
	}
	
	public int Quantity {
		get {
			return this.quantity;
		}
	}
	
	public string ItemType {
		get {
			return this.itemType;
		}
	}
	
	public string LocalizedName{
		get{
			return Localization.Get(Name);
		}
	}
	
	public InAppItemType IaType {
		get {
			return this.iaType;
		}
	}
	
	public bool RewardedNonConsumable{
		get{
			bool rewarded = PlayerPrefs.GetInt(ppKey) != 0 ? true : false;
			
			if(GameSettings.Instance.showTestLogs){
				Debug.Log("UIBaseInAppItem - playerprefs key rewarded: " + ppKey);
				Debug.Log("UIBaseInAppItem - rewarded: " + rewarded);
			}
			
			return rewarded && iaType == InAppItemType.NON_CONSUMABLE;
		}
	}
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="UIBaseInAppItem"/> class.
	/// ID,NAME,ITEM TYPE,QUANTITY TO ACQUIRE,SPRITE NAME,REAL MONEY PRICE
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="anim">Animation.</param>
	public UIBaseInAppItem(string data, Animator anim = null): base(data, anim){
		string[] atts = data.Split(ATTRIBUTES_SEPARATOR);
		init(atts);
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(string[] data){
		ppKey = PP_IN_APP_ITEM_PURCHASED+Id;
		int pQuantity;
		float pPrice;
		
		//IN APP type
		if(data.Length > 2)
			initInAppType(data[2]);
		
		//item type
		if(data.Length > 3)
			itemType = data[3];
		
		//Quantity
		if(data.Length > 4 && int.TryParse(data[4], out pQuantity))
			quantity = pQuantity;
		
		//sprite name
		if(data.Length > 5)
			spriteName = data[5];
		
		//real money price
		if(data.Length > 6 && float.TryParse(data[6], out pPrice))
			realMoneyPrice = pPrice;
	}
	
	public virtual void applyReward(){
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("UIBaseInAppItem - applying reward");
		
		//in app type
		switch(iaType){
		case InAppItemType.NON_CONSUMABLE:
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("UIBaseInAppItem - in app type non consumable saving rewarded");
			PlayerPrefs.SetInt(ppKey, 1); //save it was rewarded with this non consumable in app item
			break;
		}
		
		//item type
		switch(itemType){
		case QUIT_ADS_ITEM:
			AdsHandler.Instance.quitAds();
			break;
		case MONEY_ITEM:
			GameMoneyManager.Instance.addMoney(quantity);
			break;
			
		case GEM_ITEM:
			GameMoneyManager.Instance.addGems(quantity);
			break;
		}
		
		//dispatch events
		dispatcher.dispatch(REWARD_APPLYED, this);
	}
	
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void initInAppType(string t){
		switch(t){
		case "C": iaType = InAppItemType.COMSUMABLE; break;
		case "NC": iaType = InAppItemType.NON_CONSUMABLE; break;
		case "ARS": iaType = InAppItemType.AUTO_RENEW_SUBSCRIPTION; break;
		case "FS": iaType = InAppItemType.FREE_SUBSCRIPTION; break;
		case "NS": iaType = InAppItemType.NON_RENEWABLE_SUSCRIPTION; break;
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[UIBaseInAppItem: realMoneyPrice={0}, quantity={1}, itemType={2}]", realMoneyPrice, quantity, itemType);
	}
	
	public override void load ()
	{
		base.load ();
		
		string stats = getContent();
		
		//load stats
		if(!string.IsNullOrEmpty(stats)){
			string[] att = stats.Split(ATTRIBUTES_SEPARATOR);
			init (att);
		}
	}
}
