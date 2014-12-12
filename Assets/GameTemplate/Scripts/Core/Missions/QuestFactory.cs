using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class QuestFactory {
	public static BaseQuest create(System.Type type, string attributes, List<GameAction> actions){
		BaseQuest res = null;

		if(type == typeof(Achievement)){
			res = new Achievement(attributes, actions);
		}
		else{
			res = new BaseQuest(attributes, actions);
		}

		return res;
	}
}
