using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	GUIContent whenShowInitialAd   = new GUIContent("When show Initial Ad [?]:", "Number of times the game starts to show ads. Value of 1 indicates that in every game start we show an interstitial ad.");
	GUIContent proVersionLabel   = new GUIContent("Is Pro Version [?]:", "True for not show ads. False if we are going to show ads.");
	GUIContent buildForAmazon   = new GUIContent("Build for Amazon [?]:", "If we are building for amazon (only change when the build is only for amazon, change it after building)");
	GUIContent rankingIDs   = new GUIContent("Rangins IDs [?]:", "All of the Ranking IDs that must be equals in the different platforms.");
	GUIContent rewardPercentageLabel   = new GUIContent("% Money Reward [?]:", "This is the percentage of a money reward applied when a level was completed previously to get the reward.");
	GUIContent gameDiffLabel   = new GUIContent("Game Difficulties [?]:", "All of the difficulties available in the game. NONE for not supporting any difficult.");
	GUIContent musicFXLabel   = new GUIContent("Music & Fx are the same [?]:", "True if music and fx are the same. False if each one is handled by itself.");
	
	GUIContent SdkVersion   = new GUIContent("Plugin Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");
	GUIContent SupportEmail = new GUIContent("Support [?]", "If you have any technical quastion, feel free to drop an e-mail");
	GUIContent showMissionsWinLabel   = new GUIContent("Show missions win [?]:", "True for showing missions window at start.");
	
	private GameSettings settings;
	
	//--------------------------------------
	// Constants
	//--------------------------------------
	private const string version_info_file = "GameTemplate/Versions/GT_VersionInfo.txt"; 
	
	//--------------------------------------
	// Unity
	//--------------------------------------
	public virtual void Awake() {
		
	}
	
	public override void OnInspectorGUI() {
		#if UNITY_WEBPLAYER
		EditorGUILayout.HelpBox("Editing Game Settings not avaliable with web player platfrom. Please swith to any other platfrom under Build Seting menu", MessageType.Warning);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		if(GUILayout.Button("Switch To Android Platfrom",  GUILayout.Width(180))) {
			EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
		}
		if(GUILayout.Button("Switch To iOS Platfrom",  GUILayout.Width(180))) {
			EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iPhone);
		}
		if(GUILayout.Button("Switch To WP8 Platfrom",  GUILayout.Width(180))) {
			EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WP8Player);
		}
		EditorGUILayout.EndHorizontal();
		
		if(Application.isEditor) {
			return;
		}
		
		
		#endif
		
		
		settings = target as GameSettings;
		
		GUI.changed = false;
		
		
		GeneralOptions();
		gameInfo();
		EditorGUILayout.Space();
		moneySettings();
		EditorGUILayout.Space();
		characterControl();
		EditorGUILayout.Space();
		adsSettings();
		EditorGUILayout.Space();
		socialNetworksSettings();
		EditorGUILayout.Space();
		appLinks();
		EditorGUILayout.Space();
		rankingIdsSettings();
		
		
		EditorGUILayout.Space();
		AboutGUI();
		
		if(GUI.changed) {
			DirtyEditor();
		}
	}
	
	
	
	public static bool IsInstalled {
		get {
			if(FileStaticAPI.IsFileExists(version_info_file)) {
				return true;
			} else {
				return false;
			}
		}
	}
	
	public static bool IsUpToDate {
		get {
			if(GameSettings.VERSION_NUMBER.Equals(DataVersion)) {
				return true;
			} else {
				return false;
			}
		}
	}
	
	
	public static float Version {
		get {
			return System.Convert.ToSingle(DataVersion);
		}
	}
	
	
	public static string DataVersion {
		get {
			if(FileStaticAPI.IsFileExists(version_info_file)) {
				return FileStaticAPI.Read(version_info_file);
			} else {
				return "Unknown";
			}
		}
	}
	
	
	
	public static void UpdateVersionInfo() {
		FileStaticAPI.Write(version_info_file, GameSettings.VERSION_NUMBER);
	}
	
	private void GeneralOptions() {
		if(!IsInstalled) {
			EditorGUILayout.HelpBox("Install Required ", MessageType.Error);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			Color c = GUI.color;
			GUI.color = Color.cyan;
			if(GUILayout.Button("Install Plugin",  GUILayout.Width(120))) {
				PluginsInstalationUtil.Android_InstallPlugin();
				UpdateVersionInfo();
			}
			GUI.color = c;
			EditorGUILayout.EndHorizontal();
		}
		
		if(IsInstalled) {
			if(!IsUpToDate) {
				
				EditorGUILayout.HelpBox("Update Required \nResources version: " + DataVersion + " Plugin version: " + GameSettings.VERSION_NUMBER, MessageType.Warning);
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				Color c = GUI.color;
				GUI.color = Color.cyan;
				if(GUILayout.Button("Update to " + GameSettings.VERSION_NUMBER,  GUILayout.Width(250))) {
					UpdateVersionInfo();
				}
				
				GUI.color = c;
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();
				
			} else {
				EditorGUILayout.HelpBox("Game Template v" + GameSettings.VERSION_NUMBER + " is installed", MessageType.Info);
			}
		}
		EditorGUILayout.Space();
	}
	
	protected virtual void gameInfo(){
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("Game Settings", MessageType.Info);
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Game Name");
		GameSettings.Instance.GAME_NAME	= EditorGUILayout.TextField(GameSettings.Instance.GAME_NAME);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Show Test Logs");
		GameSettings.Instance.showTestLogs	= EditorGUILayout.Toggle(GameSettings.Instance.showTestLogs);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(musicFXLabel);
		GameSettings.Instance.FX_AND_MUSIC_ARE_THE_SAME	= EditorGUILayout.Toggle(GameSettings.Instance.FX_AND_MUSIC_ARE_THE_SAME);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Show Load Indicator in Loading scene");
		GameSettings.Instance.showLoadIndicatorInLoadingScene	= EditorGUILayout.Toggle(GameSettings.Instance.showLoadIndicatorInLoadingScene);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Has Initial Tutorial");
		GameSettings.Instance.HAS_INITIAL_TUTORIAL	= EditorGUILayout.Toggle(GameSettings.Instance.HAS_INITIAL_TUTORIAL);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(showMissionsWinLabel);
		GameSettings.Instance.showMissionsWinAtStart = EditorGUILayout.Toggle(GameSettings.Instance.showMissionsWinAtStart);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		handleDifficulties();
	}
	
	private void handleDifficulties(){
		GameSettings.Instance.showGameDifficulties = EditorGUILayout.Foldout(GameSettings.Instance.showGameDifficulties, gameDiffLabel);
		if (GameSettings.Instance.showGameDifficulties) {
			//			EditorGUILayout.LabelField("Game Difficulties");
			if(GameSettings.Instance.gameDifficulties.Count == 0) {
				EditorGUILayout.HelpBox("No Difficulties Registred",MessageType.None);
			}
			
			int i = 0;
			foreach(GameDifficulty d in GameSettings.Instance.gameDifficulties) {
				EditorGUILayout.BeginHorizontal();
				GameSettings.Instance.gameDifficulties[i] = (GameDifficulty) EditorGUILayout.EnumPopup(GameSettings.Instance.gameDifficulties[i]);
				
				
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GameSettings.Instance.gameDifficulties.Remove(d);
					break;
				}
				EditorGUILayout.EndHorizontal();
				i++;
			}
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("+",  GUILayout.Width(60))) {
				GameSettings.Instance.gameDifficulties.Add(GameDifficulty.NONE);
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}
	}
	
	private void handleWorldLevelRankings(){
		GameSettings.Instance.showWorldRankingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showWorldRankingSettings, "Rankings by World Levels");
		if (GameSettings.Instance.showWorldRankingSettings) {
			if(GameSettings.Instance.worldLevelRankingIDS.Count == 0) {
				EditorGUILayout.HelpBox("No World Level Ranking IDs Registred",MessageType.None);
			}
			
			int i = 0;
			foreach(string d in GameSettings.Instance.worldLevelRankingIDS) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Level " + (i+1), GUILayout.Width(60));
				GameSettings.Instance.worldLevelRankingIDS[i] = EditorGUILayout.TextField(GameSettings.Instance.worldLevelRankingIDS[i]).Trim();
				
				
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GameSettings.Instance.worldLevelRankingIDS.Remove(d);
					break;
				}
				EditorGUILayout.EndHorizontal();
				i++;
			}
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("+",  GUILayout.Width(60))) {
				GameSettings.Instance.worldLevelRankingIDS.Add("");
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}
	}
	
	private void handleSurvivalLevelRankings(){
		GameSettings.Instance.showSurvivalLevelsRankingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showSurvivalLevelsRankingSettings, "Rankings by Survival Levels");
		if (GameSettings.Instance.showSurvivalLevelsRankingSettings) {
			if(GameSettings.Instance.survivalLevelRankingIDS.Count == 0) {
				EditorGUILayout.HelpBox("No Survival Level Ranking IDs Registred",MessageType.None);
			}
			
			int i = 0;
			foreach(string d in GameSettings.Instance.survivalLevelRankingIDS) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Level " + (i+1), GUILayout.Width(60));
				GameSettings.Instance.survivalLevelRankingIDS[i] = EditorGUILayout.TextField(GameSettings.Instance.survivalLevelRankingIDS[i]).Trim();
				
				
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GameSettings.Instance.survivalLevelRankingIDS.Remove(d);
					break;
				}
				EditorGUILayout.EndHorizontal();
				i++;
			}
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("+",  GUILayout.Width(60))) {
				GameSettings.Instance.survivalLevelRankingIDS.Add("");
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}
	}
	
	protected virtual void moneySettings(){
		GameSettings.Instance.showMoneySettings = EditorGUILayout.Foldout(GameSettings.Instance.showMoneySettings, "Money");
		if (GameSettings.Instance.showMoneySettings) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("1 Gem value in coins:");
			GameSettings.Instance.ONE_GEM_VALUE_IN_COINS	= EditorGUILayout.IntField(GameSettings.Instance.ONE_GEM_VALUE_IN_COINS);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Initial Money:");
			GameSettings.Instance.INITIAL_MONEY	= EditorGUILayout.IntField(GameSettings.Instance.INITIAL_MONEY);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Initial Gems:");
			GameSettings.Instance.INITIAL_GEMS	= EditorGUILayout.IntField(GameSettings.Instance.INITIAL_GEMS);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(rewardPercentageLabel);
			GameSettings.Instance.PERCENTAGE_MONEY_REWARD_LEVEL_PREV_COMPLETED	= EditorGUILayout.FloatField(GameSettings.Instance.PERCENTAGE_MONEY_REWARD_LEVEL_PREV_COMPLETED);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("% Gems Reward");
			GameSettings.Instance.PERCENTAGE_GEMS_REWARD_LEVEL_PREV_COMPLETED	= EditorGUILayout.FloatField(GameSettings.Instance.PERCENTAGE_GEMS_REWARD_LEVEL_PREV_COMPLETED);
			EditorGUILayout.EndHorizontal();
			
			//			
			//			EditorGUILayout.BeginHorizontal();
			//			EditorGUILayout.LabelField("Max Loaded Image Size");
			//			GameSettings.Instance.MaxImageLoadSize	 	= EditorGUILayout.IntField(GameSettings.Instance.MaxImageLoadSize);
			//			EditorGUILayout.EndHorizontal();
			//			
			//			
			//			
			//			GUI.enabled = !GameSettings.Instance.UseProductNameAsFolderName;
			//			if(GameSettings.Instance.UseProductNameAsFolderName) {
			//				if(PlayerSettings.productName.Length > 0) {
			//					GameSettings.Instance.GalleryFolderName = PlayerSettings.productName.Trim();
			//				}
			//				
			//				
			//			}
			//			
			//			EditorGUILayout.BeginHorizontal();
			//			EditorGUILayout.LabelField("App Gallery Folder");
			//			GameSettings.Instance.GalleryFolderName	 	= EditorGUILayout.TextField(GameSettings.Instance.GalleryFolderName);
			//			if(GameSettings.Instance.GalleryFolderName.Length > 0) {
			//				GameSettings.Instance.GalleryFolderName		= GameSettings.Instance.GalleryFolderName.Trim();
			//				GameSettings.Instance.GalleryFolderName		= GameSettings.Instance.GalleryFolderName.Trim('/');
			//			}
			//			
			//			EditorGUILayout.EndHorizontal();
			//			
			//			GUI.enabled = true;
			//			
			//			EditorGUILayout.BeginHorizontal();
			//			EditorGUILayout.LabelField("Use Product Name As Folder Name");
			//			GameSettings.Instance.UseProductNameAsFolderName	 	= EditorGUILayout.Toggle(GameSettings.Instance.UseProductNameAsFolderName);
			//			EditorGUILayout.EndHorizontal();
		}
	}
	
	protected virtual void adsSettings(){
		GameSettings.Instance.showAdsSettings = EditorGUILayout.Foldout(GameSettings.Instance.showAdsSettings, "Ads Showing Settings");
		if (GameSettings.Instance.showAdsSettings) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(proVersionLabel);
			GameSettings.Instance.IS_PRO_VERSION	= EditorGUILayout.Toggle(GameSettings.Instance.IS_PRO_VERSION);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(whenShowInitialAd);
			GameSettings.Instance.TIMES_START_GAME_TO_SHOW_AD_AT_START	= EditorGUILayout.IntField(GameSettings.Instance.TIMES_START_GAME_TO_SHOW_AD_AT_START);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Default num GameOvers:");
			GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_BY_DEFAULT	= EditorGUILayout.IntField(GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_BY_DEFAULT);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Easy mode num GameOvers:");
			GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_EASY_MODE	= EditorGUILayout.IntField(GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_EASY_MODE);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Normal mode num GameOvers:");
			GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_NORMAL_MODE	= EditorGUILayout.IntField(GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_NORMAL_MODE);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Hard mode num GameOvers:");
			GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_HARD_MODE	= EditorGUILayout.IntField(GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_HARD_MODE);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Survival mode num GameOvers:");
			GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_SURVIVAL_MODE	= EditorGUILayout.IntField(GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_SURVIVAL_MODE);
			EditorGUILayout.EndHorizontal();
		}
	}
	
	protected virtual  void socialNetworksSettings(){
		GameSettings.Instance.showSocialNetworksSettings = EditorGUILayout.Foldout(GameSettings.Instance.showSocialNetworksSettings, "Social Networks");
		if (GameSettings.Instance.showSocialNetworksSettings) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(buildForAmazon);
			GameSettings.Instance.BUILD_FOR_AMAZON	= EditorGUILayout.Toggle(GameSettings.Instance.BUILD_FOR_AMAZON);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Google Play Services");
			GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES	= EditorGUILayout.Toggle(GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Game Center");
			GameSettings.Instance.USE_GAMECENTER	= EditorGUILayout.Toggle(GameSettings.Instance.USE_GAMECENTER);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Facebook");
			GameSettings.Instance.USE_FACEBOOK	= EditorGUILayout.Toggle(GameSettings.Instance.USE_FACEBOOK);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Twitter");
			GameSettings.Instance.USE_TWITTER	= EditorGUILayout.Toggle(GameSettings.Instance.USE_TWITTER);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("#Hashtag:");
			GameSettings.Instance.HASHTAG	= EditorGUILayout.TextField(GameSettings.Instance.HASHTAG).Trim();
			EditorGUILayout.EndHorizontal();
		}
	}
	
	protected virtual  void appLinks(){
		GameSettings.Instance.showAppLinksSettings = EditorGUILayout.Foldout(GameSettings.Instance.showAppLinksSettings, "App Links");
		if (GameSettings.Instance.showAppLinksSettings) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Android Link:");
			GameSettings.Instance.LINK_ANDROID_APP	= EditorGUILayout.TextField(GameSettings.Instance.LINK_ANDROID_APP).Trim();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Android Short Link:");
			GameSettings.Instance.SHORT_LINK_ANDROID_APP	= EditorGUILayout.TextField(GameSettings.Instance.SHORT_LINK_ANDROID_APP).Trim();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("iOS Link:");
			GameSettings.Instance.LINK_IOS_APP	= EditorGUILayout.TextField(GameSettings.Instance.LINK_IOS_APP).Trim();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("iOS Short Link:");
			GameSettings.Instance.SHORT_LINK_IOS_APP	= EditorGUILayout.TextField(GameSettings.Instance.SHORT_LINK_IOS_APP).Trim();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Amazon Link:");
			GameSettings.Instance.LINK_AMAZON_APP	= EditorGUILayout.TextField(GameSettings.Instance.LINK_AMAZON_APP).Trim();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Amazon Short Link:");
			GameSettings.Instance.SHORT_LINK_AMAZON_APP	= EditorGUILayout.TextField(GameSettings.Instance.SHORT_LINK_AMAZON_APP).Trim();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Game Logo Link:");
			GameSettings.Instance.LOGO_APP_LINK	= EditorGUILayout.TextField(GameSettings.Instance.LOGO_APP_LINK).Trim();
			EditorGUILayout.EndHorizontal();
		}
	}
	
	protected virtual void rankingIdsSettings(){
		GameSettings.Instance.showRankingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showRankingSettings, rankingIDs);
		if (GameSettings.Instance.showRankingSettings) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Unique Ranking");
			GameSettings.Instance.ID_UNIQUE_RANKING	= EditorGUILayout.TextField(GameSettings.Instance.ID_UNIQUE_RANKING).Trim();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Survival Ranking");
			GameSettings.Instance.ID_RANKING_SURVIVAL	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_SURVIVAL).Trim();
			EditorGUILayout.EndHorizontal();
			
			GameSettings.Instance.showDifficultiesRankingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showDifficultiesRankingSettings, "Rankings by Difficulty");
			if (GameSettings.Instance.showDifficultiesRankingSettings) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Easy Mode Ranking");
				GameSettings.Instance.ID_RANKING_EASY	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_EASY).Trim();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Normal Mode Ranking");
				GameSettings.Instance.ID_RANKING_NORMAL	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_NORMAL).Trim();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Hard Mode Ranking");
				GameSettings.Instance.ID_RANKING_HARD	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_HARD).Trim();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("PRO Mode Ranking");
				GameSettings.Instance.ID_RANKING_PRO	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_PRO).Trim();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("God Mode Ranking");
				GameSettings.Instance.ID_RANKING_GOD	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_GOD).Trim();
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Impossible Ranking");
				GameSettings.Instance.ID_RANKING_IMPOSSIBLE	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_IMPOSSIBLE).Trim();
				EditorGUILayout.EndHorizontal();
			}
			
			handleWorldLevelRankings();
			handleSurvivalLevelRankings();
			
			//			GameSettings.Instance.showSurvivalLevelsRankingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showSurvivalLevelsRankingSettings, "Rankings by Survival Levels");
			//			if (GameSettings.Instance.showSurvivalLevelsRankingSettings) {
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Level 1 Survival Ranking");
			//				GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_1	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_1);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Level 2 Survival Ranking");
			//				GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_2	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_2);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Level 3 Survival Ranking");
			//				GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_3	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_3);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Level 4 Survival Ranking");
			//				GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_4	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_4);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Level 5 Survival Ranking");
			//				GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_5	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_5);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Level 6 Survival Ranking");
			//				GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_6	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_6);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Level 7 Survival Ranking");
			//				GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_7	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_7);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Level 8 Survival Ranking");
			//				GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_8	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_8);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Level 9 Survival Ranking");
			//				GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_9	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_9);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Level 10 Survival Ranking");
			//				GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_10	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_SURVIVAL_LEVEL_10);
			//				EditorGUILayout.EndHorizontal();
			//			}
			
			//			GameSettings.Instance.showWorldRankingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showWorldRankingSettings, "Rankings by Game Worlds");
			//			if (GameSettings.Instance.showWorldRankingSettings) {
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("World 1 Ranking");
			//				GameSettings.Instance.ID_RANKING_WORLD_1	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_WORLD_1);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("World 2 Ranking");
			//				GameSettings.Instance.ID_RANKING_WORLD_2	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_WORLD_2);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("World 3 Ranking");
			//				GameSettings.Instance.ID_RANKING_WORLD_3	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_WORLD_3);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("World 4 Ranking");
			//				GameSettings.Instance.ID_RANKING_WORLD_4	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_WORLD_4);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("World 5 Ranking");
			//				GameSettings.Instance.ID_RANKING_WORLD_5	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_WORLD_5);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("World 6 Ranking");
			//				GameSettings.Instance.ID_RANKING_WORLD_6	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_WORLD_6);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("World 7 Ranking");
			//				GameSettings.Instance.ID_RANKING_WORLD_7	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_WORLD_7);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("World 8 Ranking");
			//				GameSettings.Instance.ID_RANKING_WORLD_8	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_WORLD_8);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("World 9 Ranking");
			//				GameSettings.Instance.ID_RANKING_WORLD_9	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_WORLD_9);
			//				EditorGUILayout.EndHorizontal();
			//
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("World 10 Ranking");
			//				GameSettings.Instance.ID_RANKING_WORLD_10	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_WORLD_10);
			//				EditorGUILayout.EndHorizontal();
			//			}
		}
	}
	
	protected virtual void characterControl(){
		GameSettings.Instance.showCharacterControlSettings = EditorGUILayout.Foldout(GameSettings.Instance.showCharacterControlSettings, "Character Control");
		if (GameSettings.Instance.showCharacterControlSettings) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Initial Sensitivity");
			GameSettings.Instance.INITIAL_CHAR_CONTROL_SENSITIVITY	= EditorGUILayout.FloatField(GameSettings.Instance.INITIAL_CHAR_CONTROL_SENSITIVITY);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Min Sensitivity");
			GameSettings.Instance.MIN_CHAR_CONTROL_SENSITIVITY	= EditorGUILayout.FloatField(GameSettings.Instance.MIN_CHAR_CONTROL_SENSITIVITY);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Max Sensitivity");
			GameSettings.Instance.MAX_CHAR_CONTROL_SENSITIVITY	= EditorGUILayout.FloatField(GameSettings.Instance.MAX_CHAR_CONTROL_SENSITIVITY);
			EditorGUILayout.EndHorizontal();
		}
	}
	
	private static void SuperSpace() {
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
	}
	
	
	
	
	private bool IsDigitsOnly(string str) {
		foreach (char c in str) {
			if (!char.IsDigit(c)) {
				return false;
			}
		}
		
		return true;
	}
	
	
	
	
	
	private void AboutGUI() {
		
		EditorGUILayout.HelpBox("About the Plugin", MessageType.None);
		
		SelectableLabelField(SdkVersion,   GameSettings.VERSION_NUMBER);
		
		
		SelectableLabelField(SupportEmail, "pukepixel@gmail.com");
		
		
	}
	
	private void SelectableLabelField(GUIContent label, string value) {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
		EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
		EditorGUILayout.EndHorizontal();
	}
	
	
	
	
	private static void DirtyEditor() {
		#if UNITY_EDITOR
		//		EditorUtility.SetDirty(SocialPlatfromSettings.Instance);
		EditorUtility.SetDirty(GameSettings.Instance);
		#endif
	}
	
	
}
