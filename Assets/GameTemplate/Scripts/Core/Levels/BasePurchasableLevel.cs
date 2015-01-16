using UnityEngine;
using System.Collections;

public class BasePurchasableLevel : BaseLevel {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int	moneyPrice;
	private int	gemsPrice;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public int MoneyPrice {
		get {
			return this.moneyPrice;
		}
	}

	public int GemsPrice {
		get {
			return this.gemsPrice;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="BasePurchasableLevel"/> class.
	/// 
	/// Attributes:
	/// ID, Localization Key, Player life, Game Lifes, Money Reward, Gems Reward, Money Price, Gems Price
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public BasePurchasableLevel(string attributes) : base(attributes){
		string[] att = attributes.Split(SEPARATOR_ATTRIBUTES);

		//money 
		int pMoney;
		if(att.Length > 6 && int.TryParse(att[6], out pMoney))
			moneyPrice = pMoney;

		//gems 
		if(att.Length > 7 && int.TryParse(att[7], out pMoney))
			gemsPrice = pMoney;
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[BasePurchasableLevel: moneyPrice={0}, gemsPrice={1}]", moneyPrice, gemsPrice);
	}
	
}
