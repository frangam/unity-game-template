using UnityEngine;
using System;
using UnionAssets.FLE;
using System.Collections;

public class NativeMobileRateUs : NMPopUp {

	public Action<NMDialogResult> OnComplete = delegate {};

	public NativeMobileRateUs(string title, string message, string url, string yes, string later, string no)
	{
		#if UNITY_ANDROID
		AndroidRateUsPopUp rate = AndroidRateUsPopUp.Create(title, message, url, yes, later, no);
		#endif
		#if UNITY_IPHONE
		IOSRateUsPopUp rate = IOSRateUsPopUp.Create(title, message, yes, later, no);
		#endif
		#if UNITY_WP8
		WP8RateUsPopUp rate = WP8RateUsPopUp.Create(title, message);
		#endif
		rate.addEventListener(BaseEvent.COMPLETE, OnCompleteListener);
	}
	
	public static NativeMobileRateUs Create(string title, string message, string url)
	{
		return new NativeMobileRateUs(title, message, url, "Rate app", "Later", "No, thanks");
	}
	public static NativeMobileRateUs Create(string title, string message, string url, string yes, string later, string no)
	{
		return new NativeMobileRateUs(title, message, url, yes, later, no);
	}
	public static void OpenAppPage(string url)
	{
		AndroidNativeUtility.OpenAppRatingPage(url);
	}
	private void OnCompleteListener(CEvent e)
	{
		OnComplete((NMDialogResult)e.data);
		dispatch(BaseEvent.COMPLETE, e.data);
	}
}
