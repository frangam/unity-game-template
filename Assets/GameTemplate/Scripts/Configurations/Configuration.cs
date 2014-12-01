using UnityEngine;
using System.Collections;

public class Configuration : MonoBehaviour{
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	public static bool 	purchasedForQuitAds 			= false; //if player has purchased for quit all of the ads
	public static int 	contInterstitialAdsDEFAULT 		= 0;
	public static int 	contInterstitialAdsHARDMODE 	= 0;
	public static int 	contInterstitialAdsNORMALMODE 	= 0;
	public static int 	contInterstitialAdsEASYMODE 	= 0;
	public static int 	lastLevelUnlocked 				= 1;
	public static bool 	mandatoryTutorial 				= true; //if it is mandatory to do tutorial before to play
	public static bool 	firstTimePlayerInviteFriends 	= false; //show invite friends screen if it is the firs time
	public static int 	maxLevels 						= 10;
	
	//SETTINGS
	public static bool 	musicActivated					= true;
	public static bool 	soundActivated 					= true;


	//--------------------------------------
	// Constants
	//--------------------------------------
	#region Constants
	//ADS 
	public const bool 	IS_PRO_VERSION = false; 
	public const int 	NUM_GAMEOVERS_SHOW_AD_HARD_MODE = 3; //por cada muerte mostramos un pantallazo de publi EN DIFICIL
	public const int 	NUM_GAMEOVERS_SHOW_AD_EASY_MODE = 1; //por cada muerte mostramos un pantallazo de publi EN FACIL
	public const int 	NUM_GAMEOVERS_SHOW_AD_NORMAL_MODE = 2; //por cada muerte mostramos un pantallazo de publi EN NORMAL
	public const int 	NUM_GAMEOVERS_SHOW_AD_BY_DEFAULT = 1; //por cada muerte mostramos un pantallazo de publi EN DEFECTO

	//SOCIAL NETWORKS
	public const bool buildForAmazon 						= false; //if we are building for amazon (only change when the build is only for amazon, change it after building)
	public const bool USE_GOOGLE_PLAY_SERVICES 				= true;
	public const bool USE_GAMECENTER 						= true;
	public const bool USE_TWITTER 							= false;
	public const bool USE_FACEBOOK 							= false;

	//APP LINKS
	public const string NOMBRE_JUEGO 				= "Game Template";
	public const string LINK_ANDROID_APP 			= "https://play.google.com/store/apps/details?id=com.pukepixel.gametemplate";
	public const string LINK_CORTO_ANDROID_APP 		= "";
	public const string LINK_AMAZON_APP 			= "";
	public const string LINK_IOS_APP				= "";
	public const string LINK_CORTO_IOS_APP			= "";

	//LOCALIZATION
	public const string LOC_ENGLISH 		= "English";
	public const string LOC_SPANISH 		= "Spanish";

	//TAGS
	public const string TAG_PLAYER 				= "Player";
	public const string TAG_ENEMY 				= "Enemy";
	
	//SCENES
	public const string SCENE_MOREGAMES 		= "MoreGames";
	public const string SCENE_SETTINGS 			= "Settings";
	public const string SCENE_INFO 				= "Info";
	public const string SCENE_GAME 				= "Game";
	public const string SCENE_MAINMENU 			= "MainMenu";
	public const string SCENE_LOADER 			= "Loader";
	public const string SCENE_TUTORIAL 			= "Tutorial";
	public const string SCENE_LEVEL_SELECTION 	= "LevelSelection";

	//PlayerPrefs KEYS
	public const string PP_FIRST_PLAY 									= "first_play";
	public const string PP_MUSIC 										= "music";
	public const string PP_SOUND 										= "sound";
	public const string PP_LANGUAGE_CHANGED 							= "language_changed";
	public const string PP_LAST_SCORE 									= "last_score_"; //followed by game difficulty
	public const string PP_BEST_SCORE 									= "best_score_"; //followed by game difficulty
	public const string PP_LAST_LEVEL_UNLOCKED 							= "last_level_unlocked"; //id of last level unlocked
	public const string PP_SELECTED_LEVEL 								= "selected_level"; //id of selected level
	public const string PP_MISION_POPUP_SHOWN 							= "mision_popup_shown_"; //followed by level id
	public const string PP_GAME_MODE 									= "game_mode";
	public const string PP_GAME_DIFFICULTY 								= "game_difficulty";
	public const string PP_SELECTED_WORLD 								= "selected_world";
	public const string PP_SELECTED_CHARACTER 							= "selected_character";
	public const string PP_COMPLETED_TUTORIAL 							= "completed_tutorial";
	public const string PP_LEVEL_STARS 									= "level_stars_"; //followed by level id
	public const string PP_LEVEL_SCORE	 								= "level_score_"; //followed by level id
	public const string PP_IS_SPRITE_CURRENT_LEVEL_INDICATOR_MOVED 		= "is_sprite_level_indicator_moved_";  //followed by level id
	public const string PP_TOTAL_COINS 									= "total_coins"; //total of coins player has
	public const string PP_PURCHASED_ITEM 								= "purchased_item_";  //followed by item id
	public const string PP_TOTAL_GAMES 									= "total_games"; //total games played
	public const string PP_TOTAL_SHOPPING 								= "total_shopping"; //total items purchased
	public const string PP_TOTAL_COLLECTED_COINS 						= "total_coins_collected";
	public const string PP_TOTAL_GEMS_COLLECTED 						= "total_gems_collected";

	//GooglePlay Services
	public const string ID_RANKING_FACIL 		= "";
	public const string ID_RANKING_NORMAL 		= "";
	public const string ID_RANKING_DIFICIL 		= "";
	public const string ID_RANKING_PRO 			= "";
	public const string ID_RANKING_GOD 			= "";

	//Facebook
	public const string LINK_ICONO_APP 	= "";

	//Twitter
	public const string API_KEY 		= "";
	public const string API_SECRET 		= "";
	public const string HASHTAG 		= "#GameTemplate";

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
