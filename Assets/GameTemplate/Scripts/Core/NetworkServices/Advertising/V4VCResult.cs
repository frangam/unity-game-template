/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class V4VCResult : BaseDataResult {
	private string rewardName;
	private int amount;

	public string RewardName {
		get {
			return this.rewardName;
		}
	}

	public int Amount {
		get {
			return this.amount;
		}
	}

	public V4VCResult(string pRwName, int pAmount): base(true){
		rewardName = pRwName;
		amount = pAmount;
	}
}
