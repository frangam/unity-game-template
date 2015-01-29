using UnityEngine;
using System.Collections;
using UnionAssets.FLE;
using System;

public class BaseGameManager : MonoBehaviour {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string GAME_FINISHED = "gt_game_finished";
	
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	private static EventDispatcherBase _dispatcher  = new EventDispatcherBase ();
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool 		pauseTimeAtStart = false;
	
	[SerializeField]
	private bool 		pauseTimeWhenFinishedGame = true;
	
	[SerializeField]
	private bool 		sendScoresToServer = true;
	
	[SerializeField]
	private string 		gameoverWindow = UIBaseWindowIDs.GAMEOVER;
	
	[SerializeField]
	private float 		gameOverDelay = 1.5f;
	
	[SerializeField]
	private float 		missionCompletedDelay = 2.5f;
	
	public GameObject 	explosionPrefab;
	
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool 			paused;
	private bool 			started;
	private bool 			inited;
	private bool 			finished;
	private GameDifficulty 	difficulty;
	private GameMode 		gameMode;
	private int 			currentScore;
	private int				currentLevelSelected;
	private bool			isGameOver;
	
	/// <summary>
	/// True if we are going to handle scores in the current game mode. Recommended to its value in a child class.
	/// </summary>
	protected bool 		handleScores = true;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public static EventDispatcherBase dispatcher {
		get {
			return _dispatcher;
		}
	}
	
	public GameDifficulty Difficulty {
		get {
			return this.difficulty;
		}
	}
	
	public GameMode GameMode {
		get {
			return this.gameMode;
		}
	}
	
	public int CurrentScore {
		get {
			return this.currentScore;
		}
		set {
			currentScore = value;
		}
	}
	
	public int CurrentLevelSelected {
		get {
			return this.currentLevelSelected;
		}
	}
	
	public bool Started {
		get {
			return this.started;
		}
	}
	
	public bool Inited {
		get {
			return this.inited;
		}
	}
	
	public bool Finished {
		get {
			return this.finished;
		}
	}
	
	public bool IsGameOver {
		get {
			return this.isGameOver;
		}
		set {
			isGameOver = value;
		}
	}
	
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	protected virtual void Awake(){
		initGame();
		
		if(gameMode == GameMode.CAMPAIGN){
			BaseLevelLoaderController.dispatcher.addEventListener(BaseLevelLoaderController.LEVEL_LOADED, OnLevelLoaded);
			BaseQuestManager.dispatcher.addEventListener(BaseQuestManager.ALL_QUESTS_COMPLETED, OnQuestsCompleted);
			BaseQuestManager.Instance.resetProperties();
		}
		
		
	}
	protected virtual void OnDestroy(){
		if(gameMode == GameMode.CAMPAIGN){
			BaseLevelLoaderController.dispatcher.removeEventListener(BaseLevelLoaderController.LEVEL_LOADED, OnLevelLoaded);
			BaseQuestManager.dispatcher.removeEventListener(BaseQuestManager.ALL_QUESTS_COMPLETED, OnQuestsCompleted);
		}
	}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void handleGameOverAdShowing(){
		int numGameovers = 0;
		int numGameoversToChek = GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_BY_DEFAULT;
		
		switch(gameMode){
		case GameMode.CAMPAIGN:
			numGameovers = PlayerPrefs.GetInt(GameSettings.PP_NUM_GAMEOVERS_IN_LEVEL+currentLevelSelected.ToString());
			numGameovers++;
			PlayerPrefs.SetInt(GameSettings.PP_NUM_GAMEOVERS_IN_LEVEL+currentLevelSelected.ToString(), numGameovers);
			break;
		case GameMode.QUICKGAME:
			if(difficulty != GameDifficulty.NONE){
				int dif = ((int) difficulty);
				numGameovers = PlayerPrefs.GetInt(GameSettings.PP_NUM_GAMEOVERS_WITH_DIFFICULTY+dif.ToString());
				numGameovers++;
				PlayerPrefs.SetInt(GameSettings.PP_NUM_GAMEOVERS_WITH_DIFFICULTY+dif.ToString(), numGameovers);
			}
			else{
				numGameovers = PlayerPrefs.GetInt(GameSettings.PP_NUM_GAMEOVERS_WITHOUT_DIFFICULTY);
				numGameovers++;
				PlayerPrefs.SetInt(GameSettings.PP_NUM_GAMEOVERS_WITHOUT_DIFFICULTY, numGameovers);
				
				switch(difficulty){
				case GameDifficulty.EASY: numGameoversToChek = GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_EASY_MODE; break;
				case GameDifficulty.NORMAL: numGameoversToChek = GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_NORMAL_MODE; break;
				case GameDifficulty.HARD: numGameoversToChek = GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_HARD_MODE; break;
				}
			}
			break;
		case GameMode.SURVIVAL:
			numGameoversToChek = GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_SURVIVAL_MODE;
			numGameovers = PlayerPrefs.GetInt(GameSettings.PP_NUM_GAMEOVERS_IN_SURVIVAL_LEVEL+currentLevelSelected.ToString());
			numGameovers++;
			PlayerPrefs.SetInt(GameSettings.PP_NUM_GAMEOVERS_IN_SURVIVAL_LEVEL+currentLevelSelected.ToString(), numGameovers);
			break;
		}
		
		//refresh banner ad
		AdsHandler.Instance.refrescarBanner();
		
		//show interstitial ad
		if(numGameovers % numGameoversToChek == 0)
			AdsHandler.Instance.mostrarPantallazo();
	}
	
