/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;


/*
  GAEventLabels is utility class represents event labels to classify
  events better.

  If we have an specific game that needs some more event labels, 
  we can inherits from this class.
*/
public class GAEventLabels : MonoBehaviour {
	//--------------------------------------
	// Constants
	//--------------------------------------
	#region Constants
	public const string GAMEOVER 		= "Game Over";
	public const string GAMEPLAY 		= "Game Play";
	public const string BTN_SHOW_AD 	= "Btn Show Ad";
	#endregion
}