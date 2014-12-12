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
	private bool unlocked = false;
	
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

		//Is incremental
		if(int.TryParse(atts[2], out aII)){
			isIncremental = aII != 0;
		}
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override bool loadedCorrectly (){
		return base.loadedCorrectly () && isIncremental != null;
	}

	public override void init (){
		idPlayerPrefs = "pp_achievement_unlocked_" + Id;
	}

	public override string ToString (){
		return string.Format ("[Achievement: id={1}, name={3}, description={0}, isIncremental={4}, unlocked={2}]", Description, Id, unlocked, Name, IsIncremental);
	}
}
