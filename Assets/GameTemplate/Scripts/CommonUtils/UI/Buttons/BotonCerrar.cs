using UnityEngine;
using System.Collections;

public class BotonCerrar : MonoBehaviour {
	public UIPanel panel;
	public bool panelConexionFB = false;

	void OnPress(bool pressed){
		if(!pressed){ //touch up

			if(!panelConexionFB)	
				panel.gameObject.SetActive(false);
			else
				UIHandler.Instance.abrir(GameScreen.FACEBOOK_ACCOUNT_CONNECTED, false);
		}
	}
}
