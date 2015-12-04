/***************************************************************************
Project:    Game Template
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

/*
  GTGoogleAnalyticsSettings is a class for accessing to GTGoogleAnalyticsSettings asset.
*/


public class GTGoogleAnalyticsSettings : ScriptableObject {
	public List<GTGoogleAnalyticsIDsPack> idsPacks;
	public Dictionary<GTGoogleAnalyticsIDsPack, bool> showPacks = new Dictionary<GTGoogleAnalyticsIDsPack, bool>();
	public bool showSettings = true;
	
	public GTGoogleAnalyticsIDsPack CurrentIDsPack{
		get{return idsPacks[GameSettings.Instance.currentGameMultiversion]; }
	}
	
	private const string assetName = "GoogleAnalyticsSettings";
	private const string path = "GameTemplate/Resources";
	private const string extension = ".asset";
	
	private static GTGoogleAnalyticsSettings instance = null;
	
	
	public static GTGoogleAnalyticsSettings Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(assetName) as GTGoogleAnalyticsSettings;
				
				if (instance == null) {
					
					// If not found, autocreate the asset object.
					instance = CreateInstance<GTGoogleAnalyticsSettings>();
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