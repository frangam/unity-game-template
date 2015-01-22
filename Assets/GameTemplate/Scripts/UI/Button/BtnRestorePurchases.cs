using UnityEngine;
using System.Collections;

public class BtnRestorePurchases : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private UIBaseInAppWin window;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		
		if(!window)
			window = GetComponentInParent<UIBaseInAppWin>();
		
		if(window == null)
			Debug.LogError("Not found UIBaseInAppWin");
	}
	protected override void doPress ()
	{
		base.doPress ();
		
		#if UNITY_IPHONE
		if(window)
			window.restorePurchases();
		else
			Debug.LogError("Not found UIBaseInAppWin");
		#endif
	}
}
