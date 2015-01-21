﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBaseMIssionCompletedWin : UIBaseWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Text lbMoneyReward;
	
	[SerializeField]
	private Text lbGemsReward;
	
	[SerializeField]
	private Transform pnlGemsReward;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected BaseLevel level;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void open (){
		if(level != null){
			showInfo();
		}
		
		base.open ();
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(BaseLevel pLevel){
		level = pLevel;
	}
	
	public virtual void showInfo(){
		lbMoneyReward.text = level.RealMoneyReward.ToString();
		lbGemsReward.text = level.RealGemsReward.ToString();
		pnlGemsReward.gameObject.SetActive(level.RealGemsReward > 0);
	}
}
