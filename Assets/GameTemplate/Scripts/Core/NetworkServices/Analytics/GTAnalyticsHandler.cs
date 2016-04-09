/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.Analytics;
using System.Collections;
using System.Collections.Generic;


/*
  GTAnalyticsHandler is a manager to handle Analytics
*/
public class GTAnalyticsHandler : PersistentSingleton<GTAnalyticsHandler> {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string ACTION 	= "Action";
	public const string MESSAGE = "Message";
	public const string VALUE 	= "Value";

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	#region Private Attributes
//	private GoogleAnalyticsV4 	googleAnalytics		= null;
	private bool 				useAnalytics 		= false;
	#endregion
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity Methods
	protected override void Awake(){
		useAnalytics = GTBuildSettingsConfig.Instance.UseAnalytics;
		
		if(useAnalytics){
//			googleAnalytics = FindObjectOfType<GoogleAnalyticsV4> ();
			
//			useAnalytics = useAnalytics && googleAnalytics;
//			
//			if(googleAnalytics == null)
//				GTDebug.logWarningAlways("Not found GoogleAnalyticsV4 prefab");
		}
	}
	
	public void Start(){
		startSession ();
	}
	
	
	#endregion
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	#region Public Methods
	public void startSession(){
//		if (useAnalytics)
//			googleAnalytics.StartSession();
	}
	
	public void stopSession(){
//		if (useAnalytics)
//			googleAnalytics.StopSession();
	}
	
	public void logCurrentGameSection(GameSection section){
		if (useAnalytics)
//			googleAnalytics.LogScreen (BaseGameScreenController.Instance.Section.ToString ());
			Analytics.CustomEvent (GAEventCategories.SCENE, new Dictionary<string, object> {
				{ section.ToString(), GAEventActions.LOADED }
			});
	}

	public void logEvent(string eventCategory, Dictionary<string, object> data){
		if(useAnalytics)
			Analytics.CustomEvent (eventCategory, data);
	}
	
	public void logEvent(string eventCategory, string eventAction){
		if(useAnalytics)
//			googleAnalytics.LogEvent (new EventHitBuilder().SetEventCategory(eventCategory).SetEventAction(eventAction));
			Analytics.CustomEvent (eventCategory, new Dictionary<string, object> {
				{ ACTION, eventAction }
			});
	}
	public void logEvent(string eventCategory, string eventAction, string eventLabel){
		if(useAnalytics)
//			googleAnalytics.LogEvent (new EventHitBuilder().SetEventCategory(eventCategory).SetEventAction(eventAction).SetEventLabel(eventLabel));
			Analytics.CustomEvent (eventCategory, new Dictionary<string, object> {
				{ ACTION, eventAction },
				{ MESSAGE, eventLabel }
			});
	}
	public void logEvent(string eventCategory, string eventAction, long eventValue){
		if(useAnalytics)
//			googleAnalytics.LogEvent (new EventHitBuilder().SetEventCategory(eventCategory).SetEventAction(eventAction).SetEventValue(evenValue));
			Analytics.CustomEvent (eventCategory, new Dictionary<string, object> {
				{ ACTION, eventAction },
				{ VALUE, eventValue }
			});

	}
	public void logEvent(string eventCategory, string eventAction, string eventLabel, long eventValue){
		if(useAnalytics)
//			googleAnalytics.LogEvent (eventCategory, eventAction, eventLabel, eventValue);
			Analytics.CustomEvent (eventCategory, new Dictionary<string, object> {
				{ ACTION, eventAction },
				{ MESSAGE, eventLabel },
				{ VALUE, eventValue }
			});
	}
	
	
	#endregion
}
