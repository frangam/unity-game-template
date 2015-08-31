/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BaseLevelPack{
	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public string 		id;
	public int 			initialLevel;
	public int 			totalLevelsInPack;

	/// <summary>
	/// The required levels packs to unlock this pack.
	/// </summary>
	public List<int>	reqPacksToUnlock;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private float 		progress = 0;


	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string Id {
		get {
			return this.id;
		}
		set{
			this.id = value;
		}
	}

	public int InitialLevel {
		get {
			return this.initialLevel;
		}
		set{
			this.initialLevel = value;
		}
	}

	public int FinalLevel {
		get {
			return (initialLevel+totalLevelsInPack)-1; //ex: (1+100)-1 = 99 + 1 = 100. ex2: (101+100)-1 = 201-1 = 200
		}
	}

	public int TotalLevelsInPack{
		get{
			return totalLevelsInPack;
		}
		set{
			this.totalLevelsInPack = value;
		}
	}

	public bool FullyCompleted{
		get{
			return ProgressCompleted >= 1f;
		}
	}

	private float Progress{
		get{
			loadProgress();

			return this.progress;
		}
	}

	/// <summary>
	/// Gets the progress completed a value between 0 and 1
	/// </summary>
	/// <value>The progress completed.</value>
	public float ProgressCompleted{
		get{
			return Progress / totalLevelsInPack; //total levels in pack --> 1f : progress --> progress completed
		}
	}

	/// <summary>
	/// Gets the percentage of the progress completed.
	/// </summary>
	/// <value>The percentage progress completed.</value>
	public float PercentageProgressCompleted{
		get{
			return (Progress*100)/totalLevelsInPack; //total levels in pack --> 100% : progress --> % progress completed
		}
	}

	/// <summary>
	/// Gets the required levels packs to unlock this pack.
	/// </summary>
	/// <value>The required packs to unlock this.</value>
	public List<int> RequiredPacksToUnlockThis {
		get {
			return this.reqPacksToUnlock;
		}
		set{
			this.reqPacksToUnlock = value;
		}
	}

	/// <summary>
	/// Gets a value indicating whether this instance has any required levels packs to unlock this pack.
	/// </summary>
	/// <value><c>true</c> if this instance has any required packs to unlock this; otherwise, <c>false</c>.</value>
	public bool HasAnyRequiredPacksToUnlockThis{
		get{
			return RequiredPacksToUnlockThis != null && RequiredPacksToUnlockThis.Count > 0;
		}
	}
	
	public bool AllRequiredPacksCompleted{
		get{
			bool allCompleted = true;

			if(HasAnyRequiredPacksToUnlockThis && LevelPacks.Instance.packs != null && LevelPacks.Instance.packs.Count > 0){
				foreach(int reqPackId in RequiredPacksToUnlockThis){
					foreach(BaseLevelPack pack in LevelPacks.Instance.packs){
						int packId;

						if(int.TryParse(pack.id, out packId) && packId == reqPackId){
							allCompleted = pack.FullyCompleted;
							break;
						}
					}

					if(!allCompleted)
						break;
				}
			}

			return allCompleted;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public BaseLevelPack(string pId, int pInitialLevel, int pTotalLevels, List<int> pReqsLevels = null){
		//id
		id = pId;

		//initial level
		initialLevel = pInitialLevel;

		//total levels
		totalLevelsInPack = pTotalLevels;
		
		//required levels packs to unlock this pack
		reqPacksToUnlock = pReqsLevels;

		//load progress
		loadProgress();
	}


	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[BaseLevelPack: id={0}, initialLevel={1}, totalLevelsInPack={2}, HasAnyRequiredPacksToUnlockThis={3}, FullyCompleted={4}, PercentageCompleted={5}]", id, initialLevel, totalLevelsInPack, HasAnyRequiredPacksToUnlockThis, FullyCompleted, PercentageProgressCompleted);
	}
	

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void loadProgress(){
		int lastCompletedLevelInPack = PlayerPrefs.GetInt(GameSettings.PP_LAST_CAMPAIGN_LEVEL_COMPLETED+id);
		
		if(lastCompletedLevelInPack <= initialLevel)
			progress = 0f;
		else if(lastCompletedLevelInPack >= FinalLevel)
			progress = totalLevelsInPack;
		else if(lastCompletedLevelInPack >= initialLevel && lastCompletedLevelInPack <= FinalLevel)
			progress = (lastCompletedLevelInPack % totalLevelsInPack);
	}
	
}
