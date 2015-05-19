using UnityEngine;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;
using UnionAssets.FLE;
using GameAnalyticsSDK;

public class CoreIAPManager : PersistentSingleton<CoreIAPManager>{
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string NOT_RETRIEVED_PRODUCTS = "gt_inapp_not_retrieved_product";
	public const string RETRIEVED_PRODUCTS = "gt_inapp_retrieved_product";
	public const string PURCHASE_COMPLETED = "gt_inapp_purchase_completed";
	public const string PURCHASE_FAILED = "gt_inapp_purchase_failed";
	public const string DEFERRED_PURCHASE_COMPLETED = "gt_inapp_deferred_purchase_completed";
	public const string RESTORE_PURCHASE_COMPLETED = "gt_inapp_restore_purchase_completed";
	public const string RESTORE_PURCHASE_FAILED = "gt_inapp_restore_purchase_failed";
	
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	private static EventDispatcherBase _dispatcher  = new EventDispatcherBase ();
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	[Tooltip("Text file from load the in app products")]
	private string textFile = "InAppItems";
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private List<UIBaseInAppItem> _products;
	private bool _isInited;
	private int numProducts = 0;
	
	private List<GoogleProductTemplate> productsGoogle = new List<GoogleProductTemplate>();
	private List<IOSProductTemplate> productsIOS = new List<IOSProductTemplate>();
	private List<WP8ProductTemplate> productsWP = new List<WP8ProductTemplate>();
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	#region Getters & Setters
	public static EventDispatcherBase dispatcher {
		get {
			return _dispatcher;
		}
	}
	public List<UIBaseInAppItem> Products{ 
		get { return _products; }
		set { _products = value; }
	}
	public bool IsInited { 
		get { return _isInited; }
		private set { _isInited = value; }
	}
	
	public int NumProducts {
		get {
			return this.numProducts;
		}
	}
	
	public List<GoogleProductTemplate> ProductsGoogle {
		get {
			return this.productsGoogle;
		}
	}
	
	public List<IOSProductTemplate> ProductsIOS {
		get {
			return this.productsIOS;
		}
	}
	
	public List<WP8ProductTemplate> ProductsWP {
		get {
			return this.productsWP;
		}
	}
	#endregion
	
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	protected override void Awake(){
		if(GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE){
			base.Awake();
			init();
		}
		else
			Destroy(Instance.gameObject);
	}
	
	public virtual void Start(){
		
	}
	
	public virtual void Update(){
		
	}
	
	//	public void OnEnable(){
	//		if(_isInited){
	//			#if UNITY_ANDROID
	//			AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased; 
	//			AndroidInAppPurchaseManager.ActionProductConsumed  += OnProductConsumed;
	//			AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
	//			#elif UNITY_IPHONE
	//			IOSInAppPurchaseManager.instance.OnStoreKitInitComplete += OnStoreKitInitComplete;
	//			IOSInAppPurchaseManager.instance.OnTransactionComplete += OnTransactionComplete;
	//			IOSInAppPurchaseManager.instance.OnVerificationComplete += OnVerificationComplete;
	//			IOSInAppPurchaseManager.instance.OnRestoreComplete += OnRestoreComplete;
	//			#elif WP8
	//			WP8InAppPurchasesManager.instance.addEventListener(WP8InAppPurchasesManager.INITIALIZED, OnInitComplete);
	//			WP8InAppPurchasesManager.instance.addEventListener(WP8InAppPurchasesManager.PRODUCT_PURCHASE_FINISHED, OnPurchaseFinished);
	//			#endif
	//		}
	//	}
	
