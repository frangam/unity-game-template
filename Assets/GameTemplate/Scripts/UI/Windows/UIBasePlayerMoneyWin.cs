using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBasePlayerMoneyWin : UIBaseWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Text lbTotalMoney;

	[SerializeField]
	private Text lbTotalGems;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Update ()
	{
		base.Update ();

		if(lbTotalMoney)
			lbTotalMoney.text = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_MONEY).ToString();

		if(lbTotalGems)
			lbTotalGems.text = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_GEMS).ToString();
	}
}
