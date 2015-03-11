using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class LevelPacks : ScriptableObject {
	//--------------------------------------
	// The LevelPack
	//--------------------------------------
	public List<BaseLevelPack> packs 								= new List<BaseLevelPack>();
	public Dictionary<BaseLevelPack, bool> showLevelPacks 			= new Dictionary<BaseLevelPack, bool>();
	public Dictionary<BaseLevelPack, bool> showLevelPacksReqsList 	= new Dictionary<BaseLevelPack, bool>();
	public int 	DEFAULT_LEVEL_PACKS_SIZE							= 100;
	public bool showLevelsSettings 									= false;

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public BaseLevelPack getPackById(string id){
		BaseLevelPack res = null;

		if(packs != null && packs.Count > 0){
			foreach(BaseLevelPack pack in packs){
				if(id.Equals(pack.id)){
					res = pack;
					break;
				}
			}
		}

		return res;
	}

	public int totalLevels(){
		int res = 0;
		
		if(packs != null && packs.Count > 0){
			res = packs[packs.Count-1].FinalLevel;
		}
		
		return res;
	}

	public float totalProgress(){
		float res = 0.0f;

		foreach(BaseLevelPack pack in packs){
			res += pack.ProgressCompleted;
		}

		res /= (float)packs.Count;

		return res;
	}

	public float totalProgressPercetage(){
		return totalProgress()*100;
	}

//	//--------------------------------------
//	// Unity Methods
//	//--------------------------------------
//	#region Unity
//	public virtual void Awake(){
//		DontDestroyOnLoad(this); //para que no se destruyan los atributos al cargar escenas	
//		initLevels();
//	}
//	#endregion




	//--------------------------------------
	// Instance
	//--------------------------------------
	public const string assetName = "LevelPacks";
	public const string path = "GameTemplate/Resources";
	public const string extension = ".asset";
	
	private static LevelPacks instance = null;
	public static LevelPacks Instance {
		
		get {
			if (instance == null) {
				instance = Resources.Load(assetName) as LevelPacks;
				
				if (instance == null) {
					
					// If not found, autocreate the asset object.
					instance = CreateInstance<LevelPacks>();
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
