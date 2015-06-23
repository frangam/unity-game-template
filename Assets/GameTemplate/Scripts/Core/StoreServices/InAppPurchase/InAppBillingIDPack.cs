using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class InAppBillingIDPack {
	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public int 				gameVersion;
	public List<string>		ids;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public int GameVersion {
		get {
			return this.gameVersion;
		}
		set {
			gameVersion = value;
		}
	}

	public List<string> IDs {
		get {
			return this.ids;
		}
		set {
			ids = value;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public InAppBillingIDPack(int pGameVersion, List<string> pIds = null){
		gameVersion = pGameVersion;

		if(pIds != null)
			ids = pIds;
		else
			ids = new List<string>();
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[InAppBillingIDPack: gameVersion={0}, ids={1}]", gameVersion, idsToString());
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public string idsToString(){
		string res = "";

		for(int i=0; i<ids.Count; i++){
			res += ids[i];

			if(i<ids.Count-1)
				res += ",";
		}

		return res;
	}
}
