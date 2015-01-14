using UnityEngine;
using System.Collections;


public class UIStoreServicesButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private StoreService service;
	
	[SerializeField]
	[Tooltip("Leave empty if you want to show all items. Don't leave empty if you want to show an specific item")]
	private string id;
	
	[SerializeField]
	[Tooltip("True if we want to get the ranking id from de GameManager")]
	private bool handledByGameManager = false;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		base.doPress ();
		
		switch(service){
		case StoreService.SCORES:
			string rankId = id;
			
			if(handledByGameManager)
				rankId = GameController.Instance.Manager.getRankingID();
			
			ScoresHandler.Instance.showRanking(rankId);
			break;
			
		case StoreService.ACHIEVEMENTS:
			BaseAchievementsManager.Instance.showAchievementsFromServer();
			break;
		}
	}
}
