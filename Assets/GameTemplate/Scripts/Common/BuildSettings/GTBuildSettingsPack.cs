/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GTBuildSettingsPack {
	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public int 						gameVersion;
	public GTBuildSettings			build;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public int GameVersion {
		get {
			return this.gameVersion;
		}
		set {
			gameVersion = value;
		}
	}

	public GTBuildSettings Build {
		get {
			return this.build;
		}
		set {
			build = value;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public GTBuildSettingsPack(int pGameVersion, GTBuildSettings pBuild = null){
		gameVersion = pGameVersion;

		if(pBuild != null)
			build = new GTBuildSettings(pBuild);
		else
			build = new GTBuildSettings();
	}



	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[GTBuildSettingsPack: gameVersion={0} build={1}]"
		                      , gameVersion, build.ToString());
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------

}
