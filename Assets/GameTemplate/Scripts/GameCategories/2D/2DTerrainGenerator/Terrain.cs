using UnityEngine;
using System.Collections;

public class Terrain : MonoBehaviour {
	[SerializeField]
	private TerrainType source;

	[SerializeField]
	private TerrainType destination;

	[SerializeField]
	private Priority priority = Priority.MINIMUM;

	public TerrainType Source {
		get {
			return this.source;
		}
	}

	public TerrainType Destination {
		get {
			return this.destination;
		}
	}

	public Priority Priority {
		get {
			return this.priority;
		}
	}

	/// <summary>
	/// Si el origen es diferente al destino la carretera es de transicion entre dos
	/// </summary>
	/// <returns><c>true</c>, if hibrida was esed, <c>false</c> otherwise.</returns>
	public bool isHybrid(){
		return source != destination;
	}

//	void Awake(){
//		float x = (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)).x*2)+Camera.main.transform.position.x;
//		transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
//	}
}
