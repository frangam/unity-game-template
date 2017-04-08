using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDataResult {
	//--------------------------------------
	// Attributes
	//--------------------------------------
	protected BaseError 	error 	= null;
	protected bool 		success	= true;

	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public bool Success {
		get {
			return this.success;
		}
	}
	public bool Fail {
		get {
			return !this.success;
		}
	}
	public BaseError Error {
		get {
			return this.error;
		}
		set {
			error = value;
			success = false;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public BaseDataResult(bool pSuccess) {
		success = pSuccess;
	}

	public BaseDataResult (BaseError e) {
		error = e;
	}
}
