using UnityEngine;
using System.Collections;

public class TimeFromServerExample : MonoBehaviour {
	
	void Start () {
		string dateTime = "";
		StartCoroutine(TimeUtils.getDateTimeFromServer(out dateTime));

		Debug.Log("Time on the server is now: " + dateTime);
	}

}
