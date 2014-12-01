using UnityEngine;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	public static bool 	gameOver 		= false;
	public static bool 	gameStart 		= false;
	public static int 	time;
	public static int 	score 			= 0;
	public static int 	monedas 		= 0;
	public static bool 	misionSuperada 	= false;
	public static bool 	mostrandoQuest 	= false;
	public static bool 	forceGameOver 	= false;

	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private float gameoverDelay = 3f;

	[SerializeField]
	private bool isProps = false;


	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private GameMode 		gameMode;
	private bool 			showingGOPanel;
	private bool 			enPausa;
	private Nivel 			level;
	private GameoverType 	gameoverType;
	private GameDifficulty 	difficulty;
	private int 			collectedCoins = 0;
	private int 			gemsCollected = 0;


	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public GameMode GameMode {
		get {
			return this.gameMode;
		}
	}

	public Nivel Level {
		get {
			return this.level;
		}
	}

	public bool EnPausa {
		get {
			return this.enPausa;
		}
	}
	public bool ShowingGOPanel {
		get {
			return this.showingGOPanel;
		}
	}
	public GameoverType GameoverType {
		get {
			return this.gameoverType;
		}
	}
	
	public GameDifficulty Difficulty {
		get {
			return this.difficulty;
		}
	}

	//--------------------------------------
	// Metodos de Unity
	//--------------------------------------
	#region Unity
	void Awake () {
		difficulty = (GameDifficulty)PlayerPrefs.GetInt (Configuration.PP_GAME_DIFFICULTY);
		gameMode = (GameMode) PlayerPrefs.GetInt (Configuration.PP_GAME_MODE);
		level = FindObjectOfType<Nivel> () as Nivel;


		Time.timeScale = 1f;
		time = 0;
		score = 0;
		collectedCoins = 0;
		gemsCollected = 0;
		monedas = 0;
		gameoverType = GameoverType.MISION_SUPERADA;
		misionSuperada = false;
		gameOver = false;
		forceGameOver = false;
		gameStart = false;
	}

	void Start(){
		ScreenLoaderIndicator.Instance.finCarga ();
	}

	void Update () {
		if(!isProps || !Configuration.mandatoryTutorial){

			gameOver = esGameover ();

			if(!gameOver && !enPausa && mostrandoQuest){
				UIHandler.Instance.abrir(Ventana.MOSTRAR_MISION);

			}

			//fin de la partida
			if(!showingGOPanel && !enPausa && GameManager.gameOver){
				showingGOPanel = true;
				StartCoroutine(delayedGameOver());
			}
		}
	}

	void OnApplicationPause(bool paused){
		if(Configuration.mandatoryTutorial) return;
		
		pause (paused);
		
		//		if(!gameOver && !mostrandoQuest){
		//			Time.timeScale = paused ? 0f : 1f;
		//
		//			pause (!paused);
		//		}
	}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private IEnumerator delayedGameOver(){
		yield return new WaitForSeconds (gameoverDelay);

		GestorSonidos.Instance.stopAllWhenGameOver(); //detener todos los sonidos que se tienen que parar en el game over
		GestorSonidos.Instance.play(GestorSonidos.ID_SONIDO.FX_GAME_OVER);
		RankingHandler.Instance.enviarPuntuacion(score); //enviar puntos
		UIHandler.Instance.abrir(Ventana.GAMEOVER); //mostrar la ventana finalmente

		#if  (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		
		//			//refrescamos el banner
		AdsHandler.Instance.refrescarBanner();
		
		//--
		//solo mostramos la publi pantallazo cada ciertas veces
		gestionarMostrarPantallazoPubli();
		//--
		#endif

		//gestionLogros fin partida
		checkLogrosFinPartida ();




	}
	
	private bool esGameover(){
		if(misionSuperada)
			gameoverType = GameoverType.MISION_SUPERADA;


		return !isProps && ( misionSuperada || forceGameOver);
	}

	private IEnumerator countTime(){
		yield return new WaitForSeconds(1f);
		time++;

		if(gameStart && !gameOver)
			StartCoroutine (countTime ());
	}

	private void checkLogrosFinPartida(){

	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void pause(bool pause = true){
		Time.timeScale = pause ? 0f : 1f;

		UIHandler.Instance.abrir (Ventana.PAUSA, pause);

		enPausa = pause;
	}

	// add score function
	public void AddScore(){
		AddScore( level.PuntosPorObjetivo);
	}
	public void AddScore(int _score){
		if(!gameOver){
			score += _score;
		
			//comprobar las estrellas que se consiguen
			if(level != null)
				level.comprobarConseguirEstrella (); 
		}
	}





	/// <summary>
	/// Cuando se destruye una pelota, es decir, no es capturada por un carro
	/// se decrementa el contador de pelotas en juego
	/// </summary>
	public void decrementaPuntosPorEliminacionObjeto(){



	}

	public void seFuerzaGameOver(GameoverType t){
		gameoverType = t;
		forceGameOver = true;
	}

	private void gestionarMostrarPantallazoPubli(){
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

		//finally show interstitial ad
		if(contGameovers % totalGOsToShowAd == 0)
			AdsHandler.Instance.mostrarPantallazo();
	}

	

	 
}
