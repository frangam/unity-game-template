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
	private string 		id;
	
	private int 		initialPlayerLife;
	private int 		initialGameLifes;
	
	private int 		playerLife;
	private int 		gameLifes;
	
	/// <summary>
	/// The description location key.
	/// </summary>
	private string		descriptionLocKey;
	private string		localizedDescripcion;
	
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
	
	public string DescriptionLocKey {
		get {
			return this.descriptionLocKey;
		}
		set {
			descriptionLocKey = value;
		}
	}
	
	/// <summary>
	/// Gets the localized description.
	/// </summary>
	/// <value>The localized description.</value>
	public string LocalizedDescription{
		get{return this.localizedDescripcion;}
	}
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseLevel"/> class.
	/// 
	/// Attributes:
	/// ID, Localization Key, Player life, Game Lifes
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public BaseLevel(string attributes){
		string[] att = attributes.Split(SEPARATOR_ATTRIBUTES);
		
		//id
		if(att.Length > 0){
			id = att[0];
		}
		
		//localization key
		if(att.Length > 1){
			descriptionLocKey = att[1];
			localizedDescripcion = Localization.Get(descriptionLocKey);
		}
		
		//player life
		int life;
		if(att.Length > 2 && int.TryParse(att[2], out life)){
			initialPlayerLife = life;
		}
		else{
			initialPlayerLife = INITIAL_PLAYER_LIFE;
		}
		
		//game lifes
		int gLifes;
		if(att.Length > 3 && int.TryParse(att[3], out gLifes)){
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
		return string.Format ("[BaseLevel: Id={0}, Description={1}, InitialPlayerLife={2}, InitialGameLifes={3}, PlayerLife={4}, GameLifes={5}]", Id, localizedDescripcion, InitialPlayerLife, InitialGameLifes, PlayerLife, GameLifes);
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void reset(){
		playerLife = initialPlayerLife;
		gameLifes = initialGameLifes;
	}
}
