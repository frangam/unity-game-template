/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBasePurchasableLevel : UIBasePurchasableListItem {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string ppKey;

	[SerializeField]
	private Text lbDescription;
	
	[SerializeField]
	private Text lbMoneyReward;
	
	[SerializeField]
	private Text lbGemsReward;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected BasePurchasableLevel level;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public BasePurchasableLevel Level {
		get {
			return this.level;
		}
	}

	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	public UIBasePurchasableLevel(string pPPKey, string data, Animator anim = null): base(pPPKey, data, anim){
		init (data);
	}

	public UIBasePurchasableLevel(string pPPKey, BasePurchasableLevel pLevel, string pName): base(pPPKey, pLevel.Id, pName, pLevel.Purchased, pLevel.MoneyPrice, pLevel.GemsPrice){
		this.level = pLevel;
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Call this method only in child class
	/// </summary>
	/// <param name="data">Data.</param>
	public virtual void init (string data){
		level = new BasePurchasableLevel(base.PlayerPrefsKey, data);
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void show ()
	{
		base.show ();
		
		if(lbDescription)
			lbDescription.text = level.LocalizedDescription;
		
		if(lbMoneyReward)
			lbMoneyReward.text = level.MoneyReward.ToString();
		
		if(lbGemsReward)
			lbGemsReward.text = level.GemsReward.ToString();
	}
}
