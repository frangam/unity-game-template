using UnityEngine;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	public static bool 	isGameOver 		= false;
	public static bool 	gameStart 		= false;
	public static int 	time;
	public static int 	score 			= 0;
	public static int 	monedas 		= 0;
	public static bool 	completedMission 	= false;
	public static bool 	showingMission 	= false;


	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private float gameoverDelay = 3f;

	[SerializeField]
	/// <summary>
	/// True if this object is part of a prop, a world decoration
	/// </summary>
	private bool isProps = false;

	[SerializeField]
	/// <summary>
	/// True if we want to pause time scale or not
	/// </summary>
	private bool pauseTimeWhenPaused = false;


	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private GameMode 		gameMode;
	private bool 			showingGOPanel;
	private bool 			inPause;
	private Nivel 			currentLevel;
	private GameoverType 	gameoverType;
	private GameDifficulty 	difficulty;
	private int 			collectedCoins = 0;
	private int 			gemsCollected = 0;
	private bool 			forcedGameOver 	= false;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	/// <summary>
	/// Gets the game mode of the current game
	/// </summary>
	/// <value>The game mode.</value>
	public GameMode GameMode {
		get {
			return this.gameMode;
		}
	}

	/// <summary>
	/// Gets the current level
	/// </summary>
	/// <value>The level.</value>
	public Nivel CurrentLevel {
		get {
			return this.currentLevel;
		}
	}

	/// <summary>
	/// Checks if game is paused or not
	/// </summary>
	/// <value><c>true</c> if in pause; otherwise, <c>false</c>.</value>
	public bool InPause {
		get {
			return this.inPause;
		}
	}

	/// <summary>
	/// Checks if it is showing the Game Over panel
	/// </summary>
	/// <value><c>true</c> if showing GO panel; otherwise, <c>false</c>.</value>
	public bool ShowingGOPanel {
		get {
			return this.showingGOPanel;
		}
	}

	/// <summary>
	/// Gets the type of the gameover.
	/// </summary>
	/// <value>The type of the gameover.</value>
	public GameoverType GameoverType {
		get {
			return this.gameoverType;
		}
	}

	/// <summary>
	/// Gest the current diffulty of this game
	/// </summary>
	/// <value>The difficulty.</value>
	public GameDifficulty Difficulty {
		get {
			return this.difficulty;
		}
	}

	/// <summary>
	/// Gets if Game over has been forced or not
	/// </summary>
	/// <value><c>true</c> if forced game over; otherwise, <c>false</c>.</value>
	public bool ForcedGameOver {
		get {
			return this.forcedGameOver;
		}
	}

	//--------------------------------------
	// Metodos de Unity
	//--------------------------------------
	#region Unity
	void Awake () {
		//initialize all attributes

		difficulty = (GameDifficulty)PlayerPrefs.GetInt (Configuration.PP_GAME_DIFFICULTY);
		gameMode = (GameMode) PlayerPrefs.GetInt (Configuration.PP_GAME_MODE);
		currentLevel = FindObjectOfType<Nivel> () as Nivel;


		Time.timeScale = 1f;
		time = 0;
		score = 0;
		collectedCoins = 0;
		gemsCollected = 0;
		monedas = 0;
		gameoverType = GameoverType.COMPLETED_MISSION; //default Game over
		completedMission = false;
		isGameOver = false;
		forcedGameOver = false;
		gameStart = false;
	}

	/// <summary>
	/// The Game Logic Loop
	/// </summary>
	void Update () {
		//if we are in a real game
		if(!isProps || !Configuration.mandatoryTutorial){
			//check if it is game over
			isGameOver = checkGameover ();

			//show mission
			if(!isGameOver && !inPause && showingMission){
				showingMission = false;
				UIHandler.Instance.abrir(GameScreen.SHOW_MISSION);
			}

			//It is Game over
			if(!showingGOPanel && !inPause && isGameOver){
				showingGOPanel = true;
				StartCoroutine(delayedGameOver());
			}
		}
	}

	void OnApplicationPause(bool paused){
		if(Configuration.mandatoryTutorial) return;
		pause (paused);
	}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private IEnumerator delayedGameOver(){
		yield return new WaitForSeconds (gameoverDelay);

		GestorSonidos.Instance.stopAllWhenGameOver();
		GestorSonidos.Instance.play(GestorSonidos.ID_SONIDO.FX_GAME_OVER);
		RankingHandler.Instance.enviarPuntuacion(score); //send score to server
		UIHandler.Instance.abrir(GameScreen.GAMEOVER); //show Game over screen

		//show Ads at the end
		#if  (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		AdsHandler.Instance.refrescarBanner();
		showInterstitialAd();
		//--
		#endif

		//manage achievements at the end of the game
		checkAchievementsWhenGameOver ();
	}

	/// <summary>
	/// Checks if it is Game over or not
	/// </summary>
	/// <returns><c>true</c>, if gameover was ised, <c>false</c> otherwise.</returns>
	private bool checkGameover(){
		if(completedMission)
			gameoverType = GameoverType.COMPLETED_MISSION;


		return !isProps && ( completedMission || forceGameOver);
	}

	/// <summary>
	/// Counts the game time during player is alive
	/// </summary>
	/// <returns>The time.</returns>
	private IEnumerator countTimePlayerAlive(){
		yield return new WaitForSeconds(1f); //1 second
		time++;

		if(gameStart && !isGameOver)
			StartCoroutine (countTimePlayerAlive ());
	}

	/// <summary>
	/// Checks the achievements when game over.
	/// </summary>
	private void checkAchievementsWhenGameOver(){

	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Pause the game
	/// </summary>
	/// <param name="pause">If set to <c>true</c> pause.</param>
	public void pause(bool pause = true){
		Time.timeScale = pause && pauseTimeWhenPaused ? 0f : 1f;
		UIHandler.Instance.abrir (GameScreen.PAUSE, pause);
		inPause = pause;
	}

	/// <summary>
	/// Adds the score specified in current level score by target property
	/// </summary>
	public void AddScore(){
		AddScore(currentLevel.PuntosPorObjetivo);
	}

	/// <summary>
	/// Adds an specific score
	/// </summary>
	/// <param name="_score">_score.</param>
	public void AddScore(int _score){
		if(!isGameOver){
			score += _score;
		
			//comprobar las estrellas que se consiguen
			if(currentLevel != null)
				currentLevel.comprobarConseguirEstrella (); 
		}
	}

	/// <summary>
	/// Forces the game over occurs.
	/// </summary>
	/// <param name="t">T.</param>
	public void forceGameOver(GameoverType t){
		gameoverType = t;
		forcedGameOver = true;
	}

	/// <summary>
	/// Manage when shows interstitial ad
	/// </summary>
	private void showInterstitialAd(){
		int totalGOsToShowAd = 1;
		int contGameovers = 0;

		switch(difficulty){
		case GameDifficulty.EASY:
			Configuration.contInterstitialAdsEASYMODE++;
			contGameovers = Configuration.contInterstitialAdsEASYMODE;
			totalGOsToShowAd = Configuration.NUM_GAMEOVERS_SHOW_AD_EASY_MODE;
			break;
			
		case GameDifficulty.NORMAL:
			Configuration.contInterstitialAdsNORMALMODE++;
			contGameovers = Configuration.contInterstitialAdsNORMALMODE;
			totalGOsToShowAd = Configuration.NUM_GAMEOVERS_SHOW_AD_NORMAL_MODE;
			break;
			
		case GameDifficulty.HARD:
			Configuration.contInterstitialAdsHARDMODE++;
			contGameovers = Configuration.contInterstitialAdsHARDMODE;
			totalGOsToShowAd = Configuration.NUM_GAMEOVERS_SHOW_AD_HARD_MODE;
			break;

		case GameDifficulty.NONE:
			Configuration.contInterstitialAdsDEFAULT++;
			contGameovers = Configuration.contInterstitialAdsDEFAULT;
			totalGOsToShowAd = Configuration.NUM_GAMEOVERS_SHOW_AD_BY_DEFAULT;
			break;
		}

		//finally show interstitial ad if verify
		if(contGameovers % totalGOsToShowAd == 0)
			AdsHandler.Instance.mostrarPantallazo();
	}	 
}
