using UnityEngine;
using System.Collections;

public class SplashCorporativeLogo : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	public float waitTime = 0.3f;
	public float waitTimeForWeb = 2f;

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Start () {
		StartCoroutine(splash());
	}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private IEnumerator splash(){
		waitTime = Application.isWebPlayer ? waitTimeForWeb : waitTime;
		float delay = audio.clip ? audio.clip.length + waitTime : waitTime;
		yield return new WaitForSeconds(delay);
		
		Application.LoadLevel(GameSettings.SCENE_LOADER);
	}
}
