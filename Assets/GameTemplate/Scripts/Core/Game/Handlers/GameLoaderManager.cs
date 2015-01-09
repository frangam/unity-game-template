using UnityEngine;
using System.Collections;

public class GameLoaderManager : Singleton<GameLoaderManager> {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool deletePlayerPrefs = false;
	
	
	
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
	public virtual void Awake () {
		//TEST
		if(deletePlayerPrefs)
			PlayerPrefs.DeleteAll();
		//				Localization.language = GameSettings.LOC_SPANISH;
		
		if(!GameSettings.Instance.USE_FACEBOOK)
			fbInited = true;
		if(!GameSettings.Instance.USE_TWITTER)
			twInited = true;
		
		//Easy game difficulty by default
		if(!PlayerPrefs.HasKey(GameSettings.PP_GAME_DIFFICULTY)){
			PlayerPrefs.SetInt(GameSettings.PP_GAME_DIFFICULTY, (int) GameDifficulty.EASY);
		}
		
		ScreenLoaderVisualIndicator.Instance.LoadScene ();
		
		cargarIdioma (); //idioma
		//		Localization.language = GameSettings.LOC_ENGLISH;
		
		loadInitialMoneyOnlyFirstTime();
		loadSettings();
		cargarAudio (); //musica y sonido
		cargarPuntosYNivelInicial (); //puntos
		LoadGPSandGC (); //google play services y game center
		cargarScores ();
		
		StartCoroutine (LoadTutorialOrMenu ()); //se carga el tutorial o el menu del juego
	}
	
	public virtual void LateUpdate(){
		if(BaseGameScreenController.Instance.Section != GameSection.LOAD_SCREEN) return;
		
		#if UNITY_ANDROID
		if((!GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES && twInited && fbInited && !GameSettings.mandatoryTutorial)
		   || (GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES && gpsPrepared && twInited && fbInited && !GameSettings.mandatoryTutorial)){
			AdsHandler.Instance.mostrarPantallazo();
			ScreenLoaderVisualIndicator.Instance.LoadScene (GameSettings.SCENE_MAINMENU);
		}
		else if(gpsPrepared && twInited && fbInited && GameSettings.mandatoryTutorial)
			Application.LoadLevel(GameSettings.SCENE_TUTORIAL);
		#elif UNITY_IPHONE
		if((!GameSettings.Instance.USE_GAMECENTER && twInited && fbInited && !GameSettings.mandatoryTutorial)
		   || (GameSettings.Instance.USE_GAMECENTER && gcPrepared && twInited && fbInited && !GameSettings.mandatoryTutorial)){
			AdsHandler.Instance.mostrarPantallazo();
			ScreenLoaderVisualIndicator.Instance.LoadScene (GameSettings.SCENE_MAINMENU);
		}
		else if(gcPrepared && twInited && fbInited && GameSettings.mandatoryTutorial)
			Application.LoadLevel(GameSettings.SCENE_TUTORIAL);
		#endif
		
		
		
	}
	#endregion
	
