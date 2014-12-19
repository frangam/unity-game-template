using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UpgradableStat<S, T> : BaseStat<T> where T: MyGenericType{
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private S price;
	private T valueIncrement;
	
	
	//--------------------------------------
	// Getters/MyGenericTypeetters
	//--------------------------------------
	public S Price {
		get {
			return this.price;
		}
		set {
			price = value;
		}
	}
	
	public T ValueIncrement {
		get {
			return this.valueIncrement;
		}
		set {
			valueIncrement = value;
		}
	}
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="UpgradableMyGenericTypetat`2"/> class.
	/// 
	/// ID,VALUE, MAX VALUE, PRICE, VALUE INCREMENT
	/// </summary>
	/// <param name="attributes">Attributes.</param>
	public UpgradableStat(string attributes): base(attributes){
		string[] att = attributes.Split(ATTRIBUTES_SEPARATOR);
		
		if(att.Length > 4){
			S p = default(S);
			T i = default(T);
			
			if(Casting.TryCast(att[3], out p))
				price = p;
			
			if(Casting.TryCast(att[4], out i))
				valueIncrement = i;
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override string ToString (){
		return string.Format ("[UpgradableStat: price={0}, valueIncrement={1}, {2}]", price, valueIncrement,base.ToString());
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Apply the increment to the current value and return de final money
	/// </summary>
	/// <param name="totalMoney">Total money.</param>
	public S apply(S totalMoney){
		S finalMoney = totalMoney;
		T finalValue = (T)(Value+valueIncrement);
		
		
		if((finalValue <= MaxValue)
		   && (totalMoney >= price)){
			finalMoney = (S)(finalMoney-price);
			Value = (T)(Value + valueIncrement);
		}
		
		return finalMoney;
	}
}

#region GENERIC EXPANMyGenericTypeION

public class UpgradableStat_Int_Int : UpgradableStat<int, int>{
	public UpgradableStat_Int_Int(string attributes): base(attributes){
		
	}
}
#endregion
