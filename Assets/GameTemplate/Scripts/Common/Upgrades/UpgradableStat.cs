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
	private List<float> 	prices;

	[SerializeField]
	private int 			currentUpgradeIndex;

	[SerializeField]
	private int 			maxUpgrades;
	
	//--------------------------------------
	// Getters/MyGenericTypeetters
	//--------------------------------------
	public List<float> Prices {
		get {
			return this.prices;
		}
		set {
			prices = value;
		}
	}
	
	public int CurrentUpgradeIndex {
		get {
			return this.currentUpgradeIndex;
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
		prices = new List<float>();

		if(att.Length > 4){
			string[] pPrices = att[4].Split(':');
			float p;
		
			foreach(string pP in pPrices){
				if(Casting.TryCast(pP, out p)){
					prices.Add(p);
				}
			}
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString (){
		return string.Format ("[UpgradableStat: price={0}, {1}, currentUpgradeIndex={2}, maxUpgrades={3}]", prices,base.ToString(), currentUpgradeIndex, maxUpgrades);
	}
	

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	/// <summary>
	/// Do the upgrade
	/// </summary>
	/// <returns><c>true</c>, if it was upgraded, <c>false</c> otherwise.</returns>
	private void LevelUp(){
		if(!completed()){
			currentUpgradeIndex++;
			CurrentValue += (MaxValue-InitialValue)/maxUpgrades; //(Level*1f/MaxLevel) * MaxValue;
		}
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public float currentPrice(){
		float res = -1f;
		
		if(!completed())
			res = prices[currentUpgradeIndex];
		
		return res;
	}

	public float nextPrice(){
		float res = -1f;

		if(currentUpgradeIndex<maxUpgrades-1)
			res = prices[currentUpgradeIndex+1];

		return res;
	}

	public bool completed(){
		return CurrentUpgradeIndex >= MaxUpgrades;
	}

	/// <summary>
	/// Apply the upgrade and return de final money
	/// </summary>
	/// <param name="totalMoney">Total money.</param>
	public float apply(float totalMoney){
		float finalMoney = totalMoney;
		
		if(!completed()
		   && (totalMoney >= prices[currentUpgradeIndex])){
			finalMoney -= prices[currentUpgradeIndex];
			LevelUp();
		}
		
		return finalMoney;
	}
}

