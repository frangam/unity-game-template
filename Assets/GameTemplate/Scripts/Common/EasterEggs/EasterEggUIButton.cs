/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
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
