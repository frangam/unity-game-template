/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameActionResult : BaseDataResult {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private string					currentActionId;
	private List<string>				actionsIds;
	private	GameActionOperation 		actionOperation;
	private int	gamePropertyValue;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string CurrentActionId {
		get {
			return this.currentActionId;
		}
	}

	public List<string> ActionsIds {
		get {
			return this.actionsIds;
		}
	}

	public int GamePropertyValue {
		get {
			return this.gamePropertyValue;
		}
	}

	public GameActionOperation ActionOperation {
		get {
			return this.actionOperation;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public GameActionResult(string pCurrentActionId, int pGamePropValue, GameActionOperation pActionOp = GameActionOperation.SET): base(true){
		currentActionId = pCurrentActionId;
		actionOperation = pActionOp;
		gamePropertyValue = pGamePropValue;
	}
	public GameActionResult(List<string> pActionsIds, int pGamePropValue, GameActionOperation pActionOp = GameActionOperation.SET): base(true){
		actionsIds = pActionsIds;
		actionOperation = pActionOp;
		gamePropertyValue = pGamePropValue;
	}
}
