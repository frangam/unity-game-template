/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.IO;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class GTBuildSettingsConfig : ScriptableObject {
	public Dictionary<GTBuildSettingsPack, bool> 	showBuildPackSettings = new Dictionary<GTBuildSettingsPack, bool>();
	public Dictionary<GTBuildSettingsPack, bool> 	showSpecificBuildPack = new Dictionary<GTBuildSettingsPack, bool>();
	public Dictionary<GTBuildSettingsPack, bool> 	showSpecificMainFeaturesFromBuildPack = new Dictionary<GTBuildSettingsPack, bool>();
	public Dictionary<GTBuildSettingsPack, bool> 	showKeyStoreSettingsFromBuildPack = new Dictionary<GTBuildSettingsPack, bool>();
	public Dictionary<GTBuildSettingsPack, bool> 	showSpecificFoldersFeaturesFromBuildPack = new Dictionary<GTBuildSettingsPack, bool>();
	public Dictionary<GTBuildSettingsPack, bool> 	showSpecificScenesFromBuildPack = new Dictionary<GTBuildSettingsPack, bool>();
	public Dictionary<GTBuildSettingsPack, bool> 	showSpecificAndroidIconsFromBuildPack = new Dictionary<GTBuildSettingsPack, bool>();
	public Dictionary<GTBuildSettingsPack, bool> 	showSpecificIOSIconsFromBuildPack = new Dictionary<GTBuildSettingsPack, bool>();
	public Dictionary<GTBuildSettingsPack, bool> 	showSpecificResIncludesFromBuildPack = new Dictionary<GTBuildSettingsPack, bool>();
	public bool 									showSettings = true;
	public bool 									showSharedResources = false;
	public List<GTBuildSettingsPack>				packs;
	public List<string>								sharedResources;
	public string									resourcesBeforeBuildPath;

	public GTBuildSettingsPack CurrentBuildPack{
		get{
			if(packs != null && packs.Count > GameSettings.Instance.currentGameMultiversion) return packs[GameSettings.Instance.currentGameMultiversion];
			else{ 
				GTDebug.logErrorAlways("Current Build for Game Multiversion "+GameSettings.Instance.currentGameMultiversion.ToString()+" not found");
				return null;
			}
		}
	}

	public bool UseAnalytics{
		get{
			bool use = false;
 
			if(CurrentBuildPack != null){
#if UNITY_ANDROID
				use = CurrentBuildPack.build.UseAnalyticsInAndroid;
#elif UNITY_IPHONE
				use = CurrentBuildPack.build.UseAnalyticsInIOS;
#else
				use = CurrentBuildPack.build.UseAnalyticsInOTher;
#endif
			}

			return use;
		}
	}











	private const string assetName = "GTBuildSettingsConfig";
	private const string path = "GameTemplate/Resources";
	private const string extension = ".asset";
	
	private static GTBuildSettingsConfig instance = null;
	public static GTBuildSettingsConfig Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(assetName) as GTBuildSettingsConfig;
				
				if (instance == null) {
					
					// If not found, autocreate the asset object.
					instance = CreateInstance<GTBuildSettingsConfig>();
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
