using UnityEngine;
using System.Collections;

[System.Serializable]
public class UIBaseInAppItem : UIBaseListItem {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string MONEY_ITEM 		= "MONEY";
	public const string GEM_ITEM 		= "GEM";


	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private float 	realMoneyPrice;

	[SerializeField]
	private int 	quantity;

	[SerializeField]
	private string 	itemType;

	[SerializeField]
	private string  spriteName;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public float RealMoneyPrice {
		get {
			return this.realMoneyPrice;
		}
	}

	public int Quantity {
		get {
			return this.quantity;
		}
	}

	public string ItemType {
		get {
			return this.itemType;
		}
	}

	public string LocalizedName{
		get{
			return Localization.Get(Name);
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="UIBaseInAppItem"/> class.
	/// ID,NAME,ITEM TYPE,QUANTITY TO ACQUIRE,SPRITE NAME,REAL MONEY PRICE
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="anim">Animation.</param>
	public UIBaseInAppItem(string data, Animator anim = null): base(data, anim){
		string[] atts = data.Split(ATTRIBUTES_SEPARATOR);
		init(atts);
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(string[] data){
		int pQuantity;
		float pPrice;

		//item type
		if(data.Length > 2)
			itemType = data[2];

		//Quantity
		if(data.Length > 3 && int.TryParse(data[3], out pQuantity))
			quantity = pQuantity;

		//sprite name
		if(data.Length > 4)
			spriteName = data[4];

		//real money price
		if(data.Length > 5 && float.TryParse(data[5], out pPrice))
			realMoneyPrice = pPrice;
	}

	public virtual void applyReward(){
		switch(itemType){
		case MONEY_ITEM:
			GameMoneyManager.Instance.addMoney(quantity);
			break;

		case GEM_ITEM:
			GameMoneyManager.Instance.addGems(quantity);
			break;
		}
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[UIBaseInAppItem: realMoneyPrice={0}, quantity={1}, itemType={2}]", realMoneyPrice, quantity, itemType);
	}

	public override void load ()
	{
		base.load ();

		string stats = getContent();
		
		//load stats
		if(!string.IsNullOrEmpty(stats)){
			string[] att = stats.Split(ATTRIBUTES_SEPARATOR);
			init (att);
		}
	}
}
