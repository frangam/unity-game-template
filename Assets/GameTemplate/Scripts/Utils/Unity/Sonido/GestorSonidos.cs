using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestorSonidos : Singleton<GestorSonidos> {
	public enum ID_SONIDO{
		//COMUNES
		MUS_MENU 				= 0,
		MUS_IN_GAME 			= 1,
		MUS_GAME_OVER 			= 2,
		MUS_GANAR				= 3,
		MUS_IN_PAUSE 			= 4,
		FX_CLICK_BOTON_UI 		= 5,
		FX_CLICK_BOTON_ATRAS 	= 6,
		FX_GAME_OVER 			= 7,
		FX_CONSIGUE_PUNTOS		= 8,
		FX_ABRIR_VENTANA		= 9,
		FX_CERRAR_VENTANA		= 10,

		//ESPECIFICOS
		FX_RECOLECTA_MONEDA		= 20,
		FX_RECOLECTA_GEMA		= 21,
		FX_CONSIGUE_ESTRELLA	= 22,
		FX_COMPRAR_ITEM			= 23,
		FX_GOLPE_OBSTACULO		= 24,
		FX_GOLPE_ZOMBIE			= 25,
		FX_ENCENDER_FAROS		= 26,
		FX_APAGAR_FAROS			= 27,
		FX_MOTOR_COCHE			= 28,
		FX_MOTOR_COCHE_ARRANCAR	= 29,
		FX_COGER_BOOSTER_ESCUDO	= 30,
		FX_COGER_BOOSTER_RELOJ	= 31,
		FX_MENSAJE_BROKER		= 32,
		FX_MENSAJE_TERMINATOR	= 33,
		FX_CUENTA_ATRAS_NUM		= 34,
		FX_CUENTA_ATRAS_GO		= 35,
		FX_COCHE_POLICIA		= 36,
		FX_GENERACION_ZOMBIES	= 37,
		FX_DESTROZAR_OBSTACULOS	= 38
	}

	//--------------------------------------
	// Atributos de configuracion
	//--------------------------------------
	[SerializeField]
	private Sonido[] sonidos;

	//--------------------------------------
	// Atributos privados
	//--------------------------------------
	static AudioListener mListener;
	private Dictionary<ID_SONIDO, AudioSource> sonidosReproduciendo;

	//--------------------------------------
	// Metodos Unity
	//--------------------------------------
	void Awake(){
		sonidosReproduciendo = new Dictionary<ID_SONIDO, AudioSource> ();
		DontDestroyOnLoad(this.gameObject);
	}

	void OnLevelWasLoaded(){
		List<AudioSource> sonidosEliminar = new List<AudioSource> ();
		List<ID_SONIDO> idssonidosEliminar = new List<ID_SONIDO> ();

		foreach(ID_SONIDO idSonido in sonidosReproduciendo.Keys){
			Sonido sonido = getSonido(idSonido);

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
	private Sonido getSonido(ID_SONIDO id){
		if(sonidos == null)
			return null;

		Sonido res = null;
		
		foreach(Sonido s in sonidos){
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
	public void play(ID_SONIDO id){
		Sonido sonido = getSonido (id);

		if(sonido == null) return;
//		if((sonido.Tipo == TipoSonido.FX && !Configuracion.sonidoActivo) || (sonido.Tipo == TipoSonido.MUSIC && !Configuracion.musicaActiva)) return;

		if(sonidosReproduciendo != null
		   && ((sonido.Tipo == TipoSonido.FX && Configuration.soundActivated) || (sonido.Tipo == TipoSonido.MUSIC && Configuration.musicActivated))){
			if(sonido.Tipo != TipoSonido.MUSIC && sonidosReproduciendo != null 
			   && ( !sonidosReproduciendo.ContainsKey(id)  || (sonidosReproduciendo.ContainsKey(id) && sonidosReproduciendo[id] != null &&  !sonidosReproduciendo[id].isPlaying))){
				stop (sonido.Id);
				play (sonido);

//				Debug.Log("reproduciendo: "+sonido.Id);
			}
			//se mantiene la musica en reproduccion
			else if(sonido.Tipo == TipoSonido.MUSIC && sonidosReproduciendo != null 
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

	public void stop(ID_SONIDO id){
		if(sonidosReproduciendo != null && sonidosReproduciendo.ContainsKey(id) && sonidosReproduciendo[id].isPlaying)
			sonidosReproduciendo[id].Stop();
	}

	/// <summary>
	///Stops all of the sounds that can be able to stopped when it is game over
	/// </summary>
	public void stopAllWhenGameOver(){
		foreach(Sonido s in sonidos){
			if(s.StopWhenGameOver)
				stop(s.Id);
		}
	}

	private void play(Sonido sonido){
		if (sonido.Clip != null && sonido.Volumen > 0.01f)
		{
			if(sonidosReproduciendo != null && sonidosReproduciendo.ContainsKey(sonido.Id))
				sonidosReproduciendo[sonido.Id].Play();
			else if(sonidosReproduciendo != null && !sonidosReproduciendo.ContainsKey(sonido.Id)){
				GameObject go = new GameObject("audio"+sonido.Id);//Instantiate(new GameObject()) as GameObject;
				go.transform.parent = this.transform;

				//mantenemos sonido en background
				if(sonido.EnBackground)
					go.AddComponent<SonidoEnBackground>();

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
