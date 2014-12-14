using UnityEngine;
using System.Collections;

public class BackgroundSound : MonoBehaviour {
	void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}
}
