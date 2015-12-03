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

public class GoogleAdmobSettings : ScriptableObject {
	public bool IsTestSettinsOpened = true;
	
	[SerializeField]
	//	public List<GADTestDevice> testDevices =  new List<GADTestDevice>();
	public List<AdmobIDsPack> idsPacks;
	public bool showPacks = true;
	
	public AdmobIDsPack CurrentIDsPack{
		get{return idsPacks[GameSettings.Instance.currentGameMultiversion]; }
	}
	
	private const string assetName = "GoogleAdmobSettings";
	private const string path = "GameTemplate/Resources";
	private const string extension = ".asset";
	
	private static GoogleAdmobSettings instance = null;
	
	
	public static GoogleAdmobSettings Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(assetName) as GoogleAdmobSettings;
				
				if (instance == null) {
					
					// If not found, autocreate the asset object.
					instance = CreateInstance<GoogleAdmobSettings>();
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
	
	
	
	//	public void AddDevice(GADTestDevice p) {
	//		testDevices.Add(p);
	//	}
	//	
	//	public void RemoveDevice(GADTestDevice p) {
	//		testDevices.Remove(p);
	//	}
	
	
}
