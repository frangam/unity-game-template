using UnityEngine;
using System.Collections;

public class ObjetoGeneradoInicio : MonoBehaviour {
	[SerializeField]
	private OleadaBase oleada;

	void Awake(){
		GetComponent<GOOleada> ().inicializar (oleada);
	}
}
