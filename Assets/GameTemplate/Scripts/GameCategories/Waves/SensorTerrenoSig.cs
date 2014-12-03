using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SensorTerrenoSig : MonoBehaviour {
	public List<string> tags;
	public TerrainType terrenoSig;

	void OnTriggerEnter2D (Collider2D collider) {
		if(tags.Contains(collider.tag)){
			TerrainPart t = collider.GetComponent<TerrainPart>();

			terrenoSig = t.Type;
		}
	}
}
