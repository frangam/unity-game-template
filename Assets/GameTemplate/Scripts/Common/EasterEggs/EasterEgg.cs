/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using UnionAssets.FLE;

[System.Serializable]
public class EasterEgg {
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	private static EventDispatcherBase _dispatcher = new EventDispatcherBase();
	
	//--------------------------------------
	// Constants
	//--------------------------------------
	private const string PP_EASTER_EGG_REWARDED = "pp_easter_egg_rewarded_";
	private const string PP_EASTER_EGG_UNLOCKED = "pp_easter_egg_unlocked_";
	public const string EASTER_EGG_UNLOCKED = "ee_easter_egg_unlocked";
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string name;
	
	[SerializeField]
	private EasterEggID id;
	
	[SerializeField]
	[Tooltip("Can be achieved infinity times if True if false once")] 
	private bool infinite = false;
	
	[SerializeField]
	private EasterEggCode[] codes;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	/// <summary>
	/// The index of code's array matches with the current code input given
	/// </summary>
	private int indexMatched;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public EasterEggID Id {
		get {
			return this.id;
		}
	}
	
	public EasterEggCode[] Codes {
		get {
			return this.codes;
		}
	}
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void checkInputCode(EasterEggCode code){
		if(((!infinite && !isUnlocked()) || infinite) && indexMatched < codes.Length && codes[indexMatched] == code){
			indexMatched++;
			
			if(indexMatched >= codes.Length){
				PlayerPrefs.SetInt(PP_EASTER_EGG_UNLOCKED+(int)id, 1); //save in player prefs that was unlocked
				EasterEggResult res = new EasterEggResult(this);
				_dispatcher.dispatch(EASTER_EGG_UNLOCKED, res);
				
				if(infinite)
					resetMatches();
			}
		}
		//reset code sequence matched
		else if(((!infinite && !isUnlocked()) || infinite) && indexMatched < codes.Length && codes[indexMatched] != code){
			resetMatches();
		}
	}
	
	public void resetMatches(){
		indexMatched = 0;
	}
	
	public bool isUnlocked(){
		return (PlayerPrefs.GetInt(PP_EASTER_EGG_UNLOCKED+(int)id) == 1);
	}
	
	public bool isRewarded(){
		return (PlayerPrefs.GetInt(PP_EASTER_EGG_REWARDED+(int)id) == 1);
	}
	
	public void reward(){
		//		Debug.Log("Rewarded: " + id);
		PlayerPrefs.SetInt(PP_EASTER_EGG_REWARDED+(int)id, 1);
	}
	
	public static EventDispatcherBase dispatcher{
		get{return _dispatcher;}
	}
}
