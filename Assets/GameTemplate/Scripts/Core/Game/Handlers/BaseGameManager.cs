/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using UnionAssets.FLE;
using System;

public class BaseGameManager : MonoBehaviour {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string GAME_FINISHED = "gt_game_finished";
	public const string LAUNCHING_AD_DURING_GAMEPLAY_IN_X_SECS = "gt_launching_ad_during_gameplay_in_x_secs";
	
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	private static EventDispatcherBase _dispatcher  = new EventDispatcherBase ();
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string 						playerTag = "Player";
	
	[SerializeField]
	private bool						useGameMultiversionForSelectingPlayer = false;
	
	[SerializeField]
	private bool 						startGameAtTheStartMoment = false;
	
	[SerializeField]
	private bool 						pauseTimeAtStart = false;
	
	[SerializeField]
	private bool 						pauseTimeWhenFinishedGame = true;
	
	[SerializeField]
	private bool 						sendScoresToServer = true;
	
	[SerializeField]
	private bool 						useGameControlButtonsWin = false;
	
	[SerializeField]
	private float 						gameOverDelay = 1.5f;
	
	[SerializeField]
	private float 						missionCompletedDelay = 2.5f;
	
	public GameObject 					explosionPrefab;
	
	public bool 						showAdWhenPlayerForcesFinishGame = true;
	
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private GameObject 		player;
	private bool 			paused;
	private bool 			started;
	private bool 			inited;
	private bool 			finished;
	private GameDifficulty 	difficulty;
	private GameMode 		gameMode;
	private long 			currentScore;
	private int				currentLevelSelected;
	private bool			isGameOver;
	private bool 			isLocalMultiplayerGame;
	private bool 			isOnlineMultiplayerGame;
	
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
	
