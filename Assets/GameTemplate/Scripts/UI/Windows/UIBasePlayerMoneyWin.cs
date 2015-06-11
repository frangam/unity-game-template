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
			lbTotalMoney.text = GameMoneyManager.Instance.getTotalMoney().ToString("N0", CultureInfo.CurrentCulture);
		
		if(lbTotalGems)
			lbTotalGems.text = GameMoneyManager.Instance.getTotalGems().ToString("{0:n0}", CultureInfo.CurrentCulture);
	}
	
	
}
