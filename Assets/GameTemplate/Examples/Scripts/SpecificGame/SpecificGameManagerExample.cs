using UnityEngine;
using System.Collections;

public class SpecificGameManagerExample : BaseGameManager {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private int killedZombies = 0;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public int KilledZombies {
		get {
			return this.killedZombies;
		}
		set {
			killedZombies = value;
		}
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	protected override void Awake (){
		base.Awake ();
	}
	#endregion
}
