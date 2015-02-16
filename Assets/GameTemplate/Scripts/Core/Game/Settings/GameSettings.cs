﻿using UnityEngine;
using System.IO;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class GameSettings : ScriptableObject {
	//--------------------------------------
	// Flags to control showing in editor
	//--------------------------------------
	public bool showGameDifficulties			 		= false;
	public bool showMoneySettings 						= false;
	public bool showAdsSettings 						= false;
	public bool showSocialNetworksSettings 				= false;
	public bool showAppLinksSettings 					= false;
	public bool showCharacterControlSettings 			= false;
	public bool showRankingSettings 					= false;
	public bool showWorldRankingSettings 				= false;
	public bool showSurvivalLevelsRankingSettings 		= false;
	public bool showDifficultiesRankingSettings 		= false;
	public bool showInAppBillingSettings 				= false;
	
	
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	public const string VERSION_NUMBER 					= "1.0";
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
	public List<GameDifficulty> gameDifficulties 		= new List<GameDifficulty>(){GameDifficulty.NONE};
	public List<string> survivalLevelRankingIDS 		= new List<string>();
	public List<string> worldLevelRankingIDS 			= new List<string>();
	public List<string> inAppBillingIDS 				= new List<string>();
	public bool FX_AND_MUSIC_ARE_THE_SAME 				= true;
	public bool HAS_INITIAL_TUTORIAL 					= false;
	public bool 	showMissionsWinAtStart				= false;
	public bool 	showLoadIndicatorInLoadingScene		= true;
	public bool 	showTestLogs						= false; //show debug log for test or not
	
	//--------------------------------------
	// Constants
	//--------------------------------------
	#region Constants
	//MONEY
	/// <summary>
	/// 1 gem is equals to 100 or less coins
	/// </summary>
	public int 		ONE_GEM_VALUE_IN_COINS = 100;
	public int 		INITIAL_MONEY = 0;
	public int 		INITIAL_GEMS = 5;
	public float	PERCENTAGE_MONEY_REWARD_LEVEL_PREV_COMPLETED 		= 0.3f;
	public float	PERCENTAGE_GEMS_REWARD_LEVEL_PREV_COMPLETED 		= 0f;
	
	//ADS 
	public bool 	IS_A_DEV_VERSION = true; 
	public bool 	IS_PRO_VERSION = false; 
	public int		TIMES_START_GAME_TO_SHOW_AD_AT_START 			= 1; //1: we show an interstisial ad every time user inits the game		
	public int 		NUM_GAMEOVERS_SHOW_AD_HARD_MODE 	= 5; //number of gameovers to show an ad in hard mode
	public int 		NUM_WINS_SHOW_AD_HARD_MODE 			= 5; //number of WINS to show an ad in hard mode
	public int 		NUM_GAMEOVERS_SHOW_AD_EASY_MODE 	= 3; 
	public int 		NUM_WINS_SHOW_AD_EASY_MODE 			= 3; 
	public int 		NUM_GAMEOVERS_SHOW_AD_NORMAL_MODE 	= 2;
	public int 		NUM_WINS_SHOW_AD_NORMAL_MODE 		= 2;
	public int 		NUM_GAMEOVERS_SHOW_AD_SURVIVAL_MODE	= 1;
	public int 		NUM_WINS_SHOW_AD_SURVIVAL_MODE		= 1;
	public int 		NUM_GAMEOVERS_SHOW_AD_BY_DEFAULT 	= 5;
	public int 		NUM_WINS_SHOW_AD_BY_DEFAULT 		= 3; //number of WINS to show an ad by default
	
	//SOCIAL NETWORKS
	public bool BUILD_FOR_AMAZON 						= false; //if we are building for amazon (only change when the build is only for amazon, change it after building)
	public bool USE_GOOGLE_PLAY_SERVICES 				= true;
	public bool USE_GAMECENTER 							= true;
	public bool USE_TWITTER 							= false;
	public bool USE_FACEBOOK 							= true;
	
	//APP LINKS
	public string GAME_NAME	 						= "Game Template";
	public string LINK_ANDROID_APP 					= "https://play.google.com/store/apps/details?id=com.pukepixel.";
	public string SHORT_LINK_ANDROID_APP 			= "";
	public string LINK_AMAZON_APP 					= "http://www.amazon.com/gp/mas/dl/android?p=com.pukepixel.";
	public string SHORT_LINK_AMAZON_APP				= "";
	public string LINK_IOS_APP						= "https://itunes.apple.com/app/id";
	public string SHORT_LINK_IOS_APP				= "";
	
	//CHARACTER CONTROL
	public float INITIAL_CHAR_CONTROL_SENSITIVITY 		= 2f;
	public float MAX_CHAR_CONTROL_SENSITIVITY 			= 5f;
	public float MIN_CHAR_CONTROL_SENSITIVITY 			= 0.25f;
	
	//LOCALIZATION
	public const string LOC_ENGLISH 				= "English";
	public const string LOC_SPANISH 				= "Spanish";
	
	//TAGS
	public const string TAG_PLAYER 					= "Player";
	public const string TAG_ENEMY 					= "Enemy";
	
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
	public const string SCENE_SURVIVAL_MENU_SELECTION	= "SurvivalMenu";
	public const string SCENE_INAPP			 			= "InApp";
	
	//PlayerPrefs KEYS
	public const string PP_FIRST_PLAY 									= "pp_first_play";
	public const string PP_MUSIC 										= "pp_music";
	public const string PP_SOUND 										= "pp_sound";
	public const string PP_GRAPHICS_DETAILS								= "pp_graphics_details";
	public const string PP_LANGUAGE_CHANGED 							= "pp_language_changed";
	public const string PP_LAST_SCORE 									= "pp_last_score_"; //followed by game difficulty
	public const string PP_BEST_SCORE 									= "pp_best_score_"; //followed by game difficulty
	public const string PP_LAST_LEVEL_UNLOCKED 							= "pp_last_level_unlocked"; //id of last level unlocked
	public const string PP_LAST_LEVEL_PLAYED							= "pp_last_level_played"; //id of last level played
	public const string PP_LEVEL_COMPLETED_TIMES						= "pp_level_completed_times_"; //+ level id
	public const string PP_LEVEL_TRIES_TIMES							= "pp_level_tries_times_"; //+ level id
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
	public const string PP_TOTAL_MONEY 									= "pp_total_money"; //total of the money player has
	public const string PP_TOTAL_GEMS									= "pp_total_gems"; //total of the gems player has
	public const string PP_PURCHASED_ITEM 								= "pp_purchased_item_";  //followed by item id
	public const string PP_TOTAL_GAMES 									= "pp_total_games"; //total games played
	public const string PP_TOTAL_SHOPPING 								= "pp_total_shopping"; //total items purchased
	public const string PP_TOTAL_COLLECTED_COINS 						= "pp_total_coins_collected";
	public const string PP_TOTAL_GEMS_COLLECTED 						= "pp_total_gems_collected";
	public const string PP_CHARACTER_CONTROL_SENSITIVITY 				= "pp_character_control_sensitivity";
	public const string PP_CHARACTER_CONTROL_MIN_SENSITIVITY 			= "pp_character_control_min_sensitivity";
	public const string PP_CHARACTER_CONTROL_MAX_SENSITIVITY 			= "pp_character_control_max_sensitivity";
	public const string PP_SHOW_MISSIONS_WINDOW	 						= "pp_show_mission_window";
	public const string	PP_LAST_UNLOCKED_SURVIVAL_LEVEL	 				= "pp_last_unlocked_survival_level";
	public const string	PP_TOTAL_GAME_OPENINGS				 			= "pp_total_game_openings"; 
	public const string	PP_TOTAL_GAMEOVERS					 			= "pp_total_gameovers"; //total of gameovers
	public const string	PP_TOTAL_WINS					 				= "pp_total_wins"; //total of victories
	public const string	PP_TOTAL_CAMPAIGN_WINS					 		= "pp_total_campaign_wins"; //total of victories in campaign mode
	public const string	PP_TOTAL_QUICKGAME_WINS					 		= "pp_total_quickgame_wins"; //total of victories in quickgame mode
	public const string	PP_TOTAL_SURVIVAL_WINS					 		= "pp_total_survival_wins"; //total of victories in survival mode
	public const string	PP_TOTAL_CAMPAIGN_GAMEOVERS					 	= "pp_total_campaign_gameovers"; //total of gameovers in campaing mode
	public const string	PP_TOTAL_QUICKGAME_GAMEOVERS					= "pp_total_quickgame_gameovers"; //total of gameovers in quickgame mode
	public const string	PP_TOTAL_SURVIVAL_GAMEOVERS						= "pp_total_survival_gameovers"; //total of gameovers in survival mode
	public const string	PP_NUM_GAMEOVERS_WITHOUT_DIFFICULTY	 			= "pp_num_gameovers_without_difficulty";
	public const string	PP_NUM_WINS_WITHOUT_DIFFICULTY	 				= "pp_num_wins_without_difficulty";
	public const string	PP_NUM_GAMEOVERS_WITH_DIFFICULTY				= "pp_num_gameovers_with_difficulty_"; //+ (int) GameDifficulty(enum)
	public const string	PP_NUM_WINS_WITH_DIFFICULTY						= "pp_num_wins_with_difficulty_"; //+ (int) GameDifficulty(enum)
	public const string	PP_NUM_GAMEOVERS_IN_LEVEL						= "pp_num_gameovers_in_level_"; //+ (int) level id
	public const string	PP_NUM_WINS_IN_LEVEL							= "pp_num_wins_in_level_"; //+ (int) level id
	public const string	PP_NUM_GAMEOVERS_IN_SURVIVAL_LEVEL				= "pp_num_gameovers_in_survival_level_"; //+ (int) survival level id
	public const string	PP_NUM_WINS_IN_SURVIVAL_LEVEL					= "pp_num_wins_in_survival_level_"; //+ (int) survival level id
	public const string PP_UNIQUE_LIST_CURRENT_ITEM_SELECTED_ID			= "pp_unique_list_current_item_selected_id";
	
	//GooglePlay Services
	public string ID_UNIQUE_RANKING 				= "";
	public string ID_RANKING_SURVIVAL				= "";
	public string ID_RANKING_EASY 					= "";
	public string ID_RANKING_NORMAL 				= "";
	public string ID_RANKING_HARD 					= "";
	public string ID_RANKING_PRO 					= "";
	public string ID_RANKING_GOD 					= "";
	public string ID_RANKING_IMPOSSIBLE 			= "";
	//	public string ID_RANKING_SURVIVAL_LEVEL_1		= "";
	//	public string ID_RANKING_SURVIVAL_LEVEL_2		= "";
	//	public string ID_RANKING_SURVIVAL_LEVEL_3		= "";
	//	public string ID_RANKING_SURVIVAL_LEVEL_4		= "";
	//	public string ID_RANKING_SURVIVAL_LEVEL_5		= "";
	//	public string ID_RANKING_SURVIVAL_LEVEL_6		= "";
	//	public string ID_RANKING_SURVIVAL_LEVEL_7		= "";
	//	public string ID_RANKING_SURVIVAL_LEVEL_8		= "";
	//	public string ID_RANKING_SURVIVAL_LEVEL_9		= "";
	//	public string ID_RANKING_SURVIVAL_LEVEL_10		= "";
	//	public string ID_RANKING_WORLD_1				= "";
	//	public string ID_RANKING_WORLD_2				= "";
	//	public string ID_RANKING_WORLD_3				= "";
	//	public string ID_RANKING_WORLD_4				= "";
	//	public string ID_RANKING_WORLD_5				= "";
	//	public string ID_RANKING_WORLD_6				= "";
	//	public string ID_RANKING_WORLD_7				= "";
	//	public string ID_RANKING_WORLD_8				= "";
	//	public string ID_RANKING_WORLD_9				= "";
	//	public string ID_RANKING_WORLD_10				= "";
	
	//Facebook
	public string LOGO_APP_LINK 	= "";
	
	//Twitter
	public string HASHTAG 		= "#GAME";
	
	#endregion
	
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Start(){
		DontDestroyOnLoad(this); //para que no se destruyan los atributos al cargar escenas	
	}
	#endregion
	
	
	public const string assetName = "GameSettings";
	public const string path = "GameTemplate/Resources";
	public const string extension = ".asset";
	
	private static GameSettings instance = null;
	
	
	public static GameSettings Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(assetName) as GameSettings;
				
				if (instance == null) {
					
					// If not found, autocreate the asset object.
					instance = CreateInstance<GameSettings>();
					#if UNITY_EDITOR
					//string properPath = Path.Combine(Application.dataPath, ANSettingsPath);
					
					FileStaticAPI.CreateFolder(path);
					
					/*
					if (!Directory.Exists(properPath)) {
						AssetDatabase.CreateFolder("Extensions/", "AndroidNative");
						AssetDatabase.CreateFolder("Extensions/AndroidNative", "Resources");
					}
					*/
					
					string fullPath = Path.Combine(Path.Combine("Assets", path),
					                               assetName + extension
					                               );
					
					AssetDatabase.CreateAsset(instance, fullPath);
					#endif
				}
			}
			return instance;
		}
	}
	
}
