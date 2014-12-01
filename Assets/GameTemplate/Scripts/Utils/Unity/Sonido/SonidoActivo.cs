using UnityEngine;
using System.Collections;

/// <summary>
/// Solo reproduce el audiosource adjunto si el sonido esta activo
/// </summary>
public class SonidoActivo : MonoBehaviour {

	// Use this for initialization
	void Awake(){
		if(audio && Configuration.soundActivated){
			audio.Play();
		}
	}
}
