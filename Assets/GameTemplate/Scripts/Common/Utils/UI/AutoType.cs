using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Typewriter effect
/// </summary>
[RequireComponent(typeof(Text))]
[RequireComponent(typeof(Button))]
public class AutoType : UIBaseButton {
	[Tooltip("True if a window handle the typing initialization")]
	public bool managedByWindow = true;
	public bool initStoped = false;
	public bool allowStop = true;
	public bool whenStopPutMessageForStopped = true;
	public string localForStopped;
	public float letterPause = 0.2f;
	public List<string> soundIDs;
	public AudioClip[] sounds;
	public bool loopMessage = false;
	public bool emptyWhenLoop = true;
	public string localizationMessageKey;
	
	private string message;
	private string stoppedMessage;
	private Text lbMessage;
	private bool stopType = false;
	private bool initedTyping = false;
	
	
	void Start(){
		if(!initStoped && !managedByWindow)
			initType();
	}
	
	private void init(){
		lbMessage = GetComponent<Text>();
		message = !string.IsNullOrEmpty(localizationMessageKey) ? Localization.Get(localizationMessageKey): lbMessage.text;
		stoppedMessage = !string.IsNullOrEmpty(localForStopped) ? Localization.Get(localForStopped) : "";
		lbMessage.text = "";
		stopType = false;
		
		if(initStoped){
			lbMessage.text = stoppedMessage;
		}
	}
	
	IEnumerator TypeText () {
		if(emptyWhenLoop)
			lbMessage.text = "";
		
		foreach (char letter in message.ToCharArray()) {
			lbMessage.text += letter;
			
			if (soundIDs != null && soundIDs.Count > 0 && PlayerPrefs.GetFloat(GameSettings.PP_SOUND) > 0f){
				int soundIndex = Random.Range(0, soundIDs.Count);
				string id = soundIDs[soundIndex];
				BaseSoundManager.Instance.play(id);
			}
			else if (sounds != null && sounds.Length > 0 && PlayerPrefs.GetFloat(GameSettings.PP_SOUND) > 0f){
				audio.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
			}
			
			yield return 0;
			yield return new WaitForSeconds (letterPause);
			
			if(stopType)
				break;
		}      
		
		if(loopMessage && !stopType){
			StartCoroutine(TypeText ());
		}
	}
	
	public virtual void initType(){
		init();
		
		stopType = false;
		StartCoroutine(TypeText ());
		initedTyping = true;
	}
	
	public virtual void stop(){
		if(!stopType){
			stopType = true;
			initedTyping = false;
			
			lbMessage.text = whenStopPutMessageForStopped ? stoppedMessage : message;
			
			if (sounds != null && sounds.Length > 0 && PlayerPrefs.GetFloat(GameSettings.PP_SOUND) > 0f){
				audio.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
			}
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
