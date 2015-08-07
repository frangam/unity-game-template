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
	/// <param name="excludeIfIsMuted">If set to <c>true</c> exclude if is muted.</param>
	public void muteOrActiveAllOncesMuteOncesActiveAndPlayOrStopAfter(bool forceMute = true){
		muteOrActiveOncesMuteOncesActive(SoundType.FX, false, forceMute);
		muteOrActiveOncesMuteOncesActive(SoundType.MUSIC, true, forceMute);
	}
	
	/// <summary>
	/// Mutes the or active all onces mute onces active.
	/// </summary>
	/// <returns><c>true</c>, if sound or music is active,, <c>false</c> otherwise.</returns>
	/// <param name="type">Type.</param>
	/// <param name="excludeIfIsMuted">If set to <c>true</c> exclude if is muted.</param>
	public bool muteOrActiveOncesMuteOncesActive(SoundType type, bool playOrStopMusic = true, bool forceMute = false){
		bool active = false;
		
		switch(type){
		case SoundType.FX:
			bool soundMuteForcedPreviously = PlayerPrefs.GetInt(GameSettings.PP_SOUND_MUTE_FORCED) != 0 ? true : false;
			if(forceMute && !soundMuteForcedPreviously)
				PlayerPrefs.SetInt(GameSettings.PP_SOUND_MUTE_FORCED, 1);
			else if(!forceMute || soundMuteForcedPreviously)
				PlayerPrefs.SetInt(GameSettings.PP_SOUND_MUTE_FORCED, 0);
			
			float curSoundVol = CurrentGeneralSoundVolume();
			float newSoundVol = (curSoundVol == 0f && (!forceMute || (forceMute && soundMuteForcedPreviously))) ? 1f : 0f; //change to mute or previous sound saved
			SetCurrentGeneralSoundVolume(newSoundVol); //update volume
			active = CurrentGeneralSoundVolume() > 0;
			break;
			
		case SoundType.MUSIC:
			bool musicMuteForcedPreviously = PlayerPrefs.GetInt(GameSettings.PP_MUSIC_MUTE_FORCED) != 0 ? true : false;
			if(forceMute && !musicMuteForcedPreviously)
				PlayerPrefs.SetInt(GameSettings.PP_MUSIC_MUTE_FORCED, 1);
			else if(!forceMute || musicMuteForcedPreviously)
				PlayerPrefs.SetInt(GameSettings.PP_MUSIC_MUTE_FORCED, 0);
			
			float curMusicVol = CurrentGeneralMusicVolume();
			float newMusicVol = (curMusicVol == 0f && (!forceMute || (forceMute && musicMuteForcedPreviously))) ? 1f : 0f; //change to mute or previous sound saved
			SetCurrentGeneralMusicVolume(newMusicVol); //update volume
			active = CurrentGeneralMusicVolume() > 0;
			
			if(playOrStopMusic)
				playOrStopMusicWasMutedOrActivePreviously(active);
			
			break;
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
}
