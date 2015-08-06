using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ScoresPack {
	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public int 					gameVersion;
	public List<Score>			scores;
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	public ScoresPack(int pGameVersion, List<Score> pScores = null){
		gameVersion = pGameVersion;
		
		if(pScores != null)
			scores = pScores;
		else
			scores = new List<Score>();
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[ScoresPack: gameVersion={0}, scores={1}]", gameVersion, scoresToString());
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public string scoresToString(){
		string res = "";
		
		for(int i=0; i<scores.Count; i++){
			res += scores[i].ToString();
			
			if(i<scores.Count-1)
				res += ",";
		}
		
		return res;
	}
}
