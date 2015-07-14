using UnityEngine;
using System.Collections;

public class TimeFromServerExample : MonoBehaviour {
	public string serverURL = "http://www.frillsgames.com/gettime.php";

	IEnumerator Start () {
		CoroutineWithData cd = new CoroutineWithData(this, TimeUtils.getDateTimeFromServer(serverURL));
		yield return cd.coroutine;
		Debug.Log("result is " + cd.result);
//		StartCoroutine(TimeUtils.getDateTimeFromServer());
	}

}
