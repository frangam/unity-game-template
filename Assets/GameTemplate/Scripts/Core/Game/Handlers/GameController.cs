/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

[AddComponentMenu("Base/GameController")]
public class GameController : Singleton<GameController> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private BaseGameManager manager;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public BaseGameManager Manager {
		get {
			return this.manager;
		}
	}


	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		if(manager == null){
			manager = gameObject.getFirstComponentInChildren<BaseGameManager>() as BaseGameManager;

			if(manager == null){
				Debug.LogError("Could not found the BaseGameManager in children or not attached");
			}
		}
	}
	#endregion

	//--------------------------------------
	// Public Methods
	//--------------------------------------

	
}
