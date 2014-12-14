using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelExample : BaseLevel {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private List<Vector3> spawnPositions;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public List<Vector3> SpawnPositions {
		get {
			return this.spawnPositions;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="LevelExample"/> class.
	/// 
	/// ID, Player life, Game Lifes, Spawn Positions
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public LevelExample(string attributes): base(attributes){
		string[] att = attributes.Split(SEPARATOR_ATTRIBUTES);
		spawnPositions = new List<Vector3>();

		if(att.Length > 3){
			string[] positions = att[3].Split('|');

			foreach(string p in positions){
				string[] pos = p.Split(':');
				float x, y, z;

				if(float.TryParse(pos[0], out x) && float.TryParse(pos[1], out y) && float.TryParse(pos[2], out z)){
					spawnPositions.Add(new Vector3(x, y, z));
				}
				else{
					Debug.LogWarning("Error trying to retrive spawn positions");
				}
			}
		}
	}


	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString (){
		string s = "Spawn Positions:\n";

		foreach(Vector3 v in spawnPositions){
			s += "("+v.x+","+v.y+","+v.z+")\n";
		}

		return s;
	}
	
}
