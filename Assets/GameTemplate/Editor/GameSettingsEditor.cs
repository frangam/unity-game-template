/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
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
	GUIContent secDuringGP   = new GUIContent("Sec during Game Play [?]:", "Seconds will pass during game playing to show an ad");
	GUIContent notifyAdDuringGP   = new GUIContent("Notify before than (secs) [?]:", "Launch an event to notify an ad will show in the next specified seconds");
	GUIContent buildForAmazon   = new GUIContent("Build for Amazon [?]:", "If we are building for amazon (only change when the build is only for amazon, change it after building)");
	GUIContent rankingIDs   = new GUIContent("Rankins IDs [?]:", "All of the Ranking IDs that must be equals in the different platforms.");
	GUIContent rewardPercentageLabel   = new GUIContent("% Money Reward [?]:", "This is the percentage of a money reward applied when a level was completed previously to get the reward.");
	GUIContent gameDiffLabel   = new GUIContent("Game Difficulties [?]:", "All of the difficulties available in the game. NONE for not supporting any difficult.");
	GUIContent musicFXLabel   = new GUIContent("Music & Fx are the same [?]:", "True if music and fx are the same. False if each one is handled by itself.");
	
	GUIContent SdkVersion   = new GUIContent("Plugin Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");
	GUIContent SupportEmail = new GUIContent("Support [?]", "If you have any technical quastion, feel free to drop an e-mail");
	GUIContent showMissionsWinLabel   = new GUIContent("Show missions win [?]:", "True for showing missions window at start.");
	GUIContent testLanguage   = new GUIContent("Language for test [?]:", "Leave empty for production version");
	GUIContent templateMultiversion   = new GUIContent("Game MultiVersion for Build [?]:", "Min 0 if this project has multiple versinable games.");
	GUIContent gameNamesContent   = new GUIContent("Game Names [?]:", "Min set one name. Add more names for each game version in order");
	GUIContent buildPackageIDsContent   = new GUIContent("Build Package IDs [?]:", "Min set one id. Add more ids for each game version order");
	GUIContent bundleVersionsContent   = new GUIContent("Bundle Versions [?]:", "Min set one bundle version. Add more bundle versions for each game version order");
	GUIContent uniqueRankingIDsContent   = new GUIContent("Unique Ranking IDs [?]:", "Min set one id for a unique game no versinable. Add more ids for each game version order");
	GUIContent uniqueSurvivalRankingIDsContent   = new GUIContent("Unique Survival Ranking IDs [?]:", "Min set one id for a unique game no versinable. Add more ids for each game version order");
	
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
		backendServerSettings ();
		//		EditorGUILayout.Space();
		appLinks();
		//		EditorGUILayout.Space();
		moneySettings();
		//		EditorGUILayout.Space();
		characterControl();
		//		EditorGUILayout.Space();
		adsSettings();
		//		EditorGUILayout.Space();
		socialNetworksSettings();
		//		EditorGUILayout.Space();
		rankingIdsSettings();
		achievements();
		//		EditorGUILayout.Space();
		inAppBilling();
		
		
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
		FileStaticAPI.Write(version_info_file, DataVersion);
	}
	
	private void GeneralOptions() {
		if(!IsInstalled) {
			EditorGUILayout.HelpBox("Install Required ", MessageType.Error);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			Color c = GUI.color;
			GUI.color = Color.cyan;
			// if(GUILayout.Button("Install Plugin",  GUILayout.Width(120))) {
			// 	PluginsInstalationUtil.Android_InstallPlugin();
			// 	UpdateVersionInfo();
			// }
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
		EditorGUILayout.HelpBox("Game Info", MessageType.None);
		EditorGUILayout.BeginVertical();
		
		GameSettings.Instance.showGameInfo = EditorGUILayout.Foldout(GameSettings.Instance.showGameInfo, "Game Info");
		if (GameSettings.Instance.showGameInfo) {
			EditorGUI.indentLevel++;
			
			
			//Current game multiversion
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(templateMultiversion);
			GameSettings.Instance.currentGameMultiversion = EditorGUILayout.IntField(GameSettings.Instance.currentGameMultiversion);
			EditorGUILayout.EndHorizontal();
			//			EditorGUILayout.Space();
			
			localizations();
			//			excludesResourcesForBuild();
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Is a Dev version");
			GameSettings.Instance.IS_A_DEV_VERSION	= EditorGUILayout.Toggle(GameSettings.Instance.IS_A_DEV_VERSION);
			EditorGUILayout.EndHorizontal();
			//			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Show Test Logs");
			GameSettings.Instance.showTestLogs	= EditorGUILayout.Toggle(GameSettings.Instance.showTestLogs);
			EditorGUILayout.EndHorizontal();
			//			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Test a language ?");
			GameSettings.Instance.useTestLanguage	= EditorGUILayout.Toggle(GameSettings.Instance.useTestLanguage);
			EditorGUILayout.EndHorizontal();
			
			if(GameSettings.Instance.useTestLanguage){
				EditorGUI.indentLevel++;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(testLanguage);
				GameSettings.Instance.testLanguage	= (SystemLanguage) EditorGUILayout.EnumPopup(GameSettings.Instance.testLanguage);
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;
				//			EditorGUILayout.Space();
			}
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(musicFXLabel);
			GameSettings.Instance.FX_AND_MUSIC_ARE_THE_SAME	= EditorGUILayout.Toggle(GameSettings.Instance.FX_AND_MUSIC_ARE_THE_SAME);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Show Load Indicator in Loading scene");
			GameSettings.Instance.showLoadIndicatorInLoadingScene	= EditorGUILayout.Toggle(GameSettings.Instance.showLoadIndicatorInLoadingScene);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Android Immersive Mode");
			GameSettings.Instance.ENABLE_ANDROID_IMMERSIVE_MODE	= EditorGUILayout.Toggle(GameSettings.Instance.ENABLE_ANDROID_IMMERSIVE_MODE);
			EditorGUILayout.EndHorizontal();
			//			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Has Initial Tutorial");
			GameSettings.Instance.HAS_INITIAL_TUTORIAL	= EditorGUILayout.Toggle(GameSettings.Instance.HAS_INITIAL_TUTORIAL);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(showMissionsWinLabel);
			GameSettings.Instance.showMissionsWinAtStart = EditorGUILayout.Toggle(GameSettings.Instance.showMissionsWinAtStart);
			EditorGUILayout.EndHorizontal();
			//			EditorGUILayout.Space();
			
			handleDifficulties();
			EditorGUI.indentLevel--;
		}
		EditorGUILayout.EndVertical();
	}

	protected virtual void backendServerSettings(){
		EditorGUILayout.Space();

		GameSettings.Instance.showBackendServerSettings = EditorGUILayout.Foldout(GameSettings.Instance.showBackendServerSettings, "BackendServer");
		if (GameSettings.Instance.showBackendServerSettings) {
			EditorGUI.indentLevel++;


			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Save game money");
			GameSettings.Instance.useBackendForSaveGameMoney = EditorGUILayout.Toggle(GameSettings.Instance.useBackendForSaveGameMoney);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Scores");
			GameSettings.Instance.useBackendForScores = EditorGUILayout.Toggle(GameSettings.Instance.useBackendForScores);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Achievements");
			GameSettings.Instance.useBackendForAchievements = EditorGUILayout.Toggle(GameSettings.Instance.useBackendForAchievements);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("InApps");
			GameSettings.Instance.useBackendForInApps = EditorGUILayout.Toggle(GameSettings.Instance.useBackendForInApps);
			EditorGUILayout.EndHorizontal();

			EditorGUI.indentLevel--;
		}
	}
	
	protected virtual void inAppBilling(){
		GameSettings.Instance.showInAppBillingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showInAppBillingSettings, "In App Billing");
		if (GameSettings.Instance.showInAppBillingSettings) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Use In App Purchases service");
			GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE = EditorGUILayout.Toggle(GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			
			Color prevCol = GUI.color;
			
			if(GameSettings.Instance.USE_IN_APP_PURCHASES_SERVICE){
				if(GameSettings.Instance.allInAppBillingIDS.Count == 0) {
					EditorGUILayout.HelpBox("No In App Billing IDs for Game Multiversion Registred",MessageType.None);
				}
				else{
					EditorGUILayout.HelpBox("In App Billing Ids by Each Game Multiversion", MessageType.None);
				}
				
				int i = 0;
				foreach(InAppBillingIDPack idsPack in GameSettings.Instance.allInAppBillingIDS) {
					EditorGUI.indentLevel++;
					
					EditorGUILayout.BeginHorizontal();
					if(!GameSettings.Instance.showInAppBillingIDsPack.ContainsKey(GameSettings.Instance.allInAppBillingIDS[i]))
						GameSettings.Instance.showInAppBillingIDsPack.Add(GameSettings.Instance.allInAppBillingIDS[i], true);
					
					GameSettings.Instance.showInAppBillingIDsPack[GameSettings.Instance.allInAppBillingIDS[i]] = EditorGUILayout.Foldout(GameSettings.Instance.showInAppBillingIDsPack[GameSettings.Instance.allInAppBillingIDS[i]], "InApp Billing IDs for Game Version "+i.ToString());
					
					GUI.color = Color.red;
					if(GUILayout.Button("-",  GUILayout.Width(30))) {
						GameSettings.Instance.allInAppBillingIDS.Remove(idsPack);
						break;
					}
					GUI.color = prevCol;
					EditorGUILayout.EndHorizontal();
					
					inAppBillingForEveryGameMultiversion(i);
					i++;
					EditorGUI.indentLevel--;
				}
				
				
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				GUI.color = Color.green;
				if(GUILayout.Button("New Pack")) {
					if(GameSettings.Instance.allInAppBillingIDS == null || (GameSettings.Instance.allInAppBillingIDS != null && GameSettings.Instance.allInAppBillingIDS.Count == 0))
						GameSettings.Instance.allInAppBillingIDS.Add(new InAppBillingIDPack(0, null));
					else{
						InAppBillingIDPack lastPack = GameSettings.Instance.allInAppBillingIDS[GameSettings.Instance.allInAppBillingIDS.Count-1];
						int lastPackId = lastPack.gameVersion;
						GameSettings.Instance.allInAppBillingIDS.Add(new InAppBillingIDPack(lastPackId+1, null));
					}
				}
				GUI.color = prevCol;
				EditorGUILayout.EndHorizontal();
				
				//			EditorGUILayout.Space();
				EditorGUI.indentLevel--;
			}
		}
	}
	
	protected virtual void inAppBillingForEveryGameMultiversion(int index){
		if((GameSettings.Instance.allInAppBillingIDS != null && GameSettings.Instance.allInAppBillingIDS.Count > 0)){
			Color prevCol = GUI.color;
			
			if(GameSettings.Instance.showInAppBillingIDsPack[GameSettings.Instance.allInAppBillingIDS[index]]){
				EditorGUILayout.BeginVertical(GUI.skin.box);
				
				if(GameSettings.Instance.allInAppBillingIDS[index].ids == null || (GameSettings.Instance.allInAppBillingIDS[index].ids != null && GameSettings.Instance.allInAppBillingIDS[index].ids.Count == 0)) {
					EditorGUILayout.HelpBox("No In App Billing IDs Registred",MessageType.None);
				}
				else{
					EditorGUILayout.Space();
					
					int i = 0;
					foreach(string d in GameSettings.Instance.allInAppBillingIDS[index].ids) {
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("ID "+i.ToString()+":", GUILayout.Width(120));
						GameSettings.Instance.allInAppBillingIDS[index].ids[i] = EditorGUILayout.TextField(GameSettings.Instance.allInAppBillingIDS[index].ids[i]).Trim();
						
						//						GUI.color = Color.red;
						if(GUILayout.Button("-",  GUILayout.Width(30))) {
							GameSettings.Instance.allInAppBillingIDS[index].ids.Remove(d);
							break;
						}
						//						GUI.color = prevCol;
						EditorGUILayout.EndHorizontal();
						i++;
					}
				}
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				GUI.color =  Color.cyan;
				if(GUILayout.Button("+",  GUILayout.Width(60))) {
					GameSettings.Instance.allInAppBillingIDS[index].ids.Add("");
				}
				GUI.color = prevCol;
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
				
				EditorGUILayout.EndVertical();
			}
		}
	}
	
	protected virtual void allAchievementsActions(){
		GameSettings.Instance.showAchievementsActionsSettings = EditorGUILayout.Foldout(GameSettings.Instance.showAchievementsActionsSettings, "Achievement Actions");
		if (GameSettings.Instance.showAchievementsActionsSettings) {	
			
			
			Color prevCol = GUI.color;
			
			if(GameSettings.Instance.achievementsActions.Count == 0) {
				EditorGUILayout.HelpBox("No Achievements Actions Registred",MessageType.None);
			}
			else{
				EditorGUILayout.HelpBox("Achievements Actions for ALL Game Multiversions", MessageType.None);
			}
			
			int i = 0;
			EditorGUI.indentLevel++;
			foreach(GameAction action in GameSettings.Instance.achievementsActions) {
				
				EditorGUILayout.BeginVertical(GUI.skin.box);
				
				if(!GameSettings.Instance.showAchievementsActions.ContainsKey(action))
					GameSettings.Instance.showAchievementsActions.Add(action, true);
				
				GameSettings.Instance.showAchievementsActions[action] = EditorGUILayout.Foldout(GameSettings.Instance.showAchievementsActions[action], "Action"+action.Id);
				if(GameSettings.Instance.showAchievementsActions[action]){
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("ID:", GUILayout.Width(120));
					GameSettings.Instance.achievementsActions[i].Id = EditorGUILayout.TextField(GameSettings.Instance.achievementsActions[i].Id).Trim();
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Initial Value:", GUILayout.Width(120));
					GameSettings.Instance.achievementsActions[i].InitialValue = EditorGUILayout.IntField(GameSettings.Instance.achievementsActions[i].InitialValue);
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Activation Condition:", GUILayout.Width(120));
					GameSettings.Instance.achievementsActions[i].ActivationCondition = (AchieveCondition) EditorGUILayout.EnumPopup(GameSettings.Instance.achievementsActions[i].ActivationCondition);
					EditorGUILayout.EndHorizontal();
					
					if(GameSettings.Instance.achievementsActions[i].ActivationCondition != AchieveCondition.ACTIVE_IF_BETWEEN){
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Activation Value:", GUILayout.Width(120));
						GameSettings.Instance.achievementsActions[i].ActivationValue = EditorGUILayout.IntField(GameSettings.Instance.achievementsActions[i].ActivationValue);
						EditorGUILayout.EndHorizontal();
					}
					else{
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Interval Value:", GUILayout.Width(120));
						EditorGUILayout.EndHorizontal();
						EditorGUI.indentLevel++;
						EditorGUILayout.BeginHorizontal();
						GameSettings.Instance.achievementsActions[i].ActivationInterval.From = EditorGUILayout.IntField("From", GameSettings.Instance.achievementsActions[i].ActivationInterval.From);
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.BeginHorizontal();
						GameSettings.Instance.achievementsActions[i].ActivationInterval.To = EditorGUILayout.IntField("To", GameSettings.Instance.achievementsActions[i].ActivationInterval.To);
						EditorGUILayout.EndHorizontal();
						EditorGUI.indentLevel--;
					}
					
					
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					
					GUI.color = Color.red;
					if(GUILayout.Button("Remove",  GUILayout.Width(80))) {
						GameSettings.Instance.achievementsActions.Remove(action);
						break;
					}
					GUI.color = prevCol;
					
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Space();
				}
				i++;
				EditorGUILayout.EndVertical();
			}
			EditorGUI.indentLevel--;
			
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			GUI.color = Color.green;
			if(GUILayout.Button("New Action")) {
				if(GameSettings.Instance.achievementsActions == null || (GameSettings.Instance.achievementsActions != null && GameSettings.Instance.achievementsActions.Count == 0))
					GameSettings.Instance.achievementsActions.Add(new GameAction("0", AchieveCondition.ACTIVE_IF_EQUALS_TO, 0));
				else{
					GameAction lastPack = GameSettings.Instance.achievementsActions[GameSettings.Instance.achievementsActions.Count-1];
					int lastPackId = 0;
					
					if(int.TryParse(lastPack.Id, out lastPackId))
						GameSettings.Instance.achievementsActions.Add(new GameAction((lastPackId+1).ToString(), lastPack.ActivationCondition ,lastPack.InitialValue));
				}
			}
			GUI.color = prevCol;
			EditorGUILayout.EndHorizontal();			
		}
	}
	
	protected virtual void achievements(){
		GameSettings.Instance.showAchievementsSettings = EditorGUILayout.Foldout(GameSettings.Instance.showAchievementsSettings, "Achievements Settings");
		if (GameSettings.Instance.showAchievementsSettings) {	
			EditorGUI.indentLevel++;
			
			
			//ACTIONS
			allAchievementsActions();
			//			EditorGUILayout.Space();
			
			//ACHIEVEMENTS
			GameSettings.Instance.showAchievementsPacksSettings = EditorGUILayout.Foldout(GameSettings.Instance.showAchievementsPacksSettings, "Achievements Packs");
			if (GameSettings.Instance.showAchievementsPacksSettings) {	
				
				//Toggle button to group achievements or not
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Group Achievements ?", GUILayout.Width(120));
				GameSettings.Instance.groupAchievements = EditorGUILayout.Toggle(GameSettings.Instance.groupAchievements);
				EditorGUILayout.EndHorizontal();
				
				//Prefix for Leaderboard group
				if(GameSettings.Instance.groupAchievements){
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Prefix Group Achievements:", GUILayout.Width(120));
					GameSettings.Instance.prefixAchievementsGroupOnIOS = EditorGUILayout.TextField(GameSettings.Instance.prefixAchievementsGroupOnIOS);
					EditorGUILayout.EndHorizontal();
				}
				
				
				
				//ALL AVIEVEMENTS
				Color prevCol = GUI.color;
				
				if(GameSettings.Instance.achievementsPacks.Count == 0) {
					EditorGUILayout.HelpBox("No Achievements for Game Multiversion Registred",MessageType.None);
				}
				else{
					EditorGUILayout.HelpBox("Achievements by Each Game Multiversion", MessageType.None);
				}
				
				int i = 0;
				EditorGUI.indentLevel++;
				foreach(AchievementsPack pack in GameSettings.Instance.achievementsPacks) {
					EditorGUILayout.BeginHorizontal();
					if(!GameSettings.Instance.showAchievementsPack.ContainsKey(GameSettings.Instance.achievementsPacks[i]))
						GameSettings.Instance.showAchievementsPack.Add(GameSettings.Instance.achievementsPacks[i], true);
					
					string packName = !GameSettings.Instance.groupAchievements ? "Achievements for Game Version "+i.ToString() : "Achievements Grouped";
					GameSettings.Instance.showAchievementsPack[GameSettings.Instance.achievementsPacks[i]] = EditorGUILayout.Foldout(GameSettings.Instance.showAchievementsPack[GameSettings.Instance.achievementsPacks[i]], packName);
					
					GUI.color = Color.red;
					if(GUILayout.Button("-",  GUILayout.Width(30))) {
						GameSettings.Instance.achievementsPacks.Remove(pack);
						break;
					}
					GUI.color = prevCol;
					EditorGUILayout.EndHorizontal();
					
					achievementsForEveryGameMultiversion(i);
					i++;
				}
				EditorGUI.indentLevel--;
				
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				GUI.color = Color.green;
				
				if(!GameSettings.Instance.groupAchievements || (GameSettings.Instance.groupAchievements && GameSettings.Instance.achievementsPacks.Count == 0)){
					if(GUILayout.Button("New Pack")) {
						if(GameSettings.Instance.achievementsPacks == null || (GameSettings.Instance.achievementsPacks != null && GameSettings.Instance.achievementsPacks.Count == 0))
							GameSettings.Instance.achievementsPacks.Add(new AchievementsPack(0, null));
						else{
							AchievementsPack lastPack = GameSettings.Instance.achievementsPacks[GameSettings.Instance.achievementsPacks.Count-1];
							int lastPackId = lastPack.gameVersion;
							GameSettings.Instance.achievementsPacks.Add(new AchievementsPack(lastPackId+1, lastPack.achievements));
						}
					}
				}
				GUI.color = prevCol;
				EditorGUILayout.EndHorizontal();
				
				//			EditorGUILayout.Space();
			}
			EditorGUI.indentLevel--;
		}
	}
	
	protected virtual void achievementsForEveryGameMultiversion(int packIndex){
		if((GameSettings.Instance.achievementsPacks != null && GameSettings.Instance.achievementsPacks.Count > 0)){
			
			Color prevCol = GUI.color;
			
			
			if(GameSettings.Instance.showAchievementsPack[GameSettings.Instance.achievementsPacks[packIndex]]){
				EditorGUILayout.BeginVertical(GUI.skin.box);
				
				if(GameSettings.Instance.achievementsPacks[packIndex].achievements == null || (GameSettings.Instance.achievementsPacks[packIndex].achievements != null && GameSettings.Instance.achievementsPacks[packIndex].achievements.Count == 0)) {
					EditorGUILayout.HelpBox("No Achievements Registred",MessageType.None);
				}
				else{
					EditorGUILayout.Space();
					
					int i = 0;
					
					
					foreach(Achievement achievement in GameSettings.Instance.achievementsPacks[packIndex].achievements) {
						
						
						if(!GameSettings.Instance.showSpecificAchievementsOfAPack.ContainsKey(GameSettings.Instance.achievementsPacks[packIndex].achievements[i]))
							GameSettings.Instance.showSpecificAchievementsOfAPack.Add(GameSettings.Instance.achievementsPacks[packIndex].achievements[i], true);
						
						GameSettings.Instance.showSpecificAchievementsOfAPack[GameSettings.Instance.achievementsPacks[packIndex].achievements[i]] = EditorGUILayout.Foldout(GameSettings.Instance.showSpecificAchievementsOfAPack[GameSettings.Instance.achievementsPacks[packIndex].achievements[i]], "Achievement "+(i+1).ToString());
						if(GameSettings.Instance.showSpecificAchievementsOfAPack[GameSettings.Instance.achievementsPacks[packIndex].achievements[i]]){
							EditorGUILayout.BeginVertical(GUI.skin.box);
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("ID:", GUILayout.Width(120));
							GameSettings.Instance.achievementsPacks[packIndex].achievements[i].Id = EditorGUILayout.TextField(GameSettings.Instance.achievementsPacks[packIndex].achievements[i].Id).Trim();
							EditorGUILayout.EndHorizontal();
							
							
							specificAchievementsActions(packIndex, i);
							
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.Space();
							
							if(GUILayout.Button("Remove Achievement",  GUILayout.Width(180))) {
								GameSettings.Instance.achievementsPacks[packIndex].achievements.Remove(achievement);
								break;
							}
							
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.Space();
							
							
							EditorGUILayout.EndVertical();
						}
						
						i++;
					}
				}
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				GUI.color = Color.cyan;
				if(GUILayout.Button("New Achievement",  GUILayout.Width(120))) {
					GameSettings.Instance.achievementsPacks[packIndex].achievements.Add(new Achievement());
				}
				GUI.color = prevCol;
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
				
				
				EditorGUILayout.EndVertical();
			}
		}
	}
	
	
	
	protected virtual void specificAchievementsActions(int packIndex, int achievementIndex){
		if(GameSettings.Instance.achievementsPacks != null && GameSettings.Instance.achievementsPacks.Count > 0 && GameSettings.Instance.achievementsPacks[packIndex].achievements != null && GameSettings.Instance.achievementsPacks[packIndex].achievements.Count > 0){
			if(!GameSettings.Instance.showAchActionsPack.ContainsKey(GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex]))
				GameSettings.Instance.showAchActionsPack.Add(GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex], true);
			
			GameSettings.Instance.showAchActionsPack[GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex]] = EditorGUILayout.Foldout(GameSettings.Instance.showAchActionsPack[GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex]], "Actions");
			if(GameSettings.Instance.showAchActionsPack[GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex]]){
				//				EditorGUILayout.BeginVertical(GUI.skin.box);
				Color prevCol = GUI.color;
				
				if(!GameSettings.Instance.achPackActionsSelected.ContainsKey(GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex]))
					GameSettings.Instance.achPackActionsSelected.Add(GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex], 0);
				
				bool notHasActions = GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex].Actions == null 
					|| (GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex].Actions != null 
					    && GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex].Actions.Count == 0);
				
				if(notHasActions)
					EditorGUILayout.HelpBox("No Game Actions Registred",MessageType.None);
				
				
				
				if(!notHasActions) {
					EditorGUILayout.Space();
					
					int i = 0;
					
					
					
					
					foreach(GameAction action in GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex].Actions) {
						EditorGUILayout.BeginVertical(GUI.skin.box);
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Action:", GUILayout.Width(120));
						EditorGUILayout.HelpBox("Action: "+action.Id, MessageType.None);
						
						
						
						
						
						if(GUILayout.Button("-",  GUILayout.Width(30))) {
							GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex].Actions.Remove(action);
							break;
						}
						
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.Space();
						i++;
						
						EditorGUILayout.EndVertical();
					}
				}
				
				//----------------------------------
				//ADD Button
				if(GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex].Actions.Count < GameSettings.Instance.achievementsActions.Count){
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("Select",  GUILayout.MinWidth(30));
					
					//popup to select a specific game action
					List<string> options = GameSettings.Instance.AllNotSelectedAchievementActionsNames(packIndex, achievementIndex);
					GameSettings.Instance.achPackActionsSelected[GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex]] = EditorGUILayout.Popup(GameSettings.Instance.achPackActionsSelected[GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex]], options.ToArray(), GUILayout.Width(120));
					
					
					GUI.color = Color.cyan;
					if(GUILayout.Button("Add",  GUILayout.Width(40))) {
						int selectedActionIndex = GameSettings.Instance.achPackActionsSelected[GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex]];
						int correctIndex = getGerenalActionIndexSelectedFromPopup(options, selectedActionIndex);
						
						
						if(correctIndex < GameSettings.Instance.achievementsActions.Count 
						   && !GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex].Actions.Contains(GameSettings.Instance.achievementsActions[correctIndex])
						   && !GameSettings.Instance.containedActionID(GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex].Actions, GameSettings.Instance.achievementsActions[correctIndex])){
							GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex].Actions.Add(GameSettings.Instance.achievementsActions[correctIndex]);
							GameSettings.Instance.achPackActionsSelected[GameSettings.Instance.achievementsPacks[packIndex].achievements[achievementIndex]] = 0;
						}
					}
					GUI.color = prevCol;
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Space();
				}
				else{
					EditorGUILayout.HelpBox("All Game Actions Added", MessageType.Info);
				}
				//----------------------------------
				
				
				
				//				EditorGUILayout.EndVertical();
			}
		}
	}
	
	private int getGerenalActionIndexSelectedFromPopup(List<string> options, int selectedOption){
		int index = 0;
		string selectedActionId = options[selectedOption].Replace(" ", "").Replace("Action", "");
		
		for(int i=0; i<GameSettings.Instance.achievementsActions.Count; i++){
			if(GameSettings.Instance.achievementsActions[i].Id.Equals(selectedActionId)){
				index = i;
				break;
			}
		}
		
		return index;
	}
	
	//	protected virtual void achievementsActions(){
	//		GameSettings.Instance.showAppleAppID = EditorGUILayout.Foldout(GameSettings.Instance.showAppleAppID, "Apple App IDs");
	//		if (GameSettings.Instance.showAppleAppID) {
	//			EditorGUILayout.BeginVertical(GUI.skin.box);
	//			
	//			if(GameSettings.Instance.appleAppIDs.Count == 0) {
	//				EditorGUILayout.HelpBox("No Apple App ID Registred",MessageType.Error);
	//			}
	//			
	//			int i = 0;
	//			foreach(string d in GameSettings.Instance.appleAppIDs) {
	//				EditorGUILayout.BeginHorizontal();
	//				EditorGUILayout.LabelField("ID for Vs"+i.ToString()+":",GUILayout.Width(120));
	//				GameSettings.Instance.appleAppIDs[i] = EditorGUILayout.TextField(GameSettings.Instance.appleAppIDs[i]).Trim();
	//				
	//				
	//				if(GUILayout.Button("-",  GUILayout.Width(30))) {
	//					GameSettings.Instance.appleAppIDs.Remove(d);
	//					break;
	//				}
	//				EditorGUILayout.EndHorizontal();
	//				i++;
	//			}
	//			
	//			
	//			EditorGUILayout.BeginHorizontal();
	//			EditorGUILayout.Space();
	//			if(GUILayout.Button("+",  GUILayout.Width(60))) {
	//				GameSettings.Instance.appleAppIDs.Add("");
	//			}
	//			EditorGUILayout.EndHorizontal();
	//			EditorGUILayout.Space();
	//			
	//			EditorGUILayout.EndVertical();
	//		}
	
	//	}
	
	protected virtual void scoresMultiGameVersion(){
		GameSettings.Instance.showScoresPacksSettings = EditorGUILayout.Foldout(GameSettings.Instance.showScoresPacksSettings, "Scores Packs");
		if (GameSettings.Instance.showScoresPacksSettings) {	
			Color prevCol = GUI.color;
			
			if(GameSettings.Instance.scoresPacks.Count == 0) {
				EditorGUILayout.HelpBox("No Scores for Game Multiversion Registred",MessageType.None);
			}
			else{
				EditorGUILayout.HelpBox("Scores by Each Game Multiversion", MessageType.None);
			}
			
			int i = 0;
			EditorGUI.indentLevel++;
			
			//Toggle button to group leaderboards or not
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Group Scores ?", GUILayout.Width(120));
			GameSettings.Instance.groupScores = EditorGUILayout.Toggle(GameSettings.Instance.groupScores);
			EditorGUILayout.EndHorizontal();
			
			//Prefix for Leaderboard group
			if(GameSettings.Instance.groupScores){
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Prefix Group Scores:", GUILayout.Width(120));
				GameSettings.Instance.prefixScoresGroupOnIOS = EditorGUILayout.TextField(GameSettings.Instance.prefixScoresGroupOnIOS);
				EditorGUILayout.EndHorizontal();
			}
			
			//All LEADERBOARDS
			foreach(ScoresPack pack in GameSettings.Instance.scoresPacks) {
				EditorGUILayout.BeginHorizontal();
				if(!GameSettings.Instance.showScoresPack.ContainsKey(GameSettings.Instance.scoresPacks[i]))
					GameSettings.Instance.showScoresPack.Add(GameSettings.Instance.scoresPacks[i], true);
				
				string packName = !GameSettings.Instance.groupAchievements ? "Scores for Game Version "+i.ToString() : "Scores Grouped";
				GameSettings.Instance.showScoresPack[GameSettings.Instance.scoresPacks[i]] = EditorGUILayout.Foldout(GameSettings.Instance.showScoresPack[GameSettings.Instance.scoresPacks[i]], packName);
				
				GUI.color = Color.red;
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GameSettings.Instance.scoresPacks.Remove(pack);
					break;
				}
				GUI.color = prevCol;
				EditorGUILayout.EndHorizontal();
				
				scoresForEveryGameMultiversion(i);//nei
				i++;
			}
			EditorGUI.indentLevel--;
			
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			GUI.color = Color.green;
			
			if(!GameSettings.Instance.groupScores || (GameSettings.Instance.scoresPacks.Count == 0 && GameSettings.Instance.groupScores)){
				if(GUILayout.Button("New Pack")) {
					if(GameSettings.Instance.scoresPacks == null || (GameSettings.Instance.scoresPacks != null && GameSettings.Instance.scoresPacks.Count == 0))
						GameSettings.Instance.scoresPacks.Add(new ScoresPack(0, null));
					else{
						ScoresPack lastPack = GameSettings.Instance.scoresPacks[GameSettings.Instance.scoresPacks.Count-1];
						int lastPackId = lastPack.gameVersion;
						GameSettings.Instance.scoresPacks.Add(new ScoresPack(lastPackId+1, lastPack.scores));
					}
				}
			}
			GUI.color = prevCol;
			EditorGUILayout.EndHorizontal();
			
			//			EditorGUILayout.Space();
		}
	}
	
	protected virtual void scoresForEveryGameMultiversion(int packIndex){
		if((GameSettings.Instance.scoresPacks != null && GameSettings.Instance.scoresPacks.Count > 0)){
			
			Color prevCol = GUI.color;
			
			
			if(GameSettings.Instance.showScoresPack[GameSettings.Instance.scoresPacks[packIndex]]){
				EditorGUILayout.BeginVertical(GUI.skin.box);
				
				if(GameSettings.Instance.scoresPacks[packIndex].scores == null || (GameSettings.Instance.scoresPacks[packIndex].scores != null && GameSettings.Instance.scoresPacks[packIndex].scores.Count == 0)) {
					EditorGUILayout.HelpBox("No Scores Registred",MessageType.None);
				}
				else{
					EditorGUILayout.Space();
					
					int i = 0;
					
					
					foreach(Score score in GameSettings.Instance.scoresPacks[packIndex].scores) {
						
						
						if(!GameSettings.Instance.showSpecificScoreOfAPack.ContainsKey(GameSettings.Instance.scoresPacks[packIndex].scores[i]))
							GameSettings.Instance.showSpecificScoreOfAPack.Add(GameSettings.Instance.scoresPacks[packIndex].scores[i], true);
						
						GameSettings.Instance.showSpecificScoreOfAPack[GameSettings.Instance.scoresPacks[packIndex].scores[i]] = EditorGUILayout.Foldout(GameSettings.Instance.showSpecificScoreOfAPack[GameSettings.Instance.scoresPacks[packIndex].scores[i]], "Score "+(i+1).ToString());
						if(GameSettings.Instance.showSpecificScoreOfAPack[GameSettings.Instance.scoresPacks[packIndex].scores[i]]){
							EditorGUILayout.BeginVertical(GUI.skin.box);
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Name Loc Key:", GUILayout.Width(120));
							GameSettings.Instance.scoresPacks[packIndex].scores[i].NameLocKey = EditorGUILayout.TextField(GameSettings.Instance.scoresPacks[packIndex].scores[i].NameLocKey).Trim();
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("ID:", GUILayout.Width(120));
							GameSettings.Instance.scoresPacks[packIndex].scores[i].Id = EditorGUILayout.TextField(GameSettings.Instance.scoresPacks[packIndex].scores[i].Id).Trim();
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Format:", GUILayout.Width(120));
							GameSettings.Instance.scoresPacks[packIndex].scores[i].Format = (ScoreFormat) EditorGUILayout.EnumPopup(GameSettings.Instance.scoresPacks[packIndex].scores[i].Format);
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Order:", GUILayout.Width(120));
							GameSettings.Instance.scoresPacks[packIndex].scores[i].Order = (ScoreOrder) EditorGUILayout.EnumPopup(GameSettings.Instance.scoresPacks[packIndex].scores[i].Order);
							EditorGUILayout.EndHorizontal();
							
							//---------------------------------------
							//Greater Limit
							//--------------------------------------
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Greater Limit ?", GUILayout.Width(180));
							GameSettings.Instance.scoresPacks[packIndex].scores[i].UseGreaterLimit = EditorGUILayout.Toggle(GameSettings.Instance.scoresPacks[packIndex].scores[i].UseGreaterLimit);
							EditorGUILayout.EndHorizontal();
							
							if(GameSettings.Instance.scoresPacks[packIndex].scores[i].UseGreaterLimit){
								EditorGUI.indentLevel++;
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Greater Limit:", GUILayout.Width(165));
								GameSettings.Instance.scoresPacks[packIndex].scores[i].GreaterLimit	= EditorGUILayout.LongField(GameSettings.Instance.scoresPacks[packIndex].scores[i].GreaterLimit);
								EditorGUILayout.EndHorizontal();
								EditorGUI.indentLevel--;
							}
							else{
								GameSettings.Instance.scoresPacks[packIndex].scores[i].GreaterLimit = long.MaxValue;
							}
							//---------------------------------------
							//Lower Limit
							//--------------------------------------
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Lower Limit ?", GUILayout.Width(180));
							GameSettings.Instance.scoresPacks[packIndex].scores[i].UseLowerLimit = EditorGUILayout.Toggle(GameSettings.Instance.scoresPacks[packIndex].scores[i].UseLowerLimit);
							EditorGUILayout.EndHorizontal();
							
							if(GameSettings.Instance.scoresPacks[packIndex].scores[i].UseLowerLimit){
								EditorGUI.indentLevel++;
								EditorGUILayout.BeginHorizontal();
								EditorGUILayout.LabelField("Lower Limit:", GUILayout.Width(165));
								GameSettings.Instance.scoresPacks[packIndex].scores[i].LowerLimit	= EditorGUILayout.LongField(GameSettings.Instance.scoresPacks[packIndex].scores[i].LowerLimit);
								EditorGUILayout.EndHorizontal();
								EditorGUI.indentLevel--;
							}
							else{
								GameSettings.Instance.scoresPacks[packIndex].scores[i].LowerLimit = long.MinValue;
							}
							
							
							//---------------------------------------
							//Remove Score
							//--------------------------------------
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.Space();
							
							if(GUILayout.Button("Remove Score",  GUILayout.Width(180))) {
								GameSettings.Instance.scoresPacks[packIndex].scores.Remove(score);
								break;
							}
							
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.Space();
							
							
							EditorGUILayout.EndVertical();
						}
						
						i++;
					}
				}
				
				//---------------------------------------
				//Add New Score
				//--------------------------------------
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				GUI.color = Color.cyan;
				if(GUILayout.Button("New Score",  GUILayout.Width(120))) {
					GameSettings.Instance.scoresPacks[packIndex].scores.Add(new Score());
				}
				GUI.color = prevCol;
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
				
				
				EditorGUILayout.EndVertical();
			}
		}
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
			//			EditorGUILayout.Space();
		}
	}
	
	private void handleWorldLevelRankings(){
		//		GameSettings.Instance.showWorldRankingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showWorldRankingSettings, "Rankings by World Levels");
		//		if (GameSettings.Instance.showWorldRankingSettings) {
		//			if(GameSettings.Instance.worldLevelRankingIDS.Count == 0) {
		//				EditorGUILayout.HelpBox("No World Level Ranking IDs Registred",MessageType.None);
		//			}
		//			
		//			int i = 0;
		//			foreach(string d in GameSettings.Instance.worldLevelRankingIDS) {
		//				EditorGUILayout.BeginHorizontal();
		//				EditorGUILayout.LabelField("Level " + (i+1), GUILayout.Width(60));
		//				GameSettings.Instance.worldLevelRankingIDS[i] = EditorGUILayout.TextField(GameSettings.Instance.worldLevelRankingIDS[i]).Trim();
		//				
		//				
		//				if(GUILayout.Button("-",  GUILayout.Width(30))) {
		//					GameSettings.Instance.worldLevelRankingIDS.Remove(d);
		//					break;
		//				}
		//				EditorGUILayout.EndHorizontal();
		//				i++;
		//			}
		//			
		//			
		//			EditorGUILayout.BeginHorizontal();
		//			EditorGUILayout.Space();
		//			if(GUILayout.Button("+",  GUILayout.Width(60))) {
		//				GameSettings.Instance.worldLevelRankingIDS.Add("");
		//			}
		//			EditorGUILayout.EndHorizontal();
		//			EditorGUILayout.Space();
		//		}
	}
	
	private void handleSurvivalLevelRankings(){
		//		GameSettings.Instance.showSurvivalLevelsRankingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showSurvivalLevelsRankingSettings, "Rankings by Survival Levels");
		//		if (GameSettings.Instance.showSurvivalLevelsRankingSettings) {
		//			if(GameSettings.Instance.survivalLevelRankingIDS.Count == 0) {
		//				EditorGUILayout.HelpBox("No Survival Level Ranking IDs Registred",MessageType.None);
		//			}
		//			
		//			int i = 0;
		//			foreach(string d in GameSettings.Instance.survivalLevelRankingIDS) {
		//				EditorGUILayout.BeginHorizontal();
		//				EditorGUILayout.LabelField("Level " + (i+1), GUILayout.Width(60));
		//				GameSettings.Instance.survivalLevelRankingIDS[i] = EditorGUILayout.TextField(GameSettings.Instance.survivalLevelRankingIDS[i]).Trim();
		//				
		//				
		//				if(GUILayout.Button("-",  GUILayout.Width(30))) {
		//					GameSettings.Instance.survivalLevelRankingIDS.Remove(d);
		//					break;
		//				}
		//				EditorGUILayout.EndHorizontal();
		//				i++;
		//			}
		//			
		//			
		//			EditorGUILayout.BeginHorizontal();
		//			EditorGUILayout.Space();
		//			if(GUILayout.Button("+",  GUILayout.Width(60))) {
		//				GameSettings.Instance.survivalLevelRankingIDS.Add("");
		//			}
		//			EditorGUILayout.EndHorizontal();
		//			EditorGUILayout.Space();
		//		}
	}
	
	protected virtual void moneySettings(){
		EditorGUILayout.HelpBox("Game Settings", MessageType.None);
		GameSettings.Instance.showMoneySettings = EditorGUILayout.Foldout(GameSettings.Instance.showMoneySettings, "Money");
		if (GameSettings.Instance.showMoneySettings) {
			EditorGUILayout.BeginVertical(GUI.skin.box);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Initial Money:", GUILayout.Width(180));
			GameSettings.Instance.INITIAL_MONEY	= EditorGUILayout.LongField(GameSettings.Instance.INITIAL_MONEY);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Use max long value", GUILayout.Width(180));
			GameSettings.Instance.USE_MAX_LONG_VALUE_TO_MAX_MONEY = EditorGUILayout.Toggle(GameSettings.Instance.USE_MAX_LONG_VALUE_TO_MAX_MONEY);
			EditorGUILayout.EndHorizontal();
			
			if(!GameSettings.Instance.USE_MAX_LONG_VALUE_TO_MAX_MONEY){
				EditorGUI.indentLevel++;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Max Money:", GUILayout.Width(165));
				GameSettings.Instance.MAX_MONEY	= EditorGUILayout.LongField(GameSettings.Instance.MAX_MONEY);
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;
			}
			else{
				GameSettings.Instance.MAX_MONEY = long.MaxValue;
			}
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(rewardPercentageLabel, GUILayout.Width(180));
			GameSettings.Instance.PERCENTAGE_MONEY_REWARD_LEVEL_PREV_COMPLETED	= EditorGUILayout.FloatField(GameSettings.Instance.PERCENTAGE_MONEY_REWARD_LEVEL_PREV_COMPLETED);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("1 Gem value in money:", GUILayout.Width(180));
			GameSettings.Instance.ONE_GEM_VALUE_IN_COINS	= EditorGUILayout.IntField(GameSettings.Instance.ONE_GEM_VALUE_IN_COINS);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Initial Gems:", GUILayout.Width(180));
			GameSettings.Instance.INITIAL_GEMS	= EditorGUILayout.LongField(GameSettings.Instance.INITIAL_GEMS);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Use max long value", GUILayout.Width(180));
			GameSettings.Instance.USE_MAX_LONG_VALUE_TO_MAX_GEMS = EditorGUILayout.Toggle(GameSettings.Instance.USE_MAX_LONG_VALUE_TO_MAX_GEMS);
			EditorGUILayout.EndHorizontal();
			
			if(!GameSettings.Instance.USE_MAX_LONG_VALUE_TO_MAX_GEMS){
				EditorGUI.indentLevel++;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Max Gems:", GUILayout.Width(165));
				GameSettings.Instance.MAX_GEMS	= EditorGUILayout.LongField(GameSettings.Instance.MAX_GEMS);
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;
			}
			else{
				GameSettings.Instance.MAX_GEMS = long.MaxValue;
			}
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("% Gems Reward", GUILayout.Width(180));
			GameSettings.Instance.PERCENTAGE_GEMS_REWARD_LEVEL_PREV_COMPLETED	= EditorGUILayout.FloatField(GameSettings.Instance.PERCENTAGE_GEMS_REWARD_LEVEL_PREV_COMPLETED);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.EndVertical();
		}
	}
	
	protected virtual void adsSettings(){
		GameSettings.Instance.showAdsSettings = EditorGUILayout.Foldout(GameSettings.Instance.showAdsSettings, "Ads Showing Settings");
		if (GameSettings.Instance.showAdsSettings) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(proVersionLabel);
			GameSettings.Instance.IS_PRO_VERSION	= EditorGUILayout.Toggle(GameSettings.Instance.IS_PRO_VERSION);
			EditorGUILayout.EndHorizontal();
			
			if(!GameSettings.Instance.IS_PRO_VERSION){
				EditorGUILayout.BeginVertical(GUI.skin.box);
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("HeyZap ID");
				GameSettings.Instance.heyZapID	= EditorGUILayout.TextField(GameSettings.Instance.heyZapID);
				EditorGUILayout.EndHorizontal();
				
				
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(whenShowInitialAd);
				GameSettings.Instance.TIMES_START_GAME_TO_SHOW_AD_AT_START	= EditorGUILayout.IntField(GameSettings.Instance.TIMES_START_GAME_TO_SHOW_AD_AT_START);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Video % for Random Showing (max 100)");
				GameSettings.Instance.videoPercentageInRandomShow = EditorGUILayout.IntField(GameSettings.Instance.videoPercentageInRandomShow);
				EditorGUILayout.EndHorizontal();
				
				// EditorGUILayout.BeginHorizontal();
				// EditorGUILayout.LabelField("Ad Type during Gameplay");
				// GameSettings.Instance.adTypeDuringGamePlay	=  (AdType) EditorGUILayout.EnumPopup(GameSettings.Instance.adTypeDuringGamePlay);
				// EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(secDuringGP);
				GameSettings.Instance.SECONDS_DURING_GAME_PLAYING_SHOW_AD	= EditorGUILayout.IntField(GameSettings.Instance.SECONDS_DURING_GAME_PLAYING_SHOW_AD);
				EditorGUILayout.EndHorizontal();
				
				EditorGUI.indentLevel++;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(notifyAdDuringGP);
				GameSettings.Instance.NOTIFY_AD_DURING_GAMEPLAY_WILL_BE_SHOWN_IN_NEXT_SECONDS	= EditorGUILayout.IntField(GameSettings.Instance.NOTIFY_AD_DURING_GAMEPLAY_WILL_BE_SHOWN_IN_NEXT_SECONDS);
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Default num GameOvers:");
				GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_BY_DEFAULT	= EditorGUILayout.IntField(GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_BY_DEFAULT);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Default num Wins:");
				GameSettings.Instance.NUM_WINS_SHOW_AD_BY_DEFAULT	= EditorGUILayout.IntField(GameSettings.Instance.NUM_WINS_SHOW_AD_BY_DEFAULT);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Survival mode num GameOvers:");
				GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_SURVIVAL_MODE	= EditorGUILayout.IntField(GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_SURVIVAL_MODE);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Survival mode num Wins:");
				GameSettings.Instance.NUM_WINS_SHOW_AD_SURVIVAL_MODE	= EditorGUILayout.IntField(GameSettings.Instance.NUM_WINS_SHOW_AD_SURVIVAL_MODE);
				EditorGUILayout.EndHorizontal();
				
				//				EditorGUILayout.Space();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Easy mode num GameOvers:");
				GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_EASY_MODE	= EditorGUILayout.IntField(GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_EASY_MODE);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Easy mode num Wins:");
				GameSettings.Instance.NUM_WINS_SHOW_AD_EASY_MODE	= EditorGUILayout.IntField(GameSettings.Instance.NUM_WINS_SHOW_AD_EASY_MODE);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Normal mode num GameOvers:");
				GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_NORMAL_MODE	= EditorGUILayout.IntField(GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_NORMAL_MODE);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Normal mode num Wins:");
				GameSettings.Instance.NUM_WINS_SHOW_AD_NORMAL_MODE	= EditorGUILayout.IntField(GameSettings.Instance.NUM_WINS_SHOW_AD_NORMAL_MODE);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Hard mode num GameOvers:");
				GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_HARD_MODE	= EditorGUILayout.IntField(GameSettings.Instance.NUM_GAMEOVERS_SHOW_AD_HARD_MODE);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Hard mode num Wins:");
				GameSettings.Instance.NUM_WINS_SHOW_AD_HARD_MODE	= EditorGUILayout.IntField(GameSettings.Instance.NUM_WINS_SHOW_AD_HARD_MODE);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.EndVertical();
			}
			
		}
	}
	
	
	
	protected virtual  void socialNetworksSettings(){
		GameSettings.Instance.showSocialNetworksSettings = EditorGUILayout.Foldout(GameSettings.Instance.showSocialNetworksSettings, "Social Networks");
		if (GameSettings.Instance.showSocialNetworksSettings) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(buildForAmazon);
			GameSettings.Instance.BUILD_FOR_AMAZON	= EditorGUILayout.Toggle(GameSettings.Instance.BUILD_FOR_AMAZON);
			EditorGUILayout.EndHorizontal();
			
			EditorGUI.indentLevel++;
			googlePlayServices();
			gamecenterSettings();
			EditorGUI.indentLevel--;
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Facebook", GUILayout.Width(80));
			GameSettings.Instance.USE_FACEBOOK	= EditorGUILayout.Toggle(GameSettings.Instance.USE_FACEBOOK);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Twitter", GUILayout.Width(80));
			GameSettings.Instance.USE_TWITTER	= EditorGUILayout.Toggle(GameSettings.Instance.USE_TWITTER);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("#Hashtag:");
			GUIStyle style = new GUIStyle(EditorStyles.label);
			
			Color prev = GUI.color;
			GUI.color = Color.blue;
			string hashtag = "#"+GameSettings.Instance.CurrentGameName.Replace(" ", "");
			GameSettings.Instance.HASHTAG = hashtag;
			EditorGUILayout.LabelField(hashtag, EditorStyles.whiteLabel);
			
			//			GameSettings.Instance.HASHTAG = EditorGUILayout.TextField(GameSettings.Instance.HASHTAG).Trim();
			GUI.color = prev;
			EditorGUILayout.EndHorizontal();
		}
	}
	
	protected virtual void googlePlayServices(){
		GameSettings.Instance.showGooglePlayServicesSettings = EditorGUILayout.Foldout(GameSettings.Instance.showGooglePlayServicesSettings, "Google Play Services");
		if (GameSettings.Instance.showGooglePlayServicesSettings) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Enabled", GUILayout.Width(80));
			GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES	= EditorGUILayout.Toggle(GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES);
			EditorGUILayout.EndHorizontal();
			
			if(GameSettings.Instance.USE_GOOGLE_PLAY_SERVICES){
				EditorGUILayout.BeginVertical(GUI.skin.box);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Show login window first opening");
				GameSettings.Instance.SHOW_LOGIN_GOOGLE_PLAY_SERVICES_THE_FIRST_OPENING	= EditorGUILayout.Toggle(GameSettings.Instance.SHOW_LOGIN_GOOGLE_PLAY_SERVICES_THE_FIRST_OPENING, GUILayout.Width(120));
				EditorGUILayout.EndHorizontal();
				
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Game openings to show login window");
				GameSettings.Instance.NUM_GAME_OPENING_TO_INIT_GOOLGE_PLAY_SERVICES	= EditorGUILayout.IntField(GameSettings.Instance.NUM_GAME_OPENING_TO_INIT_GOOLGE_PLAY_SERVICES, GUILayout.Width(120));
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
		}
	}
	
	protected virtual void gamecenterSettings(){
		GameSettings.Instance.showGameCenterSettings = EditorGUILayout.Foldout(GameSettings.Instance.showGameCenterSettings, "Game Center");
		if (GameSettings.Instance.showGameCenterSettings) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Enabled", GUILayout.Width(80));
			GameSettings.Instance.USE_GAMECENTER	= EditorGUILayout.Toggle(GameSettings.Instance.USE_GAMECENTER);
			EditorGUILayout.EndHorizontal();
			
			if(GameSettings.Instance.USE_GAMECENTER){
				EditorGUILayout.BeginVertical(GUI.skin.box);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Show login window first opening");
				GameSettings.Instance.SHOW_LOGIN_GAME_CENTER_THE_FIRST_OPENING	= EditorGUILayout.Toggle(GameSettings.Instance.SHOW_LOGIN_GAME_CENTER_THE_FIRST_OPENING, GUILayout.Width(120));
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Game openings to show login window");
				GameSettings.Instance.NUM_GAME_OPENING_TO_INIT_GAME_CENTER	= EditorGUILayout.IntField(GameSettings.Instance.NUM_GAME_OPENING_TO_INIT_GAME_CENTER, GUILayout.Width(120));
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
		}
	}
	
	protected virtual void localizations(){
		GameSettings.Instance.showLocalizations = EditorGUILayout.Foldout(GameSettings.Instance.showLocalizations, "Localizations");
		if (GameSettings.Instance.showLocalizations) {
			EditorGUI.indentLevel++;
			
			EditorGUILayout.BeginVertical(GUI.skin.box);
			
			if(GameSettings.Instance.localizations.Count == 0) {
				EditorGUILayout.HelpBox("No Localizations Registred",MessageType.Warning);
			}
			
			int i = 0;
			foreach(SystemLanguage d in GameSettings.Instance.localizations) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Language:",GUILayout.Width(120));
				GameSettings.Instance.localizations[i] = (SystemLanguage) EditorGUILayout.EnumPopup(GameSettings.Instance.localizations[i]);
				
				
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GameSettings.Instance.localizations.Remove(d);
					break;
				}
				EditorGUILayout.EndHorizontal();
				i++;
			}
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("+",  GUILayout.Width(60))) {
				GameSettings.Instance.localizations.Add(SystemLanguage.English);
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			
			EditorGUILayout.EndVertical();
			
			EditorGUI.indentLevel--;
		}
	}
	
	protected virtual  void appLinks(){
		GameSettings.Instance.showAppLinksSettings = EditorGUILayout.Foldout(GameSettings.Instance.showAppLinksSettings, "App Links");
		if (GameSettings.Instance.showAppLinksSettings) {
			EditorGUI.indentLevel++;
			//			EditorGUILayout.BeginHorizontal();
			//			EditorGUILayout.LabelField("Android Link path:");
			//			GameSettings.Instance.LINK_ANDROID_APP	= EditorGUILayout.TextField(GameSettings.Instance.LINK_ANDROID_APP).Trim();
			//			EditorGUILayout.EndHorizontal();
			
			//			EditorGUILayout.BeginHorizontal();
			//			EditorGUILayout.LabelField("Android Short Link:");
			//			GameSettings.Instance.SHORT_LINK_ANDROID_APP	= EditorGUILayout.TextField(GameSettings.Instance.SHORT_LINK_ANDROID_APP).Trim();
			//			EditorGUILayout.EndHorizontal();
			
			androidLinks();
			
			//			EditorGUILayout.BeginHorizontal();
			//			EditorGUILayout.LabelField("iOS Link:");
			//			GameSettings.Instance.LINK_IOS_APP	= EditorGUILayout.TextField(GameSettings.Instance.LINK_IOS_APP).Trim();
			//			EditorGUILayout.EndHorizontal();
			
			iOSLinks();
			
			//			EditorGUILayout.BeginHorizontal();
			//			EditorGUILayout.LabelField("iOS Short Link:");
			//			GameSettings.Instance.SHORT_LINK_IOS_APP	= EditorGUILayout.TextField(GameSettings.Instance.SHORT_LINK_IOS_APP).Trim();
			//			EditorGUILayout.EndHorizontal();
			
			//			EditorGUILayout.BeginHorizontal();
			//			EditorGUILayout.LabelField("Amazon Link:");
			//			GameSettings.Instance.LINK_AMAZON_APP	= EditorGUILayout.TextField(GameSettings.Instance.LINK_AMAZON_APP).Trim();
			//			EditorGUILayout.EndHorizontal();
			
			//			EditorGUILayout.BeginHorizontal();
			//			EditorGUILayout.LabelField("Amazon Short Link:");
			//			GameSettings.Instance.SHORT_LINK_AMAZON_APP	= EditorGUILayout.TextField(GameSettings.Instance.SHORT_LINK_AMAZON_APP).Trim();
			//			EditorGUILayout.EndHorizontal();
			
			gameLogoLinks();
			
			EditorGUI.indentLevel--;
		}
	}
	
	protected virtual void androidLinks(){
		GameSettings.Instance.showAndroidLinks = EditorGUILayout.Foldout(GameSettings.Instance.showAndroidLinks, "Android Short Links");
		if (GameSettings.Instance.showAndroidLinks) {
			EditorGUILayout.BeginVertical(GUI.skin.box);
			
			if(GameSettings.Instance.androidShortLinks.Count == 0) {
				EditorGUILayout.HelpBox("No Android Short Link Registred",MessageType.Warning);
			}
			
			int i = 0;
			foreach(string d in GameSettings.Instance.androidShortLinks) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Link for Vs"+i.ToString()+":",GUILayout.Width(120));
				GameSettings.Instance.androidShortLinks[i] = EditorGUILayout.TextField(GameSettings.Instance.androidShortLinks[i]).Trim();
				
				
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GameSettings.Instance.androidShortLinks.Remove(d);
					break;
				}
				EditorGUILayout.EndHorizontal();
				i++;
			}
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("+",  GUILayout.Width(60))) {
				GameSettings.Instance.androidShortLinks.Add("");
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			
			EditorGUILayout.EndVertical();
		}
		//		EditorGUILayout.Space();
	}
	
	protected virtual void iOSLinks(){
		GameSettings.Instance.showiOSLinks = EditorGUILayout.Foldout(GameSettings.Instance.showiOSLinks, "iOS Short Links");
		if (GameSettings.Instance.showiOSLinks) {
			EditorGUILayout.BeginVertical(GUI.skin.box);
			
			if(GameSettings.Instance.iOSShortLinks.Count == 0) {
				EditorGUILayout.HelpBox("No iOS Short Link Registred",MessageType.Warning);
			}
			
			int i = 0;
			foreach(string d in GameSettings.Instance.iOSShortLinks) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Link for Vs"+i.ToString()+":",GUILayout.Width(120));
				GameSettings.Instance.iOSShortLinks[i] = EditorGUILayout.TextField(GameSettings.Instance.iOSShortLinks[i]).Trim();
				
				
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GameSettings.Instance.iOSShortLinks.Remove(d);
					break;
				}
				EditorGUILayout.EndHorizontal();
				i++;
			}
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("+",  GUILayout.Width(60))) {
				GameSettings.Instance.iOSShortLinks.Add("");
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			
			EditorGUILayout.EndVertical();
		}
		//		EditorGUILayout.Space();
	}
	
	protected virtual void gameLogoLinks(){
		GameSettings.Instance.showLogoLinksIDS = EditorGUILayout.Foldout(GameSettings.Instance.showLogoLinksIDS, "Game Logo Links");
		if (GameSettings.Instance.showLogoLinksIDS) {
			EditorGUILayout.BeginVertical(GUI.skin.box);
			
			if(GameSettings.Instance.logoLinks.Count == 0) {
				EditorGUILayout.HelpBox("No Game Logo Link Registred",MessageType.Info);
			}
			
			int i = 0;
			foreach(string d in GameSettings.Instance.logoLinks) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Link for Vs"+i.ToString()+":",GUILayout.Width(120));
				GameSettings.Instance.logoLinks[i] = EditorGUILayout.TextField(GameSettings.Instance.logoLinks[i]).Trim();
				
				
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GameSettings.Instance.logoLinks.Remove(d);
					break;
				}
				EditorGUILayout.EndHorizontal();
				i++;
			}
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("+",  GUILayout.Width(60))) {
				GameSettings.Instance.logoLinks.Add("");
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			
			EditorGUILayout.EndVertical();
		}
		//		EditorGUILayout.Space();
	}
	
	protected virtual void rankingIdsSettings(){
		GameSettings.Instance.showRankingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showRankingSettings, rankingIDs);
		if (GameSettings.Instance.showRankingSettings) {
			EditorGUI.indentLevel++;
			uniqueRankingIDs();
			uniquesurvivalRankingIDs();
			scoresMultiGameVersion();
			
			//			EditorGUI.indentLevel++;
			//			GameSettings.Instance.showDifficultiesRankingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showDifficultiesRankingSettings, "Rankings by Difficulty");
			//			if (GameSettings.Instance.showDifficultiesRankingSettings) {
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Easy Mode Ranking");
			//				GameSettings.Instance.ID_RANKING_EASY	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_EASY).Trim();
			//				EditorGUILayout.EndHorizontal();
			//				
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Normal Mode Ranking");
			//				GameSettings.Instance.ID_RANKING_NORMAL	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_NORMAL).Trim();
			//				EditorGUILayout.EndHorizontal();
			//				
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Hard Mode Ranking");
			//				GameSettings.Instance.ID_RANKING_HARD	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_HARD).Trim();
			//				EditorGUILayout.EndHorizontal();
			//				
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("PRO Mode Ranking");
			//				GameSettings.Instance.ID_RANKING_PRO	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_PRO).Trim();
			//				EditorGUILayout.EndHorizontal();
			//				
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("God Mode Ranking");
			//				GameSettings.Instance.ID_RANKING_GOD	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_GOD).Trim();
			//				EditorGUILayout.EndHorizontal();
			//				
			//				EditorGUILayout.BeginHorizontal();
			//				EditorGUILayout.LabelField("Impossible Ranking");
			//				GameSettings.Instance.ID_RANKING_IMPOSSIBLE	= EditorGUILayout.TextField(GameSettings.Instance.ID_RANKING_IMPOSSIBLE).Trim();
			//				EditorGUILayout.EndHorizontal();
			//			}
			//			
			//			handleWorldLevelRankings();
			//			handleSurvivalLevelRankings();
			//			EditorGUI.indentLevel--;
			EditorGUI.indentLevel--;
		}
	}
	
	
	
	protected virtual void uniqueRankingIDs(){
		GameSettings.Instance.showUniqueRankingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showUniqueRankingSettings, uniqueRankingIDsContent);
		if (GameSettings.Instance.showUniqueRankingSettings) {
			EditorGUILayout.BeginVertical(GUI.skin.box);
			
			if(GameSettings.Instance.uniqueRankingIDS.Count == 0) {
				EditorGUILayout.HelpBox("No Unique Ranking ID Registred",MessageType.Info);
			}
			
			int i = 0;
			foreach(string d in GameSettings.Instance.uniqueRankingIDS) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("ID for Vs"+i.ToString()+":",GUILayout.Width(120));
				GameSettings.Instance.uniqueRankingIDS[i] = EditorGUILayout.TextField(GameSettings.Instance.uniqueRankingIDS[i]).Trim();
				
				
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GameSettings.Instance.uniqueRankingIDS.Remove(d);
					break;
				}
				EditorGUILayout.EndHorizontal();
				i++;
			}
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("+",  GUILayout.Width(60))) {
				GameSettings.Instance.uniqueRankingIDS.Add("");
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.Space();
	}
	
	protected virtual void uniquesurvivalRankingIDs(){
		GameSettings.Instance.showUniqueSurvivalRankingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showUniqueSurvivalRankingSettings, uniqueSurvivalRankingIDsContent);
		if (GameSettings.Instance.showUniqueSurvivalRankingSettings) {
			EditorGUILayout.BeginVertical(GUI.skin.box);
			
			if(GameSettings.Instance.uniqueSurvivalRankingIDS.Count == 0) {
				EditorGUILayout.HelpBox("No Unique Survival Ranking ID Registred",MessageType.Info);
			}
			
			int i = 0;
			foreach(string d in GameSettings.Instance.uniqueSurvivalRankingIDS) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("ID for Vs"+i.ToString()+":",GUILayout.Width(120));
				GameSettings.Instance.uniqueSurvivalRankingIDS[i] = EditorGUILayout.TextField(GameSettings.Instance.uniqueSurvivalRankingIDS[i]).Trim();
				
				
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GameSettings.Instance.uniqueSurvivalRankingIDS.Remove(d);
					break;
				}
				EditorGUILayout.EndHorizontal();
				i++;
			}
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("+",  GUILayout.Width(60))) {
				GameSettings.Instance.uniqueSurvivalRankingIDS.Add("");
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.Space();
	}
	
	protected virtual void survivalLevelRankingIDs(){
		GameSettings.Instance.showSurvivalLevelsRankingSettings = EditorGUILayout.Foldout(GameSettings.Instance.showSurvivalLevelsRankingSettings, uniqueSurvivalRankingIDsContent);
		if (GameSettings.Instance.showSurvivalLevelsRankingSettings) {
			EditorGUILayout.BeginVertical(GUI.skin.box);
			
			if(GameSettings.Instance.uniqueSurvivalRankingIDS.Count == 0) {
				EditorGUILayout.HelpBox("No Unique Survival Ranking ID Registred",MessageType.Info);
			}
			
			int i = 0;
			foreach(string d in GameSettings.Instance.uniqueSurvivalRankingIDS) {
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("ID for Vs"+i.ToString()+":",GUILayout.Width(120));
				GameSettings.Instance.uniqueSurvivalRankingIDS[i] = EditorGUILayout.TextField(GameSettings.Instance.uniqueSurvivalRankingIDS[i]).Trim();
				
				
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GameSettings.Instance.uniqueSurvivalRankingIDS.Remove(d);
					break;
				}
				EditorGUILayout.EndHorizontal();
				i++;
			}
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("+",  GUILayout.Width(60))) {
				GameSettings.Instance.uniqueSurvivalRankingIDS.Add("");
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.Space();
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
		
		
		SelectableLabelField(SupportEmail, "garmodev@gmail.com");
		
		
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
