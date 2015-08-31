/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

[System.Serializable]
public class BaseSound {
	//--------------------------------------
	// Atributos de configuracion
	//--------------------------------------
	[SerializeField]
	private string name;

	[SerializeField]
	private AudioClip clip;

	[SerializeField]
	private SoundType tipo;

	[SerializeField]
	private bool enBackground = true;

	[SerializeField]
	[Tooltip("Use BaseSoundIDs class or inherited to set this value")]
	private string id;

	[SerializeField]
	private float volumen = 1f;

	[SerializeField]
	private bool loop = false;

	[SerializeField]
	private float pitch = 1f;

	[SerializeField]
	private bool stopWhenGameOver = true;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public AudioClip Clip {
		get {
			return this.clip;
		}
	}

	public SoundType Tipo {
		get {
			return this.tipo;
		}
	}

	public string Id {
		get {
			return this.id;
		}
	}

	public float Volumen {
		get {
			return this.volumen;
		}
	}

	public bool Loop {
		get {
			return this.loop;
		}
	}

	public float Pitch {
		get {
			return this.pitch;
		}
	}
	public bool EnBackground {
		get {
			return this.enBackground;
		}
	} 

	public bool StopWhenGameOver {
		get {
			return this.stopWhenGameOver;
		}
	}
}
