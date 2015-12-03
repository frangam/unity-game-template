/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Typewriter effect
/// </summary>
[RequireComponent(typeof(Text))]
[RequireComponent(typeof(Button))]
public class UINumberProgressEffect : UIBaseButton {
	public bool addThousandPoint = true;
	public long currentNumber = 0;
	public long targetNumber = 100;
	public float percentagIncrement = 0.3f;

	[Tooltip("True if a window handle the typing initialization")]
	public bool managedByWindow = true;
	public bool initStoped = false;
	public bool allowStop = true;
	public float effectPause = 0.2f;
	public List<string> soundIDs;
//	public AudioClip[] sounds;
	public bool loopMessage = false;
	public bool emptyWhenLoop = true;

	
	private string message;
	private string stoppedMessage;
	private Text lbMessage;
	private bool stopType = false;
	private bool initedTyping = false;

	public bool isLooped{
		get{return loopMessage;}
	}
	public bool isFinished{
		get{return stopType;}
	}
	
	
	void Start(){
		if(!initStoped && !managedByWindow)
			initType(currentNumber, targetNumber);

		percentagIncrement = Mathf.Clamp(percentagIncrement, 0.1f, 1f);
	}


	
	private void init(){
		lbMessage = GetComponent<Text>();
		message = currentNumber.ToString();
		stoppedMessage = targetNumber.ToString();
		lbMessage.text = "";
		stopType = false;
		
//		if(initStoped){
//			lbMessage.text = stoppedMessage;
//		}
	}
	
	IEnumerator TypeText () {
		if(emptyWhenLoop)
			lbMessage.text = "";

		long totalStepsByOne = (targetNumber-currentNumber); //by one step
		long increment = (long) (totalStepsByOne*percentagIncrement);

		if(increment > 0){
			long totalSteps = totalStepsByOne/increment;


			for (int i=0; i<totalSteps; i++) {
				string msg = "";

				if(addThousandPoint)
					msg = currentNumber.ToString("N0", CultureInfo.CurrentCulture);
				else
					 msg = currentNumber.ToString();

				lbMessage.text = msg;

				if (soundIDs != null && soundIDs.Count > 0 && PlayerPrefs.GetFloat(GameSettings.PP_SOUND) > 0f){
					int soundIndex = Random.Range(0, soundIDs.Count);
					string id = soundIDs[soundIndex];
					BaseSoundManager.Instance.play(id);
				}
	//			else if (sounds != null && sounds.Length > 0 && PlayerPrefs.GetFloat(GameSettings.PP_SOUND) > 0f){
	//				GetComponent<AudioSource>().PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
	//			}
				
				yield return 0;
				yield return StartCoroutine(TimeUtils.WaitForRealSeconds( effectPause ) );

				if(currentNumber<targetNumber)
					currentNumber += increment;

				if(stopType)
					break;

			}      
			
			if(loopMessage && !stopType){
				StartCoroutine(TypeText ());
			}
			else{
				lbMessage.text = stoppedMessage;
				stopType = true;
			}
		}
		else{
			lbMessage.text = stoppedMessage;
			stopType = true;
		}
	}
	
	public virtual void initType(long initialValue, long nextValue){
		currentNumber = initialValue;
		targetNumber = nextValue;

		init();
		
		stopType = false;
		StartCoroutine(TypeText ());
		initedTyping = true;
	}

	public virtual void stop(){
		if(!stopType){
			stopType = true;
			initedTyping = false;
			
			lbMessage.text = stoppedMessage;
			
//			if (sounds != null && sounds.Length > 0 && PlayerPrefs.GetFloat(GameSettings.PP_SOUND) > 0f){
//				GetComponent<AudioSource>().PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
//			}
		}
	}
	
	protected override bool canPress (){
		return allowStop && base.canPress ();;
	}
	
	protected override void doPress ()
	{
		base.doPress ();
		stop();
	}


}
