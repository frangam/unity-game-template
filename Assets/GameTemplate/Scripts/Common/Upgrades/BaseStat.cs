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
	private string id;
	
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
	public string Id {
		get {
			return this.id;
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
	/// ID| NAME | Invert min max value (1 or 0)| Min VALUE : MAX VALUE | SIMULATED MIN VALUE : SIMULATED MAX VALUE
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public BaseStat(string attributes){
		string[] att = attributes.Split(ATTRIBUTES_SEPARATOR);
		
		if(att.Length > 0){
			id = att[0];
			
			//REAL VALUES
			if(att.Length > 2){
				float v;
				
				if(att.Length > 2){
					name = att[1];
				}
				
				//inver values
				int invert;
				if(int.TryParse(att[2], out invert)){
					invertMinMax = invert == 0 ? false: true;
				}
				
				//VALUES
				string[] pValues = att[3].Split(LIST_SEPARATOR);
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
			if(att.Length > 4){
				float v;
				
				//				//inver values
				//				int invert;
				//				if(int.TryParse(att[2], out invert)){
				//					invertMinMax = invert == 0 ? false: true;
				//				}
				
				//VALUES
				string[] pValues = att[4].Split(LIST_SEPARATOR);
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
	public override string ToString (){
		return string.Format ("[BaseStat: id={0}, name={1}, initialValue={2}, maxValue={3}, currentValue={4}]", id, name, initialValue, maxValue, currentValue);
	}
	
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public bool loadedCorrectly(){
		return !string.IsNullOrEmpty(id) && initialValue != null && maxValue != null && currentValue != null;
	}
	
	
}
