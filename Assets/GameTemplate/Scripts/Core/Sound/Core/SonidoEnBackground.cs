using UnityEngine;
using System.Collections;

public class SonidoEnBackground : MonoBehaviour {
//	private static SonidoEnReproduccion instance = null;
//	public static SonidoEnReproduccion Instance {
//		get { return instance; }
//	}
//	
	void Awake() {
//		if (instance != null && instance != this) {
//			Destroy(this.gameObject);
//			return;
//		} else {
//			instance = this;
//		}
		DontDestroyOnLoad(this.gameObject);
	}
}
