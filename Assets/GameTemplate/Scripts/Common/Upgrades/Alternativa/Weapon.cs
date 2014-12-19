using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Weapon : MonoBehaviour {

	public List<Stat> Stats;

	void Start()
	{
		Stats.ForEach(x => x.LevelUp());
	}
}
