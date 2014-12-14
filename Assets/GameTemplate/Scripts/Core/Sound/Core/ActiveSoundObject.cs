using UnityEngine;
using System.Collections;

/// <summary>
/// Plays the attached AudioSource only if the sound is active
/// </summary>
public class ActiveSoundObject : MonoBehaviour {
	void Awake(){
		if(audio && GameSettings.soundVolume > 0f){
			audio.Play();
		}
	}
}
