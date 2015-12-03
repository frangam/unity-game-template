/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GTBuildSettingsConfig))]
public class BuildSettingsEditor : Editor {
	private GTBuildSettingsConfig settings;
	
	//--------------------------------------
	// Unity
	//--------------------------------------
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
		
		
		settings = target as GTBuildSettingsConfig;
		
		GUI.changed = false;
		
		createFolders();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		showSharedResources();
		EditorGUILayout.Space();
		showBuildSettings();
		EditorGUILayout.Space();
		
		
		if(GUI.changed) {
			DirtyEditor();
		}
	}
	
	private void createFolders(){
		string path = GTBuildSettingsConfig.Instance.resourcesBeforeBuildPath;
		//---
		//Pre build resources folder
		//---
		bool preBuildResFolder = !string.IsNullOrEmpty(GTBuildSettingsConfig.Instance.resourcesBeforeBuildPath);
		if(!preBuildResFolder){
			Debug.LogError("Not Selected Pre Build Resources Folder");
			
			//			GTBuildSettingsConfig.Instance.resourcesBeforeBuildPath = "Assets/TempPB";
		}
		else if(!System.IO.Directory.Exists(path)){
			Color prev = GUI.color;
			GUI.color = Color.yellow;
			if(GUILayout.Button("Create Pre Build Resources Folder")){
				System.IO.Directory.CreateDirectory(path);
				AssetDatabase.Refresh();
			}
			GUI.color = prev;
		}
		
		int i=0;
		foreach(GTBuildSettingsPack pack in GTBuildSettingsConfig.Instance.packs){
			//---
			//Android Build Folder
			//---
			bool andBuildFolder = !string.IsNullOrEmpty(pack.build.androidBuildFolder);
			if(!preBuildResFolder){
				Debug.LogError("Not Selected Android Build Folder");
			}
			else{
				path = pack.build.androidBuildFolder;
				path = path.Replace(Application.dataPath.Replace("Assets", ""), "");
				
				if(!System.IO.Directory.Exists(path)){
					Color prev = GUI.color;
					GUI.color = Color.yellow;
					if(GUILayout.Button("Create Android Build " + i.ToString() + " Folder")){
						System.IO.Directory.CreateDirectory(path);
						AssetDatabase.Refresh();
					}
					GUI.color = prev;
				}
			}
			
			//---
			//iOS Build Folder
			//---
			bool iosBuildFolder = !string.IsNullOrEmpty(pack.build.androidBuildFolder);
			if(!preBuildResFolder){
				Debug.LogError("Not Selected iOS Build Folder");
			}
			else{
				path = pack.build.iosBuildFolder;
				path = path.Replace(Application.dataPath.Replace("Assets", ""), "");
				
				if(!System.IO.Directory.Exists(path)){
					Color prev = GUI.color;
					GUI.color = Color.yellow;
					if(GUILayout.Button("Create iOS Build " + i.ToString() + " Folder")){
						System.IO.Directory.CreateDirectory(path);
						AssetDatabase.Refresh();
					}
					GUI.color = prev;
				}
			}
			
			
			
			
			i++;
		}
	}
	
	
	protected virtual void showBuildSettings(){
		GTBuildSettingsConfig.Instance.showSettings = EditorGUILayout.Foldout(GTBuildSettingsConfig.Instance.showSettings, "Build Settings");
		if (GTBuildSettingsConfig.Instance.showSettings) {			
			Color prevCol = GUI.color;
			
			
			if(GTBuildSettingsConfig.Instance.packs == null || (GTBuildSettingsConfig.Instance.packs != null && GTBuildSettingsConfig.Instance.packs.Count == 0)) {
				EditorGUILayout.HelpBox("No Build Settings Packs for Game Multiversion Registred",MessageType.None);
			}
			else{
				EditorGUILayout.HelpBox("Build Settings for every Game Multiversion", MessageType.None);
			}
			
			int i = 0;
			foreach(GTBuildSettingsPack pack in GTBuildSettingsConfig.Instance.packs) {
				EditorGUI.indentLevel++;
				
				EditorGUILayout.BeginHorizontal();
				if(!GTBuildSettingsConfig.Instance.showBuildPackSettings.ContainsKey(GTBuildSettingsConfig.Instance.packs[i]))
					GTBuildSettingsConfig.Instance.showBuildPackSettings.Add(GTBuildSettingsConfig.Instance.packs[i], false);
				
				GTBuildSettingsConfig.Instance.showBuildPackSettings[GTBuildSettingsConfig.Instance.packs[i]] = EditorGUILayout.Foldout(GTBuildSettingsConfig.Instance.showBuildPackSettings[GTBuildSettingsConfig.Instance.packs[i]], "Build Settings for Game Version "+i.ToString());
				
				GUI.color = Color.red;
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GTBuildSettingsConfig.Instance.packs.Remove(pack);
					break;
				}
				GUI.color = prevCol;
				EditorGUILayout.EndHorizontal();
				
				buildSettingsForEveryGameMultiversion(i);
				i++;
				EditorGUI.indentLevel--;
			}
			
			
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			GUI.color = Color.green;
			if(GUILayout.Button("New Pack")) {
				if(GTBuildSettingsConfig.Instance.packs == null || (GTBuildSettingsConfig.Instance.packs != null && GTBuildSettingsConfig.Instance.packs.Count == 0))
					GTBuildSettingsConfig.Instance.packs.Add(new GTBuildSettingsPack(0, null));
				else{
					GTBuildSettingsPack lastPack = GTBuildSettingsConfig.Instance.packs[GTBuildSettingsConfig.Instance.packs.Count-1];
					int lastPackId = lastPack.gameVersion;
					GTBuildSettingsConfig.Instance.packs.Add(new GTBuildSettingsPack(lastPackId+1, lastPack.Build));
				}
			}
			GUI.color = prevCol;
			EditorGUILayout.EndHorizontal();
			
			//			EditorGUILayout.Space();
			EditorGUI.indentLevel--;
		}
	}
	
	
	
	protected virtual void buildSettingsForEveryGameMultiversion(int packIndex){
		if((GTBuildSettingsConfig.Instance.packs != null && GTBuildSettingsConfig.Instance.packs.Count > 0)){
			Color prevCol = GUI.color;
			
			if(GTBuildSettingsConfig.Instance.showBuildPackSettings[GTBuildSettingsConfig.Instance.packs[packIndex]]){
				EditorGUILayout.BeginVertical(GUI.skin.box);
				
				if(GTBuildSettingsConfig.Instance.packs[packIndex].build == null) {
					EditorGUILayout.HelpBox("No Build Settings Registred",MessageType.None);
				}
				else{
					EditorGUILayout.Space();
					
					showMainFeatures(packIndex);
					EditorGUILayout.Space();
					showFoldersFeatures(packIndex);
					EditorGUILayout.Space();
					
					
					//---------------------------------------
					//Lists
					//--------------------------------------
					showSpecificScenes(packIndex);
					EditorGUILayout.Space();
					showSpecificAndroidIcons(packIndex);
					EditorGUILayout.Space();
					showSpecificIOSIcons(packIndex);
					EditorGUILayout.Space();
					showSpecificResIncludes(packIndex);
				}
				EditorGUILayout.EndVertical();
			}
		}
	}
	
	private void showMainFeatures(int packIndex){
		if(!GTBuildSettingsConfig.Instance.showSpecificMainFeaturesFromBuildPack.ContainsKey(GTBuildSettingsConfig.Instance.packs[packIndex]))
			GTBuildSettingsConfig.Instance.showSpecificMainFeaturesFromBuildPack.Add(GTBuildSettingsConfig.Instance.packs[packIndex], false);
		
		
		
		
		EditorGUILayout.BeginVertical(GUI.skin.box);
		GTBuildSettingsConfig.Instance.showSpecificMainFeaturesFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]] = EditorGUILayout.Foldout(GTBuildSettingsConfig.Instance.showSpecificMainFeaturesFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]], "Main Features");
		if(GTBuildSettingsConfig.Instance.showSpecificMainFeaturesFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]]){
			//---------------------------------------
			// Company Name
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Company Name:",GUILayout.Width(130));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.companyName = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.companyName);
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Android Game Name
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Android Game Name:",GUILayout.Width(130));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.androidGameName = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.androidGameName);
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// iOS Game Name
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("iOS Game Name:",GUILayout.Width(130));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.iOSGameName = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.iOSGameName);
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// WP Game Name
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("WP Game Name:",GUILayout.Width(130));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.wpGameName = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.wpGameName);
			EditorGUILayout.EndHorizontal();
			
			//--------------------------------------
			// Cloud Project ID
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Cloud Project ID:",GUILayout.Width(130));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.cloudProjectID = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.cloudProjectID);
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Bundle Identifier
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Bundle Identifier:",GUILayout.Width(130));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.bundleIdentifier = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.bundleIdentifier).Trim();
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Android Bundle Version
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Android Bundle Version:",GUILayout.Width(180));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.androidBundleVersion = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.androidBundleVersion).Trim();
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Android Bundle Version Code
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Android Bundle Version Code:",GUILayout.Width(180));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.androidBundleVersionCode = EditorGUILayout.IntField(GTBuildSettingsConfig.Instance.packs[packIndex].build.androidBundleVersionCode);
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// iOS Bundle Version
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("iOS Bundle Version:",GUILayout.Width(180));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.iOSBundleVersion = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.iOSBundleVersion).Trim();
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Google Play Services ID
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Google Play Services ID:",GUILayout.Width(180));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.googlePlayServicesID = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.googlePlayServicesID).Trim();
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Apple ID
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Apple ID:",GUILayout.Width(180));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.appleID = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.appleID).Trim();
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Android Billing Base 64 key
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Android Base64 Key:",GUILayout.Width(180));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.androidBillingBase64Key = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.androidBillingBase64Key).Trim();
			EditorGUILayout.EndHorizontal();
			
			
			//---------------------------------------
			// Android Keystore settings
			//--------------------------------------
			showAndroidKeystoreFeatures(packIndex);
		}
		EditorGUILayout.EndVertical();
	}
	
	private void showAndroidKeystoreFeatures(int packIndex){
		if(!GTBuildSettingsConfig.Instance.showKeyStoreSettingsFromBuildPack.ContainsKey(GTBuildSettingsConfig.Instance.packs[packIndex]))
			GTBuildSettingsConfig.Instance.showKeyStoreSettingsFromBuildPack.Add(GTBuildSettingsConfig.Instance.packs[packIndex], false);
		
		EditorGUILayout.BeginVertical(GUI.skin.box);
		GTBuildSettingsConfig.Instance.showKeyStoreSettingsFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]] = EditorGUILayout.Foldout(GTBuildSettingsConfig.Instance.showKeyStoreSettingsFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]], "Android Keystore");
		if(GTBuildSettingsConfig.Instance.showKeyStoreSettingsFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]]){
			//---------------------------------------
			// Keystore path name
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Keystore Path",GUILayout.Width(100));
			EditorGUILayout.SelectableLabel(GTBuildSettingsConfig.Instance.packs[packIndex].build.androidKeystoreNamePath);
			
			if(GUILayout.Button("Browse",  GUILayout.Width(50))) {
				string initialPathForBrowsing = System.Environment.GetEnvironmentVariable("HOME");
				string preSelected = GTBuildSettingsConfig.Instance.packs[packIndex].build.androidKeystoreNamePath;
				
				if(packIndex > 0 && !string.IsNullOrEmpty(GTBuildSettingsConfig.Instance.packs[packIndex-1].build.androidKeystoreNamePath))
					initialPathForBrowsing = GTBuildSettingsConfig.Instance.packs[packIndex-1].build.androidKeystoreNamePath;
				else if(!string.IsNullOrEmpty(preSelected))
					initialPathForBrowsing = preSelected;
				
				GTBuildSettingsConfig.Instance.packs[packIndex].build.androidKeystoreNamePath = EditorUtility.OpenFilePanel("Choose your Android Keystore file", initialPathForBrowsing, "");
			}
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Keystore pass
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Keystore Pass",GUILayout.Width(120));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.androidKeystorePass = EditorGUILayout.PasswordField(GTBuildSettingsConfig.Instance.packs[packIndex].build.androidKeystorePass);
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Keystore alias
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Key Alias",GUILayout.Width(120));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.androidKeyaliasName = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.androidKeyaliasName).Trim();
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Keystore alias pass
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Key Alias Pass",GUILayout.Width(120));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.androidKeyaliasPass = EditorGUILayout.PasswordField(GTBuildSettingsConfig.Instance.packs[packIndex].build.androidKeyaliasPass);
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Keystore Facebook Hash
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Facebook Hash",GUILayout.Width(120));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.androidKeyStoreFBHash = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.androidKeyStoreFBHash).Trim();
			EditorGUILayout.EndHorizontal();
			
		}
		EditorGUILayout.EndVertical();
	}
	
	private void showFoldersFeatures(int packIndex){
		if(!GTBuildSettingsConfig.Instance.showSpecificFoldersFeaturesFromBuildPack.ContainsKey(GTBuildSettingsConfig.Instance.packs[packIndex]))
			GTBuildSettingsConfig.Instance.showSpecificFoldersFeaturesFromBuildPack.Add(GTBuildSettingsConfig.Instance.packs[packIndex], false);
		
		EditorGUILayout.BeginVertical(GUI.skin.box);
		GTBuildSettingsConfig.Instance.showSpecificFoldersFeaturesFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]] = EditorGUILayout.Foldout(GTBuildSettingsConfig.Instance.showSpecificFoldersFeaturesFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]], "Folder Paths");
		if(GTBuildSettingsConfig.Instance.showSpecificFoldersFeaturesFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]]){
			//---------------------------------------
			// Build Name
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Build Name:",GUILayout.Width(80));
			GTBuildSettingsConfig.Instance.packs[packIndex].build.buildName = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.buildName).Trim();
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Pre Build Android Folder
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Build Android Folder:",GUILayout.Width(150));
			EditorGUILayout.SelectableLabel(GTBuildSettingsConfig.Instance.packs[packIndex].build.androidBuildFolder);
			
			if(GUILayout.Button("Browse",  GUILayout.Width(50))) {
				string rootProjectPath = Application.dataPath.Replace("/Assets","");
				GTBuildSettingsConfig.Instance.packs[packIndex].build.androidBuildFolder = EditorUtility.OpenFolderPanel("Choose Build Android Folder", rootProjectPath, "");
			}
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Pre Build iOS Folder
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Build iOS Folder:",GUILayout.Width(150));
			EditorGUILayout.SelectableLabel(GTBuildSettingsConfig.Instance.packs[packIndex].build.iosBuildFolder);
			
			if(GUILayout.Button("Browse",  GUILayout.Width(50))) {
				string rootProjectPath = Application.dataPath.Replace("/Assets","");
				GTBuildSettingsConfig.Instance.packs[packIndex].build.iosBuildFolder = EditorUtility.OpenFolderPanel("Choose Build iOS Folder", rootProjectPath, "");
			}
			EditorGUILayout.EndHorizontal();
			
			
			//---------------------------------------
			// Pre Build Android Icons Folder
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("PB Android Icons Folder:",GUILayout.Width(150));
			EditorGUILayout.SelectableLabel(GTBuildSettingsConfig.Instance.packs[packIndex].build.preBuiltAndroidIconsFolder);
			
			if(GUILayout.Button("Browse",  GUILayout.Width(50))) {
				string preSelected = GTBuildSettingsConfig.Instance.packs[packIndex].build.preBuiltAndroidIconsFolder;
				string currentFolder = string.IsNullOrEmpty (preSelected) ?
					Application.dataPath : preSelected;
				string selectedPath = EditorUtility.OpenFolderPanel("Choose Pre Build Android Icons Folder", currentFolder, "").Replace(Application.dataPath, "Assets");
				GTBuildSettingsConfig.Instance.packs[packIndex].build.preBuiltAndroidIconsFolder 
					= string.IsNullOrEmpty(selectedPath) ? GTBuildSettings.PRE_BUILT_ANDROID_ICONS_FOLDER : selectedPath;
			}
			EditorGUILayout.EndHorizontal();
			
			//---------------------------------------
			// Pre Build iOS Icons Folder
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("PB iOS Icons Folder:",GUILayout.Width(150));
			EditorGUILayout.SelectableLabel(GTBuildSettingsConfig.Instance.packs[packIndex].build.preBuiltIOSIconsFolder);
			
			if(GUILayout.Button("Browse",  GUILayout.Width(50))) {
				string preSelected = GTBuildSettingsConfig.Instance.packs[packIndex].build.preBuiltIOSIconsFolder;
				string currentFolder = string.IsNullOrEmpty (preSelected) ?
					Application.dataPath : preSelected;
				
				string selectedPath = EditorUtility.OpenFolderPanel("Choose Pre Build iOS Icons Folder", currentFolder, "").Replace(Application.dataPath, "Assets");
				GTBuildSettingsConfig.Instance.packs[packIndex].build.preBuiltIOSIconsFolder
					= string.IsNullOrEmpty(selectedPath) ? GTBuildSettings.PRE_BUILT_IOS_ICONS_FOLDER : selectedPath;
			}
			EditorGUILayout.EndHorizontal();
			
			
		}
		EditorGUILayout.EndVertical();
	}
	
	protected virtual void showSharedResources(){
		//		EditorGUILayout.BeginVertical(GUI.skin.box);
		
		
		GTBuildSettingsConfig.Instance.showSharedResources = EditorGUILayout.Foldout(GTBuildSettingsConfig.Instance.showSharedResources, "Shared Resources");
		if(GTBuildSettingsConfig.Instance.showSharedResources){
			Color prevCol = GUI.color;
			
			
			//---------------------------------------
			// Pre Build Resources Excludes Folder
			//--------------------------------------
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("PB Res Excludes Folder:",GUILayout.Width(150));
			EditorGUILayout.SelectableLabel(GTBuildSettingsConfig.Instance.resourcesBeforeBuildPath);
			
			
			
			if(GUILayout.Button("Browse",  GUILayout.Width(50))) {
				string preSelected = GTBuildSettingsConfig.Instance.resourcesBeforeBuildPath;
				string currentFolder = string.IsNullOrEmpty (preSelected) ?
					Application.dataPath+"/Resources" : preSelected;
				if(!System.IO.Directory.Exists(currentFolder)){
					System.IO.Directory.CreateDirectory(currentFolder);
					AssetDatabase.Refresh();
				}
				
				GTBuildSettingsConfig.Instance.resourcesBeforeBuildPath  = EditorUtility.OpenFolderPanel("Choose Pre Build Resources Excludes Folder", currentFolder, "").Replace(Application.dataPath, "Assets");
			}
			EditorGUILayout.EndHorizontal();
			
			
			
			
			//---------------------------------------
			// All shared resources
			//--------------------------------------
			if(GTBuildSettingsConfig.Instance.sharedResources == null || (GTBuildSettingsConfig.Instance.sharedResources != null && GTBuildSettingsConfig.Instance.sharedResources.Count == 0)) {
				EditorGUILayout.HelpBox("No Shared Resources",MessageType.None);
			}
			
			int i = 0;
			foreach(string sharedRes in GTBuildSettingsConfig.Instance.sharedResources) {
				EditorGUILayout.BeginVertical(GUI.skin.box);
				
				EditorGUILayout.BeginHorizontal();
				GUI.color = Color.red;
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GTBuildSettingsConfig.Instance.sharedResources.Remove(sharedRes);
					break;
				}
				GUI.color = prevCol;
				EditorGUILayout.EndHorizontal();
				
				//---------------------------------------
				// Scene name
				//--------------------------------------
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Res "+i.ToString()+":",GUILayout.Width(65));
				EditorGUILayout.SelectableLabel(GTBuildSettingsConfig.Instance.sharedResources[i]);
				
				if(GUILayout.Button("Browse",  GUILayout.Width(50))) {
					string prev = i > 0 ?  GTBuildSettingsConfig.Instance.sharedResources[i-1]: "";
					string preSelected = GTBuildSettingsConfig.Instance.sharedResources[i];
					string currentFile = "";
					
					if(string.IsNullOrEmpty (preSelected) && !string.IsNullOrEmpty(prev))
						currentFile = prev;
					else if(string.IsNullOrEmpty (preSelected) && string.IsNullOrEmpty(prev))
						currentFile = Application.dataPath;
					else if(!string.IsNullOrEmpty (preSelected))
						currentFile = preSelected;
					
					GTBuildSettingsConfig.Instance.sharedResources[i] = EditorUtility.OpenFilePanel("Choose Shared Resource file", currentFile, "").Replace(Application.dataPath, "Assets");
				}
				EditorGUILayout.EndHorizontal();
				
				
				
				i++;
				EditorGUILayout.EndVertical();
			}
			
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			GUI.color = Color.cyan;
			if(GUILayout.Button("New Shared Resource")) {
				GTBuildSettingsConfig.Instance.sharedResources.Add("");
			}
			GUI.color = prevCol;
			EditorGUILayout.EndHorizontal();
			//			EditorGUILayout.Space();
		}
		//		EditorGUILayout.EndVertical();
	}
	
	protected virtual void showSpecificScenes(int packIndex){
		EditorGUILayout.BeginVertical(GUI.skin.box);
		if(!GTBuildSettingsConfig.Instance.showSpecificScenesFromBuildPack.ContainsKey(GTBuildSettingsConfig.Instance.packs[packIndex]))
			GTBuildSettingsConfig.Instance.showSpecificScenesFromBuildPack.Add(GTBuildSettingsConfig.Instance.packs[packIndex], false);
		
		GTBuildSettingsConfig.Instance.showSpecificScenesFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]] = EditorGUILayout.Foldout(GTBuildSettingsConfig.Instance.showSpecificScenesFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]], "Scenes");
		if(GTBuildSettingsConfig.Instance.showSpecificScenesFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]]){
			Color prevCol = GUI.color;
			
			if(GTBuildSettingsConfig.Instance.packs[packIndex].build.scenes == null || (GTBuildSettingsConfig.Instance.packs[packIndex].build.scenes != null && GTBuildSettingsConfig.Instance.packs[packIndex].build.scenes.Count == 0)) {
				EditorGUILayout.HelpBox("No Scenes Registred",MessageType.None);
			}
			
			int i = 0;
			foreach(GTBuildScene scene in GTBuildSettingsConfig.Instance.packs[packIndex].build.scenes) {
				EditorGUILayout.BeginVertical(GUI.skin.box);
				
				EditorGUILayout.BeginHorizontal();
				GUI.color = Color.red;
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GTBuildSettingsConfig.Instance.packs[packIndex].build.scenes.Remove(scene);
					break;
				}
				GUI.color = prevCol;
				EditorGUILayout.EndHorizontal();
				
				
				
				//---------------------------------------
				// Scene Path
				//--------------------------------------
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Scene "+i.ToString()+":",GUILayout.Width(65));
				EditorGUILayout.SelectableLabel(GTBuildSettingsConfig.Instance.packs[packIndex].build.scenes[i].name);
				
				if(GUILayout.Button("Browse",  GUILayout.Width(50))) {
					string prev = i > 0 ?  GTBuildSettingsConfig.Instance.packs[packIndex].build.scenes[i-1].name: "";
					string preSelected = GTBuildSettingsConfig.Instance.packs[packIndex].build.scenes[i].name;
					string currentFile = "";
					
					if(string.IsNullOrEmpty (preSelected) && !string.IsNullOrEmpty(prev))
						currentFile = prev;
					else if(string.IsNullOrEmpty (preSelected) && string.IsNullOrEmpty(prev))
						currentFile = Application.dataPath;
					else if(!string.IsNullOrEmpty (preSelected))
						currentFile = preSelected;
					
					GTBuildSettingsConfig.Instance.packs[packIndex].build.scenes[i].name = EditorUtility.OpenFilePanel("Choose Scene file", currentFile, "").Replace(Application.dataPath, "Assets");
				}
				EditorGUILayout.EndHorizontal();
				
				//---------------------------------------
				// Scene for GameSection
				//--------------------------------------
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Game Section:",GUILayout.Width(65));
				GTBuildSettingsConfig.Instance.packs[packIndex].build.scenes[i].section = (GameSection) EditorGUILayout.EnumPopup(GTBuildSettingsConfig.Instance.packs[packIndex].build.scenes[i].section);
				EditorGUILayout.EndHorizontal();
				
				i++;
				EditorGUILayout.EndVertical();
			}
			
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			GUI.color = Color.cyan;
			if(GUILayout.Button("New Scene")) {
				GTBuildSettingsConfig.Instance.packs[packIndex].build.scenes.Add(new GTBuildScene());
			}
			GUI.color = prevCol;
			EditorGUILayout.EndHorizontal();
			//			EditorGUILayout.Space();
		}
		EditorGUILayout.EndVertical();
	}
	
	protected virtual void showSpecificAndroidIcons(int packIndex){
		EditorGUILayout.BeginVertical(GUI.skin.box);
		if(!GTBuildSettingsConfig.Instance.showSpecificAndroidIconsFromBuildPack.ContainsKey(GTBuildSettingsConfig.Instance.packs[packIndex]))
			GTBuildSettingsConfig.Instance.showSpecificAndroidIconsFromBuildPack.Add(GTBuildSettingsConfig.Instance.packs[packIndex], false);
		
		GTBuildSettingsConfig.Instance.showSpecificAndroidIconsFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]] = EditorGUILayout.Foldout(GTBuildSettingsConfig.Instance.showSpecificAndroidIconsFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]], "Android Icons");
		if(GTBuildSettingsConfig.Instance.showSpecificAndroidIconsFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]]){
			Color prevCol = GUI.color;
			
			if(GTBuildSettingsConfig.Instance.packs[packIndex].build.androidIcons == null || (GTBuildSettingsConfig.Instance.packs[packIndex].build.androidIcons != null && GTBuildSettingsConfig.Instance.packs[packIndex].build.androidIcons.Count == 0)) {
				EditorGUILayout.HelpBox("No Android Icons Registred",MessageType.None);
			}
			
			int i = 0;
			foreach(string iconPath in GTBuildSettingsConfig.Instance.packs[packIndex].build.androidIcons) {
				EditorGUILayout.BeginVertical(GUI.skin.box);
				
				EditorGUILayout.BeginHorizontal();
				GUI.color = Color.red;
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GTBuildSettingsConfig.Instance.packs[packIndex].build.androidIcons.Remove(iconPath);
					break;
				}
				GUI.color = prevCol;
				EditorGUILayout.EndHorizontal();
				
				//Icon NAME
				//---------------------------------------
				// Pre Build Resources Excludes Folder
				//--------------------------------------
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Icon "+(i+1).ToString()+":",GUILayout.Width(60));					
				GTBuildSettingsConfig.Instance.packs[packIndex].build.androidIcons[i] = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.androidIcons[i]).Trim();
				EditorGUILayout.EndHorizontal();
				
				i++;
				EditorGUILayout.EndVertical();
			}
			
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			GUI.color = Color.cyan;
			if(GUILayout.Button("New Icon")) {
				GTBuildSettingsConfig.Instance.packs[packIndex].build.androidIcons.Add("");
			}
			GUI.color = prevCol;
			EditorGUILayout.EndHorizontal();
			//			EditorGUILayout.Space();
		}
		EditorGUILayout.EndVertical();
	}
	
	protected virtual void showSpecificIOSIcons(int packIndex){
		EditorGUILayout.BeginVertical(GUI.skin.box);
		if(!GTBuildSettingsConfig.Instance.showSpecificIOSIconsFromBuildPack.ContainsKey(GTBuildSettingsConfig.Instance.packs[packIndex]))
			GTBuildSettingsConfig.Instance.showSpecificIOSIconsFromBuildPack.Add(GTBuildSettingsConfig.Instance.packs[packIndex], false);
		
		GTBuildSettingsConfig.Instance.showSpecificIOSIconsFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]] = EditorGUILayout.Foldout(GTBuildSettingsConfig.Instance.showSpecificIOSIconsFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]], "iOS Icons");
		if(GTBuildSettingsConfig.Instance.showSpecificIOSIconsFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]]){
			Color prevCol = GUI.color;
			
			if(GTBuildSettingsConfig.Instance.packs[packIndex].build.iOSIcons == null || (GTBuildSettingsConfig.Instance.packs[packIndex].build.iOSIcons != null && GTBuildSettingsConfig.Instance.packs[packIndex].build.iOSIcons.Count == 0)) {
				EditorGUILayout.HelpBox("No iOS Icons Registred",MessageType.None);
			}
			
			int i = 0;
			foreach(string iconPath in GTBuildSettingsConfig.Instance.packs[packIndex].build.iOSIcons) {
				EditorGUILayout.BeginVertical(GUI.skin.box);
				
				EditorGUILayout.BeginHorizontal();
				GUI.color = Color.red;
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GTBuildSettingsConfig.Instance.packs[packIndex].build.iOSIcons.Remove(iconPath);
					break;
				}
				GUI.color = prevCol;
				EditorGUILayout.EndHorizontal();
				
				//Icon NAME
				//---------------------------------------
				// Pre Build Resources Excludes Folder
				//--------------------------------------
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Icon "+(i+1).ToString()+":",GUILayout.Width(60));					
				GTBuildSettingsConfig.Instance.packs[packIndex].build.iOSIcons[i] = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.iOSIcons[i]).Trim();
				EditorGUILayout.EndHorizontal();
				
				i++;
				EditorGUILayout.EndVertical();
			}
			
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			GUI.color = Color.cyan;
			if(GUILayout.Button("New Icon")) {
				GTBuildSettingsConfig.Instance.packs[packIndex].build.iOSIcons.Add("");
			}
			GUI.color = prevCol;
			EditorGUILayout.EndHorizontal();
			//			EditorGUILayout.Space();
		}
		EditorGUILayout.EndVertical();
	}
	
	protected virtual void showSpecificResIncludes(int packIndex){
		Color prevCol = GUI.color;
		
		EditorGUILayout.BeginVertical(GUI.skin.box);
		if(!GTBuildSettingsConfig.Instance.showSpecificResIncludesFromBuildPack.ContainsKey(GTBuildSettingsConfig.Instance.packs[packIndex]))
			GTBuildSettingsConfig.Instance.showSpecificResIncludesFromBuildPack.Add(GTBuildSettingsConfig.Instance.packs[packIndex], false);
		
		GUI.color = Color.green;
		GTBuildSettingsConfig.Instance.showSpecificResIncludesFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]] = EditorGUILayout.Foldout(GTBuildSettingsConfig.Instance.showSpecificResIncludesFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]], "Resources Includes");
		GUI.color = prevCol;
		if(GTBuildSettingsConfig.Instance.showSpecificResIncludesFromBuildPack[GTBuildSettingsConfig.Instance.packs[packIndex]]){
			
			
			
			
			
			//			//---------------------------------------
			//			// Resources Folder
			//			//--------------------------------------
			//			EditorGUILayout.BeginHorizontal();
			//			EditorGUILayout.LabelField("Resources Folder:",GUILayout.Width(120));
			//			EditorGUILayout.SelectableLabel(GTBuildSettingsConfig.Instance.packs[packIndex].build.resourcesFolder);
			//			
			//			if(GUILayout.Button("Browse",  GUILayout.Width(50))) {
			//				string preSelected = GTBuildSettingsConfig.Instance.packs[packIndex].build.resourcesFolder;
			//				string currentFile = string.IsNullOrEmpty (preSelected) ? GTBuildSettings.RESOURCES_FOLDER : preSelected;
			//				GTBuildSettingsConfig.Instance.packs[packIndex].build.resourcesFolder = EditorUtility.OpenFolderPanel("Choose Resources Folder", currentFile, "").Replace(Application.dataPath, "Assets");
			//			}
			//			EditorGUILayout.EndHorizontal();
			
			
			
			if(GTBuildSettingsConfig.Instance.packs[packIndex].build.resIncludes == null || (GTBuildSettingsConfig.Instance.packs[packIndex].build.resIncludes != null && GTBuildSettingsConfig.Instance.packs[packIndex].build.resIncludes.Count == 0)) {
				EditorGUILayout.HelpBox("No Resources Includes",MessageType.None);
			}
			
			int i = 0;
			
			foreach(string path in GTBuildSettingsConfig.Instance.packs[packIndex].build.resIncludes) {
				Color bgColor = Color.green;
				GUI.backgroundColor = bgColor;
				EditorGUILayout.BeginVertical(GUI.skin.box);
				
				
				GUI.backgroundColor = prevCol;
				EditorGUILayout.BeginHorizontal();
				GUI.color = Color.red;
				if(GUILayout.Button("-",  GUILayout.Width(30))) {
					GTBuildSettingsConfig.Instance.packs[packIndex].build.resIncludes.Remove(path);
					break;
				}
				GUI.color = prevCol;
				GUI.backgroundColor = bgColor;
				
				EditorGUILayout.EndHorizontal();
				
				//Resource files
				//Icon NAME
				//---------------------------------------
				// Pre Build Resources Includes Folder
				//--------------------------------------
				GUI.backgroundColor = prevCol;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Res "+(i+1).ToString()+":",GUILayout.Width(60));					
				//					GTBuildSettingsConfig.Instance.packs[packIndex].build.resIncludes[i] = EditorGUILayout.TextField(GTBuildSettingsConfig.Instance.packs[packIndex].build.resIncludes[i]).Trim();
				
				EditorGUILayout.SelectableLabel(GTBuildSettingsConfig.Instance.packs[packIndex].build.resIncludes[i]);
				
				if(GUILayout.Button("Browse",  GUILayout.Width(50))) {
					//						string preSelected = GTBuildSettingsConfig.Instance.packs[packIndex].build.resIncludes[i];
					//						string currentFile = string.IsNullOrEmpty (preSelected) ?  : preSelected;
					
					string prev = i > 0 ?  GTBuildSettingsConfig.Instance.packs[packIndex].build.resIncludes[i-1]: "";
					string preSelected = GTBuildSettingsConfig.Instance.packs[packIndex].build.resIncludes[i];
					string currentFile = "";
					
					if(string.IsNullOrEmpty (preSelected) && !string.IsNullOrEmpty(prev))
						currentFile = prev;
					else if(string.IsNullOrEmpty (preSelected) && string.IsNullOrEmpty(prev)){
						currentFile = Application.dataPath +"/Resources";
						if(!System.IO.Directory.Exists(currentFile)){
							System.IO.Directory.CreateDirectory(currentFile);
							AssetDatabase.Refresh();
						}
					}
					else if(!string.IsNullOrEmpty (preSelected))
						currentFile = preSelected;
					
					GTBuildSettingsConfig.Instance.packs[packIndex].build.resIncludes[i] = EditorUtility.OpenFilePanel("Choose Resources Includes", currentFile, "").Replace(Application.dataPath, "Assets");
				}
				
				EditorGUILayout.EndHorizontal();
				
				i++;
				
				
				EditorGUILayout.EndVertical();
				GUI.backgroundColor = prevCol;
			}
			
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			GUI.color = Color.cyan;
			if(GUILayout.Button("New Include")) {
				GTBuildSettingsConfig.Instance.packs[packIndex].build.resIncludes.Add("");
			}
			GUI.color = prevCol;
			EditorGUILayout.EndHorizontal();
			//			EditorGUILayout.Space();
		}
		EditorGUILayout.EndVertical();
	}
	
	
	private static void DirtyEditor() {
		#if UNITY_EDITOR
		EditorUtility.SetDirty(GTBuildSettingsConfig.Instance);
		#endif
	}
}
