/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class EasterEggUIController : Singleton<EasterEggUIController> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private EasterEggUINotificationPanel[] notificationsPanels;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public EasterEggUINotificationPanel[] NotificationsPanels {
		get {
			return this.notificationsPanels;
		}
	}
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	public virtual void Awake(){
		bool panelsNotFound = notificationsPanels == null || (notificationsPanels != null && notificationsPanels.Length == 0);
		
		if(panelsNotFound){
			EasterEggUINotificationPanel[] aux = FindObjectsOfType<EasterEggUINotificationPanel>() as EasterEggUINotificationPanel[];
			notificationsPanels = aux;
			
			
			if(notificationsPanels != null && notificationsPanels.Length > 0)
				init();
			else
				Debug.LogWarning("Not found any EasterEggUINotificationPanel");
		}
		else
			init();
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void init(){
		if(notificationsPanels != null){
			//init panels closed
			foreach(EasterEggUINotificationPanel n in notificationsPanels){
				n.gameObject.SetActive(false);
				n.show(false);
			}
		}
	}
}
