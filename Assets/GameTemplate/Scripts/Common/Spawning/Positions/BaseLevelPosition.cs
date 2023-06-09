﻿/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class BaseLevelPosition : CircleSpawn {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string id;


	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string Id {
		get {
			return this.id;
		}
	}


	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString (){
		return string.Format ("[BaseLevelPosition: id={0}]", id);
	}
}
