using UnityEngine;
using System.Collections;

public class IOSBillingInitChecker 
{
	public delegate void BillingInitListener();

	BillingInitListener _listener;


	public IOSBillingInitChecker(BillingInitListener listener) {
		_listener = listener;

		if(IOSInAppPurchaseManager.Instance.IsStoreLoaded) {
			_listener();
		} else {

			IOSInAppPurchaseManager.Instance.addEventListener(IOSInAppPurchaseManager.STORE_KIT_INITIALIZED, OnStoreKitInit);
			if(!IOSInAppPurchaseManager.Instance.IsWaitingLoadResult) {
				IOSInAppPurchaseManager.Instance.loadStore();
			}
		}
	}

	private void OnStoreKitInit() {
		IOSInAppPurchaseManager.Instance.removeEventListener(IOSInAppPurchaseManager.STORE_KIT_INITIALIZED, OnStoreKitInit);
		_listener();
	}

}

