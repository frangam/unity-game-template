using UnityEngine;
using System.Collections;

public class PlaySoundButton : MonoBehaviour {
	public enum Trigger{
		TOUCH_UP,
		TOUCH_DOWN,
	}

	[SerializeField]
	private Trigger trigger;

	[SerializeField]
	[Tooltip("Use BaseSoundIDs class or inherited to set this value")]
	private string soundId = BaseSoundIDs.UI_BUTTON_CLICK_FX;

	private bool movingFinger;

	public void OnPress (bool pressed = false){
		//touch up
		if(trigger == Trigger.TOUCH_UP && !pressed && !movingFinger){
			BaseSoundManager.Instance.play(soundId);
		}
		//touch down
		else if(trigger == Trigger.TOUCH_DOWN && pressed && !movingFinger){
			BaseSoundManager.Instance.play(soundId);
			movingFinger = false; //finger movement end
		}
		else if(!pressed){
			movingFinger = false; //finger movement end
		}
	}

	public void OnDrag(Vector2 delta){
		movingFinger = true;
	}
}
