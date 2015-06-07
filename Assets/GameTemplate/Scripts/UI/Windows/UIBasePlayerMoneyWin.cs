using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;

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
			lbTotalMoney.text = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_MONEY).ToString("N0", CultureInfo.CurrentCulture);
		
		if(lbTotalGems)
			lbTotalGems.text = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_GEMS).ToString("{0:n0}", CultureInfo.CurrentCulture);
	}
	
	
}
