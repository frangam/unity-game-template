using UnityEngine;
using System.Collections;

public class CRS_EasterEggsRewardsController : BaseEasterEggRewardsController {
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void handleReward (EasterEgg easterEgg){
		switch(easterEgg.Id){
			//--------------------------------------------------
			//Add 50000 monedas
			//--------------------------------------------------
		case EasterEggID.EASTER_EGG_0:
			int monedasPrevias = PlayerPrefs.GetInt("crs_coins");
			int monedasFinal = monedasPrevias+50000;
			PlayerPrefs.SetInt("crs_coins", monedasFinal);
			showNotificationPanel();
			break;
		}
	}

}
