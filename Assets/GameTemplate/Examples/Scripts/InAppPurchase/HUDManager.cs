using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDManager : MonoBehaviour {
	public Button Coins;
	public Text loaded;
	public Button IosRestore;
	
	void Awake()
	{
		#if UNITY_IPHONE
		IosRestore.gameObject.SetActive(true);
		#endif
	}
	public void Consume(string SKU)
	{
		if(IAP_GameName.Instance.IsInited)
		{
			//IAPManager.Instance.Purchase(SKU);
			IAP_GameName.Instance.Purchase(SKU);
		}
	}
	/// <summary>
	/// Se usa para restaurar las compras hechas en ios.
	/// </summary>
	public void RestoreIOS()
	{
		IAP_GameName.Instance.RestorePurchase();
	}
	void Update()
	{
		if(IAP_GameName.Instance.IsInited)
		{
			loaded.text = "Shop Inicialized";
			Coins.interactable = true;
		}
	}
	
}
