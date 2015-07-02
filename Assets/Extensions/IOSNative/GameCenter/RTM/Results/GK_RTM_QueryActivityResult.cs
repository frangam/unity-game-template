using UnityEngine;
using System.Collections;

public class GK_RTM_QueryActivityResult : ISN_Result {

	private int _Activity = 0;
	
	public GK_RTM_QueryActivityResult(int activity):base(true) {
		_Activity = activity;
	}
	
	public GK_RTM_QueryActivityResult(string errorData):base(false) {
		SetErrorData(errorData);
	}
	
	
	public int Activity {
		get {
			return _Activity;
		}
	}
}
