using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICloseButton : UIBaseButton {
	public Image panel;

	public override void press (){
		base.press ();

		panel.gameObject.SetActive(false);
	}
}
