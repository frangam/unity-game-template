using UnityEngine;
using System.Collections;

public class AActionResult : ISN_Result {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private AActionID 			actionId;
	private	AActionOperation 	actionOperation;
	private int					gamePropertyValue;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public AActionID ActionId {
		get {
			return this.actionId;
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
	public AActionResult(AActionID pActionId, int pGamePropValue, AActionOperation pActionOp = AActionOperation.ADD): base(true){
		actionId = pActionId;
		actionOperation = pActionOp;
		gamePropertyValue = pGamePropValue;
	}
}
