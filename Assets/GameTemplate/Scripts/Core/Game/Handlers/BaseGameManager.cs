using UnityEngine;
using System.Collections;
using UnionAssets.FLE;

public class BaseGameManager : MonoBehaviour {
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
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
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
		
		Time.timeScale = pauseTimeAtStart ? 0f : 1f;
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
	private IEnumerator finishGameWithDelay(){
		float delay = isGameOver ? gameOverDelay : missionCompletedDelay;
		
		yield return new WaitForSeconds(delay);
		
		
		//show gameover windows
		switch(gameMode){
		case GameMode.CAMPAIGN:
			if(isGameOver)
				UIController.Instance.Manager.open(UIBaseWindowIDs.MISSION_FAILED);
			else
				UIController.Instance.Manager.open(UIBaseWindowIDs.MISSION_COMPLETED);
			break;
			
		case GameMode.SURVIVAL:
		case GameMode.QUICKGAME:
			UIController.Instance.Manager.open(gameoverWindow);
			break;
			
		}
		
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
		Paused = false;
		finished = false;
		started = false;
		
		if(gameMode == GameMode.CAMPAIGN){
			currentLevelSelected = BaseLevelLoaderController.Instance.LoadTestLevel ? BaseLevelLoaderController.Instance.LevelToLoadTEST //get a test level
				: PlayerPrefs.GetInt(GameSettings.PP_SELECTED_LEVEL); //get the current level selected
			BaseQuestManager.Instance.init(currentLevelSelected);
		}
	}
	/// <summary>
	/// Do start game functions
	/// </summary>
	public virtual void startGame(){
		started = true;
	}
	
	/// <summary>
	/// Do finish game functions
	/// </summary>
	public virtual void finishGame(){
		//save current score
		PlayerPrefs.SetInt(GameSettings.PP_LAST_SCORE, currentScore);
		
		//send score
		if(sendScoresToServer){
			ScoresHandler.Instance.sendScoreToServer(currentScore);
		}
		else{
			ScoresHandler.Instance.saveScoreOnlyLocally(currentScore);
		}
		finished = true;
		
		StartCoroutine(finishGameWithDelay());
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
				UIController.Instance.Manager.open(UIBaseWindowIDs.PAUSE);
				// pause time
				Time.timeScale= 0f;
			} else {
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
