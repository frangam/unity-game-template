/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class UIButtonStartGame : UIBaseButton {
	protected override void doPress ()
	{
		base.doPress ();
		GameController.Instance.Manager.startGame();
	}
}