	/// <summary>
	/// We need unsubscribe from this events to avoid to give more rewards that needed
	/// </summary>
	public virtual void OnDestroy(){
		if(_isInited){
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("CoreIAPManager - destroying events");
			
			#if UNITY_ANDROID
			AndroidInAppPurchaseManager.ActionProductPurchased -= OnProductPurchased; 
			AndroidInAppPurchaseManager.ActionProductConsumed  -= OnProductConsumed;
			AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;
			#elif UNITY_IPHONE
			IOSInAppPurchaseManager.instance.OnStoreKitInitComplete -= OnStoreKitInitComplete;
			IOSInAppPurchaseManager.instance.OnTransactionComplete -= OnTransactionComplete;
			IOSInAppPurchaseManager.instance.OnVerificationComplete -= OnVerificationComplete;
			IOSInAppPurchaseManager.instance.OnRestoreComplete -= OnRestoreComplete;
			#elif WP8
			WP8InAppPurchasesManager.instance.removeEventListener(WP8InAppPurchasesManager.INITIALIZED, OnInitComplete);
			WP8InAppPurchasesManager.instance.removeEventListener(WP8InAppPurchasesManager.PRODUCT_PURCHASE_FINISHED, OnPurchaseFinished);
			#endif
		}
	}
	#endregion
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// If use purchased products and not were consumed or given the corresponding reward
	/// </summary>
	public void initialCheckingForApplyProductsRewards(){
		#if UNITY_ANDROID		
		if(AndroidInAppPurchaseManager.instance.IsConnectd && _products != null && _products.Count > 0){
			foreach(UIBaseInAppItem product in _products){
				//chisking if we already own some consuamble product but forget to consume those
				//consume the purchase if the inapp product type is consumable
				if(product != null && product.IaType == UIBaseInAppItem.InAppItemType.COMSUMABLE
				   && AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased(product.Id)){
					AndroidInAppPurchaseManager.instance.consume(product.Id);
				}
				
				
				//Check if non-consumable rpduct was purchased, but we do not have local data for it.
				//It can heppens if game was reinstalled or download on oher device
				//This is replacment for restore purchase fnunctionality on IOS
				else if(product != null && product.IaType == UIBaseInAppItem.InAppItemType.NON_CONSUMABLE 
				        && AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased(product.Id)) {
					product.applyReward();
				}
			}
		}
		#elif UNITY_IPHONE
		if(IOSInAppPurchaseManager.instance.IsInAppPurchasesEnabled){
			//			IOSInAppPurchaseManager.instance.restorePurchases();
		}
		#endif
	}
	
