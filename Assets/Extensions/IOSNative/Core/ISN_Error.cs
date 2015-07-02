using UnityEngine;
using System.Collections;

public class ISN_Error {

	public int code;
	public string description;


	public ISN_Error() {

	}

	public ISN_Error(string errorData) {
		string[] data = errorData.Split(IOSNative.DATA_SPLITTER[0]);

		code = System.Convert.ToInt32(data[0]);
		description = data[1];
	}


}
