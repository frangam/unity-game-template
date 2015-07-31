﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnionAssets.FLE;

public class UIBaseInAppButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	[Tooltip("The corresponding index of the inappIDs list from GameTemplate Settings depending on the game multiversion")]
	private int inAppIndexPackFromGameTemplate = 0;
	
	[SerializeField]
	private UIBaseInAppWin window;
	
	[SerializeField]
	private UIBaseInAppItem item;
	
	[SerializeField]
	private Text lbQuantity;
	
	[SerializeField]
	private Text lbRealMoneyPrice;
	
	[SerializeField]
	private bool hideWhenNonConsumableItemWasPurchased = true;
	
	[SerializeField]
	private bool restartWhenNonConsumableItemWasPurchased = false;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public UIBaseInAppItem Item {
		get {
			return this.item;
		}
	}	
	
	public UIBaseInAppWin Window {
		get {
			return this.window;
		}
		set {
			window = value;
		}
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		
		bool active = CoreIAPManager.Instance.IsInited && CoreIAPManager.Instance.NumProducts > 0;
		
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("UIBaseInAppButton - active ? " + active);
		
		//get the id from GameSettings based on the given index
		if(GameSettings.Instance.CurrentInAppBillingIDs != null && GameSettings.Instance.CurrentInAppBillingIDs.Count > 0 
		   && inAppIndexPackFromGameTemplate < GameSettings.Instance.CurrentInAppBillingIDs.Count){
			Item.Id = GameSettings.Instance.CurrentInAppBillingIDs[inAppIndexPackFromGameTemplate];
		}
		
		if(active){
			//get the id from GameSettings based on the given index
			//			if(GameSettings.Instance.CurrentInAppBillingIDs != null && GameSettings.Instance.CurrentInAppBillingIDs.Count > 0 
			//			   && inAppIndexPackFromGameTemplate < GameSettings.Instance.CurrentInAppBillingIDs.Count){
			//				Item.Id = GameSettings.Instance.CurrentInAppBillingIDs[inAppIndexPackFromGameTemplate];
			//			}
			
			Item.load();
			
			if(item != null){
				bool rewardedNonConsumable = item.RewardedNonConsumable;
				
				if(!rewardedNonConsumable){
					showInformation();
					
					getWindow();
				}
				//hide if the item is non consumable and user was rewarded yet after purchase it
				else{
					gameObject.SetActive(false);
				}
			}
		}
		else
			gameObject.SetActive(false);
	}
	
	protected override void doPress ()
	{
		base.doPress ();
		
		if(Window != null){
			if(Window != null && !string.IsNullOrEmpty(Item.Id))
				Window.purchaseItem(Item);
		}
	}
	
	public virtual void getWindow ()
	{
		if(Window == null)
			Window = GetComponentInParent<UIBaseInAppWin>();
		
		if(Window == null)
			Debug.LogError("UIBaseInAppButton - Not found UIBaseInAppWin");
	}
	
	public virtual void showInformation ()
	{
		if(item != null){
			if(item.LbName)
				item.LbName.text = item.LocalizedName;
			if(lbQuantity)
				lbQuantity.text = item.Quantity.ToString("N0", System.Globalization.CultureInfo.CurrentCulture);
			//			if(lbRealMoneyPrice){
			//				lbRealMoneyPrice.text = item.RealMoneyPrice.ToString();
			
			//				//TODO Poner moneda que corresponda
			//				lbRealMoneyPrice.text += "$";
			//			}
		}
	}
	
	public virtual void showPriceInfo(string price, string priceCurrencyCode = ""){
		if(lbRealMoneyPrice){
			lbRealMoneyPrice.text = price.ToString();
			
			//			//TODO Poner moneda que corresponda
			//			lbRealMoneyPrice.text += priceCurrencyCode;
		}
	}
	
	//--------------------------------------
	//  Public methods
	//--------------------------------------
	public void itemRewarded(UIBaseInAppItem pitem){
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("UIBaseInAppButton - item reward applyed: " + pitem + ", my item: " +this.item + ", rewarded non consumable ? " + pitem.RewardedNonConsumable);
		
		//hide in app button when the item is non consumable because user has just purchased it
		if(hideWhenNonConsumableItemWasPurchased && pitem != null && this.item != null && pitem.Id.Equals(pitem.Id) && pitem.IaType == UIBaseInAppItem.InAppItemType.NON_CONSUMABLE && pitem.RewardedNonConsumable){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("UIBaseInAppButton - hiding button " + gameObject.name);
			
			gameObject.SetActive(false);
		}
		
		if(restartWhenNonConsumableItemWasPurchased)
			ScreenLoaderVisualIndicator.Instance.LoadScene(Application.loadedLevelName);
	}
}
