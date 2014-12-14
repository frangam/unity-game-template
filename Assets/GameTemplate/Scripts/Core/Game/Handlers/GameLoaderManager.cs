using UnityEngine;
using System.Collections;

public class GameLoaderManager : Singleton<GameLoaderManager> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
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

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private const float TIEMPO_ESPERA_COMPROBAR_GPS_CONEXION = 10;
	private const float TIEMPO_ESPERA_COMPROBAR_GC_CONEXION = 10;
    private const float TIEMPO_ESPERA_COMPROBAR_WP8_CONEXION = 8; 
	private bool gpsPrepared = false;
	private bool gcPrepared = false;
	private bool twInited = false;
	private bool fbInited = false;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
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


	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake () {
		if(!GameSettings.USE_FACEBOOK)
			fbInited = true;
		if(!GameSettings.USE_TWITTER)
			twInited = true;

		//Easy game difficulty by default
		if(!PlayerPrefs.HasKey(GameSettings.PP_GAME_DIFFICULTY)){
			PlayerPrefs.SetInt(GameSettings.PP_GAME_DIFFICULTY, (int) GameDifficulty.EASY);
		}

		StartCoroutine( ScreenLoaderVisualIndicator.Instance.Load ());

		cargarIdioma (); //idioma

		//TEST
//		Localization.language = GameSettings.LOC_ENGLISH;

		loadSettings();
		cargarAudio (); //musica y sonido
		cargarPuntosYNivelInicial (); //puntos
		LoadGPSandGC (); //google play services y game center
		cargarScores ();

		StartCoroutine (LoadTutorialOrMenu ()); //se carga el tutorial o el menu del juego
	}

	void LateUpdate(){
		if(BaseGameScreenController.Instance.Section != GameSection.LOAD_SCREEN) return;

#if UNITY_ANDROID
		if((!GameSettings.USE_GOOGLE_PLAY_SERVICES && twInited && fbInited && !GameSettings.mandatoryTutorial)
		   || (GameSettings.USE_GOOGLE_PLAY_SERVICES && gpsPrepared && twInited && fbInited && !GameSettings.mandatoryTutorial)){
			AdsHandler.Instance.mostrarPantallazo();
			StartCoroutine( ScreenLoaderVisualIndicator.Instance.Load (GameSettings.SCENE_MAINMENU));
		}
		else if(gpsPrepared && twInited && fbInited && GameSettings.mandatoryTutorial)
			Application.LoadLevel(GameSettings.SCENE_TUTORIAL);
#elif UNITY_IPHONE
		if((!GameSettings.USAR_GAMECENTER && twIniciado && fbIniciado && !GameSettings.mandatoryTutorial)
		   || (GameSettings.USAR_GAMECENTER && gcConectado && twIniciado && fbIniciado && !GameSettings.mandatoryTutorial)){
			AdsHandler.Instance.mostrarPantallazo();
			StartCoroutine( ScreenLoaderVisualIndicator.Instancia.Load (GameSettings.SCENE_MAINMENU));
		}
		else if(gpsConectado && twIniciado && fbIniciado && GameSettings.mandatoryTutorial)
			Application.LoadLevel(GameSettings.SCENE_TUTORIAL);
#endif



	}
	#endregion

	/*--------------------------------
	 * Load tutorial or menu
	 -------------------------------*/
	private IEnumerator LoadTutorialOrMenu(){ 
		//tutorial
		if(!PlayerPrefs.HasKey(GameSettings.PP_COMPLETED_TUTORIAL)){
			PlayerPrefs.SetInt(GameSettings.PP_COMPLETED_TUTORIAL, 0);
			GameSettings.mandatoryTutorial = true;
		}
		else{
			GameSettings.mandatoryTutorial = PlayerPrefs.GetInt(GameSettings.PP_COMPLETED_TUTORIAL) == 0;	
		}

		#if UNITY_ANDROID
		if(GameSettings.USE_GOOGLE_PLAY_SERVICES)
			yield return new WaitForSeconds (TIEMPO_ESPERA_COMPROBAR_GPS_CONEXION);
		#elif UNITY_IPHONE
		if(GameSettings.USAR_GAMECENTER)
			yield return new WaitForSeconds (TIEMPO_ESPERA_COMPROBAR_GC_CONEXION);
		#else
        yield return new WaitForSeconds(TIEMPO_ESPERA_COMPROBAR_WP8_CONEXION);
		#endif

		if(!twInited || !fbInited)
			yield return new WaitForSeconds (TIEMPO_ESPERA_COMPROBAR_GPS_CONEXION);


		//finally load the scene: tutorial or menu
		if(GameSettings.mandatoryTutorial){
			Application.LoadLevel(GameSettings.SCENE_TUTORIAL);
		}
		else{
			AdsHandler.Instance.mostrarPantallazo();
			StartCoroutine( ScreenLoaderVisualIndicator.Instance.Load (GameSettings.SCENE_MAINMENU));
		}
	}

	/*--------------------------------
	 * Google play
	 -------------------------------*/
	private void LoadGPSandGC(){
	#if UNITY_ANDROID
		if(GameSettings.USE_GOOGLE_PLAY_SERVICES)
			GPSConnect.Instance.init();
	#elif UNITY_IPHONE
		if(GameSettings.USAR_GAMECENTER)
			GCConnect.Instance.init();
	#endif
	}
	
	/*--------------------------------
	 * Idioma seleccionado
	 -------------------------------*/
	private void cargarIdioma(){
		if(!PlayerPrefs.HasKey(GameSettings.PP_LAST_SCORE)){
			PlayerPrefs.SetInt(GameSettings.PP_LAST_SCORE, 0);
			
			//si no se ha cambiado el idioma, indicamos el idioma por defecto al del dispositivo
			Languages.seleccionarIdiomaSegunIdiomaDispositivo();
		}
	}

	/*--------------------------------
	 * Audio activo
	 -------------------------------*/
	private void cargarAudio(){
		//Musica activa
		if(!PlayerPrefs.HasKey(GameSettings.PP_MUSIC)){
			//inicializacion de sonido y musica activos
			if (musicaIgualFX) {
				PlayerPrefs.SetFloat(GameSettings.PP_MUSIC, 1f);
				PlayerPrefs.SetFloat(GameSettings.PP_SOUND, 1f);	
				GameSettings.musicVolume = 1f;
				GameSettings.soundVolume = 1f;
			}
			//inicializacion de musica activa
			else{
				PlayerPrefs.SetFloat(GameSettings.PP_MUSIC, 1f);
				GameSettings.musicVolume = 1f;
			}
		}
		else{
			//carga de valores de sonido y musica activos
			if (musicaIgualFX) {
				GameSettings.musicVolume = PlayerPrefs.GetFloat(GameSettings.PP_MUSIC);
				GameSettings.soundVolume = PlayerPrefs.GetFloat(GameSettings.PP_SOUND);
			}
			//carga solo de valor de musica activa
			else{
				GameSettings.musicVolume = PlayerPrefs.GetFloat(GameSettings.PP_MUSIC);
			}
		}


		//sonidoActivo
		if(!PlayerPrefs.HasKey(GameSettings.PP_SOUND)){
			//inicializacion de sonido y musica activos
			if (musicaIgualFX) {
				PlayerPrefs.SetFloat(GameSettings.PP_MUSIC, 1f);
				PlayerPrefs.SetFloat(GameSettings.PP_SOUND, 1f);	
				GameSettings.musicVolume = 1f;
				GameSettings.soundVolume = 1f;
			}
			//inicializacion de musica activa
			else{
				PlayerPrefs.SetFloat(GameSettings.PP_SOUND, 1f);
				GameSettings.soundVolume = 1f;
			}
		}
		else{
			//inicializacion solo de valor del sonido activo
			if(!musicaIgualFX && !PlayerPrefs.HasKey(GameSettings.PP_SOUND)){
				PlayerPrefs.SetFloat(GameSettings.PP_SOUND, 1f);	
				GameSettings.soundVolume = 1f;
			}
			//carga de valor del sonido activo
			else if(!musicaIgualFX){
				GameSettings.soundVolume = PlayerPrefs.GetFloat(GameSettings.PP_SOUND);	
			}
		}
	}

	
	/*--------------------------------
	 * Settings
	 -------------------------------*/
	private void loadSettings(){
		if(!PlayerPrefs.HasKey(GameSettings.PP_GRAPHICS_DETAILS)){
			PlayerPrefs.SetFloat(GameSettings.PP_GRAPHICS_DETAILS, 1f);
			GameSettings.graphicsDetails = 1f;
		}
		else{
			GameSettings.graphicsDetails = PlayerPrefs.GetFloat(GameSettings.PP_GRAPHICS_DETAILS);
		}
	}

	/*--------------------------------
	 * Scores
	 -------------------------------*/
	private void cargarScores(){
		if(dificultades != null){
			foreach(GameDifficulty dif in dificultades){
				string difString = ((int) dif).ToString();
				string key = GameSettings.PP_LAST_SCORE + difString; //ultima_puntuacion_0 (en facil) , _1 (normal)...
				string key2 = GameSettings.PP_BEST_SCORE + difString;

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
		if(!PlayerPrefs.HasKey(GameSettings.PP_BEST_SCORE)){
			PlayerPrefs.SetInt(GameSettings.PP_BEST_SCORE, 0);
		}
		if(!PlayerPrefs.HasKey(GameSettings.PP_LAST_SCORE)){
			PlayerPrefs.SetInt(GameSettings.PP_LAST_SCORE, 0);
		}

		if(!PlayerPrefs.HasKey(GameSettings.PP_LAST_LEVEL_UNLOCKED)){
			PlayerPrefs.SetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED, 1);
			GameSettings.lastLevelUnlocked = 1;
		}
		else{
			GameSettings.lastLevelUnlocked = PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED);
		}
	}


}
