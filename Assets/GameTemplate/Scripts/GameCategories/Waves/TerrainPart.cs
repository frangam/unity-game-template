using UnityEngine;
using System.Collections;

public class TerrainPart : MonoBehaviour {
	[SerializeField]
	private TerrainType type;

	public TerrainType Type {
		get {
			return this.type;
		}
	}
}
