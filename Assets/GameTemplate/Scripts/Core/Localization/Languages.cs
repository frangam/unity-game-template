using UnityEngine;
using System.Collections;

public class Languages {
	
	public static void seleccionarIdiomaSegunIdiomaDispositivo(){
		switch(Application.systemLanguage){
		case SystemLanguage.Spanish:
			Localization.language = GameSettings.LOC_SPANISH;
			break;
			//		case SystemLanguage.Chinese:
			//			Localization.instance.currentLanguage = Configuracion.LOC_CHINO_SIMPL;
			//			break;
		default:
			Localization.language = GameSettings.LOC_ENGLISH;
			break;
		}
	}
	
	public static void selectLanguage(string language){
		Localization.language = language;
	}
}
