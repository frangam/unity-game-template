using UnityEngine;
using System.Collections;

public class MensajeUI : MonoBehaviour {
	[SerializeField]
	private TipoMensajeUI tipo;

	[SerializeField]
	private float duration = 1.5f;

	public TipoMensajeUI Tipo {
		get {
			return this.tipo;
		}
	}

	public void init(){
		gameObject.SetActive (true);
		StartCoroutine (check ());
	}

	private IEnumerator check(){
		yield return new WaitForSeconds(duration);
		gameObject.SetActive (false);
	}
}
