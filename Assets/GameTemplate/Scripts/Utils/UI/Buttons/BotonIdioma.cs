using UnityEngine;
using System.Collections;

public class BotonIdioma : MonoBehaviour {
	/*--------------------------------
	 * Atributos de configuracion
	 -------------------------------*/
	[SerializeField]
	private SystemLanguage idioma;

	/*--------------------------------
	 * Getters/Setters
	 -------------------------------*/
	public SystemLanguage Idioma {
		get {
			return this.idioma;
		}
	}

	/*--------------------------------
	 * Metodos Unity
	 -------------------------------*/
	#region Unity
	void Awake(){ 
		switch(Localization.language){
		case Configuration.LOC_SPANISH:
			gameObject.SetActive(idioma == SystemLanguage.Spanish);
			break;
			
		case Configuration.LOC_ENGLISH:
			gameObject.SetActive(idioma == SystemLanguage.English);
			break;

//		case Configuracion.LOC_CHINO_SIMPL:
//			gameObject.SetActive(idioma == SystemLanguage.Chinese);
//			break;
		}
	}
	#endregion

	/*--------------------------------
	 * Metodos NGUI
	 -------------------------------*/
	#region NGUI
	void OnPress(bool inicioToque){
		//touch up
		if(!inicioToque){
			switch(idioma){
			case SystemLanguage.English:
				Localization.language = Configuration.LOC_SPANISH;
				BotonesIdioma.Instance.activar(SystemLanguage.Spanish);
				break;
				
			case SystemLanguage.Spanish:
				Localization.language = Configuration.LOC_ENGLISH;
				BotonesIdioma.Instance.activar(SystemLanguage.English);
				break;

//			case SystemLanguage.Chinese:
//				Localization.instance.currentLanguage = Configuracion.LOC_ENGLISH;
//				BotonesIdioma.Instancia.activar(SystemLanguage.English);
//				break;
			}

//			Debug.Log(idioma);


		}
	}
	#endregion
}
