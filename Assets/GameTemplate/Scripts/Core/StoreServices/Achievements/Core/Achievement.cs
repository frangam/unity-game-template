/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Achievement : BaseQuest{
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool isIncremental = false;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private string stgActions;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public bool IsIncremental {
		get {
			return this.isIncremental;
		}
	}
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	public Achievement():base(){
		isIncremental = false;
	}
	public Achievement(Achievement aAchievement):base(aAchievement){
		isIncremental = aAchievement.IsIncremental;
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Achievement"/> class.
	/// 
	/// Attributes:
	/// ID, Actions, isIncremental
	/// 
	/// Actions: a1.a2.a3.a4.
	/// isIncremental: 0 (false) or 1 (true)
	/// 
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	/// <param name="pAllGameActions">P all game actions.</param>
	public Achievement(string attributes, List<GameAction> pAllGameActions): base(attributes, pAllGameActions){
		string[] atts = attributes.Split(SEPARATOR_ATTRIBUTES);
		int aII;
		
		stgActions = atts[1];
		
		//Is incremental
		if(int.TryParse(atts[2], out aII)){
			isIncremental = aII != 0;
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override bool loadedCorrectly (){
		return (Id != null && Actions != null && Actions.Count > 0 && isIncremental != null);
	}
	
	public override void init (){
		idPlayerPrefs = "pp_achievement_unlocked_" + Id;
	}
	
	public override string ToString (){
		return string.Format ("[Achievement: id={0}, actions={1}, name={2}, description={3}, isIncremental={4}, unlocked={5}]", Id, actionsToString(), Name, Description, isIncremental, Completed);
	}
	
	public override string actionsToString ()
	{
		string actionsStr = "";
		
		for(int i=0; i<actions.Count; i++){
			actionsStr += actions[i].Id;
			
			if(i<actions.Count-1){
				actionsStr += SEPARATOR_ACTIONS_IDS;
			}
		}
		
		return actionsStr;
	}
}
