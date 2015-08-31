/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UpgradableStat : BaseStat{
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	[Tooltip("The PlayerPrefs key for save progress of purchase. It is formed automatically by this value + STAT OWNER ID + STAT ID")]
	private string 			ppKey;
	
	[SerializeField]
	private List<float> 	prices;
	
	[SerializeField]
	private int 			currentUpgradeIndex;
	
	[SerializeField]
	private int 			maxUpgrades;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private string 			playerPrefsKey;
	
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
	
	public float IncrementValue{
		get{
			return (MaxValue-InitialValue)/maxUpgrades;
		}
	}
	
	public float IncrementSimulatedValue{
		get{
			return (MaxSimValue-InitialSimValue)/maxUpgrades;
		}
	}
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="UpgradableMyGenericTypetat`2"/> class.
	/// 
	/// STAT OWNER ID | STAT ID | NAME | Invert min max value (1 or 0)| Min VALUE : MAX VALUE | SIMULATED MIN VALUE : SIMULATED MAX VALUE |UPGRADE PRICES 
	/// 
	/// UPGRADE PRICES: u1:u2:u3 The number of prices indicate the max upgrades number
	/// 
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public UpgradableStat(string pPPKey, string attributes): base(attributes){
		ppKey = pPPKey;
		playerPrefsKey = ppKey + StatOwnerID + "_" + StatId;
		string[] att = attributes.Split(ATTRIBUTES_SEPARATOR);
		string[] pPrices = null;
		float p;
		int pI;
		bool valid = att.Length > 6 || att.Length == 6; 
		
		prices = new List<float>();
		
		if(att.Length > 6){
			pPrices = att[6].Split(LIST_SEPARATOR);
			
			//get from playerprefs the current index, by default is 0
			currentUpgradeIndex = Mathf.Clamp(PlayerPrefs.GetInt(playerPrefsKey), 0, pPrices.Length); // max: pPrices.Length because is the last update value
		}
		else if(att.Length == 6){
			int ci;
			pPrices = att[5].Split(LIST_SEPARATOR);
			
			//get from playerprefs the current index, by default is 0
			currentUpgradeIndex = Mathf.Clamp(PlayerPrefs.GetInt(playerPrefsKey), 0, pPrices.Length);
		}
		else{
			Debug.LogError("The format of the stat is not valid. A valid format is: STAT OWNER ID | STAT ID | NAME | Invert min max value (1 or 0)| Min VALUE : MAX VALUE | SIMULATED MIN VALUE : SIMULATED MAX VALUE| UPGRADE PRICES");
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
		return base.ToString() + ATTRIBUTES_SEPARATOR + getPricesToString() + ATTRIBUTES_SEPARATOR + currentUpgradeIndex;
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
		CurrentValue += IncrementValue; //(Level*1f/MaxLevel) * MaxValue;
		CurrentSimValue += IncrementSimulatedValue; //(Level*1f/MaxLevel) * MaxValue;
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
			PlayerPrefs.SetInt(playerPrefsKey, currentUpgradeIndex); //save the progress in pp
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

