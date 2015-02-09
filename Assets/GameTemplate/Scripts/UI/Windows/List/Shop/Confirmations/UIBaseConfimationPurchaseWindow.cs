using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBaseConfimationPurchaseWindow : UIConfirmationDialog {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Transform 		pnlMoney;

	[SerializeField]
	private Text 			lbMoney;

	[SerializeField]
	private Transform 		pnlGems;

	[SerializeField]
	private Text 			lbGems;

	[SerializeField]
	private UIBaseWindow	successPurchaseWin;

	[SerializeField]
	[Tooltip("Window to notify item was purchased successfully")]
	private UIBaseShopListWindow	winToNotifyItemPurchased;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool			hasEnoughMoney;
	private bool			hasEnoughGems;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void open ()
	{
		base.open ();

		if(hasEnoughMoney){
			pnlMoney.gameObject.SetActive(true);
			pnlGems.gameObject.SetActive(false);
		}
		else if(hasEnoughGems){
			pnlMoney.gameObject.SetActive(false);
			pnlGems.gameObject.SetActive(true);
		}
		else{
			pnlMoney.gameObject.SetActive(false);
			pnlGems.gameObject.SetActive(false);
		}
	}

	public override void confirm ()
	{
		confirmPurchase();
		base.confirm ();
	}

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private UIBasePurchasableListItem purchableItem;

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void init(int moneyPrice, bool pHasEnoughMoney, bool pHasEnoughGems){
		hasEnoughMoney = pHasEnoughMoney;
		hasEnoughGems = pHasEnoughGems;

		if(lbMoney && hasEnoughMoney){
			lbMoney.text = moneyPrice.ToString();
			pnlGems.gameObject.SetActive(false);
		}
		if(!hasEnoughMoney && hasEnoughGems && lbGems){
			lbGems.text = GameMoneyManager.Instance.getEquivalentGemsInCoins(moneyPrice).ToString();
			pnlMoney.gameObject.SetActive(false);
		}
	}

	public virtual void init(UIBasePurchasableListItem pItem, bool pHasEnoughMoney, bool pHasEnoughGems){
		purchableItem = pItem;
		hasEnoughMoney = pHasEnoughMoney;
		hasEnoughGems = pHasEnoughGems;

		if(lbMoney && hasEnoughMoney){
			lbMoney.text = purchableItem.Price.ToString();
			pnlGems.gameObject.SetActive(false);
		}
		if(!hasEnoughMoney && hasEnoughGems && lbGems){
			lbGems.text = purchableItem.GemsPrice.ToString();
			pnlMoney.gameObject.SetActive(false);
		}
	}



	public virtual void confirmPurchase(){
		if((hasEnoughMoney && purchableItem.purchaseWithMoney() && successPurchaseWin)
		   || (hasEnoughGems && purchableItem.purchaseWithGems() && successPurchaseWin)){
			//notify to the window this item was purchased
			if(winToNotifyItemPurchased)
				winToNotifyItemPurchased.itemPurchased(purchableItem);

			UIController.Instance.Manager.open(successPurchaseWin);
		}

		base.confirm();
	}
}
