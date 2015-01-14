using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICloseButton : UIBaseButton {
	public Image panel;
	
	protected override void doPress ()
	{
		base.doPress ();
		
		if(!panel){
			panel = GetComponent<Image>();
		}
		
		if(panel != null)
			panel.gameObject.SetActive(false);
		else
			Debug.LogWarning("Not attached Panel to close");
	}
}
