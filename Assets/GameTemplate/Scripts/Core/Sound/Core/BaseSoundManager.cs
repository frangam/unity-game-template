using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseSoundManager : Singleton<BaseSoundManager> {
	//--------------------------------------
	// Atributos de configuracion
	//--------------------------------------
	[SerializeField]
	private BaseSound[] sonidos;

	//--------------------------------------
	// Atributos privados
	//--------------------------------------
	static AudioListener mListener;
	private Dictionary<string, AudioSource> sonidosReproduciendo;

	//--------------------------------------
	// Metodos Unity
	//--------------------------------------
	void Awake(){
		sonidosReproduciendo = new Dictionary<string, AudioSource> ();
		DontDestroyOnLoad(this.gameObject);
	}

	void OnLevelWasLoaded(){
		List<AudioSource> sonidosEliminar = new List<AudioSource> ();
		List<string> idssonidosEliminar = new List<string> ();

		foreach(string idSonido in sonidosReproduciendo.Keys){
			BaseSound sonido = getSonido(idSonido);

			if(!sonido.EnBackground){
				sonidosEliminar.Add(sonidosReproduciendo[idSonido]);
				idssonidosEliminar.Add(idSonido);
			}
		}

		for(int i=0; i<idssonidosEliminar.Count; i++){
			sonidosReproduciendo.Remove(idssonidosEliminar[i]);
			Destroy(sonidosEliminar[i].gameObject);
		}
	}

	//--------------------------------------
	// Metodos privados
	//--------------------------------------
	private BaseSound getSonido(string id){
		if(sonidos == null)
			return null;

		BaseSound res = null;
		
		foreach(BaseSound s in sonidos){
			if(s.Id == id){
				res = s;
				break;
			}
		}
		
		return res;
	}

	//--------------------------------------
	// Metodos publicos
	//--------------------------------------
	public void play(string id){
		BaseSound sonido = getSonido (id);

		if(sonido == null) return;
//		if((sonido.Tipo == TipoSonido.FX && !Configuracion.sonidoActivo) || (sonido.Tipo == TipoSonido.MUSIC && !Configuracion.musicaActiva)) return;

		if(sonidosReproduciendo != null
		   && ((sonido.Tipo == SoundType.FX && GameSettings.soundVolume > 0f) || (sonido.Tipo == SoundType.MUSIC && GameSettings.musicVolume > 0f))){
			if(sonido.Tipo != SoundType.MUSIC && sonidosReproduciendo != null 
			   && ( !sonidosReproduciendo.ContainsKey(id)  || (sonidosReproduciendo.ContainsKey(id) && sonidosReproduciendo[id] != null &&  !sonidosReproduciendo[id].isPlaying))){
				stop (sonido.Id);
				play (sonido);

//				Debug.Log("reproduciendo: "+sonido.Id);
			}
			//se mantiene la musica en reproduccion
			else if(sonido.Tipo == SoundType.MUSIC && sonidosReproduciendo != null 
			        && ( (sonidosReproduciendo.ContainsKey(id) && sonidosReproduciendo[id].isPlaying))){

//				Debug.Log("ya se esta reproduciendo");
			}
			else{
				play (sonido);

//				Debug.Log("reproduciendo: "+sonido.Id);
			}

//			if(sonidosReproduciendo.ContainsKey(id))
//				sonidosReproduciendo[id].Play();
//			else{
//				AudioSource source = NGUITools.PlaySound (sonido.Clip, sonido.Volumen, sonido.Loop);
//				sonidosReproduciendo[id] = source;
//			}
		}
	}

	public void stop(string id){
		if(sonidosReproduciendo != null && sonidosReproduciendo.ContainsKey(id) && sonidosReproduciendo[id].isPlaying)
			sonidosReproduciendo[id].Stop();
	}

	/// <summary>
	///Stops all of the sounds that can be able to stopped when it is game over
	/// </summary>
	public void stopAllWhenGameOver(){
		foreach(BaseSound s in sonidos){
			if(s.StopWhenGameOver)
				stop(s.Id);
		}
	}

	private void play(BaseSound sonido){
		if (sonido.Clip != null && sonido.Volumen > 0.01f)
		{
			if(sonidosReproduciendo != null && sonidosReproduciendo.ContainsKey(sonido.Id))
				sonidosReproduciendo[sonido.Id].Play();
			else if(sonidosReproduciendo != null && !sonidosReproduciendo.ContainsKey(sonido.Id)){
				GameObject go = new GameObject("audio"+sonido.Id);//Instantiate(new GameObject()) as GameObject;
				go.transform.parent = this.transform;

				//mantenemos sonido en background
				if(sonido.EnBackground)
					go.AddComponent<BackgroundSound>();

				AudioSource source = go.AddComponent<AudioSource>();
				source.clip = sonido.Clip;
				source.volume = sonido.Volumen;
				source.pitch = sonido.Pitch;
				source.loop = sonido.Loop;
				source.Play();//(sonido.Clip, sonido.Volumen);
				sonidosReproduciendo[sonido.Id] = source;
			}				
		}
	}
}
