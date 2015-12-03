/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class BtnRestorePurchases : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private UIBaseInAppWin window;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool active = false;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		active = InternetChecker.Instance.IsconnectedToInternet && CoreIAPManager.Instance.IsInited && CoreIAPManager.Instance.NumProducts > 0;
		gameObject.SetActive(active);

		if(!window)
			window = GetComponentInParent<UIBaseInAppWin>();
		
		if(window == null)
			Debug.LogError("Not found UIBaseInAppWin");
	}

	public override void Update ()
	{
		base.Update ();

		if(!active){
			active = InternetChecker.Instance.IsconnectedToInternet && CoreIAPManager.Instance.IsInited && CoreIAPManager.Instance.NumProducts > 0;

			if(active)
				gameObject.SetActive(true);
		}
	}

	protected override void doPress ()
	{
		base.doPress ();
		
		#if UNITY_IPHONE || UNITY_WP8 || UNITY_ANDROID
		if(window)
			window.restorePurchases();
		else
			Debug.LogError("Not found UIBaseInAppWin");
		#endif
	}
}
