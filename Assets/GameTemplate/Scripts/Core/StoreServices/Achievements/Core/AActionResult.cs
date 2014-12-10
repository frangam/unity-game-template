using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AActionResult : ISN_Result {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private AActionID			currentActionId;
	private List<AActionID>		actionsIds;
	private	AActionOperation 	actionOperation;
	private int					gamePropertyValue;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public AActionID CurrentActionId {
		get {
			return this.currentActionId;
		}
	}

	public List<AActionID> ActionsIds {
		get {
			return this.actionsIds;
		}
	}

	public int GamePropertyValue {
		get {
			return this.gamePropertyValue;
		}
	}

	public AActionOperation ActionOperation {
		get {
			return this.actionOperation;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public AActionResult(AActionID pCurrentActionId, int pGamePropValue, AActionOperation pActionOp = AActionOperation.SET): base(true){
		currentActionId = pCurrentActionId;
		actionOperation = pActionOp;
		gamePropertyValue = pGamePropValue;
	}
	public AActionResult(List<AActionID> pActionsIds, int pGamePropValue, AActionOperation pActionOp = AActionOperation.SET): base(true){
		actionsIds = pActionsIds;
		actionOperation = pActionOp;
		gamePropertyValue = pGamePropValue;
	}
}
