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
	private GameDifficulty difficulty = GameDifficulty.NONE;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void press (){
		base.press ();

		switch(service){
		case StoreService.SCORES:
			ScoresHandler.Instance.showRanking(difficulty);
			break;

		case StoreService.ACHIEVEMENTS:
			BaseAchievementsManager.Instance.showAchievementsFromServer();
			break;
		}
	}
}
