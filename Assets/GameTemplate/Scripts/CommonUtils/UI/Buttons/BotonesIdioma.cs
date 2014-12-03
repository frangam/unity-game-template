using UnityEngine;
using System.Collections;

public class BotonesIdioma : Singleton<BotonesIdioma> {
	/*--------------------------------
	 * Atributos de configuracion
	 -------------------------------*/
	[SerializeField]
	private BotonIdioma[] botones;

	/*--------------------------------
	 * Metodos Unity
	 -------------------------------*/
	#region Unity
	void Awake(){
//		botones = FindObjectsOfType<BotonIdioma> () as BotonIdioma[];
	}
	#endregion

	/*--------------------------------
	 * Metodos publicos
	 -------------------------------*/
	public void activar(SystemLanguage idioma){ 
		foreach(BotonIdioma b in botones){
			b.gameObject.SetActive(b.Idioma == idioma);
		}
	}
}
