using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAPManager : Singleton<IAPManager> {

	private List<Product> _products;
	private bool _isInited;

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
	public static void Consume(string SKU)
	{
		AndroidInAppPurchaseManager.instance.consume(SKU);
	}
	public static void Purchase(string SKU)
	{
		AndroidInAppPurchaseManager.instance.purchase(SKU);
	}
	#endregion
	#region Init
	public void Init()
	{
	//1. Fill the list of purchase into the AndroidInAppPurchaseManager products list.
		FillProducts();
	//2. Subscription to the important events.
		//When product is bought.
		AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased; 
		//When product is consumed.
		AndroidInAppPurchaseManager.ActionProductConsumed  += OnProductConsumed;
		//When shop is inicialized.
		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
	//3. Request of the shop starup with our Auth key 
		//poner asi el codigo no es seguro, mejor hacerlo por composicion de string o ponerlo en el editor.
		AndroidInAppPurchaseManager.instance.loadStore("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAg2JDbWjQAGouiZ6UW0RHYZ7s0E0o/Qml8iznLugMkf3tQs/LfXiQ0HxYE++0DGz29mm04/r443C/zaxD9N20yleH4cjQyFF42dGXtJtnOuwXEPc8ZyaHGeWX4xLvAXa7Qoe/nqo7EWVlhPF79GofFA1DcvaCSMTAJnH/meBA37UMvEB3J38dcyon+G4gl/NNyAOSOyL1MMfg/HwlN6owR80brTUAfmiQauCZJaTTXLrN83N2oiG+47fQnEs+fxIsLnRRM6pU+8Gw/fqMk0LIjussjDokBkU6AcKkQA4n/PWWGGmKXUeToFAaatmNmPISAdn5gsgQ/ewX5UY+wOG6rQIDAQAB");

		Debug.Log("IAPManger Init finish");
	}
	#endregion
	//When product is bought.
	#region OnProductPurchased
	void OnProductPurchased (BillingResult result)
	{
		//if all goes right...
		if(result.isSuccess)
		{
			//este metodo se encargara de activar la compra.
			OnProcessingPurchasedProduct (result.purchase);
		} 
	}
	//TODO Abstraer este metodo para no tocar aqui.
	void OnProcessingPurchasedProduct (GooglePurchaseTemplate purchase)
	{
		switch(purchase.SKU) {
		case "100_coins":
			//Consumimos nuestra compra que llamara al evento "OnProductConsumed"
			Consume("100_coins");
			break;
//		case BONUS_TRACK:
//			GameData.UnlockBonusTrack();
//			break;
		}
	}
	#endregion
	//When product is consumed.
	#region OnProductConsumed
	void OnProductConsumed (BillingResult result)
	{
		if(result.isSuccess) {
			OnProcessingConsumeProduct (result.purchase);
		}
	}

	//TODO Abstraer este metodo para no tocar aqui. 
	void OnProcessingConsumeProduct (GooglePurchaseTemplate purchase)
	{
		switch(purchase.SKU) {
		case "100_coins":
			//Le damos al jugador su producto.
			//TODO Buscar una manera de abstraerse de esta clase.
			//GameDataExample.AddCoins(100);
			AndroidMessage.Create("Añadido al jugador", "100 Monedas de oro");
			break;
		}
	}
	#endregion
	//Cuando nos conectamos a la tienda
	#region OnBillingConnected
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
			//TODO metodo para comprobar si ya tiene le objeto ese comprado.
		}
	}
	#endregion
	private void FillProducts()
	{
		Products.ForEach(x => AndroidInAppPurchaseManager.instance.addProduct(x.SKU));
		Debug.Log("Added :"+Products.Count+" to InApp Purchase");
	}
}
[System.Serializable]
public struct Product
{
	public string SKU;
	public Texture2D texture;
}

