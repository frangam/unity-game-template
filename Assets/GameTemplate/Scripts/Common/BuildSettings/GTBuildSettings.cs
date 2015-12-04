/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GTBuildSettings {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string PRE_BUILT_ANDROID_ICONS_FOLDER 			= "Assets/Icons/Android";
	public const string PRE_BUILT_IOS_ICONS_FOLDER 				= "Assets/Icons/iOS";
	public const string DEFAULT_KEYSTORE_FB_HASH_FRILLSGAMES	= "2QYMs5g2X8L29Bpec0kYA1TPS3E";
	
	//--------------------------------------
	// Public Attributes
	//--------------------------------------
	public string 				buildName;
	public string				androidBuildFolder;			//for Windows
	public string				iosBuildFolder;
//	public string				resourcesFolder;
	public List<GTBuildScene>	scenes;
	public string				preBuiltAndroidIconsFolder;
	public string				preBuiltIOSIconsFolder;
	public List<string>			androidIcons;
	public List<string>			iOSIcons;
	public List<string>			resIncludes;
	public string				companyName;
	public string 				androidGameName;
	public string 				iOSGameName;
	public string 				wpGameName;
	public string				appleID;
	public string				googlePlayServicesID;
	public string 				cloudProjectID;
	public string				bundleIdentifier;
	public string				androidBundleVersion;
	public string				iOSBundleVersion;
	public int					androidBundleVersionCode;
	public string 				androidKeystoreNamePath;		//for Windows
	public string 				androidKeystoreNamePathOnMAc;	//for Mac
	public string 				androidKeystorePass;
	public string 				androidKeyaliasName;
	public string 				androidKeyaliasPass;
	public string				androidKeyStoreFBHash			= "2QYMs5g2X8L29Bpec0kYA1TPS3E=";
	public string				androidBillingBase64Key;

	public bool					useAnalyticsInAndroid			= false;
	public bool					useAnalyticsInIOS				= false;
	public bool					useAnalyticsInOTher				= false;
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public List<string> ScenesNames{
		get{
			List<string> res = new List<string>();

			for(int i=0; i<scenes.Count; i++)
//			foreach(GTBuildScene s in scenes)
				res.Add(scenes[i].name);

			return res;
		}
	}

	public string AndroidKeystoreNamePath {
		get {
			return this.androidKeystoreNamePath;
		}
		set {
			androidKeystoreNamePath = value;
		}
	}

	public string AndroidKeystorePass {
		get {
			return this.androidKeystorePass;
		}
		set {
			androidKeystorePass = value;
		}
	}

	public string AndroidKeyaliasName {
		get {
			return this.androidKeyaliasName;
		}
		set {
			androidKeyaliasName = value;
		}
	}

	public string AndroidKeyaliasPass {
		get {
			return this.androidKeyaliasPass;
		}
		set {
			androidKeyaliasPass = value;
		}
	}
	public string CompanyName {
		get {
			return this.companyName;
		}
		set {
			companyName = value;
		}
	}

	public string AndroidGameName {
		get {
			return this.androidGameName;
		}
		set {
			androidGameName = value;
		}
	}

	public string IOSGameName {
		get {
			return this.iOSGameName;
		}
		set {
			iOSGameName = value;
		}
	}

	public string WpGameName {
		get {
			return this.wpGameName;
		}
		set {
			wpGameName = value;
		}
	}

	public string AppleID {
		get {
			return this.appleID;
		}
		set {
			appleID = value;
		}
	}

	public string GooglePlayServicesID {
		get {
			return this.googlePlayServicesID;
		}
		set {
			googlePlayServicesID = value;
		}
	}

	public string CloudProjectID {
		get {
			return this.cloudProjectID;
		}
		set {
			cloudProjectID = value;
		}
	}

	public string BundleIdentifier {
		get {
			return this.bundleIdentifier;
		}
		set {
			bundleIdentifier = value;
		}
	}

	public string AndroidBundleVersion {
		get {
			return this.androidBundleVersion;
		}
		set {
			androidBundleVersion = value;
		}
	}

	public string IOSBundleVersion {
		get {
			return this.iOSBundleVersion;
		}
		set {
			iOSBundleVersion = value;
		}
	}

	public int AndroidBundleVersionCode {
		get {
			return this.androidBundleVersionCode;
		}
		set {
			androidBundleVersionCode = value;
		}
	}
	public string BuildName {
		get {
			return this.buildName;
		}
		set {
			buildName = value;
		}
	}
	public string AndroidBuildFolder {
		get {
			return this.androidBuildFolder;
		}
		set {
			androidBuildFolder = value;
		}
	}

	public string IosBuildFolder {
		get {
			return this.iosBuildFolder;
		}
		set {
			iosBuildFolder = value;
		}
	}

	public List<GTBuildScene> Scenes {
		get {
			return this.scenes;
		}
		set {
			scenes = value;
		}
	}
	
	public string PreBuiltAndroidIconsFolder {
		get {
			return this.preBuiltAndroidIconsFolder;
		}
		set {
			preBuiltAndroidIconsFolder = value;
		}
	}
	
	public string PreBuiltIOSIconsFolder {
		get {
			return this.preBuiltIOSIconsFolder;
		}
		set {
			preBuiltIOSIconsFolder = value;
		}
	}
	
	public List<string> AndroidIcons {
		get {
			return this.androidIcons;
		}
		set {
			androidIcons = value;
		}
	}
	
	public List<string> IOSIcons {
		get {
			return this.iOSIcons;
		}
		set {
			iOSIcons = value;
		}
	}
	

	
	public List<string> ResIncludes {
		get {
			return this.resIncludes;
		}
		set {
			resIncludes = value;
		}
	}

	public string AndroidBillingBase64Key {
		get {
			return this.androidBillingBase64Key;
		}
		set {
			androidBillingBase64Key = value;
		}
	}

	public bool UseAnalyticsInAndroid {
		get {
			return this.useAnalyticsInAndroid;
		}
		set {
			useAnalyticsInAndroid = value;
		}
	}

	public bool UseAnalyticsInIOS {
		get {
			return this.useAnalyticsInIOS;
		}
		set {
			useAnalyticsInIOS = value;
		}
	}

	public bool UseAnalyticsInOTher {
		get {
			return this.useAnalyticsInOTher;
		}
		set {
			useAnalyticsInOTher = value;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public GTBuildSettings(GTBuildSettings build)
		:this(build.companyName, build.androidGameName, build.iOSGameName, build.wpGameName, build.bundleIdentifier, build.androidBundleVersion, build.iOSBundleVersion
		     , build.androidBundleVersionCode, build.androidBillingBase64Key, build.androidKeyStoreFBHash, build.cloudProjectID, build.appleID
		     , build.googlePlayServicesID, build.buildName, build.androidBuildFolder, build.iosBuildFolder, build.scenes
		     , build.preBuiltAndroidIconsFolder, build.preBuiltIOSIconsFolder, build.androidIcons, build.iOSIcons, build.resIncludes
		     , build.androidKeystoreNamePath, build.androidKeystorePass, build.androidKeyaliasName, build.androidKeyaliasPass){}

	public GTBuildSettings(string pCompany, string pAndGameName, string pIOSGameName, string pWPGameName, string pBundleId, string pABV, string pIBV
       	, int pBVC, string pAndroidBillingBase64Key, string pAndroidKeyStoreFBHash, string pCloudProjectID, string pAppleID, string pGPSID
       , string pBName, string pAndFolder, string pIOSFolder, List<GTBuildScene> pScenes, List<string> pAndroidIcons, List<string> pIOSIcons, List<string> pResIncludes
       , string pAndroidKeystoreNamePath, string pAndroidKeystorePass, string pAndroidKeyaliasName, string pAndroidKeyaliasPass)
		: this(pCompany,pAndGameName,pIOSGameName,pWPGameName,pBundleId,pABV,pIBV,pBVC,pAndroidBillingBase64Key,pAndroidKeyStoreFBHash,pCloudProjectID,pAppleID,pGPSID
		      , pBName,pAndFolder,pIOSFolder,pScenes, PRE_BUILT_ANDROID_ICONS_FOLDER, PRE_BUILT_IOS_ICONS_FOLDER
		      , pAndroidIcons, pIOSIcons, pResIncludes, pAndroidKeystoreNamePath, pAndroidKeystorePass, pAndroidKeyaliasName, pAndroidKeyaliasPass){}
	
	public GTBuildSettings() 
		: this("","","","","","","",1,"",DEFAULT_KEYSTORE_FB_HASH_FRILLSGAMES,"","","","","","",null, PRE_BUILT_ANDROID_ICONS_FOLDER, PRE_BUILT_IOS_ICONS_FOLDER, null, null
		      , null, "", "", "", ""){}
	
	public GTBuildSettings(string pCompany, string pAndGameName, string pIOSGameName, string pWPGameName, string pBundleId, string pABV, string pIBV
       , int pBVC,string pAndroidBillingBase64Key, string pAndroidKeyStoreFBHash, string pCloudProjectID, string pAppleID, string pGPSID
       , string pBName, string pAndFolder, string pIOSFolder, List<GTBuildScene> pScenes, string pPreBuiltAndroidIconsFolder, string pPreBuiltIOSIconsFolder
	                       , List<string> pAndroidIcons, List<string> pIOSIcons, List<string> pResIncludes, string pAndroidKeystoreNamePath, string pAndroidKeystorePass
	                       , string pAndroidKeyaliasName, string pAndroidKeyaliasPass){

		companyName = pCompany;
		androidGameName = pAndGameName;
		iOSGameName = pIOSGameName;
		wpGameName = pWPGameName;
		bundleIdentifier = pBundleId;
		androidBundleVersion = pABV;
		iOSBundleVersion = pIBV;
		androidBundleVersionCode = pBVC;
		androidKeyStoreFBHash = pAndroidKeyStoreFBHash;
		androidBillingBase64Key = pAndroidBillingBase64Key;
		buildName = pBName;
		androidBuildFolder = pAndFolder;
		iosBuildFolder = pIOSFolder;
		cloudProjectID = pCloudProjectID;
		appleID = pAppleID;
		googlePlayServicesID = pGPSID;

		androidKeystoreNamePath = pAndroidKeystoreNamePath;
		androidKeystorePass = pAndroidKeystorePass;
		androidKeyaliasName = pAndroidKeyaliasName;
		androidKeyaliasPass = pAndroidKeyaliasPass;
			
		if(pScenes != null){
			scenes = new List<GTBuildScene>();
			foreach(GTBuildScene s in pScenes)
				scenes.Add(new GTBuildScene(s));
		}
		else
			scenes = new List<GTBuildScene>();
		
		preBuiltAndroidIconsFolder = pPreBuiltAndroidIconsFolder;
		preBuiltIOSIconsFolder = pPreBuiltIOSIconsFolder;
		
		if(pAndroidIcons != null)
			androidIcons = new List<string>(pAndroidIcons);
		else
			androidIcons = new List<string>();
		
		if(pIOSIcons != null)
			iOSIcons = new List<string>(pIOSIcons);
		else
			iOSIcons = new List<string>();
		
	
		
		if(pResIncludes != null)
			resIncludes = new List<string>(pResIncludes);
		else
			resIncludes = new List<string>();
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[GTBuildSettings: scenes={0}, AndroidIOSPath={1}, AndroidIcons={2}, iOSPath={3}, iOSIcons={4}" +
		                      ", resourcesIncludes={5}]"
          , scenesToString(), preBuiltAndroidIconsFolder,androidIconsToString(), preBuiltIOSIconsFolder
          , iOSIconsToString(), ResIncludesToString());
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public string scenesToString(){
		string res = "[";
		
		for(int i=0; i<scenes.Count; i++){
			res += scenes[i];
			
			if(i<scenes.Count-1)
				res += ",";
		}
		res += "]";
		
		return res;
	}
	
	public string androidIconsToString(){
		string res = "[";
		
		for(int i=0; i<androidIcons.Count; i++){
			res += androidIcons[i];
			
			if(i<androidIcons.Count-1)
				res += ",";
		}
		res += "]";
		
		return res;
	}
	
	public string iOSIconsToString(){
		string res = "[";
		
		for(int i=0; i<iOSIcons.Count; i++){
			res += iOSIcons[i];
			
			if(i<iOSIcons.Count-1)
				res += ",";
		}
		res += "]";
		
		return res;
	}
	
	public string ResIncludesToString(){
		string res = "[";
		
		for(int i=0; i<resIncludes.Count; i++){
			res += resIncludes[i];
			
			if(i<resIncludes.Count-1)
				res += ",";
		}
		res += "]";
		
		return res;
	}
}
