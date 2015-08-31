/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AchievementsPack {
	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public int 					gameVersion;
	public List<Achievement>	achievements;

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public AchievementsPack(int pGameVersion, List<Achievement> pAchievements = null){
		gameVersion = pGameVersion;
		
		if(pAchievements != null)
			achievements = pAchievements;
		else
			achievements = new List<Achievement>();
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[AchievementsPack: gameVersion={0}, achievements={1}]", gameVersion, achievementsToString());
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public string achievementsToString(){
		string res = "";
		
		for(int i=0; i<achievements.Count; i++){
			res += achievements[i].ToString();
			
			if(i<achievements.Count-1)
				res += ",";
		}
		
		return res;
	}
}
