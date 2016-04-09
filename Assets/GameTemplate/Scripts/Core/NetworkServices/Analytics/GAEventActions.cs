/***************************************************************************
Project:     Game Template
Copyright (c) Altasy
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

/*
  GAEventActions is utility class represents event actions to classify
  events better.

  If we have an specific game that needs some more event actions, 
  we can inherits from this class.
*/
public class GAEventActions : MonoBehaviour {
	//--------------------------------------
	// Constants
	//--------------------------------------
	#region Constants
	public const string LOADED 			= "Loaded";
	public const string PRESSED 		= "Pressed";
	public const string OPENED 			= "Opened";
	public const string CLOSED 			= "Closed";
	public const string EXITED 			= "Exited";
	public const string LOCKED 			= "Locked";
	public const string UNLOCKED 		= "Unlocked";
	public const string PURCHASED 		= "Purchased";
	public const string CANCELED 		= "Canceled";
	public const string FAILED	 		= "Failed";
	public const string REJECTED 		= "Rejected";
	public const string RESTORED 		= "Restored";
	public const string NOT_RESTORED 	= "Not Restored";
	public const string GOT 			= "Got";
	public const string PAUSED 			= "Paused";
	public const string RESUMED 		= "Resumed";
	public const string RETRIED 		= "Retried";
	public const string SENT 			= "Sent";
	public const string UPDATED 		= "Updated";
	public const string STARTED 		= "Started";
	public const string FORCED 			= "Forced";
	public const string QUIT 			= "Quit";
	public const string ENABLED 		= "Enabled";
	public const string DISABLED 		= "Disabled";
	public const string ALLOWED 		= "Allowed";
	public const string DENIED 			= "Denied";
	public const string STOPPED 		= "Stopped";
	public const string SHOWN 			= "Shown";
	public const string HIDDEN 			= "Hidden";
	public const string COMPLETED		= "Completed";
	public const string INCOMPLETED		= "Incompleted";
	public const string REQUESTED		= "Requested";
	public const string SKIPPED			= "Skipped";
	public const string REWARDED		= "Rewarded";
	#endregion
}