using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ScoresPack {
	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public int 					gameVersion;
	public List<string>			scoreIDs;
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	public ScoresPack(int pGameVersion, List<string> pScoresIDs = null){
		gameVersion = pGameVersion;
		
		if(pScoresIDs != null)
			scoreIDs = pScoresIDs;
		else
			scoreIDs = new List<string>();
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[ScoresPack: gameVersion={0}, scoreIDs={1}]", gameVersion, scoresToString());
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public string scoresToString(){
		string res = "";
		
		for(int i=0; i<scoreIDs.Count; i++){
			res += scoreIDs[i].ToString();
			
			if(i<scoreIDs.Count-1)
				res += ",";
		}
		
		return res;
	}
}
