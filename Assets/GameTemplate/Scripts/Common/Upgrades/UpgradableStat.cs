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
	/// ID| NAME | Invert min max value (1 or 0)| Min VALUE : MAX VALUE | SIMULATED MIN VALUE : SIMULATED MAX VALUE | CURRENT UPGRADE (0: INITIAL) |UPGRADE PRICES 
	/// 
	/// UPGRADE PRICES: u1:u2:u3 The number of prices indicate the max upgrades number
	/// 
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public UpgradableStat(string attributes): base(attributes){
		string[] att = attributes.Split(ATTRIBUTES_SEPARATOR);
		string[] pPrices = null;
		float p;
		int pI;
		bool valid = att.Length > 6 || att.Length == 6; 
		
		prices = new List<float>();
		
		if(att.Length > 6){
			int ci;
			pPrices = att[6].Split(LIST_SEPARATOR);
			
			if(int.TryParse(att[5], out ci))
				currentUpgradeIndex = ci;
		}
		else if(att.Length == 6){
			int ci;
			pPrices = att[5].Split(LIST_SEPARATOR);
			
			if(int.TryParse(att[4], out ci))
				currentUpgradeIndex = ci;
		}
		else{
			Debug.LogError("The format of the stat is not valid. A valid format is: ID| NAME | Invert min max value (1 or 0)| Min VALUE : MAX VALUE | SIMULATED MIN VALUE : SIMULATED MAX VALUE| CURRENT UPGRADE (0: INITIAL) | UPGRADE PRICES");
		}
		
		if(valid){
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
			
			
			//init the current value depending of the current update index
			initCurrentValue();
		}
		
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="UpgradableStat"/>.
	/// ID| NAME | Invert min max value (1 or 0)| Min VALUE : MAX VALUE | SIMULATED MIN VALUE : SIMULATED MAX VALUE | UPGRADE PRICES 
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="UpgradableStat"/>.</returns>
	public override string ToString (){
		return base.ToString() + ATTRIBUTES_SEPARATOR + currentUpgradeIndex + ATTRIBUTES_SEPARATOR + getPricesToString();
	}
	
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private string getPricesToString(){
		string res = "";
		
		for(int i=0; i<prices.Count; i++){
			res += prices[i];
			
			if(i<prices.Count-1){
				res += ":";
			}
		}
		
		return res;
	}
	
	private void updateCurrentValue(){
		CurrentValue += (MaxValue-InitialValue)/maxUpgrades; //(Level*1f/MaxLevel) * MaxValue;
		CurrentSimValue += (MaxSimValue-InitialSimValue)/maxUpgrades; //(Level*1f/MaxLevel) * MaxValue;
	}
	
	private void initCurrentValue(){
		for(int i=0; i<currentUpgradeIndex; i++){
			updateCurrentValue();
		}
	}
	
	/// <summary>
	/// Do the upgrade
	/// </summary>
	/// <returns><c>true</c>, if it was upgraded, <c>false</c> otherwise.</returns>
	private void LevelUp(){
		if(!completed()){
			currentUpgradeIndex++;
			updateCurrentValue();
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
	public bool apply(bool payWithMoney = true){
		bool applied = false;
		
		if(!completed()
		   && (
			(payWithMoney && GameMoneyManager.Instance.payWithMoney((int)prices[currentUpgradeIndex]))
			|| (!payWithMoney && GameMoneyManager.Instance.payWithGems((int)prices[currentUpgradeIndex], true))
			) 
		   ){
			applied = true;
			LevelUp();
		}
		
		return applied;
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

