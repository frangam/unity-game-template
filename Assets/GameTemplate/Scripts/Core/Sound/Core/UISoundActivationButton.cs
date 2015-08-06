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
	private bool active = true;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake(){
		switch(type){
		case SoundType.FX:
			active = GameSettings.soundVolume > 0;
			break;
			
		case SoundType.MUSIC:
			active = GameSettings.musicVolume > 0;
			break;
		}
		
		if(hideActive)
			imgActive.gameObject.SetActive(active);
		else{
			imgActive.gameObject.SetActive(true);
		}
		
		imgInactive.gameObject.SetActive(!active);
	}
	
	protected override void doPress ()
	{
		base.doPress ();
		
		active = BaseSoundManager.Instance.muteOrActiveOncesMuteOncesActive(type, false, true);
		
		if(hideActive)
			imgActive.gameObject.SetActive(active);
		
		imgInactive.gameObject.SetActive(!active);
	}
}
