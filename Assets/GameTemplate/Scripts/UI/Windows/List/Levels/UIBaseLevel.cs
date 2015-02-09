using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBaseLevel : UIBaseListItem {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Text lbDescription;

	[SerializeField]
	private Text lbMoneyReward;

	[SerializeField]
	private Text lbGemsReward;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected BaseLevel level;


	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public BaseLevel Level {
		get {
			return this.level;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public UIBaseLevel(string data, Animator anim = null): base(data, anim){
		level = new BaseLevel(data);
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------


	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void show ()
	{
		base.show ();

		if(lbDescription)
			lbDescription.text = level.LocalizedDescription;

		if(lbMoneyReward)
			lbMoneyReward.text = level.MoneyReward.ToString();

		if(lbGemsReward)
			lbGemsReward.text = level.GemsReward.ToString();
	}
}
