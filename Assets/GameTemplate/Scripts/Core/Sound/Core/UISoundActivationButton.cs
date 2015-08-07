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
			active = BaseSoundManager.Instance.IsSoundActive();
			break;
			
		case SoundType.MUSIC:
			active = BaseSoundManager.Instance.IsMusicActive();
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
		
		active = BaseSoundManager.Instance.muteOrActiveOncesMuteOncesActive(type);
		
		if(hideActive)
			imgActive.gameObject.SetActive(active);
		
		imgInactive.gameObject.SetActive(!active);
	}
}
