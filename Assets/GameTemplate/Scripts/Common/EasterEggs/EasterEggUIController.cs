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

}
