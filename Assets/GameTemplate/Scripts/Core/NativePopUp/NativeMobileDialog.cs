using UnityEngine;
using System;
using UnionAssets.FLE;
using System.Collections;

public class NativeMobileDialog : EventDispatcherBase {
	
	public Action<NMDialogResult> OnComplete = delegate {};
	
	public NativeMobileDialog(string title, string message, string yes, string no )
	{
		#if UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8
		#if UNITY_ANDROID
		AndroidDialog dialog  = AndroidDialog.Create(title, message, yes, no);
		#endif
		#if UNITY_IPHONE
		IOSDialog dialog = IOSDialog.Create(title, message, yes, no);
		#endif
		#if UNITY_WP8
		WP8Dialog dialog  = WP8Dialog.Create(title, message);
		#endif
		dialog.addEventListener(BaseEvent.COMPLETE, OnCompleteListener);
		#endif
	}
	
	public static NativeMobileDialog Create(string title, string message)
	{
		return new NativeMobileDialog(title, message, "Yes", "No");
	}
	public static NativeMobileDialog Create(string title, string message, string yes, string no)
	{
		return new NativeMobileDialog(title, message, yes, no);
	}
	
	private void OnCompleteListener(CEvent e)
	{
		OnComplete((NMDialogResult)e.data);
		dispatch(BaseEvent.COMPLETE, e.data);
	}
}
