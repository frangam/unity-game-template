using UnityEngine;
using System.Collections;

public class BotonActivacionSonido : MonoBehaviour {
	[SerializeField]
	private TipoSonido tipo;

	[SerializeField]
	private UI2DSprite prohibido;

	private bool activo = true;

	void Awake(){
		switch(tipo){
		case TipoSonido.FX:
			activo = Configuration.soundActivated;
			break;
			
		case TipoSonido.MUSIC:
			activo = Configuration.musicActivated;
			break;
		}

		prohibido.gameObject.SetActive(!activo);
	}
	
	void OnPress(bool pressed){
		if(!pressed){ //touch up
			switch(tipo){
			case TipoSonido.FX:
				int sonidoActivado = !Configuration.soundActivated ? 1: 0;
				Configuration.soundActivated = sonidoActivado == 1 ? true : false;
				activo = Configuration.soundActivated;
				PlayerPrefs.SetInt(Configuration.PP_SOUND, sonidoActivado);
				break;

			case TipoSonido.MUSIC:
				int musActivada = !Configuration.musicActivated ? 1: 0;
				Configuration.musicActivated = musActivada == 1 ? true : false;
				activo = Configuration.musicActivated;
				PlayerPrefs.SetInt(Configuration.PP_MUSIC, musActivada);
				break;
			}



			prohibido.gameObject.SetActive(!activo);
			
			if(activo)
				GestorSonidos.Instance.play(GestorSonidos.ID_SONIDO.MUS_MENU);
			else
				GestorSonidos.Instance.stop(GestorSonidos.ID_SONIDO.MUS_MENU);
		}
	}
}
