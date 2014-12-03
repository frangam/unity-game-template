using UnityEngine;
using System.Collections;

public class ExclusivoPlataforma : MonoBehaviour {
	[SerializeField]
	private RuntimePlatform[] plataformas;

	void Awake(){
		bool mostrar = false;

		foreach (RuntimePlatform p in plataformas) {
			mostrar = Application.platform == p;

			if(mostrar)
				break;
		}

		gameObject.SetActive (mostrar);
	}
}
