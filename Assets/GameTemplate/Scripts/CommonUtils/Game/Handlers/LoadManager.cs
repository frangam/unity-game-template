using UnityEngine;
using System.Collections;

public class LoadManager : Singleton<LoadManager> {
	[SerializeField]
	/// <summary>
	/// La musica y los fx se controlan con un mismo boton, se mutean o se activan
	/// </summary>
	private bool musicaIgualFX = true;

	[SerializeField]
	/// <summary>
	/// Las dificultades del juego
	/// </summary>
	private GameDifficulty[] dificultades;

	private const float TIEMPO_ESPERA_COMPROBAR_GPS_CONEXION = 10;
	private const float TIEMPO_ESPERA_COMPROBAR_GC_CONEXION = 10;
    private const float TIEMPO_ESPERA_COMPROBAR_WP8_CONEXION = 8; 
	private bool gpsPrepared = false;
	private bool gcPrepared = false;
	private bool twInited = false;
	private bool fbInited = false;



	public bool GPSPrepared {
		get {
			return this.gpsPrepared;
		}
		set {
			gpsPrepared = value;
		}
	}

	public bool GCPrepared {
		get {
			return this.gcPrepared;
		}
		set {
			gcPrepared = value;
		}
	}

	public bool TwInited {
		get {
			return this.twInited;
		}
		set {
			twInited = value;
		}
	}

	public bool FbInited {
		get {
			return this.fbInited;
		}
		set {
			fbInited = value;
		}
	}



	#region Unity
	void Awake () {
		if(!Configuration.USE_FACEBOOK)
			fbInited = true;
		if(!Configuration.USE_TWITTER)
			twInited = true;

//		GestorLogros.Instancia.comprobacionInicial ();

//		//TEST
//		int nivel = 36;
//		PlayerPrefs.SetInt (Configuration.CLAVE_CARRITO_MOVIDO_NIVEL + nivel.ToString(), 0);
//		PlayerPrefs.SetInt (Configuration.CLAVE_ULTIMO_NIVEL_DESBLOQUEADO, nivel);


//		PlayerPrefs.DeleteKey (Configuration.CLAVE_TUTORIAL_SUPERADO); //solo para test
//		PlayerPrefs.DeleteAll ();

		//dificultad facil por defecto
		if(!PlayerPrefs.HasKey(Configuration.PP_GAME_DIFFICULTY)){
			PlayerPrefs.SetInt(Configuration.PP_GAME_DIFFICULTY, (int) GameDifficulty.EASY);
		}

		StartCoroutine( ScreenLoaderIndicator.Instance.Load ());

		cargarIdioma (); //idioma

		//TEST
//		Localization.language = Configuration.LOC_ENGLISH;

		cargarAudio (); //musica y sonido
		cargarPuntosYNivelInicial (); //puntos
		cargarGPSyGC (); //google play services y game center
		cargarScores ();
//
		StartCoroutine (cargarTutorialOMenu ()); //se carga el tutorial o el menu del juego
	}
	void Start(){
//		((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
	}

	void LateUpdate(){
		if(StartScene.Instance.Section != Section.LOAD_SCREEN) return;

#if UNITY_ANDROID
		if((!Configuration.USE_GOOGLE_PLAY_SERVICES && twInited && fbInited && !Configuration.mandatoryTutorial)
		   || (Configuration.USE_GOOGLE_PLAY_SERVICES && gpsPrepared && twInited && fbInited && !Configuration.mandatoryTutorial)){
			AdsHandler.Instance.mostrarPantallazo();
			StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_MAINMENU));
		}
		else if(gpsPrepared && twInited && fbInited && Configuration.mandatoryTutorial)
			Application.LoadLevel(Configuration.SCENE_TUTORIAL);
#elif UNITY_IPHONE
		if((!Configuration.USAR_GAMECENTER && twIniciado && fbIniciado && !Configuration.mandatoryTutorial)
		   || (Configuration.USAR_GAMECENTER && gcConectado && twIniciado && fbIniciado && !Configuration.mandatoryTutorial)){
			AdsHandler.Instance.mostrarPantallazo();
			StartCoroutine( ScreenLoaderIndicator.Instancia.Load (Configuration.SCENE_MAINMENU));
		}
		else if(gpsConectado && twIniciado && fbIniciado && Configuration.mandatoryTutorial)
			Application.LoadLevel(Configuration.SCENE_TUTORIAL);
