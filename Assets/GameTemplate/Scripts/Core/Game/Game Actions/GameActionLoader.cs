using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameActionLoader {
	public static List<GameAction> getGameActionsFromTextFile(string filename){
		List<GameAction> res = new List<GameAction>();
		TextAsset text = Resources.Load(filename) as TextAsset;
		string fileContents = text.text;
		string[] lines = fileContents.Split('\n');

		foreach(string line in lines){
			if(line != null && line != "" && line.Length > 0){
				GameAction action = new GameAction(line);

				if(action != null && action.loadedCorrectly()){
					res.Add(action);
				}
			}
		}

		return res;
	}
}
