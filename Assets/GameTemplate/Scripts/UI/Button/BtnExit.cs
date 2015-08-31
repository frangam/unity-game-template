/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

/// <summary>
/// Exit application.
/// </summary>
public class BtnExit : UIBaseButton {
	protected override void doPress ()
	{
		base.doPress ();
		Application.Quit();
	}
}
