/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class ButtonChangeInputBackButtonBehaviour : UIBaseButton {
	[SerializeField]
	private ChangeInputBackButtonBehaviour changeIpBB;
	
	protected override void doPress ()
	{
		base.doPress ();
		
		if(changeIpBB != null)
			changeIpBB.change();
		else
			Debug.LogError("Not found ChangeInputBackButtonBehaviour");
	}
}