#endif



	}
	#endregion

	/*--------------------------------
	 * Cargar tutorial o menu
	 -------------------------------*/
	private IEnumerator cargarTutorialOMenu(){ 
		//tutorial
		if(!PlayerPrefs.HasKey(Configuration.PP_COMPLETED_TUTORIAL)){
			PlayerPrefs.SetInt(Configuration.PP_COMPLETED_TUTORIAL, 0);
			Configuration.mandatoryTutorial = true;
		}
		else{
			Configuration.mandatoryTutorial = PlayerPrefs.GetInt(Configuration.PP_COMPLETED_TUTORIAL) == 0;	
		}

		
		#if UNITY_ANDROID
		if(Configuration.USE_GOOGLE_PLAY_SERVICES)
			yield return new WaitForSeconds (TIEMPO_ESPERA_COMPROBAR_GPS_CONEXION);
		#elif UNITY_IPHONE
		if(Configuration.USAR_GAMECENTER)
			yield return new WaitForSeconds (TIEMPO_ESPERA_COMPROBAR_GC_CONEXION);
		#else
        yield return new WaitForSeconds(TIEMPO_ESPERA_COMPROBAR_WP8_CONEXION);
		#endif

		if(!twInited || !fbInited)
			yield return new WaitForSeconds (TIEMPO_ESPERA_COMPROBAR_GPS_CONEXION);
		
		if(Configuration.mandatoryTutorial){
			Application.LoadLevel(Configuration.SCENE_TUTORIAL);
		}
		else{
			AdsHandler.Instance.mostrarPantallazo();
			StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_MAINMENU));
		}
		

	}

	/*--------------------------------
	 * Google play
	 -------------------------------*/
	private void cargarGPSyGC(){
	#if UNITY_ANDROID
		if(Configuration.USE_GOOGLE_PLAY_SERVICES)
			GPSConnect.Instance.init();
	#elif UNITY_IPHONE
		if(Configuration.USAR_GAMECENTER)
			GCConnect.Instance.init();
	#endif
	}
	
	/*--------------------------------
	 * Idioma seleccionado
	 -------------------------------*/
	private void cargarIdioma(){
		if(!PlayerPrefs.HasKey(Configuration.PP_LAST_SCORE)){
			PlayerPrefs.SetInt(Configuration.PP_LAST_SCORE, 0);
			
			//si no se ha cambiado el idioma, indicamos el idioma por defecto al del dispositivo
			Languages.seleccionarIdiomaSegunIdiomaDispositivo();
		}
	}

	/*--------------------------------
	 * Audio activo
	 -------------------------------*/
	private void cargarAudio(){
		//Musica activa
		if(!PlayerPrefs.HasKey(Configuration.PP_MUSIC)){
			//inicializacion de sonido y musica activos
			if (musicaIgualFX) {
				PlayerPrefs.SetInt(Configuration.PP_MUSIC, 1);
				PlayerPrefs.SetInt(Configuration.PP_SOUND, 1);	
				Configuration.musicActivated = true;
				Configuration.soundActivated = true;
			}
			//inicializacion de musica activa
			else{
				PlayerPrefs.SetInt(Configuration.PP_MUSIC, 1);
				Configuration.musicActivated = true;
			}
		}
		else{
			//carga de valores de sonido y musica activos
			if (musicaIgualFX) {
				Configuration.musicActivated = PlayerPrefs.GetInt(Configuration.PP_MUSIC) == 1 ? true : false;
				Configuration.soundActivated = PlayerPrefs.GetInt(Configuration.PP_SOUND) == 1 ? true : false;
			}
			//carga solo de valor de musica activa
			else{
				Configuration.musicActivated = PlayerPrefs.GetInt(Configuration.PP_MUSIC) == 1 ? true : false;
			}
		}


		//sonidoActivo
		if(!PlayerPrefs.HasKey(Configuration.PP_SOUND)){
			//inicializacion de sonido y musica activos
			if (musicaIgualFX) {
				PlayerPrefs.SetInt(Configuration.PP_MUSIC, 1);
				PlayerPrefs.SetInt(Configuration.PP_SOUND, 1);	
				Configuration.musicActivated = true;
				Configuration.soundActivated = true;
			}
			//inicializacion de musica activa
			else{
				PlayerPrefs.SetInt(Configuration.PP_SOUND, 1);
				Configuration.soundActivated = true;
			}
		}
		else{
			//inicializacion solo de valor del sonido activo
			if(!musicaIgualFX && !PlayerPrefs.HasKey(Configuration.PP_SOUND)){
				PlayerPrefs.SetInt(Configuration.PP_SOUND, 1);	
				Configuration.soundActivated = true;
			}
			//carga de valor del sonido activo
			else if(!musicaIgualFX){
				Configuration.soundActivated = PlayerPrefs.GetInt(Configuration.PP_SOUND) == 1 ? true : false;	
			}
		}
	}

	private void cargarScores(){
		if(dificultades != null){
			foreach(GameDifficulty dif in dificultades){
				string difString = ((int) dif).ToString();
				string key = Configuration.PP_LAST_SCORE + difString; //ultima_puntuacion_0 (en facil) , _1 (normal)...
				string key2 = Configuration.PP_BEST_SCORE + difString;

				//puntuacion actual en dificultad
				if(!PlayerPrefs.HasKey(key)){
					PlayerPrefs.SetInt(key, 0);
				}

				//mejor puntuacion en dificultad
				if(!PlayerPrefs.HasKey(key2)){
					PlayerPrefs.SetInt(key2, 0);
				}
			}
		}
	}

	/*--------------------------------
	 * Puntos
	 -------------------------------*/
	public void cargarPuntosYNivelInicial(){
		if(!PlayerPrefs.HasKey(Configuration.PP_BEST_SCORE)){
			PlayerPrefs.SetInt(Configuration.PP_BEST_SCORE, 0);
		}
		if(!PlayerPrefs.HasKey(Configuration.PP_LAST_SCORE)){
			PlayerPrefs.SetInt(Configuration.PP_LAST_SCORE, 0);
		}

		if(!PlayerPrefs.HasKey(Configuration.PP_LAST_LEVEL_UNLOCKED)){
			PlayerPrefs.SetInt(Configuration.PP_LAST_LEVEL_UNLOCKED, 1);
			Configuration.lastLevelUnlocked = 1;
		}
	}


}
