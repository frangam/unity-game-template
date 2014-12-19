﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class WeaponUpgradesExample : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	public int totalMoney = 10000;
	public Text lbTotalMoney;
	public Text lbPower;
	public Text lbStability;
	public Text lbZoom;
	public Text lbCapacity;
	public List<UpgradableStat> stats;

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	public void Start(){
		lbTotalMoney.text = "Money: "+ totalMoney.ToString();

		foreach(UpgradableStat s in stats){
			changeLabelStatText(s);
		}
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void upgrade(string id){
		foreach(UpgradableStat s in stats){
			if(s.Id == id){
				int finalMoney = (int) s.apply(totalMoney);

				if(finalMoney != totalMoney){
					totalMoney = finalMoney;
					lbTotalMoney.text = "Money: "+ finalMoney.ToString();
					changeLabelStatText(s);
				}
				else{
					Debug.Log("You do not have enough money to purchase or just reached max number of upgrades");
				}
				break;
			}
		}
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void changeLabelStatText(UpgradableStat stat){
		string value = stat.CurrentValue.ToString();

		switch(stat.Id){
		case "s_0": lbPower.text = value; break;
		case "s_1": lbStability.text = value; break;
		case "s_2": lbZoom.text = value; break;
		case "s_3": lbCapacity.text = value; break;
		}
	}
}