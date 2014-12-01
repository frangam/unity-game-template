using UnityEngine;
using System.Collections;

public class ContinuarTutorial : MonoBehaviour {
	public float espera = 0f;
	public int toques = 1;
	public bool pausarAlFinal = true;
	
	private int numToques = 0;
	private bool continuandoTuto = false;

	void OnPress (bool inicioToque){
		//touch up
		if(!inicioToque){
			numToques++;
		}

		if(!inicioToque && numToques >= toques && !continuandoTuto){
			continuandoTuto = true;
			StartCoroutine(continuar());
		}

	}

	void Awake(){
		if(toques <= 0)
			Debug.LogError("Toques no puede ser menor o igual a cero");
	}

	private IEnumerator continuar(){
		yield return new WaitForSeconds (espera);

		GestorTutorial.Instance.siguiente();
		Time.timeScale = pausarAlFinal ? 0f : 1f;
	}
}