	public GameObject Player {
		get {
			return this.player;
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
	
	public long CurrentScore {
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
	
	public bool IsLocalMultiplayerGame {
		get {
			return this.isLocalMultiplayerGame;
		}
	}
	
	public bool IsOnlineMultiplayerGame {
		get {
			return this.isOnlineMultiplayerGame;
		}
	}
	
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	protected virtual void Awake(){
		initGame();
		
		//Game Analytics event name
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
		
		//multiplayer options
		isLocalMultiplayerGame = PlayerPrefs.GetInt(GameSettings.PP_LOCAL_MULTIPLAYER) != 0 ? true:false;
		isOnlineMultiplayerGame = PlayerPrefs.GetInt(GameSettings.PP_ONLINE_MULTIPLAYER) != 0 ? true:false;
		
		//GA
		//TODO Analytics
	}
	protected virtual void Start(){
		if(startGameAtTheStartMoment)
			startGame();
	}
	protected virtual void Update(){
		
	}
	
	protected virtual void LateUpdate(){
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
	/// <summary>
	/// Checks the ad showing during game play.
	/// 
	/// We use WaitForSeconds because we want to pause this process when game is paused
	/// </summary>
	/// <returns>The ad showing during game play.</returns>
	float timerForCheckAdShowingDuringGameplay = 0;
	private IEnumerator checkAdShowingDuringGamePlay(){
		yield return null;
		bool canShowAd = canShowAdDuringGamePlay();
		
		if(canShowAd){
			int secsToNotify = GameSettings.Instance.SECONDS_DURING_GAME_PLAYING_SHOW_AD - GameSettings.Instance.NOTIFY_AD_DURING_GAMEPLAY_WILL_BE_SHOWN_IN_NEXT_SECONDS;
			
			//wait time for notify ad will be shown in the next seconds
			while(canShowAd && timerForCheckAdShowingDuringGameplay < secsToNotify){
				canShowAd = canShowAdDuringGamePlay(); 
				timerForCheckAdShowingDuringGameplay += Time.deltaTime;
				yield return null;
			}
			
			resetTimerForAdShowingDuringGameplay();
			
			if(canShowAd){
				//launch event to notify an ad will be shown in the next seconds
				launchEventToNotifyAdShowingDurginGamePlay();
			}
			
			//wait the next seconds to show the ad
			while(canShowAd && timerForCheckAdShowingDuringGameplay < GameSettings.Instance.NOTIFY_AD_DURING_GAMEPLAY_WILL_BE_SHOWN_IN_NEXT_SECONDS){
				canShowAd = canShowAdDuringGamePlay(); 
				timerForCheckAdShowingDuringGameplay += Time.deltaTime;
				yield return null;
			}
			
			//finally show the ad
			if(canShowAd){			
				showAdDuringGamePlay();
			}
			
			//reset the timer
			resetTimerForAdShowingDuringGameplay();
		}
		
		//repeat again
		if(!isGameOver && !finished)
			StartCoroutine(checkAdShowingDuringGamePlay());
	}
	
	public virtual void resetTimerForAdShowingDuringGameplay(){
		timerForCheckAdShowingDuringGameplay = 0;
	}
	
	public virtual bool canShowAdDuringGamePlay(){
		return AdsHandler.Instance.canShowAdRandom();
	}
	
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
			AdsHandler.Instance.showInterstitial();
			
			//GA
			//TODO Analytics GAEvents.INTERSTITIAL_AD_SHOWN_AT_GO
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
				//TODO Analytics	GAEvents.CAMPAIGN_LEVEL_GAMEOVERS +":"+ currentLevelSelected.ToString(), prevTries)
			}
			else{
				int prevCompleted = PlayerPrefs.GetInt(GameSettings.PP_LEVEL_COMPLETED_TIMES+currentLevelSelected.ToString()); //get the previous completed times
				prevCompleted++;
				//				((UIBaseMIssionCompletedWin) UIController.Instance.Manager.getWindow(UIBaseWindowIDs.MISSION_COMPLETED)).init(BaseLevelLoaderController.Instance.CurrentLevel);
				
				//window mission completed is inited in event OnQuestCompleted!!
				UIController.Instance.Manager.open(UIBaseWindowIDs.MISSION_COMPLETED); //first show window
				PlayerPrefs.SetInt(GameSettings.PP_LEVEL_COMPLETED_TIMES+currentLevelSelected.ToString(), prevCompleted); //update completed times
				
				//GA
				//TODO Analytics GAEvents.CAMPAIGN_LEVEL_GAMEOVERS +":"+ currentLevelSelected.ToString(), prevCompleted
			}
			break;
			
		case GameMode.SURVIVAL:
			gaEvent = GAEvents.SURVIVAL_LEVEL_PLAYED;
			UIController.Instance.Manager.open(UIBaseWindowIDs.GAMEOVER);
			break;
		case GameMode.QUICKGAME:
			gaEvent = GAEvents.QUICKGAME_LEVEL_PLAYED;
			UIController.Instance.Manager.open(UIBaseWindowIDs.GAMEOVER);
			break;
			
		}
		
		//GA
		//TODO Analytics		gaEvent
		
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
		//find the player gameobject
		GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);
		if(players != null && players.Length > 0 && players.Length > 0){
			if(useGameMultiversionForSelectingPlayer){
				//get the selected player gameobject by selected player order criteria
				for(int i=0;i<players.Length; i++){
					SelectedPlayerOrder so = players[i].GetComponent<SelectedPlayerOrder>();
					
					if(so != null && so.Order.Equals(GameSettings.Instance.currentGameMultiversion)){
						player = so.gameObject;
						break;
					}
				}
				
				if(player == null)
					GTDebug.logErrorAlways("Selected player index from current GameMultiversion index [" + GameSettings.Instance.currentGameMultiversion + "] is not presented in players array");
			}
			else{
				int selected = PlayerPrefs.GetInt(GameSettings.PP_SELECTED_CHARACTER);
				
				if(players.Length > selected){
					player = players[selected];
				}
				else{
					GTDebug.logErrorAlways("Selected player index [" + selected + "] is out of range ["+players.Length+"]");
				}
			}
		}
		
		
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
	
	public virtual void showAdDuringGamePlay(){
		GTDebug.log("Showing Ad during Gameplay");
		switch(GameSettings.Instance.adTypeDuringGamePlay){
		case AdType.VIDEO: AdsHandler.Instance.PlayAVideo(); break;
		case AdType.RANDOM_INTERSTITIAL_VIDEO: AdsHandler.Instance.showRandomGameplayInterstitialOrVideoAd(); break;
		default: AdsHandler.Instance.showInterstitial(); break; 
		}
	}
	
	public void playerForcesFinishGame(){
		if(showAdWhenPlayerForcesFinishGame){
			AdsHandler.Instance.showInterstitial();
			
			//GA
			//TODO Analytics GAEvents.INTERSTITIAL_AD_SHOWN_AT_GO
		}
	}
	
	/// <summary>
	/// Do start game functions
	/// </summary>
	public virtual void startGame(){
		started = true;
		Paused = false;
		
		//checks if initing coroutine to check the ad showing during the game play
		if(!GameSettings.Instance.IS_PRO_VERSION && GameSettings.Instance.SECONDS_DURING_GAME_PLAYING_SHOW_AD > 0)
			StartCoroutine(checkAdShowingDuringGamePlay());
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
			ScoresHandler.Instance.sendScoreToServerByID(getRankingID(), currentScore);
		}
		//save only locally
		else{
			ScoresHandler.Instance.saveScoreOnlyLocallyByID(getRankingID(), currentScore);
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
		ScreenLoaderVisualIndicator.Instance.LoadScene(Application.loadedLevelName);
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
			//set Timemanager fixed timestep
			BaseGameScreenController.Instance.resetFixedTimeStepOfTimeManager();	
			
			// set paused 
			paused = value;
			
			if (paused)
			{
				//release touch down of game control buttons when pause a started game
				if(started && useGameControlButtonsWin){
					UIBaseGameControlButtonsWindow win = (UIBaseGameControlButtonsWindow)UIController.Instance.Manager.getWindow(UIBaseWindowIDs.GAME_CONTROL_BUTTONS);
					if(win != null)	win.releaseTouchDown();
				}
				
				if(!AdsHandler.Instance.HasPausedGame && started)
					UIController.Instance.Manager.open(UIBaseWindowIDs.PAUSE);
				
				//				//mute fx
				//				BaseSoundManager.Instance.muteOrActiveOncesMuteOncesActive(SoundType.FX, true, true);
				
				// pause time
				Time.timeScale= 0f;
			} else {
				if(!AdsHandler.Instance.HasPausedGame && started)
					UIController.Instance.Manager.close(UIBaseWindowIDs.PAUSE);
				
				//				//unmute fx
				//				BaseSoundManager.Instance.muteOrActiveOncesMuteOncesActive(SoundType.FX, true, true);
				
				// unpause Unity
				Time.timeScale = 1f;
			}
		}
	}
	
	public virtual void launchEventToNotifyAdShowingDurginGamePlay(bool notInterrupt = false){
		//launch event to notify an ad will be shown in the next seconds
		dispatcher.dispatch(LAUNCHING_AD_DURING_GAMEPLAY_IN_X_SECS, notInterrupt);
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
			w.initBestTime(BaseLevelLoaderController.Instance.CurrentLevel);
			
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
			//TODO Analytics GAEvents.CAMPAIGN_LEVEL_UNLOCKED +":"+ nextLevel.ToString()
		}
		
		finishGame();
	}
}