	public void Purchase(string SKU){
		#if UNITY_ANDROID
		AndroidInAppPurchaseManager.instance.purchase(SKU);
		#elif UNITY_IPHONE
		IOSInAppPurchaseManager.instance.buyProduct(SKU);
		#elif UNITY_WP8
		WP8InAppPurchasesManager.instance.purchase(SKU);			
		#endif
	}
	public void RestorePurchase(){
		#if UNITY_ANDROID
		bool restore = false;
		
		if(AndroidInAppPurchaseManager.instance.IsConnectd && _products != null && _products.Count > 0){
			foreach(UIBaseInAppItem product in _products){
				//chisking if we already own some consuamble product but forget to consume those
				//consume the purchase if the inapp product type is consumable
				if(product != null && product.IaType == UIBaseInAppItem.InAppItemType.COMSUMABLE
				   && AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased(product.Id)){
					AndroidInAppPurchaseManager.instance.consume(product.Id);
				}
				
				
				//Check if non-consumable rpduct was purchased, but we do not have local data for it.
				//It can heppens if game was reinstalled or download on oher device
				//This is replacment for restore purchase fnunctionality on IOS
				else if(product != null && product.IaType == UIBaseInAppItem.InAppItemType.NON_CONSUMABLE 
				        && AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased(product.Id)) {
					restore = true;
				}
			}
			
			//if it needed to apply restoring products
			if(restore)
				OnProcessingRestorePurchases();
		}
		#elif UNITY_IPHONE
		if(IOSInAppPurchaseManager.instance.IsInAppPurchasesEnabled){
			IOSInAppPurchaseManager.instance.restorePurchases();
		}
		#endif
	}
	
	
	#region Init
	public void init(){
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("CoreIAPManager - initializing");
		
		if(GameSettings.Instance.inAppBillingIDS != null && GameSettings.Instance.inAppBillingIDS.Count > 0)
			Init(GameSettings.Instance.inAppBillingIDS);
		else
			Debug.LogError("Not found any In App Billing ID");
	}
	/// <summary>
	/// Init loading productIDs that are the same in all platforms
	/// </summary>
	/// <param name="productIDs">Product I ds.</param>
	public void Init(List<string> productIDs){
		_products = new List<UIBaseInAppItem>();
		
		//add products
		foreach(string id in productIDs){
			UIBaseInAppItem item = new UIBaseInAppItem(getInAppFileContent(id));
			
			if(item != null && !_products.Contains(item))
				_products.Add(item);
		}
		
		if(_products != null && _products.Count > 0){
			FillProducts(productIDs);
			endInit();
		}
	}
	
	
	private void endInit(){
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("CoreIAPManager - Loading Store products ids");
		
		#if UNITY_ANDROID
		//2. Subscription to the important events.
		//When product is bought.
		AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased; 
		//When product is consumed.
		AndroidInAppPurchaseManager.ActionProductConsumed  += OnProductConsumed;
		//When shop is inicialized.
		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
		//3. Request of the shop starup with our Auth key 
		AndroidInAppPurchaseManager.instance.loadStore();
		
		#elif UNITY_IPHONE
		IOSInAppPurchaseManager.instance.OnStoreKitInitComplete += OnStoreKitInitComplete;
		IOSInAppPurchaseManager.instance.OnTransactionComplete += OnTransactionComplete;
		IOSInAppPurchaseManager.instance.OnVerificationComplete += OnVerificationComplete;
		IOSInAppPurchaseManager.instance.OnRestoreComplete += OnRestoreComplete;
		IOSInAppPurchaseManager.instance.loadStore();
		#elif UNITY_WP8
		WP8InAppPurchasesManager.instance.addEventListener(WP8InAppPurchasesManager.INITIALIZED, OnInitComplete);
		WP8InAppPurchasesManager.instance.addEventListener(WP8InAppPurchasesManager.PRODUCT_PURCHASE_FINISHED, OnPurchaseFinished);
		WP8InAppPurchasesManager.instance.init();	
		#endif
		
		//		if(GameSettings.Instance.showTestLogs)
		//			Debug.Log("CoreIAPManager - Init finished");
	}
	#endregion
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	#region AndroidEvents
	void OnProductPurchased (BillingResult result){
		//if all goes right...
		if(result.isSuccess){
			UIBaseInAppItem product = getProductByID(result.purchase.SKU); //get the product purchase info
			
			//consume the purchase if the inapp product type is consumable
			if(product != null && product.IaType == UIBaseInAppItem.InAppItemType.COMSUMABLE)
				AndroidInAppPurchaseManager.instance.consume(result.purchase.SKU);
			
			//event to process this purchase
			OnProcessingPurchaseProduct (result.purchase.SKU);
		} 
		else{
			OnProcessingPurchaseProduct (result.purchase.SKU, false);
		}
	}
	void OnProductConsumed (BillingResult result){
		if(result.isSuccess) {
			//			OnProcessingPurchaseProduct (result.purchase.SKU);
		}
	}
	void OnBillingConnected (BillingResult result){
		//Como ya se ha iniciado la tienda no nos hace falta estar suscritos al evento que nos
		//avise cuando se inicia la tienda.
		AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;
		//si todo fue bien...
		if(result.isSuccess) {
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("In App Service connected");
			
			//ahora que estamos conectados en la tienda podemos pedir la lista de productos.
			AndroidInAppPurchaseManager.instance.retrieveProducDetails();
			//nos vamos a suscribir a este evento que nos avisara cuando este cargada.
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetriveProductsFinised;
		}
		else{
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("Failed in init Stoke Kit :(");
			
			OnRetrievedProducts(false);
		}
	}
	
