using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EasterEggUIButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private EasterEggCode code;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		EasterEggsController.Instance.checkCode(code);
	}
}
