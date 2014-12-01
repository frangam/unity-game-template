using UnityEngine;
using System.Collections;

public class SplashLogoCorporativo : MonoBehaviour {
	public float waitTime = 0.3f;
	public float waitTimeForWeb = 2f;

	void Start () {
		StartCoroutine(splash());
	}

	private IEnumerator splash(){
		waitTime = Application.isWebPlayer ? waitTimeForWeb : waitTime;
		float delay = audio.clip ? audio.clip.length + waitTime : waitTime;
		yield return new WaitForSeconds(delay);
		
		Application.LoadLevel(Configuration.SCENE_LOADER);
	}
}
