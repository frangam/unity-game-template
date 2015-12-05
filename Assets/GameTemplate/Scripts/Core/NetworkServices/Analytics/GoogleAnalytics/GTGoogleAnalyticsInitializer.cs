/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

/*
  GTGoogleAnalyticsInitializer is a class for initilizing GoogleAnalyticsV4
  attributes based on Game Multiversion Approach, because, for 1 project
  it is just enough with GoogleAnalyticsV4 prefab configuration.

  For this works fine it needs you put GTGoogleAnalyticsInitializer in an higher 
  order of execution of GoogleAnalyticsV4.

  So, go to Edit / Project Settings / Script Execution Order and put this script
  above Default time if it is not appear yet here.
*/
public class GTGoogleAnalyticsInitializer : MonoBehaviour {
	void Awake(){
		GoogleAnalyticsV4 ga = FindObjectOfType<GoogleAnalyticsV4>();

		if(ga == null){
			GTDebug.logError("Not found GoogleAnalyticsV4 instance");
			return;
		}

		if(GTBuildSettingsConfig.Instance.UseAnalytics){
			//Product Name
#if UNITY_IPHONE
			ga.productName = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.IOSGameName;
#elif UNITY_ANDROID
			ga.productName = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.AndroidGameName;
#elif UNITY_WP8
			ga.productName = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.WpGameName;
#endif

			//Bundle ID
			ga.bundleIdentifier = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.BundleIdentifier;

			//Bundle Version
#if UNITY_IPHONE
			ga.bundleVersion = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.IOSBundleVersion;
#elif UNITY_ANDROID
			ga.bundleVersion = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.AndroidBundleVersion;
#endif

			//Tracking codes
			ga.androidTrackingCode = GTGoogleAnalyticsSettings.Instance.CurrentIDsPack.androidTrackingID;
			ga.IOSTrackingCode = GTGoogleAnalyticsSettings.Instance.CurrentIDsPack.iOSTrackingID;
			ga.otherTrackingCode = GTGoogleAnalyticsSettings.Instance.CurrentIDsPack.otherTrackingID;
		}
		//if not use, we destroy immediately
		else{
			GameObject.DestroyImmediate(ga.gameObject);
		}
	}
}
