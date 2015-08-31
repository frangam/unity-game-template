/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseLevelPositionsManager : Singleton<BaseLevelPositionsManager> {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private List<BaseLevelPosition> positions;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public List<BaseLevelPosition> Positions {
		get {
			return this.positions;
		}
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		BaseLevelPosition[] pos = FindObjectsOfType<BaseLevelPosition>() as BaseLevelPosition[];

		if(pos != null)
			positions = new List<BaseLevelPosition>(pos);
		else
			Debug.LogError("Not found any ZSPosition in the map");
	}
	#endregion

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Gets the level position matches with the identifier given.
	/// </summary>
	/// <returns>The level position by identifier.</returns>
	/// <param name="id">Identifier.</param>
	public BaseLevelPosition getLevelPositionById(string id){
		BaseLevelPosition res = null;

		foreach(BaseLevelPosition lp in positions){
			if(lp.Id == id){
				res = lp;
				break;
			}
		}

		return res;
	}
}
