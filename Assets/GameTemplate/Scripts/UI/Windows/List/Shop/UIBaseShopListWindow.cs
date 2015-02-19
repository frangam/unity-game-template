using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIBaseShopListWindow : UIBaseListWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	protected string							PP_KEY = UIBaseListItem.PP_ITEM_LIST;
	
	[SerializeField]
	private Text 								lbPrice;
	
	[SerializeField]
	private Text 								lbGemsPrice;
	
	[SerializeField]
	private Text 								lbTotalMoney;
	
	[SerializeField]
	private Text 								lbTotalGems;
	
	[SerializeField]
	private Image 								imgLock;
	
	[SerializeField]
	private Button 								btnSelect;
	
	[SerializeField]
	private Button 								btnPurchase;
	
	[SerializeField]
	private Button 								btnUpgrade;
	
	[SerializeField]
	private Transform 							pnlItemPrice;
	
	[SerializeField]
	[Tooltip("True if we will use gems to purchase when user has not enough money")]
	private bool 								useGemsIfNotEnoughMoney = true;
	
	[SerializeField]
	protected UIBaseWindow						processingPurchaseWin;
	
	[SerializeField]
	protected UIBaseWindow						succesedPurchaseWin;
	
	[SerializeField]
	protected UIBaseWindow						deferredPurchaseWin;
	
	[SerializeField]
	protected UIBaseWindow						failedPurchaseWin;
	
	[SerializeField]
	protected UIBaseConfimationPurchaseWindow 	confirmationPurchaseWin;
	
	[SerializeField]
	protected UIBaseWindow						addMoreGemsWin;
	
	[SerializeField]
	protected List<UIBasePurchaseButton>		purchaseButtons;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		
		base.Awake ();
		
		if(purchaseButtons == null || (purchaseButtons != null && purchaseButtons.Count == 0)){
			UIBasePurchaseButton[] bs = GetComponentsInChildren<UIBasePurchaseButton>() as UIBasePurchaseButton[];
			
			if(bs != null && bs.Length > 0)
				purchaseButtons = new List<UIBasePurchaseButton>(bs);
		}
		
		if(lbTotalMoney)
			lbTotalMoney.text = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_MONEY).ToString();
		
		if(lbTotalGems)
			lbTotalGems.text = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_GEMS).ToString();
	}
	
	public override UIBaseListItem createListItem (string data)
	{
		return new UIBasePurchasableListItem(PP_KEY, data);
	}
	
	public override void showItem (UIBaseListItem item){
		base.showItem (item);
		UIBasePurchasableListItem pItem = (UIBasePurchasableListItem) item;
		
		if(pItem != null){
			//show info of each buttons if they exist
			if(purchaseButtons != null && purchaseButtons.Count > 0){
				foreach(UIBasePurchaseButton b in purchaseButtons){
					if(b.Item != null && b.Item.Id.Equals(pItem.Id)){
						b.showInformation();
						break;
					}
				}
			}
			
			if(imgLock)
				imgLock.gameObject.SetActive(!pItem.Purchased);
			
			if(btnPurchase)
				btnPurchase.gameObject.SetActive(!pItem.Purchased);
			
			if(pnlItemPrice)
				pnlItemPrice.gameObject.SetActive(!pItem.Purchased);
			
			if(btnUpgrade){
				UIBaseUpgradableStatListItem uItem = (UIBaseUpgradableStatListItem) pItem;
				btnUpgrade.gameObject.SetActive(uItem != null && uItem.Purchased && !uItem.allUpgradesCompleted());
			}
			
			if(btnSelect)
				btnSelect.gameObject.SetActive(pItem.Purchased);
			
			if(lbPrice)
				lbPrice.text = pItem.Price.ToString();
			
			if(lbGemsPrice)
				lbGemsPrice.text = pItem.GemsPrice.ToString();
		}
	}
	
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void purchaseItem(UIBaseListItem item = null){
		UIBasePurchasableListItem pItem = item == null ? (UIBasePurchasableListItem) currentItemSelected : (UIBasePurchasableListItem) item;
		
		if(pItem != null && pItem.Purchased){
			pItem.select();
		}
		else if(pItem != null && !pItem.Purchased){
			//open confirmation purchase window
			if(GameMoneyManager.Instance.hasEnoughMoney(pItem) || (useGemsIfNotEnoughMoney && GameMoneyManager.Instance.hasEnoughGems(pItem))){
				confirmationPurchaseWin.init(pItem, GameMoneyManager.Instance.hasEnoughMoney(pItem), GameMoneyManager.Instance.hasEnoughGems(pItem)); //first init the window
				UIController.Instance.Manager.open(confirmationPurchaseWin);
			}
			//open add more gems window
			else if(!GameMoneyManager.Instance.hasEnoughMoney(pItem) && (useGemsIfNotEnoughMoney && !GameMoneyManager.Instance.hasEnoughGems(pItem))){
				UIController.Instance.Manager.open(addMoreGemsWin);
			}
		}
		else if(pItem == null){
			Debug.LogError("Not Selected item to purchase");
		}
	}
	
	public virtual void itemPurchased(UIBasePurchasableListItem item){
		showItem(item);
	}
	
	
}
