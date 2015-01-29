using UnityEngine;
using System.Collections;

/// <summary>
/// Base easter egg awards controller, it is needed to implement a child class inherits from it
/// to handle specific rewards
/// </summary>
public class BaseEasterEggRewardsController : PersistentSingleton<BaseEasterEggRewardsController> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool reloadScene = true;
	
	[SerializeField]
	private float delayBeforReload = 2.5f;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private EasterEgg easterEgg;
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	
	/// <summary>
	/// For specific implementation in every game
	/// we handle which reward give to the user
	/// </summary>
	/// <param name="easterEgg">Easter egg.</param>
	public virtual void handleReward(EasterEgg easterEgg){
		
	}
	
	public void reward(EasterEgg pEasterEgg){
		easterEgg = pEasterEgg;
		handleReward(easterEgg);
		markAsRewarded(easterEgg);
	}
	
	public void markAsRewarded(EasterEgg pEasterEgg){
		pEasterEgg.reward();
	}
	
	public void showNotificationPanel(){
		if(easterEgg != null && easterEgg.Id != EasterEggID.EASTER_EGG_NONE && EasterEggUIController.Instance.NotificationsPanels != null && EasterEggUIController.Instance.NotificationsPanels.Length > 0){
			foreach(EasterEggUINotificationPanel n in EasterEggUIController.Instance.NotificationsPanels){
				if(n!= null && n.Id == easterEgg.Id){
					n.gameObject.SetActive(true);
					n.show();
					StartCoroutine(reloadSceneProcess(n));
					break;
				}
			}
		}
	}
	
	public IEnumerator reloadSceneProcess(EasterEggUINotificationPanel notification){
		yield return new WaitForSeconds(delayBeforReload);
		
		if(notification != null){
			notification.gameObject.SetActive(false);
		}
		
		if(reloadScene)
			ScreenLoaderVisualIndicator.Instance.LoadScene(Application.loadedLevelName);
		
		
	}
}
