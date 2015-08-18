using UnityEngine;
using System.Collections;

public class SelectedPlayerOrder : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private int order;
	
	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public int Order {
		get {
			return this.order;
		}
	}
}
