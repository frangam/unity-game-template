/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class GameLoaderManager : Singleton<GameLoaderManager> {
	//--------------------------------------
	// Constants
	//--------------------------------------
	private const int totalLoadingSteps 						= 5;
	private const float DUMMY_WAIT_TIME 						= 3.5f;
	private const float MAX_WAIT_TIME_TO_CHECK_GPS_CONNECTION 	= 10; 	//Google Play Services
	private const float MAX_WAIT_TIME_TO_CHECK_GC_CONNECTION 	= 10; 	//Game Center
	private const float MAX_WAIT_TIME_TO_CHECK_WP_CONNECTION 	= 8; 	//Windows Phone
	public const float 	WAIT_TIME_TO_INIT_INAPP 				= 15; 	//In app Billing
	private const float MAX_WAIT_TIME_TO_CHECK_FB_CONNECTION 	= 5; 	//Facebook
	private const float MAX_WAIT_TIME_TO_RESTORE_PURCHASES 		= 15; 	//restore purchases
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private UILoadingPanel loadingPanel;
	private bool showLoadingPanel;
	private int  currentLoadingStepProgress = 1;
	private bool gpsPrepared = false;
	private bool gcPrepared = false;
	private bool twInited = false;
	private bool fbInited = false;
	private bool inAppInited = false;
	private bool inAppNeedRestoreProducts = false;
	private bool inAppAllProductsRestored = false;
	private bool initedGralServices = false;
	private bool showLoginWindowGameServices = false;
	private bool loadingNextScene = false;
	private bool languagesLoaded = false;
	
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
	
	public bool InAppInited {
		get {
			return this.inAppInited;
		}
		set {
			inAppInited = value;
		}
	}
	
	public bool InAppNeedRestoreProducts {
		get {
			return this.inAppNeedRestoreProducts;
		}
		set {
			inAppNeedRestoreProducts = value;
		}
	}
	
	public bool InAppAllProductsRestored {
		get {
			return this.inAppAllProductsRestored;
		}
		set {
			inAppAllProductsRestored = value;
		}
	}
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	
	
	public virtual void Awake () {
		loadingPanel = FindObjectOfType<UILoadingPanel>();
		showLoadingPanel = loadingPanel != null;
		
		GTDebug.log("Initializing");
		
		
		loadInitialMoneyOnlyFirstTime();
		loadSettings();
		loadAudio (); 
		loadScoresAndInitialLevel ();
		loadScoresWithDifficulty ();
		LoadGPSandGC (); //google play services y game center
		loadLanguage (); 
	}
	
	public virtual void Start(){
		StartCoroutine (waitTimeForLoadServicesAndLoadNextSceneIfNotLoaded ()); 
	}
	
	public virtual void LateUpdate(){
		if(BaseGameScreenController.Instance.Section != GameSection.LOAD_SCREEN || loadingNextScene) return;
		
		initedGralServices =
			//inapp
			(!GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE || (GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE && ((inAppInited && !inAppNeedRestoreProducts) || (inAppInited && inAppNeedRestoreProducts && inAppAllProductsRestored))))
				//facebook
				&& (!GameSettings.Instance.USE_FACEBOOK || (GameSettings.Instance.USE_FACEBOOK && fbInited))
				//twitter
				&& (!GameSettings.Instance.USE_TWITTER || (GameSettings.Instance.USE_TWITTER && twInited));
		bool initedAllSevices = initedGralServices && languagesLoaded;
		
		#if UNITY_ANDROID
		initedAllSevices = initedAllSevices && 
			(!GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES || (GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES && gpsPrepared));
		
		#elif UNITY_IPHONE
		initedAllSevices = initedAllSevices &&
			(!GameSettings.Instance.USE_GAMECENTER || (GameSettings.Instance.USE_GAMECENTER && gcPrepared));
		
		#elif UNITY_WP8
		
		#endif
		
		
		if(initedAllSevices){
			GTDebug.log("Inited all Services -> Loading next scene");
			
			handleInitialAdShowing();
			loadingNextScene = true;
			
			if(GameSettings.mandatoryTutorial)
				ScreenLoaderVisualIndicator.Instance.LoadScene(GameSection.TUTORIAL, GameSettings.Instance.showLoadIndicatorInLoadingScene);
			else
				ScreenLoaderVisualIndicator.Instance.LoadScene (GameSection.MAIN_MENU, GameSettings.Instance.showLoadIndicatorInLoadingScene);
		}
	}
	#endregion
	
	/*--------------------------------
	 * Show loading progress by second
	 -------------------------------*/
	private void incProgressAndShow(){
		float loadingProgressForEverySecond = currentLoadingStepProgress / totalLoadingSteps;
		
		if(showLoadingPanel){
			loadingPanel.show();
			UILoadingPanel.Instance.changeProgress(loadingProgressForEverySecond, true);
		}
		currentLoadingStepProgress++;
	}
	
	private IEnumerator showProgressBySeconds(int totalSeconds){
		float loadingProgressForEverySecond = (currentLoadingStepProgress*1.0f/totalSeconds) / (totalLoadingSteps*1.0f);
		
		if(showLoadingPanel){
			loadingPanel.show();
			
			for(int i=0; i<totalSeconds; i++){
				yield return new WaitForSeconds(1);
				
				UILoadingPanel.Instance.changeProgress(loadingProgressForEverySecond, true);
			}
		}
		
		currentLoadingStepProgress++;
	}
	
	//	/*--------------------------------
	//	 * In App Billing
	//	 -------------------------------*/
	//	private IEnumerator waitForInAppBillingRestoreProductsAtStartIfNeeded(){
	//
	//	}
	
	/*--------------------------------
	 * Load tutorial or menu
	 -------------------------------*/
	public virtual IEnumerator waitTimeForLoadServicesAndLoadNextSceneIfNotLoaded(){ 
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
		
		///-----
		//Wait time if languages are not loaded
		///-----
		GTDebug.log("Languages Loaded ? "+languagesLoaded);
		
		if(!languagesLoaded){
			do{
				yield return null;
			}
			while(!languagesLoaded);
			GTDebug.log("Languages loaded");
		}
		
		///-----
		//Wait time if INAPP is not inited
		///-----
		GTDebug.log("InApp Billing inited ? "+inAppInited);
		
		if(!Application.isEditor && GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE && !inAppInited){
			float startTime = 0, currentTime = 0, elapsedTime = 0;
			startTime = Time.realtimeSinceStartup;
			StartCoroutine(showProgressBySeconds((int)WAIT_TIME_TO_INIT_INAPP));
			do{
				currentTime = Time.realtimeSinceStartup;
				elapsedTime = currentTime-startTime;
				yield return null;
			}
			while(!inAppInited && elapsedTime<WAIT_TIME_TO_INIT_INAPP);
			
			GTDebug.log("InApp Billing inited ? "+inAppInited);
			GTDebug.log("InApp Billing inited Check time out ? "+(elapsedTime>=WAIT_TIME_TO_INIT_INAPP));
			
		}
		
		///-----
		//Wait time if we are restoring purchases
		///-----
		GTDebug.log("Wait time if wa are restoring purchases ? "+InAppNeedRestoreProducts);
		
		if(!Application.isEditor && GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE && InAppNeedRestoreProducts){
			float startTime = 0, currentTime = 0, elapsedTime = 0;
			startTime = Time.realtimeSinceStartup;
			StartCoroutine(showProgressBySeconds((int)MAX_WAIT_TIME_TO_RESTORE_PURCHASES));
			do{
				currentTime = Time.realtimeSinceStartup;
				elapsedTime = currentTime-startTime;
				yield return null;
			}
			while(!inAppAllProductsRestored && elapsedTime<MAX_WAIT_TIME_TO_RESTORE_PURCHASES);
			
			GTDebug.log("All purchases restored ? "+inAppAllProductsRestored);
		}
		
		if (!Application.isEditor) {
			///-----
			/// Wait time for each platform
			///-----
			#if UNITY_ANDROID
			if(GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES){
				//			StartCoroutine(showProgressBySeconds((int)MAX_WAIT_TIME_TO_CHECK_GPS_CONNECTION));
				//			yield return new WaitForSeconds (MAX_WAIT_TIME_TO_CHECK_GPS_CONNECTION);
				
				float startTime = 0, currentTime = 0, elapsedTime = 0;
				startTime = Time.realtimeSinceStartup;
				StartCoroutine(showProgressBySeconds((int)MAX_WAIT_TIME_TO_CHECK_GPS_CONNECTION));
				do{
					currentTime = Time.realtimeSinceStartup;
					elapsedTime = currentTime-startTime;
					yield return null;
				}
				while(!gcPrepared && elapsedTime<MAX_WAIT_TIME_TO_CHECK_GPS_CONNECTION);
				
				GTDebug.log("Google Play Services Inited ? "+gpsPrepared);
				GTDebug.log("Google Play Services Check time out ? "+(elapsedTime>=MAX_WAIT_TIME_TO_CHECK_GPS_CONNECTION));
				
				loadSceneAfterChecking();
			}
			#elif UNITY_IPHONE
			if (GameSettings.Instance.USE_GAMECENTER) {
				//			StartCoroutine(showProgressBySeconds((int)MAX_WAIT_TIME_TO_CHECK_GC_CONNECTION));
				//			yield return new WaitForSeconds (MAX_WAIT_TIME_TO_CHECK_GC_CONNECTION);
				
				float startTime = 0, currentTime = 0, elapsedTime = 0;
				startTime = Time.realtimeSinceStartup;
				StartCoroutine (showProgressBySeconds ((int)MAX_WAIT_TIME_TO_CHECK_GC_CONNECTION));
				do {
					currentTime = Time.realtimeSinceStartup;
					elapsedTime = currentTime - startTime;
					yield return null;
				} while(!gcPrepared && elapsedTime<MAX_WAIT_TIME_TO_CHECK_GC_CONNECTION);
				
				GTDebug.log ("Game Center Inited ? " + gcPrepared);
				GTDebug.log ("Game Center Inited Check time out ? " + (elapsedTime >= MAX_WAIT_TIME_TO_CHECK_GC_CONNECTION));
				
				loadSceneAfterChecking ();
			}
			#else
			//		StartCoroutine(showProgressBySeconds((int)TIEMPO_ESPERA_COMPROBAR_WP8_CONEXION));
			//		yield return new WaitForSeconds(TIEMPO_ESPERA_COMPROBAR_WP8_CONEXION);
			
			//		float startTime = 0, currentTime = 0, elapsedTime = 0;
			//		startTime = Time.realtimeSinceStartup;
			//		StartCoroutine(showProgressBySeconds((int)TIEMPO_ESPERA_COMPROBAR_WP8_CONEXION));
			//		do{
			//			currentTime = Time.realtimeSinceStartup;
			//			elapsedTime = currentTime-startTime;
			//			yield return null;
			//		}
			//		while(! && elapsedTime<TIEMPO_ESPERA_COMPROBAR_WP8_CONEXION);
			//		
			//		GTDebug.log("Google Play Services Inited ? "+gpsPrepared);
			//		GTDebug.log("Google Play Services Check time out ? "+(elapsedTime>=TIEMPO_ESPERA_COMPROBAR_WP8_CONEXION));
			
			loadSceneAfterChecking();
			#endif
		} //end_if(!Application.isEditor)
		
		float wait = 0;
		bool inappNotInited = (GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE && (!inAppInited || (inAppInited && inAppNeedRestoreProducts && !inAppAllProductsRestored)));
		bool twNotInited = (GameSettings.Instance.USE_TWITTER && !twInited);
		bool fbNotInited = (GameSettings.Instance.USE_FACEBOOK && !fbInited);
		///-----
		/// Wait more time
		///-----
		if(!Application.isEditor && (twNotInited ||  fbNotInited || inappNotInited)){
			if(inappNotInited)
				wait += WAIT_TIME_TO_INIT_INAPP;
			else if(twNotInited || fbNotInited)
				wait += MAX_WAIT_TIME_TO_CHECK_FB_CONNECTION;
			
			
			//			yield return new WaitForSeconds (wait);
			
			
			float startTime = 0, currentTime = 0, elapsedTime = 0;
			startTime = Time.realtimeSinceStartup;
			StartCoroutine(showProgressBySeconds((int)wait));
			do{
				currentTime = Time.realtimeSinceStartup;
				elapsedTime = currentTime-startTime;
				yield return null;
			}
			while((twNotInited || inappNotInited || fbNotInited) && elapsedTime<wait);
			
			GTDebug.log("InApp Inited ? "+!inappNotInited+", Twitter Inited ? " +!twNotInited+", FB Inited ? "+fbNotInited);
			GTDebug.log("Check time out ? "+(elapsedTime>=wait));
			
			
			loadSceneAfterChecking();
		}
		//Wait a dummy time if we not use any service
		else if(Application.isEditor || 
		        (!GameSettings.Instance.USE_TWITTER && !GameSettings.Instance.USE_FACEBOOK 
		 && !GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES && !GameSettings.Instance.USE_GAMECENTER 
		 && !GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE
		 )
		        ){
			
			//Wait a dummy time
			float startTime = 0, currentTime = 0, elapsedTime = 0;
			startTime = Time.realtimeSinceStartup;
			StartCoroutine(showProgressBySeconds((int)DUMMY_WAIT_TIME));
			do{
				currentTime = Time.realtimeSinceStartup;
				elapsedTime = currentTime-startTime;
				yield return null;
			}
			while(elapsedTime<wait);
			
			loadSceneAfterChecking();
		}
	}
	
	
	
	private void loadSceneAfterChecking(){
		if(!loadingNextScene){
			loadingNextScene = true;
			
			//finally load the scene: tutorial or menu
			if(GameSettings.mandatoryTutorial){
				ScreenLoaderVisualIndicator.Instance.LoadScene(GameSection.TUTORIAL, GameSettings.Instance.showLoadIndicatorInLoadingScene);
			}
			else{
				handleInitialAdShowing();
				ScreenLoaderVisualIndicator.Instance.LoadScene (GameSection.MAIN_MENU, GameSettings.Instance.showLoadIndicatorInLoadingScene);
			}
		}
	}
	
	/*--------------------------------
	 * Google play
	 -------------------------------*/
	public virtual void LoadGPSandGC(){
		#if UNITY_ANDROID
		if(GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES)
			GPSConnect.Instance.init(showLoginWindowGameServices);
		#elif UNITY_IPHONE
		if(GameSettings.Instance.USE_GAMECENTER)
			GCConnect.Instance.init(showLoginWindowGameServices);
		#endif
	}
	
	/*--------------------------------
	 * Language selected
	 -------------------------------*/
	public virtual void loadLanguage(){
		//load in a list all SystemLanguage
		Languages.Instance.loadAllLanguagesInGameSettings();
		
		if(GameSettings.Instance.useTestLanguage){
			Languages.Instance.selectLanguage(GameSettings.Instance.testLanguage);
		}
		else{
			//if not changed language
			if(!PlayerPrefs.HasKey(GameSettings.PP_LANGUAGE_CHANGED)){
				PlayerPrefs.SetInt(GameSettings.PP_LANGUAGE_CHANGED, 0);
			}
			
			// if player has not changed the language, we load the device language if we are supporting it if not we set English by default
			if(PlayerPrefs.GetInt(GameSettings.PP_LANGUAGE_CHANGED) == 0){
				Languages.Instance.selectLanguageOfDeviceIfSupported();
			}
			else{
				Languages.Instance.selectCurrentLanguage();
			}
		}
		
		languagesLoaded = true;
		
		GTDebug.log("Current Language: " + Languages.Instance.CurrentLanguage);
	}
	
	/*--------------------------------
	 * Audio activo
	 -------------------------------*/
	public virtual void loadAudio(){
		//Musica activa
		if(!PlayerPrefs.HasKey(GameSettings.PP_MUSIC)){
			//inicializacion de sonido y musica activos
			if (GameSettings.Instance.FX_AND_MUSIC_ARE_THE_SAME) {
				BaseSoundManager.Instance.SetCurrentGeneralMusicVolume();
				BaseSoundManager.Instance.SetCurrentGeneralSoundVolume();
			}
			//inicializacion de musica activa
			else{
				BaseSoundManager.Instance.SetCurrentGeneralMusicVolume();
			}
		}
		
		//sonidoActivo
		if(!PlayerPrefs.HasKey(GameSettings.PP_SOUND)){
			//inicializacion de sonido y musica activos
			if (GameSettings.Instance.FX_AND_MUSIC_ARE_THE_SAME) {
				BaseSoundManager.Instance.SetCurrentGeneralMusicVolume();
				BaseSoundManager.Instance.SetCurrentGeneralSoundVolume();
			}
			//inicializacion de musica activa
			else{
				BaseSoundManager.Instance.SetCurrentGeneralSoundVolume();
			}
		}
		else{
			//inicializacion solo de valor del sonido activo
			if(!GameSettings.Instance.FX_AND_MUSIC_ARE_THE_SAME && !PlayerPrefs.HasKey(GameSettings.PP_SOUND)){
				BaseSoundManager.Instance.SetCurrentGeneralSoundVolume();
			}
		}
	}
	
	
	/*--------------------------------
	 * Settings
	 -------------------------------*/
	public virtual void loadSettings(){
		//Android immersive mode
		#if UNITY_ANDROID
		if(GameSettings.Instance.ENABLE_ANDROID_IMMERSIVE_MODE)
			ImmersiveMode.instance.EnableImmersiveMode();
		#endif
		
		//show ads or not
		bool purchasedQuitAds = PlayerPrefs.GetInt(GameSettings.PP_PURCHASED_QUIT_ADS) == 1;
		if(purchasedQuitAds) GameSettings.Instance.IS_PRO_VERSION = true;
		
		GTDebug.log("Is Pro version ? " + GameSettings.Instance.IS_PRO_VERSION);
		
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
		
		//Show missions window at start
		if(GameSettings.Instance.showMissionsWinAtStart)
			PlayerPrefs.SetInt(GameSettings.PP_SHOW_MISSIONS_WINDOW, 1);
		else
			PlayerPrefs.SetInt(GameSettings.PP_SHOW_MISSIONS_WINDOW, 0);
		
		//game opening + 1 
		int totalOpenings = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_GAME_OPENINGS);
		totalOpenings++;
		PlayerPrefs.SetInt(GameSettings.PP_TOTAL_GAME_OPENINGS, totalOpenings);
		
		//GA
		//TODO Analytics GAEvents.GAME_OPENING
		
		GTDebug.log("Total openings: " + totalOpenings);
		
		//game store services
		storeGameServicesSettings(totalOpenings);
		
		//Social Networks use
		if(!GameSettings.Instance.USE_FACEBOOK)
			fbInited = true;
		if(!GameSettings.Instance.USE_TWITTER)
			twInited = true;
		
		//Easy game difficulty by default
		if(!PlayerPrefs.HasKey(GameSettings.PP_GAME_DIFFICULTY)){
			PlayerPrefs.SetInt(GameSettings.PP_GAME_DIFFICULTY, (int) GameDifficulty.EASY);
		}
		
		//loading indicator
		if(GameSettings.Instance.showLoadIndicatorInLoadingScene)
			ScreenLoaderVisualIndicator.Instance.ShowLoadIndicator();
	}
	
	/// <summary>
	/// Init store services or not (Google Play Services and Game Center)
	/// </summary>
	/// <param name="totalOpenings">Total openings.</param>
	public virtual void storeGameServicesSettings(int totalOpenings){
		bool lastOpeningWasConnected = PlayerPrefs.GetInt(GameSettings.PP_LAST_OPENNING_USER_CONNECTED_TO_STORE_SERVICE) != 0 ? true : false;
		
		GTDebug.log("Was Last opening connected to store services ? " + lastOpeningWasConnected);
		
		bool loadGameSettings = !lastOpeningWasConnected;
		showLoginWindowGameServices = lastOpeningWasConnected;
		
		if(loadGameSettings){
			#if UNITY_ANDROID
			loadGameSettings = GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES;
			#elif UNITY_IPHONE
			loadGameSettings = GameSettings.Instance.USE_GAMECENTER;
			#endif
			
			if(loadGameSettings){
				//first time
				if(totalOpenings == 1){
					#if UNITY_ANDROID
					showLoginWindowGameServices = GameSettings.Instance.SHOW_LOGIN_GOOGLE_PLAY_SERVICES_THE_FIRST_OPENING;
					#elif UNITY_IPHONE
					showLoginWindowGameServices = GameSettings.Instance.SHOW_LOGIN_GAME_CENTER_THE_FIRST_OPENING;
					#endif
				}
				//other times
				else{
					int eachOpening = 0;
					#if UNITY_ANDROID
					eachOpening = GameSettings.Instance.NUM_GAME_OPENING_TO_INIT_GOOLGE_PLAY_SERVICES;
					#elif UNITY_IPHONE
					eachOpening = GameSettings.Instance.NUM_GAME_OPENING_TO_INIT_GAME_CENTER;
					#endif
					
					showLoginWindowGameServices =  eachOpening > 0 && totalOpenings % eachOpening == 0;
				}
			}//end_if_loadGameSettings
		}//end_if_loadGameSettings
	}
	
	/*--------------------------------
	 * Scores
	 -------------------------------*/
	public virtual void loadScoresWithDifficulty(){
		if(GameSettings.Instance.gameDifficulties != null && GameSettings.Instance.gameDifficulties.Count > 0){
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
	public virtual void loadScoresAndInitialLevel(){
		//scores
		if(!PlayerPrefs.HasKey(GameSettings.PP_BEST_SCORE)){
			PlayerPrefs.SetInt(GameSettings.PP_BEST_SCORE, 0);
		}
		if(!PlayerPrefs.HasKey(GameSettings.PP_LAST_SCORE)){
			PlayerPrefs.SetInt(GameSettings.PP_LAST_SCORE, 0);
		}
		
		//initial unlocked level
		if(!PlayerPrefs.HasKey(GameSettings.PP_LAST_LEVEL_UNLOCKED)){
			PlayerPrefs.SetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED, 1);
			GameSettings.lastLevelUnlocked = 1;
		}
		else{
			GameSettings.lastLevelUnlocked = PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED);
		}
		
		//last unlocked survival level
		if(!PlayerPrefs.HasKey(GameSettings.PP_LAST_UNLOCKED_SURVIVAL_LEVEL))
			PlayerPrefs.SetInt(GameSettings.PP_LAST_UNLOCKED_SURVIVAL_LEVEL, 1);
		
		//select level 1 at start
		if(!PlayerPrefs.HasKey(GameSettings.PP_SELECTED_LEVEL))
			PlayerPrefs.SetInt(GameSettings.PP_SELECTED_LEVEL, 1);
		if(!PlayerPrefs.HasKey(GameSettings.PP_SELECTED_SURVIVAL_LEVEL))
			PlayerPrefs.SetInt(GameSettings.PP_SELECTED_SURVIVAL_LEVEL, 1);
		
	}
	
	/*--------------------------------
	 * Money
	 -------------------------------*/
	public virtual void loadInitialMoneyOnlyFirstTime(){
		//money
		if(!PlayerPrefs.HasKey(GameSettings.PP_TOTAL_MONEY)){
			GameMoneyManager.Instance.addMoney(GameSettings.Instance.INITIAL_MONEY);
		}
		
		//gems
		if(!PlayerPrefs.HasKey(GameSettings.PP_TOTAL_GEMS)){
			GameMoneyManager.Instance.addMoney(GameSettings.Instance.INITIAL_GEMS);
		}
	}
	
	
	/*--------------------------------
	 * Initial Ad
	 -------------------------------*/
	public virtual void handleInitialAdShowing(){
		int totalOpenings = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_GAME_OPENINGS);
		int openingsForShowInitialAd = GameSettings.Instance.TIMES_START_GAME_TO_SHOW_AD_AT_START;
		
		//show initial Add
		if(openingsForShowInitialAd > 0 && totalOpenings % openingsForShowInitialAd == 0){
			GTDebug.log("Showing interstitial ad when start game");
			AdsHandler.Instance.showInterstitial();
		}
	}
}
