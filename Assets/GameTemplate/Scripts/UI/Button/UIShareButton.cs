using UnityEngine;
using System.Collections;

public class UIShareButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private SocialNetwork network;
	
	[SerializeField]
	private bool shareLevelCompleted = false;
	
	[SerializeField]
	private bool doSnapshot = false;
	
	
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		
		#if UNITY_WP8
		gameObject.SetActive(false);
		#else
		switch(network){
		case SocialNetwork.FACEBOOK:
			gameObject.SetActive(FB.IsLoggedIn);
			break;
			
		case SocialNetwork.TWITTER:
			
			break;
		}
		#endif
	}
	public override void press (){
		base.press ();
		
		BaseSocialController.Instance.post(network, shareLevelCompleted, doSnapshot);
	}
}
