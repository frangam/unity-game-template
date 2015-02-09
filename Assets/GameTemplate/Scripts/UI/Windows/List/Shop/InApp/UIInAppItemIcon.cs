using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIInAppItemIcon : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Image icon;

	[SerializeField]
	private string inAppItemType = UIBaseInAppItem.GEM_ITEM;
}
