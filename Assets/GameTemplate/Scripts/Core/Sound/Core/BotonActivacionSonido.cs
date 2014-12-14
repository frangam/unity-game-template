using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BotonActivacionSonido : MonoBehaviour {
	[SerializeField]
	private TipoSonido tipo;

	[SerializeField]
	private Image prohibido;

	private bool activo = true;

	void Awake(){
		switch(tipo){
		case TipoSonido.FX:
			activo = GameSettings.soundVolume > 0;
			break;
			
		case TipoSonido.MUSIC:
			activo = GameSettings.musicVolume > 0;
			break;
		}

		prohibido.gameObject.SetActive(!activo);
	}
	
	void OnPress(bool pressed = false){
		if(!pressed){ //touch up
			switch(tipo){
			case TipoSonido.FX:
				float soundVolume = GameSettings.soundVolume > 0f ? 0f : GameSettings.soundVolume; //change to mute or previous sound saved
				GameSettings.soundVolume = soundVolume; //update volume
				activo = GameSettings.soundVolume > 0;
				PlayerPrefs.SetFloat(GameSettings.PP_SOUND, soundVolume);
				break;

			case TipoSonido.MUSIC:
				float musicVolume = GameSettings.musicVolume > 0f ? 0f : GameSettings.musicVolume; //change to mute or previous sound saved
				GameSettings.musicVolume = musicVolume; //update volume
				activo = GameSettings.musicVolume > 0;
				PlayerPrefs.SetFloat(GameSettings.PP_MUSIC, musicVolume);
				break;
			}

			prohibido.gameObject.SetActive(!activo);
			
			if(activo)
				BaseSoundManager.Instance.play(BaseSoundIDs.MENU_MUSIC);
			else
				BaseSoundManager.Instance.stop(BaseSoundIDs.MENU_MUSIC);
		}
	}
}
