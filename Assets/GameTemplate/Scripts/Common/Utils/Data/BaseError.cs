using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseError {
	//--------------------------------------
	// Attributes
	//--------------------------------------
	protected int 		code;
	protected string 	description;

	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public int Code {
		get {
			return this.code;
		}
	}

	public string Description {
		get {
			return this.description;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public BaseError(int pCode, string pDescription) {
		code = pCode;
		description = pDescription;
	} 
}
