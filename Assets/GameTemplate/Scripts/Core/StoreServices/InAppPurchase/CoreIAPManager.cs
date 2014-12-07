using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoreIAPManager : Singleton<CoreIAPManager>{

	private List<Product> _products;
	private bool _isInited;
	public string AndroidCode;

	#region Getters & Setters
	public List<Product> Products
	{ 
		get { return _products; }
		set { _products = value; }
	}
	public bool IsInited { 
		get { return _isInited; }
		private set { _isInited = value; }
	}
	public void Consume(string SKU)
	{
		#if UNITY_ANDROID
			AndroidInAppPurchaseManager.instance.consume(SKU);
		#endif
		#if UNITY_IPHONE

		#endif
		#if UNITY_WP8

		#endif
	}
	public void Purchase(string SKU)
	{
		#if UNITY_ANDROID
			AndroidInAppPurchaseManager.instance.purchase(SKU);
		#endif
		#if UNITY_IPHONE
			IOSInAppPurchaseManager.instance.buyProduct(SKU);
		#endif
		#if UNITY_WP8
		
		#endif
	}
	public void IOSRestorePurchase()
	{
		if(IOSInAppPurchaseManager.instance.IsInAppPurchasesEnabled)
		{
			IOSInAppPurchaseManager.instance.restorePurchases();
		}

	}
	#endregion

	#region Init
	public void Init(List<Product> products)
	{
		FillProducts(products);//1. Fill the list of purchase into the AndroidInAppPurchaseManager products list.
	#if UNITY_ANDROID
		//2. Subscription to the important events.
		//When product is bought.
		AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased; 
		//When product is consumed.
		AndroidInAppPurchaseManager.ActionProductConsumed  += OnProductConsumed;
		//When shop is inicialized.
		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
		//3. Request of the shop starup with our Auth key 
		//poner asi el codigo no es seguro, mejor hacerlo por composicion de string o ponerlo en el editor.
		AndroidInAppPurchaseManager.instance.loadStore(AndroidCode);
	#endif
	#if UNITY_IPHONE
		IOSInAppPurchaseManager.instance.OnStoreKitInitComplete += OnStoreKitInitComplete;
		IOSInAppPurchaseManager.instance.OnTransactionComplete += OnTransactionComplete;
		IOSInAppPurchaseManager.instance.OnVerificationComplete += OnVerificationComplete;
		IOSInAppPurchaseManager.instance.OnRestoreComplete += OnRestoreComplete;
		IOSInAppPurchaseManager.instance.loadStore();
	#endif
	#if UNITY_WP8
	
	#endif
		Debug.Log("IAPManger Init finish");
	}
	#endregion

	#region AndroidEvents
	void OnProductPurchased (BillingResult result)
	{
		//if all goes right...
		if(result.isSuccess)
		{
			//este metodo se encargara de activar la compra.
			OnProcessingPurchasedProduct (result.purchase.SKU);
		} 
	}
	void OnProductConsumed (BillingResult result)
	{
		if(result.isSuccess) {
			OnProcessingConsumeProduct (result.purchase.SKU);
		}
	}
	void OnBillingConnected (BillingResult result)
	{
		//Como ya se ha iniciado la tienda no nos hace falta estar suscritos al evento que nos
		//avise cuando se inicia la tienda.
		AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;
		//si todo fue bien...
		if(result.isSuccess) {
			//ahora que estamos conectados en la tienda podemos pedir la lista de productos.
			AndroidInAppPurchaseManager.instance.retrieveProducDetails();
			//nos vamos a suscribir a este evento que nos avisara cuando este cargada.
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetriveProductsFinised;
		} 
	}
	void OnRetriveProductsFinised (BillingResult result)
	{
		//igual que antes ya no nos hace falta estar suscritos al evento.
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetriveProductsFinised;
		//si todo fue bien...
		if(result.isSuccess) {
			//cambiamos la bandera para avisar que la tienda esta lista
			IsInited = true;
			//es buena practica comprobar que todos los productos adquiridos esta consumidos (por ejemplo pudimos
			//comprar un prodcutos pero se nos salio del movil y no lo consumimos, o iniciamos desde otro dispositivo)
			UpdateProducts();
		}
	}
	#endregion

	#region IOSEvents
	void OnStoreKitInitComplete (ISN_Result result)
	{
		IOSInAppPurchaseManager.instance.OnStoreKitInitComplete -= OnStoreKitInitComplete;
		
		if(result.IsSucceeded) {
			Debug.Log("Inited successfully, Avaliable products cound: " + IOSInAppPurchaseManager.instance.products.Count.ToString());
			IsInited = true;
		} else {
			Debug.Log("Failed in init Stoke Kit :(");
		}
	}
	void OnTransactionComplete (IOSStoreKitResponse responce)
	{
		Debug.Log("OnTransactionComplete: " + responce.productIdentifier);
		Debug.Log("OnTransactionComplete: state: " + responce.state);
		
		switch(responce.state) {
		case InAppPurchaseState.Purchased:
		case InAppPurchaseState.Restored:
			//Our product been succsesly purchased or restored
			//So we need to provide content to our user 
			//depends on productIdentifier
			OnProcessingConsumeProduct(responce.productIdentifier);
			//Warning: Use 
			//SANDBOX_VERIFICATION_SERVER url (https://sandbox.itunes.apple.com/verifyReceipt) during app testing  and 
			//APPLE_VERIFICATION_SERVER url  (https://buy.itunes.apple.com/verifyReceipt) on production.
			IOSInAppPurchaseManager.instance.verifyLastPurchase(IOSInAppPurchaseManager.SANDBOX_VERIFICATION_SERVER);
			break;
		case InAppPurchaseState.Deferred:
			//iOS 8 introduces Ask to Buy, which lets 
			//parents approve any purchases initiated by children
			//You should update your UI to reflect this 
			//deferred state, and expect another Transaction 
			//Complete  to be called again with a new transaction state 
			//reflecting the parent's decision or after the 
			//transaction times out. Avoid blocking your UI 
			//or gameplay while waiting for the transaction to be updated.
			break;
		case InAppPurchaseState.Failed:
			//Our purchase flow is failed.
			//We can unlock interface and report user that the purchase is failed. 
			Debug.Log("Transaction failed with error, code: " + responce.error.code);
			Debug.Log("Transaction failed with error, description: " + responce.error.description);
			break;
		}
		
		IOSNativePopUpManager.showMessage("Store Kit Response", "product " + responce.productIdentifier + " state: " + responce.state.ToString());
	}
	void OnVerificationComplete (IOSStoreKitVerificationResponse responce)
	{
		IOSNativePopUpManager.showMessage("Verification", "Transaction verification status: " + responce.status.ToString());
	}
	void OnRestoreComplete (ISN_Result responce)
	{
		if(responce.IsSucceeded)
		{
			IOSNativePopUpManager.showMessage("Restore Purchase", "All purchases has been restored.");
		}
		else
		{
			IOSNativePopUpManager.showMessage("Restore Purchase", "Restore failed: "+ responce.error.description);
		}
	}
	#endregion

	#region WP8Events
	#endregion

	private void FillProducts(List<Product> products)
	{
	#if UNITY_ANDROID
		products.ForEach(x => AndroidInAppPurchaseManager.instance.addProduct(x.AndroidSKU));
	#endif
	#if UNITY_IPHONE
		products.ForEach(x => IOSInAppPurchaseManager.instance.addProductId(x.IOS_SKU));
	#endif
	#if UNITY_WP8
	
	#endif
	}
	#region Virtual
	public virtual void OnProcessingPurchasedProduct (string SKU)
	{
//		switch(SKU) {
//		case "100_coins":
//			//Consumimos nuestra compra que llamara al evento "OnProductConsumed"
//			Consume("100_coins");
//			break;
//			//		case BONUS_TRACK:
//			//			GameData.UnlockBonusTrack();
//			//			break;
//		}
	}
	public virtual void OnProcessingConsumeProduct (string SKU)
	{
//		switch(SKU) {
//		case "100_coins":
//			//Le damos al jugador su producto.
//			//GameDataExample.AddCoins(100);
//			AndroidMessage.Create("Añadido al jugador", "100 Monedas de oro");
//			break;
//		}
	}
	public virtual void UpdateProducts(){}

	#endregion
}
[System.Serializable]
public struct Product
{
	public string AndroidSKU;
	public string IOS_SKU;
	public string WP8_SKU;
	public Texture2D texture;
}