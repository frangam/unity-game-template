using UnityEngine;
using System.Collections;
using UnionAssets.FLE;
using System;
using GameAnalyticsSDK;

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
		
		string gaEvent = GAEvents.CAMPAIGN_LEVEL_OPENED;
		switch(gameMode){
		case GameMode.QUICKGAME:
			gaEvent = GAEvents.QUICKGAME_LEVEL_OPENED;
			break;
		case GameMode.CAMPAIGN:
			gaEvent = GAEvents.CAMPAIGN_LEVEL_OPENED;
			BaseLevelLoaderController.dispatcher.addEventListener(BaseLevelLoaderController.LEVEL_LOADED, OnLevelLoaded);
			BaseQuestManager.dispatcher.addEventListener(BaseQuestManager.ALL_QUESTS_COMPLETED, OnQuestsCompleted);
			BaseQuestManager.Instance.resetProperties();
			break;
			
		case GameMode.SURVIVAL:
			gaEvent = GAEvents.SURVIVAL_LEVEL_OPENED;
			break;
			
		}
		
		//GA
		GameAnalytics.NewDesignEvent(gaEvent);
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
		int numGameovers = 0, numWins = 0;
		int numGameoversToChek = GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_BY_DEFAULT, numWinsToCheck = GameSettings.Instance.NUM_WINS_SHOW_AD_BY_DEFAULT;
		
		//handle total
		
		//update total of gameovers
		if(isGameOver){
			numGameovers = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_GAMEOVERS);
			numGameovers++;
			PlayerPrefs.SetInt(GameSettings.PP_TOTAL_GAMEOVERS, numGameovers);
		}
		//update total of wins
		else{
			numWins = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_WINS);
			numWins++;
			PlayerPrefs.SetInt(GameSettings.PP_TOTAL_WINS, numWins);
		}
		
		
		//hanlde by each game mode
		switch(gameMode){
		case GameMode.CAMPAIGN:
			//handle campaign gameovers
			if(isGameOver){
				//total of campaign
				numGameovers = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_CAMPAIGN_GAMEOVERS);
				numGameovers++;
				PlayerPrefs.SetInt(GameSettings.PP_TOTAL_CAMPAIGN_GAMEOVERS, numGameovers);
				
				//specific of this level
				int go = PlayerPrefs.GetInt(GameSettings.PP_NUM_GAMEOVERS_IN_LEVEL+currentLevelSelected.ToString());
				go++;
				PlayerPrefs.SetInt(GameSettings.PP_NUM_GAMEOVERS_IN_LEVEL+currentLevelSelected.ToString(), go);
			}
			//handle campaign wins
			else{
				//total wins
				numWins = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_CAMPAIGN_WINS);
				numWins++;
				PlayerPrefs.SetInt(GameSettings.PP_TOTAL_CAMPAIGN_WINS, numWins);
				
				//specific wins
				int w = PlayerPrefs.GetInt(GameSettings.PP_NUM_WINS_IN_LEVEL+currentLevelSelected.ToString());
				w++;
				PlayerPrefs.SetInt(GameSettings.PP_NUM_WINS_IN_LEVEL+currentLevelSelected.ToString(), w);
			}
			
			
			break;
		case GameMode.QUICKGAME:
			//update total of quickgame gameovers
			if(isGameOver){
				//total of campaign
				numGameovers = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_QUICKGAME_GAMEOVERS);
				numGameovers++;
				PlayerPrefs.SetInt(GameSettings.PP_TOTAL_QUICKGAME_GAMEOVERS, numGameovers);
				
				//specifi by difficulty
				if(difficulty != GameDifficulty.NONE){
					int dif = ((int) difficulty);
					int go  = PlayerPrefs.GetInt(GameSettings.PP_NUM_GAMEOVERS_WITH_DIFFICULTY+dif.ToString());
					go++;
					PlayerPrefs.SetInt(GameSettings.PP_NUM_GAMEOVERS_WITH_DIFFICULTY+dif.ToString(), go);
					
					switch(difficulty){
					case GameDifficulty.EASY: numGameoversToChek = GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_EASY_MODE; break;
					case GameDifficulty.NORMAL: numGameoversToChek = GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_NORMAL_MODE; break;
					case GameDifficulty.HARD: numGameoversToChek = GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_HARD_MODE; break;
					}
				}
				else if(difficulty == GameDifficulty.NONE){
					int go = PlayerPrefs.GetInt(GameSettings.PP_NUM_GAMEOVERS_WITHOUT_DIFFICULTY);
					go++;
					PlayerPrefs.SetInt(GameSettings.PP_NUM_GAMEOVERS_WITHOUT_DIFFICULTY, go);
				}
			}
			//handle quickgame wins
			else{
				//total wins
				numWins = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_QUICKGAME_WINS);
				numWins++;
				PlayerPrefs.SetInt(GameSettings.PP_TOTAL_QUICKGAME_WINS, numWins);
				
				//specific wins by difficulty
				if(difficulty != GameDifficulty.NONE){
					int dif = ((int) difficulty);
					int w  = PlayerPrefs.GetInt(GameSettings.PP_NUM_WINS_WITH_DIFFICULTY+dif.ToString());
					w++;
					PlayerPrefs.SetInt(GameSettings.PP_NUM_WINS_WITH_DIFFICULTY+dif.ToString(), w);
					
					switch(difficulty){
					case GameDifficulty.EASY: numWinsToCheck = GameSettings.Instance.NUM_WINS_SHOW_AD_EASY_MODE; break;
					case GameDifficulty.NORMAL: numWinsToCheck = GameSettings.Instance.NUM_WINS_SHOW_AD_NORMAL_MODE; break;
					case GameDifficulty.HARD: numWinsToCheck = GameSettings.Instance.NUM_WINS_SHOW_AD_HARD_MODE; break;
					}
				}
				else if(difficulty == GameDifficulty.NONE){
					int w = PlayerPrefs.GetInt(GameSettings.PP_NUM_WINS_WITHOUT_DIFFICULTY);
					w++;
					PlayerPrefs.SetInt(GameSettings.PP_NUM_WINS_WITHOUT_DIFFICULTY, w);
				}
			}
			
			
			break;
		case GameMode.SURVIVAL:
			//update total of survival gameovers
			if(isGameOver){
				//total go of SURVIVAL
				numGameovers = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_SURVIVAL_GAMEOVERS);
				numGameovers++;
				PlayerPrefs.SetInt(GameSettings.PP_TOTAL_SURVIVAL_GAMEOVERS, numGameovers);
				
				//specific go of survival
				int go = PlayerPrefs.GetInt(GameSettings.PP_NUM_GAMEOVERS_IN_SURVIVAL_LEVEL+currentLevelSelected.ToString());
				go++;
				PlayerPrefs.SetInt(GameSettings.PP_NUM_GAMEOVERS_IN_SURVIVAL_LEVEL+currentLevelSelected.ToString(), go);
				
				//num go to check to show ads
				numGameoversToChek = GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_SURVIVAL_MODE;
			}
			//handle survival wins
			else{
				//total wins
				numWins = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_SURVIVAL_WINS);
				numWins++;
				PlayerPrefs.SetInt(GameSettings.PP_TOTAL_SURVIVAL_WINS, numWins);
				
				//specific wins
				int w = PlayerPrefs.GetInt(GameSettings.PP_NUM_WINS_IN_SURVIVAL_LEVEL+currentLevelSelected.ToString());
				w++;
				PlayerPrefs.SetInt(GameSettings.PP_NUM_WINS_IN_SURVIVAL_LEVEL+currentLevelSelected.ToString(), w);
				
				//num WINS to check to show ads
				numGameoversToChek = GameSettings.Instance.NUM_WINS_SHOW_AD_SURVIVAL_MODE;
			}
			
			
			break;
		}
		
		//refresh banner ad
		AdsHandler.Instance.refrescarBanner();
		
		//show interstitial ad
		if((isGameOver && numGameovers % numGameoversToChek == 0) || (!isGameOver && numWins % numWinsToCheck == 0)){
			AdsHandler.Instance.mostrarPantallazo();
			
			//GA
			GameAnalytics.NewDesignEvent(GAEvents.INTERSTITIAL_AD_SHOWN_AT_GO);
		}
	}
	
	private IEnumerator finishGameWithDelay(){
		float delay = isGameOver ? gameOverDelay : missionCompletedDelay;
		
		yield return new WaitForSeconds(delay);
		
		PlayerPrefs.SetInt(GameSettings.PP_LAST_LEVEL_PLAYED, currentLevelSelected); //for analytics
		
		string gaEvent = GAEvents.CAMPAIGN_LEVEL_PLAYED; //analytics
		
		//show gameover windows
		switch(gameMode){
		case GameMode.CAMPAIGN:
			if(isGameOver){
				int prevTries = PlayerPrefs.GetInt(GameSettings.PP_LEVEL_TRIES_TIMES+currentLevelSelected.ToString()); //get the previous tries
				prevTries++;
				PlayerPrefs.SetInt(GameSettings.PP_LEVEL_TRIES_TIMES+currentLevelSelected.ToString(), prevTries); //update tries
				UIController.Instance.Manager.open(UIBaseWindowIDs.MISSION_FAILED);
				
				//GA
				GameAnalytics.NewDesignEvent(GAEvents.CAMPAIGN_LEVEL_GAMEOVERS +":"+ currentLevelSelected.ToString(), prevTries);
			}
			else{
				int prevCompleted = PlayerPrefs.GetInt(GameSettings.PP_LEVEL_COMPLETED_TIMES+currentLevelSelected.ToString()); //get the previous completed times
				prevCompleted++;
				//				((UIBaseMIssionCompletedWin) UIController.Instance.Manager.getWindow(UIBaseWindowIDs.MISSION_COMPLETED)).init(BaseLevelLoaderController.Instance.CurrentLevel);
				
				//window mission completed is inited in event OnQuestCompleted!!
				UIController.Instance.Manager.open(UIBaseWindowIDs.MISSION_COMPLETED); //first show window
				PlayerPrefs.SetInt(GameSettings.PP_LEVEL_COMPLETED_TIMES+currentLevelSelected.ToString(), prevCompleted); //update completed times
				
				//GA
				GameAnalytics.NewDesignEvent(GAEvents.CAMPAIGN_LEVEL_GAMEOVERS +":"+ currentLevelSelected.ToString(), prevCompleted);
			}
			break;
			
		case GameMode.SURVIVAL:
			gaEvent = GAEvents.SURVIVAL_LEVEL_PLAYED;
			UIController.Instance.Manager.open(gameoverWindow);
			break;
		case GameMode.QUICKGAME:
			gaEvent = GAEvents.QUICKGAME_LEVEL_PLAYED;
			UIController.Instance.Manager.open(gameoverWindow);
			break;
			
		}
		
		//GA
		GameAnalytics.NewDesignEvent(gaEvent);
		
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
			: lastSelectedLevel(); //get the current level selected
		
		if(gameMode == GameMode.CAMPAIGN){
			BaseQuestManager.Instance.init(currentLevelSelected);
		}
	}
	
	public int lastSelectedLevel(){
		int last = gameMode == GameMode.SURVIVAL ? PlayerPrefs.GetInt(GameSettings.PP_SELECTED_SURVIVAL_LEVEL) : PlayerPrefs.GetInt(GameSettings.PP_SELECTED_LEVEL); 
		
		return last;
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
	
	public virtual bool Paused
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
	
	public virtual void OnLevelCompleted(int levelCompleted){
		
	}
	
	public virtual void OnQuestsCompleted(CEvent e){
		int lastUnlockedLevel = PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED);
		
		//first load info of completed mission window
		if(gameMode == GameMode.CAMPAIGN){
			UIBaseMIssionCompletedWin w = (UIBaseMIssionCompletedWin) UIController.Instance.Manager.getWindow(UIBaseWindowIDs.MISSION_COMPLETED);
			w.init(BaseLevelLoaderController.Instance.CurrentLevel);
			
			if(w.LoadInfoWhenQuesCompleted)
				w.showInfo();
		}
		
		
		//save completed level
		int lastCompletedLevel = PlayerPrefs.GetInt(GameSettings.PP_LAST_CAMPAIGN_LEVEL_COMPLETED);
		if(currentLevelSelected > lastCompletedLevel){
			PlayerPrefs.SetInt(GameSettings.PP_LAST_CAMPAIGN_LEVEL_COMPLETED, currentLevelSelected);
			
			OnLevelCompleted(currentLevelSelected);
		}
		
		//unlock the next level
		if(currentLevelSelected == lastUnlockedLevel && BaseLevelLoaderController.Instance.Levels != null && currentLevelSelected < BaseLevelLoaderController.Instance.Levels.Count){
			int nextLevel = currentLevelSelected+1;
			PlayerPrefs.SetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED, nextLevel);
			
			//GA
			GameAnalytics.NewDesignEvent(GAEvents.CAMPAIGN_LEVEL_UNLOCKED +":"+ nextLevel.ToString());
		}
		
		finishGame();
	}
}
