﻿/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class BasePurchasableLevel : BaseLevel {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int	moneyPrice;
	private int	gemsPrice;
	private string ppKey;
	private bool purchased;
	
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
	
	public string PpKey {
		get {
			return this.ppKey;
		}
	}
	
	public bool Purchased{
		get{
			return this.purchased;
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
	public BasePurchasableLevel(string pPPKey, string attributes) : base(attributes){
		ppKey = pPPKey;
		string[] att = attributes.Split(SEPARATOR_ATTRIBUTES);
		
		//money 
		int pMoney;
		if(att.Length > 6 && int.TryParse(att[6], out pMoney))
			moneyPrice = pMoney;
		
		//gems 
		if(att.Length > 7 && int.TryParse(att[7], out pMoney))
			gemsPrice = pMoney;
		
		//level purchased or not
		int p = (moneyPrice > 0 || gemsPrice > 0) ? PlayerPrefs.GetInt(ppKey+Id.ToString()) : 1; // this is usefull for set initial level as purchased if its price and gems price are 0
		purchased = p != 0 ? true:false;
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[BasePurchasableLevel: moneyPrice={0}, gemsPrice={1}]", moneyPrice, gemsPrice);
	}
	
}