	private IEnumerator finishGameWithDelay(){
		float delay = isGameOver ? gameOverDelay : missionCompletedDelay;
		
		yield return new WaitForSeconds(delay);
		
		PlayerPrefs.SetInt(GameSettings.PP_LAST_LEVEL_PLAYED, currentLevelSelected); //for analytics
		
		//show gameover windows
		switch(gameMode){
		case GameMode.CAMPAIGN:
			if(isGameOver){
				int prevTries = PlayerPrefs.GetInt(GameSettings.PP_LEVEL_TRIES_TIMES+currentLevelSelected.ToString()); //get the previous tries
				PlayerPrefs.SetInt(GameSettings.PP_LEVEL_TRIES_TIMES+currentLevelSelected.ToString(), prevTries+1); //update tries
				UIController.Instance.Manager.open(UIBaseWindowIDs.MISSION_FAILED);
			}
			else{
				int prevCompleted = PlayerPrefs.GetInt(GameSettings.PP_LEVEL_COMPLETED_TIMES+currentLevelSelected.ToString()); //get the previous completed times
				UIController.Instance.Manager.open(UIBaseWindowIDs.MISSION_COMPLETED); //first show window
				PlayerPrefs.SetInt(GameSettings.PP_LEVEL_COMPLETED_TIMES+currentLevelSelected.ToString(), prevCompleted+1); //update completed times
			}
			break;
			
		case GameMode.SURVIVAL:
		case GameMode.QUICKGAME:
			UIController.Instance.Manager.open(gameoverWindow);
			break;
			
		}
		
		//handle ad showing
		handleGameOverAdShowing();
		
		//pause game if needed
		if(pauseTimeWhenFinishedGame){
			Time.timeScale = 0f;
		}
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void initGame(){
		difficulty = (GameDifficulty) PlayerPrefs.GetInt(GameSettings.PP_GAME_DIFFICULTY); //get the selected game difficulty
		gameMode = (GameMode) PlayerPrefs.GetInt(GameSettings.PP_GAME_MODE); //get the selected game mode
		currentScore = 0;
		inited = true;
		finished = false;
		started = false;
		Paused = pauseTimeAtStart;
		
		currentLevelSelected = BaseLevelLoaderController.Instance.LoadTestLevel ? BaseLevelLoaderController.Instance.LevelToLoadTEST //get a test level
			: PlayerPrefs.GetInt(GameSettings.PP_SELECTED_LEVEL); //get the current level selected
		
		if(gameMode == GameMode.CAMPAIGN){
			BaseQuestManager.Instance.init(currentLevelSelected);
		}
	}
	/// <summary>
	/// Do start game functions
	/// </summary>
	public virtual void startGame(){
		started = true;
		Paused = false;
	}
	
	/// <summary>
	/// Do finish game functions
	/// </summary>
	public virtual void finishGame(){
		if(!finished){
			dispatcher.dispatch(GAME_FINISHED);
			finished = true;
			
			if(handleScores)
				manageScores();
			
			StartCoroutine(finishGameWithDelay());
		}
	}
	
	public virtual void manageScores(){
		//send score to the server
		if(sendScoresToServer){
			ScoresHandler.Instance.sendScoreToServer(getRankingID(), currentScore);
		}
		//save only locally
		else{
			ScoresHandler.Instance.saveScoreOnlyLocally(getRankingID(), currentScore);
		}
	}
	
	public virtual string getRankingID(){
		//implement in child
		throw new NotImplementedException();
	}
	
	public virtual void PlayerLostLife (){
		// deal with player life lost (update U.I. etc.)
	}
	
	public virtual void SpawnPlayer ()
	{
		// the player needs to be spawned
	}
	
	public virtual void Respawn ()
	{
		// the player is respawning
	}
	
	
	
	public virtual void Explode ( Vector3 aPosition )
	{		
		// instantiate an explosion at the position passed into this function
		Instantiate( explosionPrefab,aPosition, Quaternion.identity );
	}
	
	public virtual void EnemyDestroyed( Vector3 aPosition, int pointsValue, int hitByID )
	{
		// deal with a enemy destroyed
	}
	
	public virtual void BossDestroyed()
	{
		// deal with the end of a boss battle
	}
	
	public virtual void RestartGameButtonPressed()
	{
		// deal with restart button (default behaviour re-loads the currently loaded scene)
		Application.LoadLevel(Application.loadedLevelName);
	}
	
	public bool Paused
	{
		get 
		{ 
			// get paused
			return paused; 
		}
		set
		{
			
			// set paused 
			paused = value;
			
			if (paused)
			{
				if(started)
					UIController.Instance.Manager.open(UIBaseWindowIDs.PAUSE);
				
				// pause time
				Time.timeScale= 0f;
			} else {
				if(started)
					UIController.Instance.Manager.close(UIBaseWindowIDs.PAUSE);
				
				// unpause Unity
				Time.timeScale = 1f;
			}
		}
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	public virtual void OnLevelLoaded(CEvent e){
		
	}
	
	public virtual void OnQuestsCompleted(CEvent e){
		int lastUnlockedLevel = PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED);
		
		//unlock the next level
		if(currentLevelSelected == lastUnlockedLevel)
			PlayerPrefs.SetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED, currentLevelSelected+1);
		
		finishGame();
	}
}
