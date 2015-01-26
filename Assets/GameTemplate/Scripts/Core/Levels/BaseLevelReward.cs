using UnityEngine;
using System.Collections;

public class BaseLevelReward {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private int moneyReward;
	private int gemsReward;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public int MoneyReward {
		get {
			return this.moneyReward;
		}
		set {
			moneyReward = value;
		}
	}

	public int GemsReward {
		get {
			return this.gemsReward;
		}
		set {
			gemsReward = value;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public BaseLevelReward(int money, int gems){
		moneyReward = money;
		gemsReward = gems;
	}
}