	/*--------------------------------
	 * Load tutorial or menu
	 -------------------------------*/
	private IEnumerator LoadTutorialOrMenu(){ 
		//tutorial
		GameSettings.mandatoryTutorial = GameSettings.Instance.HAS_INITIAL_TUTORIAL;
		if(GameSettings.Instance.HAS_INITIAL_TUTORIAL){
			if(!PlayerPrefs.HasKey(GameSettings.PP_COMPLETED_TUTORIAL)){
				PlayerPrefs.SetInt(GameSettings.PP_COMPLETED_TUTORIAL, 0);
				GameSettings.mandatoryTutorial = true;
			}
			else{
				GameSettings.mandatoryTutorial = PlayerPrefs.GetInt(GameSettings.PP_COMPLETED_TUTORIAL) == 0;	
			}
		}
		
		#if UNITY_ANDROID
		if(GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES)
			yield return new WaitForSeconds (TIEMPO_ESPERA_COMPROBAR_GPS_CONEXION);
		#elif UNITY_IPHONE
		if(GameSettings.Instance.USE_GAMECENTER)
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
			ScreenLoaderVisualIndicator.Instance.LoadScene (GameSettings.SCENE_MAINMENU);
		}
	}
	
	/*--------------------------------
	 * Google play
	 -------------------------------*/
	private void LoadGPSandGC(){
		#if UNITY_ANDROID
		if(GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES)
			GPSConnect.Instance.init();
		#elif UNITY_IPHONE
		if(GameSettings.Instance.USE_GAMECENTER)
			GCConnect.Instance.init();
		#endif
	}
	
	/*--------------------------------
	 * Idioma seleccionado
	 -------------------------------*/
	private void cargarIdioma(){
		if(!PlayerPrefs.HasKey(GameSettings.PP_LANGUAGE_CHANGED)){
			PlayerPrefs.SetInt(GameSettings.PP_LANGUAGE_CHANGED, 0);
			
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
			if (GameSettings.Instance.FX_AND_MUSIC_ARE_THE_SAME) {
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
			if (GameSettings.Instance.FX_AND_MUSIC_ARE_THE_SAME) {
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
			if (GameSettings.Instance.FX_AND_MUSIC_ARE_THE_SAME) {
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
			if(!GameSettings.Instance.FX_AND_MUSIC_ARE_THE_SAME && !PlayerPrefs.HasKey(GameSettings.PP_SOUND)){
				PlayerPrefs.SetFloat(GameSettings.PP_SOUND, 1f);	
				GameSettings.soundVolume = 1f;
			}
			//carga de valor del sonido activo
			else if(!GameSettings.Instance.FX_AND_MUSIC_ARE_THE_SAME){
				GameSettings.soundVolume = PlayerPrefs.GetFloat(GameSettings.PP_SOUND);	
			}
		}
	}
	
	
	/*--------------------------------
	 * Settings
	 -------------------------------*/
	private void loadSettings(){
		//graphics details
		if(!PlayerPrefs.HasKey(GameSettings.PP_GRAPHICS_DETAILS)){
			PlayerPrefs.SetFloat(GameSettings.PP_GRAPHICS_DETAILS, 1f);
			GameSettings.graphicsDetails = 1f;
		}
		else{
			GameSettings.graphicsDetails = PlayerPrefs.GetFloat(GameSettings.PP_GRAPHICS_DETAILS);
		}
		
		//character control sensitivity
		if(!PlayerPrefs.HasKey(GameSettings.PP_CHARACTER_CONTROL_SENSITIVITY)){
			PlayerPrefs.SetFloat(GameSettings.PP_CHARACTER_CONTROL_SENSITIVITY, GameSettings.Instance.INITIAL_CHAR_CONTROL_SENSITIVITY);
		}
		if(!PlayerPrefs.HasKey(GameSettings.PP_CHARACTER_CONTROL_MAX_SENSITIVITY)){
			PlayerPrefs.SetFloat(GameSettings.PP_CHARACTER_CONTROL_MAX_SENSITIVITY, GameSettings.Instance.MAX_CHAR_CONTROL_SENSITIVITY);
		}
		if(!PlayerPrefs.HasKey(GameSettings.PP_CHARACTER_CONTROL_MIN_SENSITIVITY)){
			PlayerPrefs.SetFloat(GameSettings.PP_CHARACTER_CONTROL_MIN_SENSITIVITY, GameSettings.Instance.MIN_CHAR_CONTROL_SENSITIVITY);
		}
		
		//Show missions window
		PlayerPrefs.SetInt(GameSettings.PP_SHOW_MISSIONS_WINDOW, 0);
		
		//last unlocked survival level
		if(!PlayerPrefs.HasKey(GameSettings.PP_LAST_UNLOCKED_SURVIVAL_LEVEL))
			PlayerPrefs.SetInt(GameSettings.PP_LAST_UNLOCKED_SURVIVAL_LEVEL, 1);
	}
	
	/*--------------------------------
	 * Scores
	 -------------------------------*/
	private void cargarScores(){
		if(GameSettings.Instance.gameDifficulties != null){
			foreach(GameDifficulty dif in GameSettings.Instance.gameDifficulties){
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
	
	/*--------------------------------
	 * Money
	 -------------------------------*/
	private void loadInitialMoneyOnlyFirstTime(){
		if(!PlayerPrefs.HasKey(GameSettings.PP_TOTAL_MONEY)){
			PlayerPrefs.SetInt(GameSettings.PP_TOTAL_MONEY, GameSettings.Instance.INITIAL_MONEY);
		}
		if(!PlayerPrefs.HasKey(GameSettings.PP_TOTAL_GEMS)){
			PlayerPrefs.SetInt(GameSettings.PP_TOTAL_GEMS, GameSettings.Instance.INITIAL_GEMS);
		}
	}
}
