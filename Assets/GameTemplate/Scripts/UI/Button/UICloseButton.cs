using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICloseButton : UIBaseButton {
	public Image panel;
	
	public override void press (){
		base.press ();
		
		
		if(!panel){
			panel = GetComponent<Image>();
		}
		
		if(panel != null)
			panel.gameObject.SetActive(false);
		else
			Debug.LogWarning("Not attached Panel to close");
	}
}
