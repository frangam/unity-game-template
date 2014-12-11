using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameActionResult : ISN_Result {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private GameActionID				currentActionId;
	private List<GameActionID>			actionsIds;
	private	GameActionOperation 		actionOperation;
	private int	gamePropertyValue;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public GameActionID CurrentActionId {
		get {
			return this.currentActionId;
		}
	}

	public List<GameActionID> ActionsIds {
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
	public GameActionResult(GameActionID pCurrentActionId, int pGamePropValue, GameActionOperation pActionOp = GameActionOperation.SET): base(true){
		currentActionId = pCurrentActionId;
		actionOperation = pActionOp;
		gamePropertyValue = pGamePropValue;
	}
	public GameActionResult(List<GameActionID> pActionsIds, int pGamePropValue, GameActionOperation pActionOp = GameActionOperation.SET): base(true){
		actionsIds = pActionsIds;
		actionOperation = pActionOp;
		gamePropertyValue = pGamePropValue;
	}
}
