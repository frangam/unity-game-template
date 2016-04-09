/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;


/*
  GAEventCategories is utility class represents event categories to classify
  events better.

  If we have an specific game that needs some more event categories, 
  we can inherits from this class.
*/
public class GAEventCategories {
	//--------------------------------------
	// Constants
	//--------------------------------------
	#region Constants
	public const string SCENE 								= "Scene";
	public const string GAME 								= "Game";
	public const string SOUND 								= "Sound";
	public const string MUSIC 								= "Music";
	public const string BILLS_CURRENCY 						= "Bills";
	public const string GEMS_CURRENCY 						= "Gems";
	public const string	WINDOW 								= "Window";


	public const string CAMPAIGN_LEVEL 						= "Campaign Level";
	public const string SURVIVAL_LEVEL 						= "Survival Level";
	public const string QUICKGAME_LEVEL 					= "Quick Game Level";
		   
	public const string BANNER_AD 							= "Banner Ad";
	public const string INTERSTITIAL_AD 					= "Interstitial Ad";
	public const string RANDOM_AD 							= "Random Ad";
	public const string VIDEO_AD 							= "Video Ad";
	public const string VIDEO_REWARD_AD 					= "Video Reward Ad";
		   
	public const string INAPP 								= "InApp";
	
	#endregion
}
