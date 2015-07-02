using UnityEngine;
using System.Collections;

public class ISN_Result  {


	public ISN_Error error = null;
	protected bool _IsSucceeded = true;



	public ISN_Result(bool IsResultSucceeded) {
		_IsSucceeded = IsResultSucceeded;
	}

	public void SetError(ISN_Error e) {
		error = e;
		_IsSucceeded = false;
	}


	public void SetErrorData(string errorData) {
		ISN_Error e =  new ISN_Error(errorData);
		SetError(e);
	}

	public bool IsSucceeded {
		get {
			return _IsSucceeded;
		}
	}

	public bool IsFailed {
		get {
			return !_IsSucceeded;
		}
	}
}
