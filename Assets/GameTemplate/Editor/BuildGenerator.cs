/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

/// <summary>
/// Build generator.
/// 
/// based on: http://answers.unity3d.com/questions/1006132/unable-to-build-to-android-and-ios-using-buildpipe.html
/// </summary>
public class BuildGenerator {
	public enum IOSArchitecture{
		ARMv7 		= 0,
		ARM64 		= 1,
		Universal 	= 2
	}
	
	//	//TODO change this
	//	private static string [] scenes = { "Assets/Scenes/CorporativeLogoSplash.unity", "Assets/Scenes/Loader.unity", "Assets/Scenes/MainMenu.unity", "Assets/Scenes/Game.unity" };
	
	private static string path;
	private static GTBuildSettings build;
	
	[MenuItem("GameTemplate/BuildSettings", false, -101)]
	public static void BuildSettings (){
		Selection.activeObject = GTBuildSettingsConfig.Instance;
	}
	
	[MenuItem("GameTemplate/★ Build and Run Project ★", false, -100)]
	public static void BuildAndRunGame (){
		preProcessBuild();
		
		if(build != null){
			//		// Get filename.
			//		string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
			//		string[] levels = new string[] {"Assets/Scene1.unity", "Assets/Scene2.unity"};
			//		
			//		// Build player.
			//		BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
			//		
			//		// Copy a file from the project folder to the build folder, alongside the built game.
			//		FileUtil.CopyFileOrDirectory("Assets/WebPlayerTemplates/Readme.txt", path + "Readme.txt");
			//		
			//		// Run the game (Process class from System.Diagnostics).
			//		Process proc = new Process();
			//		proc.StartInfo.FileName = path + "BuiltGame.exe";
			//		proc.Start();
			
			#if UNITY_IOS
			buildForIOS();
			#elif UNITY_ANDROID
			//then build
			buildForAndroid();
			#endif
			postProcessBuild();
		}
	}
	
	
	[MenuItem("GameTemplate/Specific Build/Build and Run for iOS", false, -99)]
	public static void BuildAndRunIOSGame (){
		preProcessBuild();
		if(build != null){
			buildForIOS();
			postProcessBuild();
		}
	}
	
	[MenuItem("GameTemplate/Specific Build/Build and Run for Android", false, -98)]
	public static void BuildAndRunAndroidGame (){
		preProcessBuild();
		
		if(build != null){
			buildForAndroid();
			postProcessBuild();
		}
	}
	
	
	private static bool getCurrentBuildSettings(){
		build = GTBuildSettingsConfig.Instance.CurrentBuildPack.build;
		bool valid = build != null;
		
		if(!valid)
			GTDebug.logErrorAlways("Not found a Build Settings for current Game Multiversion "+GameSettings.Instance.currentGameMultiversion);
		
		return valid;
	}
	
	private static void preProcessBuild(){
		if(getCurrentBuildSettings()){
			EditorUserBuildSettings.connectProfiler = GameSettings.Instance.IS_A_DEV_VERSION;
			EditorUserBuildSettings.development = GameSettings.Instance.IS_A_DEV_VERSION;
			
			PlayerSettings.companyName = build.companyName;
			PlayerSettings.bundleIdentifier = build.bundleIdentifier;
			//			PlayerSettings.cloudProjectId = build.cloudProjectID;
			moveAssetsFromResources();
		}
	}
	
	private static void postProcessBuild(){
		restoreAssetsAfterBuilt();
		
		//		//Run
		//		Process proc = new Process();
		//		proc.StartInfo.FileName = path;
		//		proc.Start();
	}
	
	private static void buildForAndroid(){
		string path = build.androidBuildFolder 
			+"/"+ build.buildName + GameSettings.Instance.currentGameMultiversion+".apk";
		
		//		UnityEngine.Debug.Log("path: "+path);
		
		AndroidNativeSettings.Instance.base64EncodedPublicKey = build.androidBillingBase64Key;
		
		PlayerSettings.bundleVersion = build.androidBundleVersion;
		PlayerSettings.Android.bundleVersionCode = build.androidBundleVersionCode;
		PlayerSettings.productName = build.androidGameName;
		PlayerSettings.Android.keystoreName = build.androidKeystoreNamePath;
		PlayerSettings.Android.keystorePass = build.androidKeystorePass;
		PlayerSettings.Android.keyaliasName = build.androidKeyaliasName;
		PlayerSettings.Android.keyaliasPass = build.androidKeyaliasPass;
		//		PlayerSettings.use32BitDisplayBuffer = true;
		//		PlayerSettings.Android.disableDepthAndStencilBuffers = true;
		
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
		
		moveAndroidGameIconsBeforeBuilt();
		BuildPipeline.BuildPlayer(build.ScenesNames.ToArray(), path, BuildTarget.Android, BuildOptions.AutoRunPlayer);
		restoreAndroidGameIconsBeforeBuilt();
	}
	
