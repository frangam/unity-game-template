using UnityEngine;
using System;
using UnionAssets.FLE;
using System.Collections;

public class NativeMobileMessage : NMPopUp {

	public Action OnComplete = delegate {};

	public NativeMobileMessage(string title, string message, string ok)
	{
		#if UNITY_ANDROID
		AndroidMessage msg = AndroidMessage.Create(title, message, ok);
		#endif
		#if UNITY_IPHONE
		IOSMessage msg = IOSMessage.Create(title, message, ok);
		#endif
		#if UNITY_WP8
		WP8Message msg = WP8Message.Create(title, message);
		#endif
		msg.addEventListener(BaseEvent.COMPLETE, OnCompleteListener);
	}

	public static NativeMobileMessage Create(string title, string message)
	{
		return new NativeMobileMessage(title, message, "Ok");
	}
	public static NativeMobileMessage Create(string title, string message, string ok)
	{
		return new NativeMobileMessage(title, message, ok);
	}

	private void OnCompleteListener(CEvent e)
	{
		OnComplete();
		dispatch(BaseEvent.COMPLETE, e.data);
	}
}
