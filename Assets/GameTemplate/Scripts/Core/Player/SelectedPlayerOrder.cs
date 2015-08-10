using UnityEngine;
using System.Collections;

public class SelectedPlayerOrder : MonoBehaviour {
	[SerializeField]
	private int order;
	
	public int Order {
		get {
			return this.order;
		}
	}
}
