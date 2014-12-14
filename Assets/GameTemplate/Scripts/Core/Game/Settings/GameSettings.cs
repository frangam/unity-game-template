using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	public static bool 	purchasedForQuitAds 			= false; //if player has purchased for quit all of the ads
	public static int 	contInterstitialAdsDEFAULT 		= 0;
	public static int 	contInterstitialAdsHARDMODE 	= 0;
	public static int 	contInterstitialAdsNORMALMODE 	= 0;
	public static int 	contInterstitialAdsEASYMODE 	= 0;
	public static int 	lastLevelUnlocked 				= 1;
	public static bool 	mandatoryTutorial 				= false; //if it is mandatory to do tutorial before to play
	public static bool 	firstTimePlayerInviteFriends 	= false; //show invite friends screen if it is the firs time
	public static int 	maxLevels 						= 10;
	
	//SETTINGS
	public static float	musicVolume						= 1f;
	public static float soundVolume 					= 1f;
	public static float graphicsDetails					= 1f;
	
	
	//--------------------------------------
	// Constants
	//--------------------------------------
	#region Constants
	//ADS 
	public const bool 	IS_PRO_VERSION = false; 
	public const int 	NUM_GAMEOVERS_SHOW_AD_HARD_MODE 	= 5; //number of gameovers to show an ad in hard mode
	public const int 	NUM_GAMEOVERS_SHOW_AD_EASY_MODE 	= 3; 
	public const int 	NUM_GAMEOVERS_SHOW_AD_NORMAL_MODE 	= 2;
	public const int 	NUM_GAMEOVERS_SHOW_AD_NONE_MODE 	= 1;
	public const int 	NUM_GAMEOVERS_SHOW_AD_BY_DEFAULT 	= 5;
	
	//SOCIAL NETWORKS
	public const bool BUILD_FOR_AMAZON 						= false; //if we are building for amazon (only change when the build is only for amazon, change it after building)
	public const bool USE_GOOGLE_PLAY_SERVICES 				= true;
	public const bool USE_GAMECENTER 						= true;
	public const bool USE_TWITTER 							= false;
	public const bool USE_FACEBOOK 							= false;
	
	//APP LINKS
	public const string GAME_NAME	 				= "Game Template";
	public const string LINK_ANDROID_APP 			= "https://play.google.com/store/apps/details?id=com.pukepixel.";
	public const string SHORT_LINK_ANDROID_APP 		= "";
	public const string LINK_AMAZON_APP 			= "";
	public const string LINK_IOS_APP				= "";
	public const string SHORT_LINK_IOS_APP			= "";
	
	//LOCALIZATION
	public const string LOC_ENGLISH 			= "English";
	public const string LOC_SPANISH 			= "Spanish";
	
	//TAGS
	public const string TAG_PLAYER 				= "Player";
	public const string TAG_ENEMY 				= "Enemy";
	
	//SCENES
	public const string SCENE_MOREGAMES 				= "MoreGames";
	public const string SCENE_SETTINGS 					= "Settings";
	public const string SCENE_CREDITS 					= "Credits";
	public const string SCENE_GAME 						= "Game";
	public const string SCENE_MAINMENU 					= "MainMenu";
	public const string SCENE_LOADER 					= "Loader";
	public const string SCENE_TUTORIAL 					= "Tutorial";
	public const string SCENE_LEVEL_SELECTION 			= "LevelSelection";
	public const string SCENE_CHARACTER_SELECTION 		= "CharacterSelection";
	public const string SCENE_ENVIRONMENT_SELECTION 	= "EnvironmentSelection";
	public const string SCENE_ITEMS_SELECTION 			= "ItemsSelection";
	
	//PlayerPrefs KEYS
	public const string PP_FIRST_PLAY 									= "pp_first_play";
	public const string PP_MUSIC 										= "pp_music";
	public const string PP_SOUND 										= "pp_sound";
	public const string PP_GRAPHICS_DETAILS								= "pp_graphics_details";
	public const string PP_LANGUAGE_CHANGED 							= "pp_language_changed";
	public const string PP_LAST_SCORE 									= "pp_last_score_"; //followed by game difficulty
	public const string PP_BEST_SCORE 									= "pp_best_score_"; //followed by game difficulty
	public const string PP_LAST_LEVEL_UNLOCKED 							= "pp_last_level_unlocked"; //id of last level unlocked
	public const string PP_SELECTED_LEVEL 								= "pp_selected_level"; //id of selected level
	public const string PP_MISION_POPUP_SHOWN 							= "pp_mision_popup_shown_"; //followed by level id
	public const string PP_GAME_MODE 									= "pp_game_mode";
	public const string PP_GAME_DIFFICULTY 								= "pp_game_difficulty";
	public const string PP_SELECTED_WORLD 								= "pp_selected_world";
	public const string PP_SELECTED_CHARACTER 							= "pp_selected_character";
	public const string PP_COMPLETED_TUTORIAL 							= "pp_completed_tutorial";
	public const string PP_LEVEL_STARS 									= "pp_level_stars_"; //followed by level id
	public const string PP_LEVEL_SCORE	 								= "pp_level_score_"; //followed by level id
	public const string PP_IS_SPRITE_CURRENT_LEVEL_INDICATOR_MOVED 		= "pp_is_sprite_level_indicator_moved_";  //followed by level id
	public const string PP_TOTAL_COINS 									= "pp_total_coins"; //total of coins player has
	public const string PP_PURCHASED_ITEM 								= "pp_purchased_item_";  //followed by item id
	public const string PP_TOTAL_GAMES 									= "pp_total_games"; //total games played
	public const string PP_TOTAL_SHOPPING 								= "pp_total_shopping"; //total items purchased
	public const string PP_TOTAL_COLLECTED_COINS 						= "pp_total_coins_collected";
	public const string PP_TOTAL_GEMS_COLLECTED 						= "pp_total_gems_collected";
	
	//GooglePlay Services
	public const string ID_RANKING_FACIL 		= "";
	public const string ID_RANKING_NORMAL 		= "";
	public const string ID_RANKING_DIFICIL 		= "";
	public const string ID_RANKING_PRO 			= "";
	public const string ID_RANKING_GOD 			= "";
	
	//Facebook
	public const string LOGO_APP_LINK 	= "";
	
	//Twitter
	public const string HASHTAG 		= "#GAME";
	
	#endregion
	
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Start(){
		DontDestroyOnLoad(this); //para que no se destruyan los atributos al cargar escenas	
	}
	#endregion
}
