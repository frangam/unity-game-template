/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

[System.Serializable]
public class GTBuildScene {
	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public string name;
	public string path;
	public GameSection section;


	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	public string Path {
		get {
			return this.path;
		}
		set {
			path = value;
		}
	}

	public GameSection Section {
		get {
			return this.section;
		}
		set {
			section = value;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public GTBuildScene() : this("", ""){}
	public GTBuildScene(GTBuildScene scene) :this(scene.name, scene.path, scene.section){}
	public GTBuildScene(string pName, string pPath, GameSection pSection = GameSection.GAME){
		name = pName;
		path = pPath;
		section = pSection;
	}
}
