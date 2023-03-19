/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class SplashCorporativeLogo : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	public float waitTime = 0.3f;

	[SerializeField]
	private GameSection sectionToGo;
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Start(){
		StartCoroutine(splash());
	}


	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private IEnumerator splash(){
		float delay = GetComponent<AudioSource>().clip ? GetComponent<AudioSource>().clip.length + waitTime : waitTime;
		yield return new WaitForSeconds(delay);

//
//		float delay = GetComponent<AudioSource>().clip ? GetComponent<AudioSource>().clip.length + waitTime : waitTime;
//		float startTime = 0, currentTime = 0, elapsedTime = 0;
//		startTime = Time.realtimeSinceStartup;
////		StartCoroutine(showProgressBySeconds((int)WAIT_TIME_TO_INIT_INAPP));
//		do{
//			currentTime = Time.realtimeSinceStartup;
//			elapsedTime = currentTime-startTime;
//			yield return null;
//		}
//		while(elapsedTime<delay);
		
		ScreenLoaderVisualIndicator.Instance.LoadScene(sectionToGo, false);
	}
}
