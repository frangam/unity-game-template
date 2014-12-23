using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Typewriter effect
/// </summary>
[RequireComponent(typeof(Text))]
[RequireComponent(typeof(Button))]
public class AutoType : UIBaseButton {
	public float letterPause = 0.2f;
//	public string[] soundIDs;
	public AudioClip[] sounds;
	public bool loopMessage = false;
	public bool emptyWhenLoop = true;
	public string localizationMessageKey;

	private string message;
	private Text lbMessage;
	private bool stopType = false;

	// Use this for initialization
	void Awake () {
		lbMessage = GetComponent<Text>();
		message = Localization.Get(localizationMessageKey);
		lbMessage.text = "";
		stopType = false;
	}

	void Start(){
		StartCoroutine(TypeText ());
	}
	
	IEnumerator TypeText () {
		if(emptyWhenLoop)
			lbMessage.text = "";

		foreach (char letter in message.ToCharArray()) {
			lbMessage.text += letter;

//			if (soundIDs != null && soundIDs.Length > 0 && PlayerPrefs.GetFloat(GameSettings.PP_SOUND) > 0f){
			if (sounds != null && sounds.Length > 0 && PlayerPrefs.GetFloat(GameSettings.PP_SOUND) > 0f){
//				int soundIndex = Random.Range(0, soundIDs.Length);
//				string id = soundIDs[soundIndex];
//				BaseSoundManager.Instance.play(id);
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

	public override void press (){
		base.press ();

		if(!stopType){
			stopType = true;
			lbMessage.text = message;

			if (sounds != null && sounds.Length > 0 && PlayerPrefs.GetFloat(GameSettings.PP_SOUND) > 0f){
				audio.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
			}
		}
	}
}
