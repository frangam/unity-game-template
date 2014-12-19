using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//public class BaseStatInt : BaseStat<int>{}
//public class BaseStatFloat : BaseStat<float>{}
//public class BaseStatString : BaseStat<string>{}
//public class BaseStatBool : BaseStat<bool>{}
//
//public class BaseStatListInt : BaseStat<List<int>>{}
//public class BaseStatListFloat : BaseStat<List<float>>{}
//public class BaseStatListString : BaseStat<List<string>>{}
//public class BaseStatListBool : BaseStat<List<bool>>{}

public class BaseStat<MyGenericType> {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const char ATTRIBUTES_SEPARATOR = ',';

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private string id;
	private MyGenericType initialValue;
	private MyGenericType maxValue;
	private MyGenericType value;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string Id {
		get {
			return this.id;
		}
	}

	public MyGenericType Value {
		get {
			return this.value;
		}
		set{
			this.value = value;
		}
	}

	public MyGenericType InitialValue {
		get {
			return this.initialValue;
		}
	}

	public MyGenericType MaxValue {
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
	/// ID,VALUE, MAX VALUE
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public BaseStat(string attributes){
		string[] att = attributes.Split(ATTRIBUTES_SEPARATOR);

		if(att.Length > 0){
			id = att[0];

			if(att.Length > 1){
				MyGenericType v = default(MyGenericType);

				if(Casting.TryCast(att[1], out v)){
					value = v;
					initialValue = v;
				}

				if(att.Length > 2){
					if(Casting.TryCast(att[2], out v)){
						maxValue = v;
					}
				}
			}
		}
	}

	public BaseStat(string pId, MyGenericType pValue){
		id = pId;
		value = pValue;
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString (){
		return string.Format ("[BaseStat: Id={0}, Value={1}]", Id, Value);
	}
	
}
