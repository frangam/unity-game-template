using UnityEngine;
using System.Collections;

/// <summary>
/// This window is a grid scroll where we show all of the levels contained in the same pack
/// and we center the scroll to focus the last unlocked level in this pack 
/// </summary>
public class UIGridScrollLevelSelectorWin : UIBaseLevelSelectorWin {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private int initialLevel = 0;

	[SerializeField]
	private int finallLevel = 0;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------


	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
//		totalLevelsPack =  LevelPacks.Instance.getPackById(LevelPackId).TotalLevelsInPack;//(finallLevel-initialLevel)+1; //Ex: (100-1)+1 = 99+1 = 100 levels in the pack
	}

	public override void open ()
	{
		base.open ();
		StartCoroutine(centerScrollOntheCurrentLevelButtonUnlocked());
	}

	public override void initIndexCurrentItem ()
	{
		int last = PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED+LevelPackId);
		
		if(last == 0)
			PlayerPrefs.SetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED+LevelPackId, 1);
		
		lastUnlockedLevel = last == 0 ? 1: last;

		if(lastUnlockedLevel <= initialLevel)
			indexCurrentItem = 0;
		else if(lastUnlockedLevel >= finallLevel)
			indexCurrentItem = finallLevel-initialLevel;
		else if(lastUnlockedLevel >= initialLevel && lastUnlockedLevel <= finallLevel)
			indexCurrentItem = (lastUnlockedLevel % TotalLevelsPack) - 1;
		else
			indexCurrentItem = lastUnlockedLevel - 1; //-1 porque el primer nivel es el 1 y el indice debe empezar en 0;
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public IEnumerator centerScrollOntheCurrentLevelButtonUnlocked(){
		yield return null;// new WaitForSeconds(0.01f);
		if(CurrentLevelButton != null){
			
			ScrollRectEnsureVisible scroll = GetComponent<ScrollRectEnsureVisible>();
			
			if(scroll != null){
				RectTransform rt = CurrentLevelButton.GetComponent<RectTransform>();
				
				if(rt != null)
					scroll.CenterOnItem(rt);
			}
		}
	}

}
