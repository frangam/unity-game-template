/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

/*
  GTAnalyticsHandler is a manager to handle Analytics
*/
public class GTAnalyticsHandler : PersistentSingleton<GTAnalyticsHandler> {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	#region Private Attributes
	private GoogleAnalyticsV4 	googleAnalytics		= null;
	private bool 				useAnalytics 		= false;
	#endregion
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity Methods
	public void Awake(){
		useAnalytics = GTBuildSettingsConfig.Instance.UseAnalytics;
		
		if(useAnalytics){
			googleAnalytics = FindObjectOfType<GoogleAnalyticsV4> ();
			
			useAnalytics = useAnalytics && googleAnalytics;
			
			if(googleAnalytics == null)
				GTDebug.logWarningAlways("Not found GoogleAnalyticsV4 prefab");
		}
	}
	#endregion
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	#region Public Methods
	public void logCurrentGameSection(){
		if(useAnalytics)
			googleAnalytics.LogScreen (BaseGameScreenController.Instance.Section.ToString ());
	}
	
	public void logEvent(string eventCategory, string eventAction){
		if(useAnalytics)
			googleAnalytics.LogEvent (new EventHitBuilder().SetEventCategory(eventCategory).SetEventAction(eventAction));
	}
	public void logEvent(string eventCategory, string eventAction, string eventLabel){
		if(useAnalytics)
			googleAnalytics.LogEvent (new EventHitBuilder().SetEventCategory(eventCategory).SetEventAction(eventAction).SetEventLabel(eventLabel));
	}
	public void logEvent(string eventCategory, string eventAction, long evenValue){
		if(useAnalytics)
			googleAnalytics.LogEvent (new EventHitBuilder().SetEventCategory(eventCategory).SetEventAction(eventAction).SetEventValue(evenValue));
	}
	public void logEvent(string eventCategory, string eventAction, string eventLabel, long eventValue){
		if(useAnalytics)
			googleAnalytics.LogEvent (eventCategory, eventAction, eventLabel, eventValue);
	}
	
	
	#endregion
}
