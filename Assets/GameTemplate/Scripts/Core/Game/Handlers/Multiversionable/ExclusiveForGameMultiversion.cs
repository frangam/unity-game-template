using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExclusiveForGameMultiversion : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private List<int> multiversionIndexes;
	
	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public List<int> MultiversionIndexes {
		get {
			return this.multiversionIndexes;
		}
	}
	
	//--------------------------------------
	// Unity Method
	//--------------------------------------
	void Awake(){
		gameObject.SetActive (multiversionIndexes.Contains(GameSettings.Instance.currentGameMultiversion));
	}
	
	
	
}
