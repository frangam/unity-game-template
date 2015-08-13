using UnityEngine;
using System.Collections;

public class V4VCResult : ISN_Result {
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
