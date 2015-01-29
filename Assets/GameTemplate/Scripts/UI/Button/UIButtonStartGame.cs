using UnityEngine;
using System.Collections;

public class UIButtonStartGame : UIBaseButton {
	protected override void doPress ()
	{
		base.doPress ();
		GameController.Instance.Manager.startGame();
	}
}
