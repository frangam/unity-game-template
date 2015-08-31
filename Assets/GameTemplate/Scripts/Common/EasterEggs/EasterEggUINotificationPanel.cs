/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class EasterEggUINotificationPanel : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private EasterEggID id;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public EasterEggID Id {
		get {
			return this.id;
		}
	}
	
	//	//--------------------------------------
	//	// Unity Methods
	//	//--------------------------------------
	//	void Awake(){
	//		show(false);
	//	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void show(bool pShow = true){
		gameObject.SetActive(pShow);
	}
}
