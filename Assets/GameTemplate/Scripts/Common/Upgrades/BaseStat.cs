using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class BaseStat{
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const char ATTRIBUTES_SEPARATOR = ',';
	
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
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="BaseStat`1"/> class.
	/// 
	/// ID, NAME, VALUE, MAX VALUE
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public BaseStat(string attributes){
		string[] att = attributes.Split(ATTRIBUTES_SEPARATOR);
		
		if(att.Length > 0){
			id = att[0];
			
			if(att.Length > 2){
				float v;

				if(att.Length > 3){
					name = att[3];
				}

				if(Casting.TryCast(att[2], out v)){
					currentValue = v;
					initialValue = v;
				}
				
				if(att.Length > 3){
					if(Casting.TryCast(att[3], out v)){
						maxValue = v;
					}
				}
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
