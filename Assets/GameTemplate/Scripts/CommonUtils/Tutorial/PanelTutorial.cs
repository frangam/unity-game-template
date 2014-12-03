using UnityEngine;
using System;
using System.Collections;

public class PanelTutorial : MonoBehaviour, IComparable {
	[SerializeField]
	private int order = 0;

	[SerializeField]
	private bool gameStart = false;

	[SerializeField]
	private bool quitarPausa = true;

	public int Order {
		get {
			return this.order;
		}
	}

	public int CompareTo (object obj){
		return this.order.CompareTo (((PanelTutorial)obj).Order);
	}

	void Start(){
		Time.timeScale = quitarPausa ? 1f : 0f;

		GameManager.gameStart = gameStart;
	}
}
