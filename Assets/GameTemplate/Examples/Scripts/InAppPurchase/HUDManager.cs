using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDManager : MonoBehaviour {
	public Button Coins;
	public Text loaded;
	
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
		IAP_GameName.Instance.IOSRestorePurchase();
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
