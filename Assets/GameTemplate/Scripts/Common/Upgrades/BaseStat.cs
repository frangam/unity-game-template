using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class BaseStat{
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const char ATTRIBUTES_SEPARATOR = '|';
	public const char LIST_SEPARATOR = ':';
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	[SerializeField]
	private string name;
	
	[SerializeField]
	[Tooltip("Who is the id of the owner of this stat")]
	private string statOwnerID;
	
	[SerializeField]
	private string statID;
	
	[SerializeField]
	private float initialValue;
	
	[SerializeField]
	private float currentValue;
	
	[SerializeField]
	private float maxValue;
	
	[SerializeField]
	private float minValue;
	
	[SerializeField]
	private float initialSimValue;
	
	[SerializeField]
	private float currentSimValue;
	
	[SerializeField]
	private float maxSimValue;
	
	[SerializeField]
	private float minSimValue;
	
	[SerializeField]
	[Tooltip("If true, indicate in the string max value before min value")]
	private bool invertMinMax = false;
	
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string StatOwnerID {
		get {
			return this.statOwnerID;
		}
	}
	public string StatId {
		get {
			return this.statID;
		}
	}
	
	public string Name {
		get {
			return this.name;
		}
	}
	
	public float CurrentValue {
		get {
			return this.currentValue;
		}
		set{
			this.currentValue = value;
		}
	}
	
	public float InitialValue {
		get {
			return this.initialValue;
		}
	}
	
	public float MinValue {
		get {
			return this.minValue;
		}
	}
	
	public float MaxValue {
		get {
			return this.maxValue;
		}
		set {
			maxValue = value;
		}
	}
	
	public float InitialSimValue {
		get {
			return this.initialSimValue;
		}
		set {
			initialSimValue = value;
		}
	}
	
	public float CurrentSimValue {
		get {
			return this.currentSimValue;
		}
		set {
			currentSimValue = value;
		}
	}
	
	public float MaxSimValue {
		get {
			return this.maxSimValue;
		}
		set {
			maxSimValue = value;
		}
	}
	
	public float MinSimValue {
		get {
			return this.minSimValue;
		}
		set {
			minSimValue = value;
		}
	}
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseStat`1"/> class.
	/// 
	/// STAT OWNER ID | STAT ID| NAME | Invert min max value (1 or 0)| Min VALUE : MAX VALUE | SIMULATED MIN VALUE : SIMULATED MAX VALUE
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public BaseStat(string attributes){
		string[] att = attributes.Split(ATTRIBUTES_SEPARATOR);
		
		if(att.Length > 1){
			statOwnerID = att[0];
			statID = att[1];
			
			//REAL VALUES
			if(att.Length > 2){
				float v;
				
				if(att.Length > 2){
					name = att[2];
				}
				
				//inver values
				int invert;
				if(int.TryParse(att[3], out invert)){
					invertMinMax = invert == 0 ? false: true;
				}
				
				//VALUES
				string[] pValues = att[4].Split(LIST_SEPARATOR);
				float pIV, pMV;
				string value1 = pValues[0];
				string value2 = pValues[1];
				
				if(float.TryParse(value1, out v)){
					if(!invertMinMax){
						minValue = v;
					}
					else{
						maxValue = v;
					}
				}
				
				if(pValues.Length > 1){
					if(float.TryParse(value2, out v)){
						if(!invertMinMax){
							maxValue = v;
						}
						else{
							minValue = v;
						}
					}
				}
				
				initialValue = minValue;
				currentValue = minValue;
			}
			
			
			//SIMULATED VALUES
			if(att.Length > 5){
				float v;
				
				//				//inver values
				//				int invert;
				//				if(int.TryParse(att[2], out invert)){
				//					invertMinMax = invert == 0 ? false: true;
				//				}
				
				//VALUES
				string[] pValues = att[5].Split(LIST_SEPARATOR);
				float pIV, pMV;
				string value1 = pValues[0];
				string value2 = pValues[1];
				
				if(float.TryParse(value1, out v)){
					//					if(!invertMinMax){
					minSimValue = v;
					//					}
					//					else{
					//						maxSimValue = v;
					//					}
				}
				
				if(pValues.Length > 1){
					if(float.TryParse(value2, out v)){
						//						if(!invertMinMax){
						maxSimValue = v;
						//						}
						//						else{
						//							minSimValue = v;
						//						}
					}
				}
				
				initialSimValue = minSimValue;
				currentSimValue = minSimValue;
			}
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="BaseStat"/>.
	/// 
	/// STAT OWNER ID | STAT ID | NAME | Invert min max value (1 or 0)| Min VALUE : MAX VALUE | SIMULATED MIN VALUE : SIMULATED MAX VALUE
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="BaseStat"/>.</returns>
	public override string ToString (){
		int invert = invertMinMax ? 1: 0;
		
		return statOwnerID + ATTRIBUTES_SEPARATOR + statID + ATTRIBUTES_SEPARATOR + name + ATTRIBUTES_SEPARATOR + invert + ATTRIBUTES_SEPARATOR + 
			minValue + ":" + maxValue + ATTRIBUTES_SEPARATOR + minSimValue + ":" + maxSimValue;
	}
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public bool loadedCorrectly(){
		return !string.IsNullOrEmpty(statID) && initialValue != null && maxValue != null && currentValue != null;
	}
	
	
}
