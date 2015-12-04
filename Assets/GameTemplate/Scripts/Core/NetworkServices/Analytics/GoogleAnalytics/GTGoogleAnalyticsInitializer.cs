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
		//Tracking codes
		GoogleAnalyticsV4.instance.androidTrackingCode = GTGoogleAnalyticsSettings.Instance.CurrentIDsPack.androidTrackingID;
		GoogleAnalyticsV4.instance.IOSTrackingCode = GTGoogleAnalyticsSettings.Instance.CurrentIDsPack.iOSTrackingID;
		GoogleAnalyticsV4.instance.otherTrackingCode = GTGoogleAnalyticsSettings.Instance.CurrentIDsPack.otherTrackingID;

		//Product Name
#if UNITY_IPHONE
		GoogleAnalyticsV4.instance.productName = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.IOSGameName;
#elif UNITY_ANDROID
		GoogleAnalyticsV4.instance.productName = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.AndroidGameName;
#elif UNITY_WP8
		GoogleAnalyticsV4.instance.productName = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.WpGameName;
#endif

		//Bundle ID
		GoogleAnalyticsV4.instance.bundleIdentifier = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.BundleIdentifier;

		//Bundle Version
#if UNITY_IPHONE
		GoogleAnalyticsV4.instance.bundleVersion = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.IOSBundleVersion;
#elif UNITY_ANDROID
		GoogleAnalyticsV4.instance.bundleVersion = GTBuildSettingsConfig.Instance.CurrentBuildPack.build.AndroidBundleVersion;
#endif



	}
}
