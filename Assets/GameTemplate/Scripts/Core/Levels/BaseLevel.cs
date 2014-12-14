using UnityEngine;
using System.Collections;

[System.Serializable]
public class BaseLevel {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const char 	SEPARATOR_ATTRIBUTES 		= ',';
	public const int 	INITIAL_PLAYER_LIFE 		= 1000;
	public const int 	INITIAL_GAME_LIFES			= 1;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private string id;

	private int initialPlayerLife;
	private int initialGameLifes;

	private int playerLife;
	private int gameLifes;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string Id {
		get {
			return this.id;
		}
	}
	public int InitialPlayerLife {
		get {
			return this.initialPlayerLife;
		}
	}
	public int InitialGameLifes {
		get {
			return this.initialGameLifes;
		}
	}
	public int PlayerLife {
		get {
			return this.playerLife;
		}
		set {
			playerLife = value;
		}
	}
	public int GameLifes {
		get {
			return this.gameLifes;
		}
		set {
			gameLifes = value;
		}
	}
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseLevel"/> class.
	/// 
	/// Attributes:
	/// ID, Player life, Game Lifes
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public BaseLevel(string attributes){
		string[] att = attributes.Split(SEPARATOR_ATTRIBUTES);

		//id
		if(att.Length > 0){
			id = att[0];
		}

		//player life
		int life;
		if(att.Length > 1 && int.TryParse(att[1], out life)){
			initialPlayerLife = life;
		}
		else{
			initialPlayerLife = INITIAL_PLAYER_LIFE;
		}

		//game lifes
		int gLifes;
		if(att.Length > 2 && int.TryParse(att[2], out gLifes)){
			initialGameLifes = gLifes;
		}
		else{
			initialGameLifes = INITIAL_GAME_LIFES;
		}

		//reset to set all attributes to initial values
		reset();
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString (){
		return string.Format ("[BaseLevel: Id={0}, InitialPlayerLife={1}, InitialGameLifes={2}, PlayerLife={3}, GameLifes={4}]", Id, InitialPlayerLife, InitialGameLifes, PlayerLife, GameLifes);
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void reset(){
		playerLife = initialPlayerLife;
		gameLifes = initialGameLifes;
	}
}
