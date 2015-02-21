﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnionAssets.FLE;

public class UIBaseInAppWin : UIBaseShopListWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private List<UIInAppItemIcon> inAppItemIcons;
	
	[SerializeField]
	private List<UIBaseInAppButton> inAppButtons;
	
	[SerializeField]
	private GameObject pnlShopLoading;
	
	[SerializeField]
	private UIBaseWindow pnlNoInternet;
	
	[SerializeField]
	private UIBaseWindow pnlInternetResumed;
	
	[SerializeField]
	private UIBaseWindow pnlInAppServiceNotInited;
	
	[SerializeField]
	private UIBaseWindow restorePurchasesCheckingWin;
	
	[SerializeField]
	private UIBaseWindow restorePurchasesSuccessWin;
	
	[SerializeField]
	private UIBaseWindow restorePurchasesFailedWin;
	
	[SerializeField]
	private bool hideLoadPanelInEditor = true;
	
	[SerializeField]
	private float delayForCloseInAppWinAtStart = 5f;
	
	[SerializeField]
	private float delayForCloseWin = 2.5f;
	
	
	
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private UIBaseInAppItem currentItem;
	private bool productsRetrieved = false;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public UIBaseInAppItem CurrentItem {
		get {
			return this.currentItem;
		}
	}
	
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		productsRetrieved = false;
		
		
		
		inAppButtons = new List<UIBaseInAppButton> (GetComponentsInChildren<UIBaseInAppButton>() as UIBaseInAppButton[]);
		
		if(inAppButtons == null)
			Debug.LogError("Not found any UIBaseInAppButton");
		
		currentItem = null;
		//		InternetChecker.dispatcher.addEventListener(InternetChecker.NO_INTERNET_CONNECTION, OnNoInternetConnection);
		
	}
	
	/// <summary>
	/// We need unsubscribe from this events to avoid to give more rewards that needed
	/// </summary>
	public override void OnDisable ()
	{
		base.OnDisable ();
		destroyEvents();
	}
	
	/// <summary>
	/// We need unsubscribe from this events to avoid to give more rewards that needed
	/// </summary>
	public override void OnDestroy ()
	{
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("UIBaseInAppWin - destroying events");
		
		base.OnDestroy ();
		destroyEvents();
	}
	
	public override void open ()
	{
		base.open ();
		
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("UIBaseInAppWin - Opening InApp window");
		
		CoreIAPManager.dispatcher.addEventListener(CoreIAPManager.NOT_RETRIEVED_PRODUCTS, OnInAppServiceNotInited);
		CoreIAPManager.dispatcher.addEventListener(CoreIAPManager.RETRIEVED_PRODUCTS, OnRetrievedProducts);
		CoreIAPManager.dispatcher.addEventListener(CoreIAPManager.PURCHASE_COMPLETED, OnPurchaseCompleted);
		CoreIAPManager.dispatcher.addEventListener(CoreIAPManager.PURCHASE_FAILED, OnPurchaseFailed);
		CoreIAPManager.dispatcher.addEventListener(CoreIAPManager.DEFERRED_PURCHASE_COMPLETED, OnDestroy);
		CoreIAPManager.dispatcher.addEventListener(CoreIAPManager.RESTORE_PURCHASE_COMPLETED, OnRestorePurchaseError);
		CoreIAPManager.dispatcher.addEventListener(CoreIAPManager.RESTORE_PURCHASE_FAILED, OnRestorePurchaseError);
		
		if(!CoreIAPManager.Instance.IsInited)
			CoreIAPManager.Instance.init();
		
		if(pnlShopLoading){
			bool hide = (hideLoadPanelInEditor && RuntimePlatformUtils.IsEditor());
			pnlShopLoading.SetActive(!hide);
		}
		
		StartCoroutine(waitAtStartForCloseIfNotLoaded());
	}
	
	public override void purchaseItem (UIBaseListItem item){
		if(processingPurchaseWin)
			UIController.Instance.Manager.open(processingPurchaseWin);
		
		currentItem = (UIBaseInAppItem) item;
		CoreIAPManager.Instance.Purchase(item.Id);
	}
	
	public virtual void restorePurchases(){
		#if UNITY_IPHONE
		if(restorePurchasesCheckingWin)
			UIController.Instance.Manager.open(restorePurchasesCheckingWin);
		
		CoreIAPManager.Instance.IOSRestorePurchase();
		#endif
	}
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void destroyEvents(){
		//		InternetChecker.dispatcher.removeEventListener(InternetChecker.NO_INTERNET_CONNECTION, OnNoInternetConnection);
		CoreIAPManager.dispatcher.removeEventListener(CoreIAPManager.NOT_RETRIEVED_PRODUCTS, OnInAppServiceNotInited);
		CoreIAPManager.dispatcher.removeEventListener(CoreIAPManager.RETRIEVED_PRODUCTS, OnRetrievedProducts);
		CoreIAPManager.dispatcher.removeEventListener(CoreIAPManager.PURCHASE_COMPLETED, OnPurchaseCompleted);
		CoreIAPManager.dispatcher.removeEventListener(CoreIAPManager.PURCHASE_FAILED, OnPurchaseFailed);
		CoreIAPManager.dispatcher.removeEventListener(CoreIAPManager.DEFERRED_PURCHASE_COMPLETED, OnDestroy);
		CoreIAPManager.dispatcher.removeEventListener(CoreIAPManager.RESTORE_PURCHASE_COMPLETED, OnRestorePurchaseError);
		CoreIAPManager.dispatcher.removeEventListener(CoreIAPManager.RESTORE_PURCHASE_FAILED, OnRestorePurchaseError);
	}
	
	private IEnumerator waitAtStartForCloseIfNotLoaded(){
		if(GameSettings.Instance.showTestLogs){
			Debug.Log("InApp Inited ? " + CoreIAPManager.Instance.IsInited);
			Debug.Log("InApp num products loaded: " + CoreIAPManager.Instance.NumProducts);
			Debug.Log("InApp num products retrieved: " + productsRetrieved);
		}
		
		if(CoreIAPManager.Instance.IsInited && CoreIAPManager.Instance.NumProducts > 0){
			//Try to retreive products from InApp Handler
			if(!productsRetrieved){
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("Trying to load products from CoreIAPManager");
				#if UNITY_ANDROID
				if(CoreIAPManager.Instance.ProductsGoogle != null){
					if(GameSettings.Instance.showTestLogs)
						Debug.Log("Products loaded in CoreIAPManager, total: " + CoreIAPManager.Instance.ProductsGoogle.Count);
					
					loadProductsReceived(CoreIAPManager.Instance.ProductsGoogle);
				}
				#elif UNITY_IOS
				if(CoreIAPManager.Instance.ProductsIOS != null){
					if(GameSettings.Instance.showTestLogs)
						Debug.Log("Products loaded in CoreIAPManager, total: " + CoreIAPManager.Instance.ProductsIOS.Count);
					
					loadProductsReceived(CoreIAPManager.Instance.ProductsIOS);
				}
				#elif UNITY_WP8
				if(CoreIAPManager.Instance.ProductsWP != null){
					if(GameSettings.Instance.showTestLogs)
						Debug.Log("Products loaded in CoreIAPManager, total: " + CoreIAPManager.Instance.ProductsWP.Count);
					
					loadProductsReceived(CoreIAPManager.Instance.ProductsWP);
				}
				#endif
				
				
				//if we not have retrieved any product
				if(!productsRetrieved){
					yield return new WaitForSeconds(delayForCloseInAppWinAtStart);
					
					if(!CoreIAPManager.Instance.IsInited || CoreIAPManager.Instance.NumProducts < 0 || !productsRetrieved){
						closeWinWhenErrorAtInit();
					}
				}
				else{
					if(pnlShopLoading)
						pnlShopLoading.SetActive(false);
					
					yield return null;
				}
			} //end_if_!productsRetrieved
			else{
				if(pnlShopLoading)
					pnlShopLoading.SetActive(false);
				
				yield return null;
			}
		}
		else{
			
			yield return new WaitForSeconds(delayForCloseInAppWinAtStart);
			
			if(!CoreIAPManager.Instance.IsInited || CoreIAPManager.Instance.NumProducts < 0 || !productsRetrieved){
				closeWinWhenErrorAtInit();
			}
		}
	}
	
	private void closeWinWhenErrorAtInit(){
		if(pnlInAppServiceNotInited)
			UIController.Instance.Manager.open(pnlInAppServiceNotInited);
		
		UIController.Instance.Manager.waitForClose(pnlInAppServiceNotInited, delayForCloseWin);
		UIController.Instance.Manager.waitForClose(this, delayForCloseWin);
	}
	
	
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	public void OnResumedInternetConnection(CEvent e){
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("Internet connection retrieved");
		
		if(pnlInternetResumed){
			UIController.Instance.Manager.open(pnlInternetResumed);
		}
	}
	
	public void OnNoInternetConnection(CEvent e){
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("No Internet connection");
		
		if(pnlNoInternet){
			UIController.Instance.Manager.open(pnlNoInternet);
		}
		
		UIController.Instance.Manager.waitForClose(pnlNoInternet, delayForCloseWin);
		UIController.Instance.Manager.waitForClose(this, delayForCloseWin);
		
	}
	
	public void OnInAppServiceNotInited(CEvent e){
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("In App Service not inited");
		
		closeWinWhenErrorAtInit();
	}
	
	public void OnRetrievedProducts(CEvent e){
		//		loadProductsReceived(e.data);
		//#if UNITY_ANDROID
		//		List<GoogleProductTemplate> products = e.data as List<GoogleProductTemplate>;
		//		if(products != null && products.Count > 0 && inAppButtons != null){
		//			if(GameSettings.Instance.showTestLogs)
		//				Debug.Log("In App Products retrieved");
		//
		//			foreach(GoogleProductTemplate p in products){
		//				if(GameSettings.Instance.showTestLogs)
		//					Debug.Log("product [id: " + p.SKU + ", title: " + p.title + ", desc: " + p.description + ", price: " + p.price + ", priceAmountMicros: " + p.priceAmountMicros
		//					          + ", price code: " + p.priceCurrencyCode + "]");
		//
		//				//show product price on the button
		//				foreach(UIBaseInAppButton button in inAppButtons){
		//					if(p.SKU.Equals(button.Item.Id)){
		//						button.showPriceInfo(p.price, p.priceCurrencyCode);
		//						break;
		//					}
		//				}
		//			}
		//			productsRetrieved = true;
		//
		//			if(pnlShopLoading){
		//				pnlShopLoading.SetActive(false);
		//			}
		//		}
		//		else{
		//			if(GameSettings.Instance.showTestLogs)
		//				Debug.Log("In App Products: no products retrieved");
		//			closeWinWhenErrorAtInit();
		//		}
		//#elif UNITY_IPHONE
		//		List<IOSProductTemplate> products = e.data as List<IOSProductTemplate>;
		//		if(products != null && products.Count > 0 && inAppButtons != null){
		//			if(GameSettings.Instance.showTestLogs)
		//				Debug.Log("In App Products retrieved");
		//			
		//			foreach(IOSProductTemplate p in products){
		//				if(GameSettings.Instance.showTestLogs)
		//					Debug.Log("product [id: " + p.id + ", title: " + p.title + ", desc: " + p.description + ", price: " + p.price + ", localizedPrice: " + p.localizedPrice
		//					          + ", currency symbol" + p.currencySymbol + ", price code: " + p.currencyCode + "]");
		//				
		//				//show product price on the button
		//				foreach(UIBaseInAppButton button in inAppButtons){
		//					if(p.id.Equals(button.Item.Id)){
		//						button.showPriceInfo(p.localizedPrice);
		//						break;
		//					}
		//				}
		//			}
		//			productsRetrieved = true;
		//
		//			if(pnlShopLoading){
		//				pnlShopLoading.SetActive(false);
		//			}
		//		}
		//		else{
		//			if(GameSettings.Instance.showTestLogs)
		//				Debug.Log("In App Products: no products retrieved");
		//			closeWinWhenErrorAtInit();
		//		}
		//#elif WP8
		//		List<WP8ProductTemplate> products = e.data as List<WP8ProductTemplate>;
		//		if(products != null && products.Count > 0 && inAppButtons != null){
		//			if(GameSettings.Instance.showTestLogs)
		//				Debug.Log("In App Products retrieved");
		//			
		//			foreach(WP8ProductTemplate p in products){
		//				if(GameSettings.Instance.showTestLogs)
		//					Debug.Log("product [id: " + p.ProductId + ", title: " + p.Name + ", desc: " + p.Description + ", price: " + p.Price);
		//				
		//				//show product price on the button
		//				foreach(UIBaseInAppButton button in inAppButtons){
		//					if(p.ProductId.Equals(button.Item.Id)){
		//						button.showPriceInfo(p.Price);
		//						break;
		//					}
		//				}
		//			}
		//			productsRetrieved = true;
		//
		//			if(pnlShopLoading){
		//				pnlShopLoading.SetActive(false);
		//			}
		//		}
		//		else{
		//			if(GameSettings.Instance.showTestLogs)
		//				Debug.Log("In App Products: no products retrieved");
		//			closeWinWhenErrorAtInit();
		//		}
		//#endif
	}
	
	public void loadProductsReceived(object productsData){
		#if UNITY_ANDROID
		List<GoogleProductTemplate> products = productsData as List<GoogleProductTemplate>;
		if(products != null && products.Count > 0 && inAppButtons != null){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("In App Products retrieved");
			
			foreach(GoogleProductTemplate p in products){
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("product [id: " + p.SKU + ", title: " + p.title + ", desc: " + p.description + ", price: " + p.price + ", priceAmountMicros: " + p.priceAmountMicros
					          + ", price code: " + p.priceCurrencyCode + "]");
				
				//show product price on the button
				foreach(UIBaseInAppButton button in inAppButtons){
					if(p.SKU.Equals(button.Item.Id)){
						button.showPriceInfo(p.price, p.priceCurrencyCode);
						break;
					}
				}
			}
			productsRetrieved = true;
			
			if(pnlShopLoading){
				pnlShopLoading.SetActive(false);
			}
		}
		else{
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("In App Products: no products retrieved");
			closeWinWhenErrorAtInit();
		}
		#elif UNITY_IPHONE
		List<IOSProductTemplate> products = productsData as List<IOSProductTemplate>;
		if(products != null && products.Count > 0 && inAppButtons != null){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("In App Products retrieved");
			
			foreach(IOSProductTemplate p in products){
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("product [id: " + p.id + ", title: " + p.title + ", desc: " + p.description + ", price: " + p.price + ", localizedPrice: " + p.localizedPrice
					          + ", currency symbol" + p.currencySymbol + ", price code: " + p.currencyCode + "]");
				
				//show product price on the button
				foreach(UIBaseInAppButton button in inAppButtons){
					if(p.id.Equals(button.Item.Id)){
						button.showPriceInfo(p.localizedPrice);
						break;
					}
				}
			}
			productsRetrieved = true;
			
			if(pnlShopLoading){
				pnlShopLoading.SetActive(false);
			}
		}
		else{
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("In App Products: no products retrieved");
			closeWinWhenErrorAtInit();
		}
		#elif WP8
		List<WP8ProductTemplate> products = productsData as List<WP8ProductTemplate>;
		if(products != null && products.Count > 0 && inAppButtons != null){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("In App Products retrieved");
			
			foreach(WP8ProductTemplate p in products){
				if(GameSettings.Instance.showTestLogs)
					Debug.Log("product [id: " + p.ProductId + ", title: " + p.Name + ", desc: " + p.Description + ", price: " + p.Price);
				
				//show product price on the button
				foreach(UIBaseInAppButton button in inAppButtons){
					if(p.ProductId.Equals(button.Item.Id)){
						button.showPriceInfo(p.Price);
						break;
					}
				}
			}
			productsRetrieved = true;
			
			if(pnlShopLoading){
				pnlShopLoading.SetActive(false);
			}
		}
		else{
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("In App Products: no products retrieved");
			closeWinWhenErrorAtInit();
		}
		#endif
	}
	
	public void OnPurchaseCompleted(CEvent e){
		string id = e.data as string;
		
		if(currentItem != null && !string.IsNullOrEmpty(id) && currentItem.Id.Equals(id)){
			if(processingPurchaseWin)
				UIController.Instance.Manager.close(processingPurchaseWin);
			
			if(deferredPurchaseWin)
				UIController.Instance.Manager.close(deferredPurchaseWin);
			
			if(succesedPurchaseWin)
				UIController.Instance.Manager.open(succesedPurchaseWin);
			
			currentItem.applyReward();
		}
		else
			Debug.LogError("InApp OnPurchaseCompleted item id [" +id+ "] not correspond to the current item selected id [" +currentItem.Id+ "]");
	}
	
	public void OnPurchaseFailed(CEvent e){
		if(processingPurchaseWin)
			UIController.Instance.Manager.close(processingPurchaseWin);
		
		if(deferredPurchaseWin)
			UIController.Instance.Manager.close(deferredPurchaseWin);
		
		if(failedPurchaseWin)
			UIController.Instance.Manager.open(failedPurchaseWin);
	}
	
	
	public void OnRestorePurchaseError(CEvent e){
		if(restorePurchasesCheckingWin)
			UIController.Instance.Manager.close(restorePurchasesCheckingWin);
		
		if(restorePurchasesFailedWin)
			UIController.Instance.Manager.open(restorePurchasesFailedWin);
	}
	
	public void OnRestorePurchaseSuccess(CEvent e){
		if(restorePurchasesCheckingWin)
			UIController.Instance.Manager.close(restorePurchasesCheckingWin);
		
		if(restorePurchasesSuccessWin)
			UIController.Instance.Manager.open(restorePurchasesSuccessWin);
	}
	
	public void OnPurchaseDeffered(CEvent e){
		if(processingPurchaseWin)
			UIController.Instance.Manager.close(processingPurchaseWin);
		
		if(deferredPurchaseWin)
			UIController.Instance.Manager.open(deferredPurchaseWin);
	}
}
