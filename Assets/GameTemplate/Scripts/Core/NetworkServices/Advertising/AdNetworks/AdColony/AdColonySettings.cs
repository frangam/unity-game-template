using UnityEngine;
using System.IO;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class AdColonySettings : ScriptableObject {
	public List<AdColonyIDsPack> idsPacks;
	public bool showPacks = true;
	
	public AdColonyIDsPack CurrentIDsPack{
		get{return idsPacks[GameSettings.Instance.currentGameMultiversion]; }
	}
	public string GetZoneIDByIndex(int index){
		string res = "";
		
		if(CurrentIDsPack != null){
			#if UNITY_ANDROID
			if(CurrentIDsPack.android_adZoneIDs != null && CurrentIDsPack.android_adZoneIDs.Count > index){
				res = CurrentIDsPack.android_adZoneIDs[index];
			}
			
			#elif UNITY_IPHONE
			if(CurrentIDsPack.iOS_adZoneIDs != null && CurrentIDsPack.iOS_adZoneIDs.Count > index){
				res = CurrentIDsPack.iOS_adZoneIDs[index];
			}
			#endif
			
		}
		
		return res;
	}
	
	private const string assetName = "AdColonySettings";
	private const string path = "GameTemplate/Resources";
	private const string extension = ".asset";
	
	private static AdColonySettings instance = null;
	
	
	public static AdColonySettings Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(assetName) as AdColonySettings;
				
				if (instance == null) {
					
					// If not found, autocreate the asset object.
					instance = CreateInstance<AdColonySettings>();
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
