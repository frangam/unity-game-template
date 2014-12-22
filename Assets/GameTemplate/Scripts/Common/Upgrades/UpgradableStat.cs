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
	/// ID| NAME | Invert min max value (1 or 0)| Min VALUE : MAX VALUE | SIMULATED MIN VALUE : SIMULATED MAX VALUE | UPGRADE PRICES 
	/// 
	/// UPGRADE PRICES: u1:u2:u3 The number of prices indicate the max upgrades number
	/// 
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public UpgradableStat(string attributes): base(attributes){
		string[] att = attributes.Split(ATTRIBUTES_SEPARATOR);
		prices = new List<float>();
		
		if(att.Length > 5){
			string[] pPrices = att[5].Split(LIST_SEPARATOR);
			float p;
			int pI;
			
			//prices for each upgrade
			foreach(string pP in pPrices){
				//float price
				if(float.TryParse(pP, out p)){
					prices.Add(p);
				}
				//int price
				else if(int.TryParse(pP, out pI)){
					prices.Add(pI);
				}
			}
			
			
			//total upgrades
			maxUpgrades = prices.Count;
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
	
	public float[] getAllValuesFromActualToInitial(){
		float[] res = new float[currentUpgradeIndex+1];
		
		for(int i=0; i<currentUpgradeIndex+1; i++){
			res[i] = InitialValue + i*((MaxValue-InitialValue)/maxUpgrades); 
		}
		
		return res;
	}
	
	public float[] getAllSimValuesFromActualToInitial(){
		float[] res = new float[currentUpgradeIndex+1];
		
		for(int i=0; i<currentUpgradeIndex+1; i++){
			res[i] = InitialSimValue + i*((MaxSimValue-InitialSimValue)/maxUpgrades); 
		}
		
		return res;
	}
}

