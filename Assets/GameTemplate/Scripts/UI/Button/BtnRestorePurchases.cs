using UnityEngine;
using System.Collections;

public class BtnRestorePurchases : UIBaseButton {
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		base.doPress ();

#if UNITY_IPHONE
		IAP_GameName.Instance.IOSRestorePurchase();
#endif
	}
}
