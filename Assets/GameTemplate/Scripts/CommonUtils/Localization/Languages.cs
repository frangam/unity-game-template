using UnityEngine;
using System.Collections;

public class Languages {

	public static void seleccionarIdiomaSegunIdiomaDispositivo(){
		switch(Application.systemLanguage){
		case SystemLanguage.Spanish:
			Localization.language = Configuration.LOC_SPANISH;
			break;
//		case SystemLanguage.Chinese:
//			Localization.instance.currentLanguage = Configuracion.LOC_CHINO_SIMPL;
//			break;
		default:
			Localization.language = Configuration.LOC_ENGLISH;
			break;
		}
	}
}
