using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartScene : Singleton<StartScene> {
	[SerializeField]
	private Section section = Section.MAIN_MENU;

	public Section Section {
		get {
			return this.section;
		}
	}

	void Awake(){

	}

	
	void Start () {
#if  (UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8)
		//refresh banner in every section loaded
		AdsHandler.Instance.refrescarBanner();
#endif


		ScreenLoaderIndicator.Instance.finCarga ();

		if(section != Section.GAME)
			Time.timeScale = 1f;

		switch(section){


		case Section.MAIN_MENU:
			GestorSonidos.Instance.stop (GestorSonidos.ID_SONIDO.MUS_IN_GAME);
			GestorSonidos.Instance.play (GestorSonidos.ID_SONIDO.MUS_MENU);

			break;

		case Section.GAME:
			GestorSonidos.Instance.stop (GestorSonidos.ID_SONIDO.MUS_MENU);
			GestorSonidos.Instance.play (GestorSonidos.ID_SONIDO.MUS_IN_GAME);


			break;

		case Section.LEVEL_SELECTION:
			if(Configuration.firstTimePlayerInviteFriends && SPFacebook.instance.IsLoggedIn){
				Configuration.firstTimePlayerInviteFriends = false;
				SPFacebook.instance.AppRequest (Localization.Get(ExtraLocalizations.FACEBOOK_INVITATION));
			}
			break;
		
		}
	}

	#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_ANDROID)
	void OnApplicationPause(bool paused){
		if(!paused){ //resume
			ScreenLoaderIndicator.Instance.finCarga();
		}
	}
	#endif
}
