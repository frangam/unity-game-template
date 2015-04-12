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
	private Image imgActive;
	
	[SerializeField]
	private Image imgInactive;
	
	[SerializeField]
	private bool hideActive = false;
	
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
		
		if(hideActive)
			imgActive.gameObject.SetActive(activo);
		else{
			imgActive.gameObject.SetActive(true);
		}
		
		imgInactive.gameObject.SetActive(!activo);
	}
	
	protected override void doPress ()
	{
		base.doPress ();
		
		switch(type){
		case SoundType.FX:
			float soundVolume = GameSettings.soundVolume == 0f ? 1f : 0f; //change to mute or previous sound saved
			GameSettings.soundVolume = soundVolume; //update volume
			activo = GameSettings.soundVolume > 0;
			PlayerPrefs.SetFloat(GameSettings.PP_SOUND, soundVolume);
			break;
			
		case SoundType.MUSIC:
			float musicVolume = GameSettings.musicVolume == 0f ? 1f : 0f; //change to mute or previous sound saved
			GameSettings.musicVolume = musicVolume; //update volume
			activo = GameSettings.musicVolume > 0;
			PlayerPrefs.SetFloat(GameSettings.PP_MUSIC, musicVolume);
			break;
		}
		
		if(hideActive)
			imgActive.gameObject.SetActive(activo);
		
		imgInactive.gameObject.SetActive(!activo);
		
		if(type == SoundType.MUSIC){
			if(activo){
				if(BaseGameScreenController.Instance.Section == GameSection.MAIN_MENU)
					BaseSoundManager.Instance.play(BaseSoundIDs.MENU_MUSIC);
				else if(BaseGameScreenController.Instance.Section == GameSection.GAME)
					BaseSoundManager.Instance.play(BaseSoundIDs.GAME_MUSIC);
			}
			else{
				if(BaseGameScreenController.Instance.Section == GameSection.MAIN_MENU)
					BaseSoundManager.Instance.stop(BaseSoundIDs.MENU_MUSIC);
				else if(BaseGameScreenController.Instance.Section == GameSection.GAME)
					BaseSoundManager.Instance.stop(BaseSoundIDs.GAME_MUSIC);
			}
		}
	}
}
