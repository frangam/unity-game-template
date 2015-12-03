/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
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
		UIBaseWindow win = GetComponent<UIBaseWindow>();
		bool active = multiversionIndexes.Contains(GameSettings.Instance.currentGameMultiversion);

		if(win != null && !active && win.NotStartWithManager)
			UIController.Instance.Manager.close(win, !win.NotStartWithManager);
		else if(win != null && active && win.NotStartWithManager)
			UIController.Instance.Manager.open(win, !win.NotStartWithManager);
		else 
			gameObject.SetActive (active);
	}


	
}