	void OnRetriveProductsFinised (BillingResult result){
		//si todo fue bien...
		if(result.isSuccess) {
			//cambiamos la bandera para avisar que la tienda esta lista
			IsInited = true;
			GameLoaderManager.Instance.InAppInited = true;
			
			numProducts = AndroidInAppPurchaseManager.instance.inventory.products.Count;
			productsGoogle = AndroidInAppPurchaseManager.instance.inventory.products;
			
			//es buena practica comprobar que todos los productos adquiridos esta consumidos (por ejemplo pudimos
			//comprar un prodcutos pero se nos salio del movil y no lo consumimos, o iniciamos desde otro dispositivo)
			//			dispatcher.dispatch(RETRIEVED_PRODUCTS, AndroidInAppPurchaseManager.instance.inventory.products);
			OnRetrievedProducts(true);
			
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("Inited successfully, Avaliable products cound: " + AndroidInAppPurchaseManager.instance.inventory.products.Count.ToString());
		}
		else{
			//			dispatcher.dispatch(NOT_RETRIEVED_PRODUCTS);
			OnRetrievedProducts(false);
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("Failed in init Stoke Kit :(");
		}
		
		//igual que antes ya no nos hace falta estar suscritos al evento.
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetriveProductsFinised;
	}
	#endregion
	
	
	#region IOSEvents
	void OnStoreKitInitComplete (ISN_Result result){
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("CoreIAPManager - OnStoreKitInitComplete, success ? " +result.IsSucceeded);
		
		if(result.IsSucceeded) {
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("Inited successfully, Avaliable products count: " + IOSInAppPurchaseManager.instance.products.Count.ToString());
			IsInited = true;
			GameLoaderManager.Instance.InAppInited = true;
			
			numProducts = IOSInAppPurchaseManager.instance.products.Count;
			productsIOS = IOSInAppPurchaseManager.instance.products;
			//			dispatcher.dispatch(RETRIEVED_PRODUCTS, IOSInAppPurchaseManager.instance.products);
			OnRetrievedProducts(true);
		} else {
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("Failed in init Stoke Kit :(");
			
			//			dispatcher.dispatch(NOT_RETRIEVED_PRODUCTS);
			OnRetrievedProducts(false);
			
		}
		
		IOSInAppPurchaseManager.instance.OnStoreKitInitComplete -= OnStoreKitInitComplete;
	}
	void OnTransactionComplete (IOSStoreKitResponse responce){
		if(GameSettings.Instance.showTestLogs){
			Debug.Log("OnTransactionComplete: " + responce.productIdentifier);
			Debug.Log("OnTransactionComplete: state: " + responce.state);
		}
		
		switch(responce.state) {
		case InAppPurchaseState.Purchased:
		case InAppPurchaseState.Restored:
			//Our product been succsesly purchased or restored
			//So we need to provide content to our user 
			//depends on productIdentifier
			OnProcessingPurchaseProduct(responce.productIdentifier);
			//Warning: Use 
			//SANDBOX_VERIFICATION_SERVER url (https://sandbox.itunes.apple.com/verifyReceipt) during app testing  and 
			//APPLE_VERIFICATION_SERVER url  (https://buy.itunes.apple.com/verifyReceipt) on production.
			string verifURL = GameSettings.Instance.IS_A_DEV_VERSION ? IOSInAppPurchaseManager.SANDBOX_VERIFICATION_SERVER : IOSInAppPurchaseManager.APPLE_VERIFICATION_SERVER;
			IOSInAppPurchaseManager.instance.verifyLastPurchase(verifURL);
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
			
			OnProcessingPurchaseProduct(responce.productIdentifier, false, true);
			break;
		case InAppPurchaseState.Failed:
			OnProcessingPurchaseProduct(responce.productIdentifier, false);
			
			//Our purchase flow is failed.
			//We can unlock interface and report user that the purchase is failed. 
			if(GameSettings.Instance.showTestLogs){
				Debug.Log("Transaction failed with error, code: " + responce.error.code);
				Debug.Log("Transaction failed with error, description: " + responce.error.description);
			}
			break;
		}
		
		//		IOSNativePopUpManager.showMessage("Store Kit Response", "product " + responce.productIdentifier + " state: " + responce.state.ToString());
	}
	void OnVerificationComplete (IOSStoreKitVerificationResponse responce){
		//		IOSNativePopUpManager.showMessage("Verification", "Transaction verification status: " + responce.status.ToString());
		
		if(GameSettings.Instance.showTestLogs)
			Debug.Log("Transaction verification status: " + responce.status.ToString());
	}
	void OnRestoreComplete (ISN_Result responce){
		if(responce.IsSucceeded){
			OnProcessingRestorePurchases();
			//			IOSNativePopUpManager.showMessage("Restore Purchase", "All purchases has been restored.");
		}
		else{
			OnProcessingRestorePurchases(false);
			//			IOSNativePopUpManager.showMessage("Restore Purchase", "Restore failed: "+ responce.error.description);
		}
	}
	#endregion
	
