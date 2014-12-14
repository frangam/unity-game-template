using UnityEngine;
using System.Collections;

[AddComponentMenu("Base/GameController")]

public class BaseGameController: BaseGameController<BaseGameController>{}

public class BaseGameController<T> : Singleton<T> where T: MonoBehaviour{
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool sendScoresToServer = true;

	[SerializeField]
	private string gameoverWindow = UIBaseWindowIDs.GAMEOVER;


	public GameObject explosionPrefab;


	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool paused;
	private bool started;
	private bool inited;
	private bool finished;
	private GameDifficulty difficulty;
	private int currentScore;


	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public GameDifficulty Difficulty {
		get {
			return this.difficulty;
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

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		initGame();
	}
	#endregion

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void initGame(){
		difficulty = (GameDifficulty) PlayerPrefs.GetInt(GameSettings.PP_GAME_DIFFICULTY); //get the selected game difficulty
		currentScore = 0;
		inited = true;
		paused = false;
		finished = false;
		started = false;
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

		//show gameover windows
		UIBaseController.Instance.open(gameoverWindow);

		finished = true;
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
	


	public void Explode ( Vector3 aPosition )
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
                // pause time
                Time.timeScale= 0f;
			} else {
                // unpause Unity
				Time.timeScale = 1f;
            }
        }
    }
	
}
