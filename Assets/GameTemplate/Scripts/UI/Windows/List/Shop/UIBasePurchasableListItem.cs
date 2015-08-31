/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

[System.Serializable]
public class UIBasePurchasableListItem : UIBaseListItem {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	[Tooltip("The PlayerPrefs key for save progress of purchase. It is formed automatically by this value + ID attribute")]
	/// <summary>
	/// The partial player prefs key: only the name, so it is not the complete key composed by this name + item id
	/// </summary>
	protected string 		ppKey;
	
	[SerializeField]
	private long 			price;
	
	[SerializeField]
	private long 			gemsPrice;
	
	[SerializeField]
	private bool 			purchased = false;
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	/// <summary>
	/// The complete player prefs key: a name + id of item
	/// </summary>
	private string 			playerPrefsKey;
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public long Price {
		get {
			return this.price;
		}
		set{
			this.price = value;
		}
	}
	
	public long GemsPrice {
		get {
			return this.gemsPrice;
		}
		set{
			this.gemsPrice = value;
		}
	}
	
	public bool Purchased {
		get {
			return this.purchased;
		}
		set {
			purchased = value;
		}
	}
	
	public bool AvailableForPurchase{
		get{return ((Price > 0 && GemsPrice > 0 && !Purchased) || (Price <= 0 && GemsPrice <= 0));}
	}
	
	public string PlayerPrefsKey {
		get {
			return this.playerPrefsKey;
		}
	}
	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="UIBasePurchasableListItem"/> class.
	/// 
	/// ID,NAME,PRICE,GEMSPRICE
	/// </summary>
	/// <param name="data">Data.</param>
	public UIBasePurchasableListItem(string pPPKey, string data, Animator anim = null) : base(data, anim){
		this.ppKey = pPPKey;
		this.playerPrefsKey = pPPKey + Id.ToString();
		string[] atts = data.Split(ATTRIBUTES_SEPARATOR);
		init(atts);
	}
	
	public UIBasePurchasableListItem(string pPPKey, string pId, string pName, bool pPurchased, long pPrice, long pGemsPrice): base(pId, pName){
		this.ppKey = pPPKey;
		this.playerPrefsKey = pPPKey + Id.ToString();
		purchased = pPurchased;
		price = pPrice;
		gemsPrice = pGemsPrice;
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	//	public override void Awake (){
	//		base.Awake ();
	//	}
	
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="UIBaseListItem"/> with this format: 
	/// ID,NAME,PURCHASED,PRICE,GEMSPRICE
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="UIBaseListItem"/>.</returns>
	public override string ToString(){
		string purchasedString = purchased ? "1":"0";
		return base.ToString() +ATTRIBUTES_SEPARATOR+ purchasedString +ATTRIBUTES_SEPARATOR+ price.ToString() +ATTRIBUTES_SEPARATOR+ gemsPrice.ToString();
	}
	
	public override void load (){
		base.load ();
		
		string stats = getContent();
		
		//load stats
		if(!string.IsNullOrEmpty(stats)){
			string[] att = stats.Split(ATTRIBUTES_SEPARATOR);
			
			init (att);
		}
	}
	
	public virtual void save ()
	{
		int pPurchase = purchased ? 1: 0;
		PlayerPrefs.SetInt(playerPrefsKey, pPurchase);
	}
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Init the specified data.
	/// 
	/// ID,NAME,PRICE,GEMSPRICE
	/// </summary>
	/// <param name="data">Data.</param>
	public void init(string[] data){
		long pPrice, pGemsPrice;
		int pPurchased;
		
		playerPrefsKey = ppKey+Id.ToString();
		
		if(long.TryParse(data[2], out pPrice))
			Price = pPrice;
		if(data.Length > 3 && long.TryParse(data[3], out pGemsPrice))
			GemsPrice = pGemsPrice;
		
		pPurchased = (price > 0 || gemsPrice > 0) ? PlayerPrefs.GetInt(playerPrefsKey) : 1; // this is usefull for set initial item as purchased if its price and gems price are 0
		purchased = pPurchased != 0 ? true:false;
	}
	
	public virtual bool purchaseWithMoney(){
		bool res = false;
		
		if(GameMoneyManager.Instance.purchaseWithMoney(price)){
			finalizePurchase();
			res = true;
		}
		
		return res;
	}
	
	public virtual bool purchaseWithGems(){
		bool res = false;
		
		if(GameMoneyManager.Instance.purchaseWithGems(GemsPrice)){
			finalizePurchase();
			res = true;
		}
		
		return res;
	}
	
	public virtual void finalizePurchase(){
		Purchased = true;
		save();
	}
}
