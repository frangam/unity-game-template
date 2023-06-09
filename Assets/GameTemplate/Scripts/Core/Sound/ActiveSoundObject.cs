/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

/// <summary>
/// Plays the attached AudioSource only if the sound is active
/// </summary>
public class ActiveSoundObject : MonoBehaviour {
	void Awake(){
		if(GetComponent<AudioSource>() && BaseSoundManager.Instance.IsSoundActive()){
			GetComponent<AudioSource>().Play();
		}
	}
}
