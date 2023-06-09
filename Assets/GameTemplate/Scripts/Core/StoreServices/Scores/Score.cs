﻿/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Score {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string nameLocKey;
	
	[SerializeField]
	private string id;

	[SerializeField]
	private ScoreFormat format;

	[SerializeField]
	private ScoreOrder order;

	[SerializeField]
	private bool useGreaterLimit;
	
	[SerializeField]
	private bool useLowerLimit;

	[SerializeField]
	[Tooltip("Optional")]
	private long greaterLimit;

	[SerializeField]
	[Tooltip("Optional")]
	private long lowerLimit;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private long value;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string NameLocKey {
		get {
			return this.nameLocKey;
		}
		set {
			nameLocKey = value;
		}
	}

	public string IdForSaveOniOSStore{
		get {  
			string res = this.id.Replace("-", "_");

			if(GameSettings.Instance.groupScores){ 
				res = res.Replace(GameSettings.Instance.prefixScoresGroupOnIOS, "");
				res = res.Insert(0, GameSettings.Instance.prefixScoresGroupOnIOS);
				GTDebug.log("id for ios: " +res + ". Prefix: " +GameSettings.Instance.prefixScoresGroupOnIOS);
			}

			return res;
		}
	}

	public string Id {
		get {
			string res = this.id;

//#if !UNITY_EDITOR && UNITY_IPHONE
//			if(GameSettings.Instance.groupScores) 
//				res = res.Insert(0, GameSettings.Instance.prefixScoresGroupOnIOS);
//#endif

			return res;
		}
		set {
			id = value;
		}
	}

	public ScoreFormat Format {
		get {
			return this.format;
		}
		set {
			format = value;
		}
	}

	public ScoreOrder Order {
		get {
			return this.order;
		}
		set {
			order = value;
		}
	}

	public long GreaterLimit {
		get {
			return this.greaterLimit;
		}
		set {
			greaterLimit = value;
		}
	}

	public long LowerLimit {
		get {
			return this.lowerLimit;
		}
		set {
			lowerLimit = value;
		}
	}

	public long Value {
		get {
			return this.value;
		}
		set {
			value = value;
		}
	}

	public bool UseGreaterLimit {
		get {
			return this.useGreaterLimit;
		}
		set {
			useGreaterLimit = value;
		}
	}

	public bool UseLowerLimit {
		get {
			return this.useLowerLimit;
		}
		set {
			useLowerLimit = value;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public Score(){
		nameLocKey = "";
		id = "";
		format = ScoreFormat.NUMERIC;
		order = ScoreOrder.DESCENDING;

		useGreaterLimit = false;
		greaterLimit = long.MaxValue;
		useLowerLimit = false;
		lowerLimit = long.MinValue;
	}

	public Score(string pId){
		nameLocKey = "";
		id = pId;
		format = ScoreFormat.NUMERIC;
		order = ScoreOrder.DESCENDING;
		
		useGreaterLimit = false;
		greaterLimit = long.MaxValue;
		useLowerLimit = false;
		lowerLimit = long.MinValue;
	}
}
