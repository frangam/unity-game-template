using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISoundActivationButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private SoundType type;

	[SerializeField]
	private Image imgForbidden;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool activo = true;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake(){
		switch(type){
		case SoundType.FX:
			activo = GameSettings.soundVolume > 0;
			break;
			
		case SoundType.MUSIC:
			activo = GameSettings.musicVolume > 0;
			break;
		}

		imgForbidden.gameObject.SetActive(!activo);
	}

	public override void press (){
		base.press ();

		switch(type){
		case SoundType.FX:
			float soundVolume = GameSettings.soundVolume > 0f ? 0f : GameSettings.soundVolume; //change to mute or previous sound saved
			GameSettings.soundVolume = soundVolume; //update volume
			activo = GameSettings.soundVolume > 0;
			PlayerPrefs.SetFloat(GameSettings.PP_SOUND, soundVolume);
			break;
			
		case SoundType.MUSIC:
			float musicVolume = GameSettings.musicVolume > 0f ? 0f : GameSettings.musicVolume; //change to mute or previous sound saved
			GameSettings.musicVolume = musicVolume; //update volume
			activo = GameSettings.musicVolume > 0;
			PlayerPrefs.SetFloat(GameSettings.PP_MUSIC, musicVolume);
			break;
		}
		
		imgForbidden.gameObject.SetActive(!activo);
		
		if(activo)
			BaseSoundManager.Instance.play(BaseSoundIDs.MENU_MUSIC);
		else
			BaseSoundManager.Instance.stop(BaseSoundIDs.MENU_MUSIC);
	}
}
