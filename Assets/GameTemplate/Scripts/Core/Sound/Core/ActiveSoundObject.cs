using UnityEngine;
using System.Collections;

/// <summary>
/// Plays the attached AudioSource only if the sound is active
/// </summary>
public class ActiveSoundObject : MonoBehaviour {
	void Awake(){
		if(GetComponent<AudioSource>() && GameSettings.soundVolume > 0f){
			GetComponent<AudioSource>().Play();
		}
	}
}