	#region WP8Events
	void OnInitComplete() {
		
		
		
		if(WP8InAppPurchasesManager.instance.products != null){
			IsInited = true;
			GameLoaderManager.Instance.InAppInited = true;
			numProducts = WP8InAppPurchasesManager.instance.products.Count;
			productsWP = WP8InAppPurchasesManager.instance.products;
			//			dispatcher.dispatch(RETRIEVED_PRODUCTS, WP8InAppPurchasesManager.instance.products);
			OnRetrievedProducts(true);
			
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("Inited successfully, Avaliable products cound: " + WP8InAppPurchasesManager.instance.products.Count.ToString());
		}
		else{
			//			dispatcher.dispatch(NOT_RETRIEVED_PRODUCTS);
			OnRetrievedProducts(false);
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("Failed in init Stoke Kit :(");
		}
		
		//check if have a durable object but no its assigned yet.
		foreach(WP8ProductTemplate product in WP8InAppPurchasesManager.instance.products){
			if(product.Type == WP8PurchaseProductType.Durable) {
				if(product.isPurchased) {
					//The Durable product was purchased, we should check here 
					//if the content is unlocked for our Durable product.
					OnProcessingPurchaseProduct(product.ProductId);
					//					Debug.Log("Product " + product.Name + " is purchased");
					
				}
			}
		}
		
		WP8InAppPurchasesManager.instance.removeEventListener(WP8InAppPurchasesManager.INITIALIZED, OnInitComplete);
		
		//		WP8Dialog.Create("market Initted", "Total products avaliable: " + WP8InAppPurchasesManager.instance.products.Count);
	}
	void OnPurchaseFinished(CEvent e) {
		WP8PurchseResponce responce = e.data as WP8PurchseResponce;
		
		if(responce.IsSuccses) {
			//Unlock logic for product with id recponce.productId should be here
			OnProcessingPurchaseProduct(responce.productId);
		} else {
			OnProcessingPurchaseProduct(responce.productId, false);
			//Purchase fail logic for product with id recponce.productId should be here
			//			WP8Dialog.Create("Purchase Failed", "Error: " + responce.error);
		}
	}
	#endregion
	
	/// <summary>
	/// Load the products ids that are the same in all platforms
	/// </summary>
	/// <param name="productsIDs">Products I ds.</param>
	private void FillProducts(List<string> productsIDs){
		#if UNITY_ANDROID
		if(GameSettings.Instance.showTestLogs && productsIDs != null){
			Debug.Log("Filling products. count: " + productsIDs.Count);
			productsIDs.ForEach(x => Debug.Log("product id: " + x));
		}
		productsIDs.ForEach(x => AndroidInAppPurchaseManager.instance.addProduct(x));
		#elif UNITY_IPHONE
		productsIDs.ForEach(x => IOSInAppPurchaseManager.instance.addProductId(x));
		#elif UNITY_WP8
		//este se rellena desde microsoft.
		#endif
	}
	
	
	#region Virtual
	/// <summary>
	/// Este metodo se encarga de darle al jugador el objecto comprado. Si es un consumible(100 monedas) o durable 
	/// (una capa). 
	/// </summary>
	/// <param name="SKU">SKU</param>
	public virtual void OnProcessingPurchaseProduct (string SKU, bool success = true, bool deferred = false)
	{
		if(success && !deferred){
			//GA
			GameAnalytics.NewDesignEvent(GAEvents.INAPP_ITEM_PURCHASED +":"+ SKU);
			
			dispatcher.dispatch(PURCHASE_COMPLETED, getProductByID(SKU));
		}
		else if(!success && !deferred){
			//GA
			GameAnalytics.NewDesignEvent(GAEvents.INAPP_ITEM_PURCHASE_FAILED +":"+ SKU);
			
			dispatcher.dispatch(PURCHASE_FAILED, getProductByID(SKU));
		}
		else if(deferred){
			//GA
			GameAnalytics.NewDesignEvent(GAEvents.INAPP_ITEM_PURCHASE_CANCELED +":"+ SKU);
			
			dispatcher.dispatch(DEFERRED_PURCHASE_COMPLETED, getProductByID(SKU));
		}
	}
	