	private static void buildForIOS(){
		string path = build.iosBuildFolder 
			+"/"+build.buildName+GameSettings.Instance.currentGameMultiversion;
		
		if(!System.IO.Directory.Exists(path))
			System.IO.Directory.CreateDirectory(path);
		
		//		UnityEngine.Debug.Log("path: "+path);
		
		moveIOSGameIconsBeforeBuilt();
		IOSNativeSettings.Instance.AppleId = build.appleID;
		PlayerSettings.bundleVersion = build.iOSBundleVersion;
		PlayerSettings.productName = build.iOSGameName;
		
		
		#if UNITY_5
		PlayerSettings.SetPropertyInt("ScriptingBackend", (int)ScriptingImplementation.IL2CPP, BuildTargetGroup.iOS);
		PlayerSettings.SetPropertyInt("Architecture", (int)IOSArchitecture.Universal, BuildTargetGroup.iOS);
		#else
		PlayerSettings.SetPropertyInt("ScriptingBackend", (int)ScriptingImplementation.IL2CPP, BuildTargetGroup.iPhone);
		PlayerSettings.SetPropertyInt("Architecture", (int)IOSArchitecture.Universal, BuildTargetGroup.iPhone);
		#endif
		
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iOS);
		BuildPipeline.BuildPlayer(build.ScenesNames.ToArray(), path, BuildTarget.iOS, BuildOptions.AutoRunPlayer);
	}
	
	private static void moveAssetsFromResources(bool restore = false){
		int multiversion = GameSettings.Instance.currentGameMultiversion;
		string resourcesPath = "Assets/Resources/";
		string resourcesBeforeBuildPath = GTBuildSettingsConfig.Instance.resourcesBeforeBuildPath + "/";
		List<string> includes = build.resIncludes;
		List<string> sharedResBetweenAllVersions = GTBuildSettingsConfig.Instance.sharedResources;

		if(!System.IO.Directory.Exists(resourcesBeforeBuildPath))
			System.IO.Directory.CreateDirectory(resourcesBeforeBuildPath);
		
		//move all shared res between multiversion, and only keep the current includes for this build
		foreach(string sharedRes in sharedResBetweenAllVersions){
			if(!includes.Contains(sharedRes)){
				// Move assets from Resources
				string source = sharedRes;
				string dest = resourcesBeforeBuildPath + sharedRes.Replace(resourcesPath,"");
				string ret = !restore ? AssetDatabase.MoveAsset(source, dest) : AssetDatabase.MoveAsset(dest, source);
				
				UnityEngine.Debug.Log("moving resources ? " + (ret == ""));
				
				if(ret != ""){
					if(!restore)
						AssetDatabase.ImportAsset(source);
					else
						AssetDatabase.ImportAsset(dest);
					
					ret = !restore ? AssetDatabase.MoveAsset(source, dest) : AssetDatabase.MoveAsset(dest, source);
					UnityEngine.Debug.Log("moving resources ? " + (ret == ""));
					
					if(ret != "")
						UnityEngine.Debug.Log(ret);
				}
			}
		}
	}
	
	private static void restoreAssetsAfterBuilt(){
		moveAssetsFromResources(true);
		
		// Refresh database
		AssetDatabase.Refresh();
	}
	
	private static void moveAndroidGameIconsBeforeBuilt(){
		int multiversion = GameSettings.Instance.currentGameMultiversion;
		string sourcePath = build.preBuiltAndroidIconsFolder + "/" + multiversion.ToString() + "/";
		string destinationPath = "Assets/Plugins/Android/res/";
		string iconName = "app_icon.png";
		List<string> paths = build.androidIcons;
		//			= "drawable/app_icon.png";
		//		string path2 = "drawable-hdpi/app_icon.png";
		//		string path3 = "drawable-ldpi/app_icon.png";
		//		string path4 = "drawable-mdpi/app_icon.png";
		//		string path5 = "drawable-xhdpi/app_icon.png";
		//		string path6 = "drawable-xxhdpi/app_icon.png";
		//		string path7 = "drawable-xxhdpi/app_icon.png";
		//		string[] sourcePaths = {sourcePath+path1, sourcePath+path2, sourcePath+path3,sourcePath+path4, sourcePath+path5, sourcePath+path6,sourcePath+path7};
		//		string[] destPaths = {destinationPath+path1, destinationPath+path2, destinationPath+path3,destinationPath+path4, destinationPath+path5, destinationPath+path6,destinationPath+path7};

//		List<string> iconTypes = new List<string> (){"drawable", "drawable-hdpi" ,"drawable-ldpi", "drawable-mdpi", "drawable-xhdpi", "drawable-xxhdpi", "drawable-xxhdpi"};

		foreach(string iconFolder in paths){
			string source = sourcePath+iconFolder+"/"+iconName;
			string destFolder = destinationPath+iconFolder; //path/drawable.../

			if(!System.IO.Directory.Exists(destFolder)){
				System.IO.Directory.CreateDirectory(destFolder);
				AssetDatabase.Refresh();
			}
			
			//			UnityEngine.Debug.Log(ret);
			//			UnityEngine.Debug.Log ("moving from: "+source+" to: "+dest); 

			string dest = destFolder+"/"+iconName;
			string ret = AssetDatabase.MoveAsset(source, dest);

			if(ret != ""){
				AssetDatabase.ImportAsset(source);
				ret = AssetDatabase.MoveAsset(source, dest);
				
				if(ret != "")
					UnityEngine.Debug.Log(ret);
			}
		}
	}
	
	private static void restoreAndroidGameIconsBeforeBuilt(){
		int multiversion = GameSettings.Instance.currentGameMultiversion;
		string sourcePath = "Assets/Plugins/Android/res/";
		string destinationPath = build.preBuiltAndroidIconsFolder + "/" + multiversion.ToString() + "/";
		string iconName = "app_icon.png";
		List<string> paths = build.androidIcons;
		
		foreach(string iconFolder in paths){
			string source = sourcePath+iconFolder + "/" + iconName;
			string dest = destinationPath+iconFolder + "/" + iconName;
            string destFolder = destinationPath + iconFolder; //path/drawable.../
            //create a icons folder if not exist
            if (!System.IO.Directory.Exists(destFolder))
            {
                System.IO.Directory.CreateDirectory(destFolder);
                AssetDatabase.Refresh();
            }
            string ret = AssetDatabase.MoveAsset(source, dest);
			
			//			UnityEngine.Debug.Log(ret);
			//			UnityEngine.Debug.Log ("moving from: "+source+" to: "+dest);
			
			if(ret != ""){
				AssetDatabase.ImportAsset(source);
				ret = AssetDatabase.MoveAsset(source, dest);
				
				if(ret != "")
					UnityEngine.Debug.Log(ret);
			}
		}
	}
	
	private static void moveIOSGameIconsBeforeBuilt(){
		int multiversion = GameSettings.Instance.currentGameMultiversion;
		string initalPath = build.preBuiltIOSIconsFolder+"/"+multiversion.ToString() + "/";
		List<string> iOSIcons = build.iOSIcons;
		
		if(iOSIcons != null){
			Texture2D[] icons = new Texture2D[iOSIcons.Count]; 
			
			for(int i=0; i<iOSIcons.Count; i++)
				icons[i] = AssetDatabase.LoadMainAssetAtPath(initalPath + iOSIcons[i]) as Texture2D;
			
			//			{
			//				AssetDatabase.LoadMainAssetAtPath(initalPath + "AppIcon57x57.png") as Texture2D,
			//				AssetDatabase.LoadMainAssetAtPath(initalPath + "AppIcon57x57@2x.png") as Texture2D,
			//				AssetDatabase.LoadMainAssetAtPath(initalPath + "AppIcon60x60@2x.png") as Texture2D,
			//				AssetDatabase.LoadMainAssetAtPath(initalPath + "AppIcon60x60@3x.png") as Texture2D,
			//				AssetDatabase.LoadMainAssetAtPath(initalPath + "AppIcon72x72@2x~ipad.png") as Texture2D,
			//				AssetDatabase.LoadMainAssetAtPath(initalPath + "AppIcon72x72~ipad.png") as Texture2D,
			//				AssetDatabase.LoadMainAssetAtPath(initalPath + "AppIcon76x76~ipad.png") as Texture2D,
			//				AssetDatabase.LoadMainAssetAtPath(initalPath + "AppIcon76x76~ipad@2x.png") as Texture2D
			//			};
			
			PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.iOS, icons);
		}
	}
}
