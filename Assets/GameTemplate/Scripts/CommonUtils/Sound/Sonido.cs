using UnityEngine;
using System.Collections;

[System.Serializable]
public class Sonido {
	//--------------------------------------
	// Atributos de configuracion
	//--------------------------------------
	[SerializeField]
	private AudioClip clip;

	[SerializeField]
	private TipoSonido tipo;

	[SerializeField]
	private bool enBackground = true;

	[SerializeField]
	private GestorSonidos.ID_SONIDO id;

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

	public TipoSonido Tipo {
		get {
			return this.tipo;
		}
	}

	public GestorSonidos.ID_SONIDO Id {
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
