using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnionAssets.FLE;

public class EasterEggsController : PersistentSingleton<EasterEggsController> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private List<EasterEgg> easterEggs;

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		resetAllInputs();
		EasterEgg.dispatcher.addEventListener(EasterEgg.EASTER_EGG_UNLOCKED, OnEasterEggUnlocked);
	}
	#endregion

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void checkCode(EasterEggCode code){
//		Debug.Log("Checking EasterEgg code: " + code);

		if(code == EasterEggCode.CODE_RESET){
			resetAllInputs();
		}
		else{
			foreach(EasterEgg ee in easterEggs){
				ee.checkInputCode(code);
			}
		}
	}

	public void resetAllInputs(){
		foreach(EasterEgg ee in easterEggs){
			ee.resetMatches();
		}
	}

	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	public void OnEasterEggUnlocked(CEvent e){
		EasterEggResult result = e.data as EasterEggResult;

		if(result.IsSucceeded){
			//reward of easter egg
			BaseEasterEggRewardsController.Instance.reward(result.EasterEgg);
		}
	}
}
