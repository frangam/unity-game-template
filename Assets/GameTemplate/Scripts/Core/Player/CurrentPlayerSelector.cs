/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class CurrentPlayerSelector : Singleton<CurrentPlayerSelector> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool useTheGameMultiversionAsOrder = true;

	[SerializeField]
	private int anotherOrder = 0;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private SelectedPlayerOrder[] allPlayerObjects;
	private SelectedPlayerOrder selected;
	private int currentOrder;

	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public SelectedPlayerOrder Selected {
		get {
			return this.selected;
		}
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	void Awake(){
		currentOrder = useTheGameMultiversionAsOrder ? GameSettings.Instance.currentGameMultiversion : anotherOrder;
		allPlayerObjects = GetComponentsInChildren<SelectedPlayerOrder>();

		if(allPlayerObjects != null && allPlayerObjects.Length > 0){
			//get the selected car index
			foreach(SelectedPlayerOrder sp in allPlayerObjects){		
				//enable only the car corresponding to this game multiversion
				bool active = sp != null && sp.Order == currentOrder;
				sp.gameObject.SetActive(active);

				if(active && selected == null){
					selected = sp;
				}
			}
		}
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void nextPlayer(){
		if(allPlayerObjects != null){
			if(currentOrder+1 > allPlayerObjects.Length)
				currentOrder = 0;
			else
				currentOrder++;

			selected.gameObject.SetActive(false); //hide the previous selected object
			selected = allPlayerObjects[currentOrder];
			selected.gameObject.SetActive(true); //show the current selected object
		}
		else{
			GTDebug.logErrorAlways("Not found all player objects for selecting");
		}
	}

	public void previousPlayer(){
		if(allPlayerObjects != null){
			if(currentOrder-1 < 0)
				currentOrder = allPlayerObjects.Length-1;
			else
				currentOrder--;
			
			selected.gameObject.SetActive(false); //hide the previous selected object
			selected = allPlayerObjects[currentOrder];
			selected.gameObject.SetActive(true); //show the current selected object
		}
		else{
			GTDebug.logErrorAlways("Not found all player objects for selecting");
		}
	}
}
