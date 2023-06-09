﻿/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
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
	private string 		levelPackId;
	
	private int 		initialPlayerLife;
	private int 		initialGameLifes;
	
	private int 		playerLife;
	private int 		gameLifes;
	
	/// <summary>
	/// The description location key.
	/// </summary>
	private string		descriptionLocKey;
	private string		localizedDescripcion;
	private int			moneyReward;
	private int			gemsReward;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string Id {
		get {
			return this.id;
		}
	}
	public string LevelPackId {
		get {
			return this.levelPackId;
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
	
	public int MoneyReward {
		get {
			return this.moneyReward;
		}
	}
	
	public int RealMoneyReward{
		get{
			int res = moneyReward;
			int prevCompleted = PlayerPrefs.GetInt(GameSettings.PP_LEVEL_COMPLETED_TIMES+id); //get the previous completed times for this level
			int totalGames = prevCompleted; //total games for this level
			int lastUnlockedLevel = PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED+levelPackId);
			int idLevel;
			
			if(int.TryParse(Id, out idLevel) && prevCompleted > 0){
				res = (int) (moneyReward*GameSettings.Instance.PERCENTAGE_MONEY_REWARD_LEVEL_PREV_COMPLETED);
			}
			
			return res;
		}
	}
	
	public int GemsReward {
		get {
			return this.gemsReward;
		}
	}
	
	public int RealGemsReward{
		get{
			int res = gemsReward;
			//			int prevTries = PlayerPrefs.GetInt(GameSettings.PP_LEVEL_TRIES_TIMES+id.ToString()); //get the previous tries for this level
			int prevCompleted = PlayerPrefs.GetInt(GameSettings.PP_LEVEL_COMPLETED_TIMES+id); //get the previous completed times for this level
			int totalGames = prevCompleted; //total games for this level
			int lastUnlockedLevel = PlayerPrefs.GetInt(GameSettings.PP_LAST_LEVEL_UNLOCKED+levelPackId);
			int idLevel;
			
			if(int.TryParse(Id, out idLevel) && prevCompleted > 0){
				res = (int) (gemsReward*GameSettings.Instance.PERCENTAGE_GEMS_REWARD_LEVEL_PREV_COMPLETED);
			}
			
			return res;
		}
	}
	
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseLevel"/> class.
	/// 
	/// Attributes:
	/// ID, Localization Key, Player life, Game Lifes, Money Reward, Gems Reward
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public BaseLevel(string attributes){
		string[] att = attributes.Split(SEPARATOR_ATTRIBUTES);
		
		//id
		if(att.Length > 0){
			id = att[0];
		}
		
		//level pack id
		if(att.Length > 1){
			levelPackId = att[1];
		}
		
		//localization key
		if(att.Length > 2){
			descriptionLocKey = att[2];
			localizedDescripcion = Localization.Get(descriptionLocKey);
		}
		
		//player life
		int life;
		if(att.Length > 3 && int.TryParse(att[3], out life)){
			initialPlayerLife = life;
		}
		else{
			initialPlayerLife = INITIAL_PLAYER_LIFE;
		}
		
		//game lifes
		int gLifes;
		if(att.Length > 4 && int.TryParse(att[4], out gLifes)){
			initialGameLifes = gLifes;
		}
		else{
			initialGameLifes = INITIAL_GAME_LIFES;
		}
		
		//money & gems reward
		int pMoney, pGems;
		if(att.Length > 5 && int.TryParse(att[5], out pMoney))
			moneyReward = pMoney;
		if(att.Length > 6 && int.TryParse(att[6], out pGems))
			gemsReward = pGems;
		
		//reset to set all attributes to initial values
		reset();
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString (){
		return string.Format ("[BaseLevel: Id={0}, LevelPackId={1}, Description={2}, InitialPlayerLife={3}, InitialGameLifes={4}, PlayerLife={5}, GameLifes={6}, MoneyReward={7}, GemsReward={8}]", Id, LevelPackId, localizedDescripcion, InitialPlayerLife, InitialGameLifes, PlayerLife, GameLifes, RealMoneyReward, GemsReward);
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void reset(){
		playerLife = initialPlayerLife;
		gameLifes = initialGameLifes;
	}
	
	
}
