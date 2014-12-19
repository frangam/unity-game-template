using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseUpgradableObject<S,T> : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string currentModel;

	[SerializeField]
	private List<S> stats;

	[SerializeField]
	private S currentStat;


	//--------------------------------------
	// Public Methods
	//--------------------------------------

}
