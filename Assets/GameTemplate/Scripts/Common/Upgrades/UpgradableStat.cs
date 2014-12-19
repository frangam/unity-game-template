using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UpgradableStat : BaseStat{
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	[SerializeField]
	private float 	price;

	[SerializeField]
	private int 	upgradeIndex;

	[SerializeField]
	private int 	maxUpgrades;
	
	//--------------------------------------
	// Getters/MyGenericTypeetters
	//--------------------------------------
	public float Price {
		get {
			return this.price;
		}
		set {
			price = value;
		}
	}
	
	public int UpgradeIndex {
		get {
			return this.upgradeIndex;
		}
	}

	public int MaxUpgrades {
		get {
			return this.maxUpgrades;
		}
	}
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="UpgradableMyGenericTypetat`2"/> class.
	/// 
	/// ID, NAME, VALUE, MAX VALUE, UPGRADE PRICE, 
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public UpgradableStat(string attributes): base(attributes){
		string[] att = attributes.Split(ATTRIBUTES_SEPARATOR);
		
		if(att.Length > 4){
			float p;
		
			if(Casting.TryCast(att[4], out p))
				price = p;
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString (){
		return string.Format ("[UpgradableStat: price={0}, {1}, upgradeIndex={2}, maxUpgrades={3}]", price,base.ToString(), upgradeIndex, maxUpgrades);
	}
	

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	/// <summary>
	/// Do the upgrade
	/// </summary>
	/// <returns><c>true</c>, if it was upgraded, <c>false</c> otherwise.</returns>
	private void LevelUp(){
		bool doUpgrade = UpgradeIndex < MaxUpgrades;
		
		if(doUpgrade){
			upgradeIndex++;
			CurrentValue += (MaxValue-InitialValue)/maxUpgrades; //(Level*1f/MaxLevel) * MaxValue;
		}
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Apply the upgrade and return de final money
	/// </summary>
	/// <param name="totalMoney">Total money.</param>
	public float apply(float totalMoney){
		float finalMoney = totalMoney;
		
		if(UpgradeIndex < MaxUpgrades
		   && (totalMoney >= price)){
			finalMoney -= price;
			LevelUp();
		}
		
		return finalMoney;
	}
}

