using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnionAssets.FLE;

public class UIBaseInAppButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private UIBaseInAppWin window;
	
	[SerializeField]
	private UIBaseInAppItem item;
	
	[SerializeField]
	private Text lbQuantity;
	
	[SerializeField]
	private Text lbRealMoneyPrice;
	
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
	// Overriden Methods
	//--------------------------------------
	public override void OnEnable ()
	{
		base.OnEnable ();
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("UIBaseInAppButton- enabling quit ads button");
		CoreIAPManager.dispatcher.removeEventListener(UIBaseInAppItem.REWARD_APPLYED, OnItemRewardApplyed);
	}
	
	public override void OnDisable ()
	{
		base.OnDisable ();
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("UIBaseInAppButton - disabling quit ads button");
		CoreIAPManager.dispatcher.removeEventListener(UIBaseInAppItem.REWARD_APPLYED, OnItemRewardApplyed);
	}
	
	public override void OnDestroy ()
	{
		base.OnDestroy ();
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("UIBaseInAppButton - disabling quit ads button");
		CoreIAPManager.dispatcher.removeEventListener(UIBaseInAppItem.REWARD_APPLYED, OnItemRewardApplyed);
	}
	
	public virtual  void Awake ()
	{
		base.Awake ();
		
		bool active = CoreIAPManager.Instance.IsInited && CoreIAPManager.Instance.NumProducts > 0;
		
		if(active){
			
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
				lbQuantity.text = item.Quantity.ToString();
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
			
			//TODO Poner moneda que corresponda
			lbRealMoneyPrice.text += priceCurrencyCode;
		}
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	public void OnItemRewardApplyed(CEvent e){
		UIBaseInAppItem pitem = e.data as UIBaseInAppItem;
		
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("UIBaseInAppButton - item reward applyed: " + pitem + ", my item: " +this.item + ", rewarded non consumable ? " + pitem.RewardedNonConsumable);
		
		//hide in app button when the item es non consumable because user has just purchased it
		if(pitem != null && this.item != null && pitem == this.item && pitem.RewardedNonConsumable){
			gameObject.SetActive(false);
		}
	}
}
