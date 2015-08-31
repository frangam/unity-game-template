/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBasePurchaseButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	protected string PPKEY;

	[SerializeField]
	private Image spLock;
	
	[SerializeField]
	private Image pnlPrice;
	
	[SerializeField]
	private Text lbMoneyPrice;
	
	[SerializeField]
	private Text lbGemsPrice;

	[SerializeField]
	private UIBaseShopListWindow window;

	[SerializeField]
	private UIBasePurchasableListItem item;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool locked = false;
	private bool canPurchase = false;

	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public UIBaseShopListWindow Window {
		get {
			return this.window;
		}
		set {
			window = value;
		}
	}

	public UIBasePurchasableListItem Item {
		get {
			return this.item;
		}
		set {
			item = value;
		}
	}

	public bool Locked {
		get {
			return this.locked;
		}
		set {
			locked = value;
		}
	}

	public bool CanPurchase {
		get {
			return this.canPurchase;
		}
	}


	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		getWindow();
	}

	public override void Update ()
	{
		base.Update ();
		
		if(button != null && button.animator != null){
			button.transition = locked ? UnityEngine.UI.Selectable.Transition.None : UnityEngine.UI.Selectable.Transition.Animation;
		}
	}


	protected override void doPress ()
	{
		base.doPress ();

		if(window != null){
			if(item != null && !string.IsNullOrEmpty(item.Id)){
				window.purchaseItem(item);
			}
			else
				window.purchaseItem();
		}
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	public virtual void SetLocked(){
		locked = item != null && !item.Purchased;
	}

	protected virtual void getWindow(){
		if(window == null)
			window = GetComponentInParent<UIBaseShopListWindow>();
		
		if(window == null)
			Debug.LogError("Not found UIBaseShopListWindow");
	}

	public virtual void showInformation(){
		SetLocked();

		if(item != null){
			if(item.LbName != null)
			item.LbName.text = item.Name;

			if(lbMoneyPrice != null)
				lbMoneyPrice.text = item.Price.ToString();

			if(lbGemsPrice != null)
				lbGemsPrice.text = item.GemsPrice.ToString();
		}

		if(spLock){	
			spLock.gameObject.SetActive(locked);
		}
		
		if(pnlPrice)
			pnlPrice.gameObject.SetActive(locked);

	}
}