	public virtual void OnProcessingRestorePurchases (bool success = true)
	{
		if(success){
			//GA
			GameAnalytics.NewDesignEvent(GAEvents.INAPP_PURCHASES_RESTORED);
			
			dispatcher.dispatch(RESTORE_PURCHASE_COMPLETED);
		}
		else{
			//GA
			GameAnalytics.NewDesignEvent(GAEvents.INAPP_PURCHASES_NO_RESTORED);
			
			dispatcher.dispatch(RESTORE_PURCHASE_FAILED);
		}
	}
	
	public virtual void OnRetrievedProducts (bool success = true)
	{
		if(success){
			#if UNITY_ANDROID
			List<GoogleProductTemplate> products = AndroidInAppPurchaseManager.instance.inventory.products;
			dispatcher.dispatch(RETRIEVED_PRODUCTS, products);
			bool needRestore = false;
			
			foreach(UIBaseInAppItem product in _products){
				//chisking if we already own some consuamble product but forget to consume those
				//consume the purchase if the inapp product type is consumable
				if(product != null && product.IaType == UIBaseInAppItem.InAppItemType.COMSUMABLE
				   && AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased(product.Id)){
					AndroidInAppPurchaseManager.instance.consume(product.Id);
				}
				
				
				//Check if non-consumable rpduct was purchased, but we do not have local data for it.
				//It can heppens if game was reinstalled or download on oher device
				//This is replacment for restore purchase fnunctionality on IOS
				//we apply the product reward if not was applyied but did purchase
				else if(product != null && product.IaType == UIBaseInAppItem.InAppItemType.NON_CONSUMABLE 
				        && !product.RewardedNonConsumable //not was rewarded
				        && AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased(product.Id)) {
					if(GameSettings.Instance.showTestLogs)
						Debug.Log("CoreIAPManager - restoring purchase of non consumable product: " + product.ItemType);
					
					if(!needRestore)
						GameLoaderManager.Instance.InAppNeedRestoreProducts = true;
					
					needRestore = true;
					product.applyReward();
				}
			}
			
			if(needRestore){
				GameLoaderManager.Instance.InAppAllProductsRestored = true;
				GameLoaderManager.Instance.InAppInited = true;
			}
			
			IsInited = true;
			#elif UNITY_IPHONE
			List<IOSProductTemplate> products = IOSInAppPurchaseManager.instance.products;
			dispatcher.dispatch(RETRIEVED_PRODUCTS, products);
			
			#elif UNITY_WP8
			List<WP8ProductTemplate> products = WP8InAppPurchasesManager.instance.products;
			dispatcher.dispatch(RETRIEVED_PRODUCTS, products);
			
			#endif
		}
		else{
			dispatcher.dispatch(NOT_RETRIEVED_PRODUCTS);
		}
	}
	
	
	
	public virtual string getInAppFileContent(string productID){
		string stats = "";
		
		//Try to load list item from file
		TextAsset text = Resources.Load(textFile) as TextAsset;
		
		if(text == null){
			Debug.LogError("You must provide a correct filename for InApp products file");
		}
		else{
			string[] allItems = text.text.Split('\n');
			
			foreach(string w in allItems){
				string[] atts = w.Split(',');
				
				if(atts.Length > 0 && atts[0] == productID){
					stats = w;
					break;
				}
			}
		}
		
		return stats;
	}
	
	private UIBaseInAppItem getProductByID(string id){
		UIBaseInAppItem product = null;
		
		foreach(UIBaseInAppItem p in _products){
			if(p.Id.Equals(id)){
				product = p;
				break;
			}
		}
		
		return product;
	}
	
	//	public bool purchased(string productId){
	//		return AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased(productId);
	////		return IOSInAppPurchaseManager.instance.products.
	//
	//	
	//	}
	
	#endregion
}
