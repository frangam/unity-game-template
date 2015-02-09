using UnityEngine;
using System.Collections;

public class UIConfirmDialogPayWithGemsUpgradeWin : UIBaseConfimationPurchaseWindow {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private UpgradeStatButton upgradeButton;

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(UpgradeStatButton button){
		upgradeButton = button;
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void confirmPurchase ()
	{
		if(upgradeButton != null)
			upgradeButton.upgrade(false);
	}
}
