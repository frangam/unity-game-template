/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
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
