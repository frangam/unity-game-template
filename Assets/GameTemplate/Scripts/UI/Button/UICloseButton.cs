/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Show or hide an image panel
/// </summary>
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
