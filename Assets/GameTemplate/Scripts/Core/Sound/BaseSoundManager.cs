/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseSoundManager : PersistentSingleton<BaseSoundManager> {
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
	protected override void Awake ()
	{
		base.Awake ();
		sonidosReproduciendo = new Dictionary<string, AudioSource> ();
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
	public bool IsSoundActive(){
		return CurrentGeneralSoundVolume() > 0;
	}
	public bool IsMusicActive(){
		return CurrentGeneralMusicVolume() > 0;
	}
	public float CurrentGeneralSoundVolume(){
		return PlayerPrefs.GetFloat(GameSettings.PP_SOUND);
	}
	public float CurrentGeneralMusicVolume(){
		return PlayerPrefs.GetFloat(GameSettings.PP_MUSIC);
	}
	public void SetCurrentGeneralSoundVolume(float newVol = 1f){
		PlayerPrefs.SetFloat(GameSettings.PP_SOUND, newVol);
	}
	public void SetCurrentGeneralMusicVolume(float newVol = 1f){
		PlayerPrefs.SetFloat(GameSettings.PP_MUSIC, newVol);
	}
	
	
	
	/// <summary>
	/// Mutes the or active all onces mute onces active.
	/// </summary>
	/// <returns><c>true</c>, if sound or music is active, <c>false</c> otherwise.</returns>
	/// <param name="forceVolChange">If set to <c>true</c> exclude if is muted.</param>
	public void muteOrActiveAllOncesMuteOncesActiveAndPlayOrStopAfter(bool forceVolChange = true){
		muteOrActiveOncesMuteOncesActive(SoundType.FX, false, forceVolChange);
		muteOrActiveOncesMuteOncesActive(SoundType.MUSIC, true, forceVolChange);
	}
	
	/// <summary>
	/// Mutes the or active all onces mute onces active.
	/// </summary>
	/// <returns><c>true</c>, if sound or music is active,, <c>false</c> otherwise.</returns>
	/// <param name="type">Type.</param>
	public bool muteOrActiveOncesMuteOncesActive(SoundType type, bool playOrStopMusic = true, bool forceVolChange = false){
		bool active = type == SoundType.FX ? IsSoundActive() : IsMusicActive();
		string VolChangeForcedPrevKey = "";
		bool volumeChangeForced = false;
		float curVol = 0;
		float newVol = 0;
		bool changeVolume = false;
		
		GTDebug.log(type+" Active ? "+active);
		
		switch(type){
		case SoundType.FX:
			VolChangeForcedPrevKey = GameSettings.PP_SOUND_MUTE_FORCED;
			curVol = CurrentGeneralSoundVolume();
			break;
			
		case SoundType.MUSIC:
			VolChangeForcedPrevKey = GameSettings.PP_MUSIC_MUTE_FORCED;
			curVol = CurrentGeneralMusicVolume();
			break;
		}
		
		volumeChangeForced = PlayerPrefs.GetInt(VolChangeForcedPrevKey) != 0 ? true : false;
		changeVolume = (!forceVolChange || ((forceVolChange && !volumeChangeForced && curVol != 0f) || (forceVolChange && volumeChangeForced)));
		
		GTDebug.log(type+" Current Vol: "+curVol + ". Force change volume ? " +forceVolChange+ ". Prev Forced ? " +volumeChangeForced + ". So Change Vol ? " + changeVolume);
		
		if(changeVolume){
			newVol = curVol == 0f ? 1f : 0f; //change to mute or previous sound saved
			
			//update volume
			if(type == SoundType.FX){
				SetCurrentGeneralSoundVolume(newVol); 
				active = IsSoundActive();
			}
			else if(type == SoundType.MUSIC){
				SetCurrentGeneralMusicVolume(newVol);
				active = IsMusicActive();
				
				if(playOrStopMusic)
					playOrStopMusicWasMutedOrActivePreviously(active);
			}
			
			GTDebug.log(type+" Changing Vol to: " + newVol + ". Active ? " + active);
			
			//set flag PP_SOUND_MUTE_FORCED if we force mute or not
			if(forceVolChange){
				GTDebug.log(type+" Set flag Force Change Vol to True");
				PlayerPrefs.SetInt(VolChangeForcedPrevKey, 1);
			}
			else{
				GTDebug.log(type+" Reset flag Force Change Vol to False");
				PlayerPrefs.SetInt(VolChangeForcedPrevKey, 0);
			}
		}
		
		
		
		
		return active;
	}
	
	public void playOrStopMusicWasMutedOrActivePreviously(bool active = true){
		if(active){
			if(BaseGameScreenController.Instance.Section == GameSection.MAIN_MENU)
				BaseSoundManager.Instance.play(BaseSoundIDs.MENU_MUSIC);
			else if(BaseGameScreenController.Instance.Section == GameSection.GAME)
				BaseSoundManager.Instance.play(BaseSoundIDs.GAME_MUSIC);
		}
		else{
			if(BaseGameScreenController.Instance.Section == GameSection.MAIN_MENU)
				BaseSoundManager.Instance.stop(BaseSoundIDs.MENU_MUSIC);
			else if(BaseGameScreenController.Instance.Section == GameSection.GAME)
				BaseSoundManager.Instance.stop(BaseSoundIDs.GAME_MUSIC);
		}
	}
	
	public void play(string id){
		BaseSound sonido = getSonido (id);
		
		if(sonido == null) return;
		//		if((sonido.Tipo == TipoSonido.FX && !Configuracion.sonidoActivo) || (sonido.Tipo == TipoSonido.MUSIC && !Configuracion.musicaActiva)) return;
		
		if(sonidosReproduciendo != null
		   && ((sonido.Tipo == SoundType.FX && CurrentGeneralSoundVolume() > 0f) || (sonido.Tipo == SoundType.MUSIC && CurrentGeneralMusicVolume() > 0f))){
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

	public void playWithDelay(string id, float delay){
		StartCoroutine(doPlayWithDelay(id, delay));
	}

	private IEnumerator doPlayWithDelay(string id, float delay){
		yield return new WaitForSeconds (delay);
		play (id);
	}

	public void stopWithDelay(string id, float delay){
		StartCoroutine(doStopWithDelay(id, delay));
	}

	private IEnumerator doStopWithDelay(string id, float delay){
		yield return new WaitForSeconds (delay);
		stop (id);
	}
}
