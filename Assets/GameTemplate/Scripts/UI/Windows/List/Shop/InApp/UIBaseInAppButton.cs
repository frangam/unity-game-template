using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
	public virtual  void Awake ()
	{
		base.Awake ();
		Item.load();
		showInformation();
		
		getWindow();
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
			Debug.LogError("Not found UIBaseInAppWin");
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
}
