/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
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
	
	[SerializeField]
	private bool activeIfLoggedIn = false;
	
	
	
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
			gameObject.SetActive(GameSettings.Instance.USE_FACEBOOK);
			
			if(activeIfLoggedIn)
				gameObject.SetActive(FB.IsLoggedIn);
			break;
			
		case SocialNetwork.TWITTER:
			gameObject.SetActive(GameSettings.Instance.USE_TWITTER);
			break;
		}
		#endif
	}
	protected override void doPress ()
	{
		base.doPress ();
		
		BaseSocialController.Instance.post(network, shareLevelCompleted, doSnapshot);
	}
	
}
