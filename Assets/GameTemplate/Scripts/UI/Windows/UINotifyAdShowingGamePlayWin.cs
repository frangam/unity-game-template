/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnionAssets.FLE;

[RequireComponent(typeof(CanvasGroup))]
public class UINotifyAdShowingGamePlayWin : UIBaseWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Text lbSecondsToShowAd;

	[SerializeField]
	private bool useLocalization = true;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private CanvasGroup cvGroup;


	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		cvGroup = GetComponent<CanvasGroup>();
		cvGroup.alpha = 0;
	}

	public override void OnEnable ()
	{
		base.OnEnable ();
		subscribeEvents();
	}

	public override void OnDisable ()
	{
		base.OnDisable ();
		unsubscribeEvents();
	}

	public override void OnDestroy ()
	{
		base.OnDestroy ();
		unsubscribeEvents();
	}

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void subscribeEvents(){
		BaseGameManager.dispatcher.addEventListener(BaseGameManager.LAUNCHING_AD_DURING_GAMEPLAY_IN_X_SECS, OnLaunchingAd);
	}

	private void unsubscribeEvents(){
		BaseGameManager.dispatcher.removeEventListener(BaseGameManager.LAUNCHING_AD_DURING_GAMEPLAY_IN_X_SECS, OnLaunchingAd);
	}

	private IEnumerator showProgress(bool notInterrupt = false){
		bool canShow = notInterrupt || (!notInterrupt && GameController.Instance.Manager.canShowAdDuringGamePlay());

		if(canShow){
			cvGroup.alpha = 1;

			//using WaitForSeconds because we want to do it with time scale and not ignoring
			for(int i=GameSettings.Instance.NOTIFY_AD_DURING_GAMEPLAY_WILL_BE_SHOWN_IN_NEXT_SECONDS; i>0 && canShow; i--){
				lbSecondsToShowAd.text = useLocalization ? Localization.Get(ExtraLocalizations.AD_IN) + " " + i.ToString() : i.ToString();
				canShow = notInterrupt || (!notInterrupt && GameController.Instance.Manager.canShowAdDuringGamePlay());
				yield return new WaitForSeconds(1);
			}
		}

		cvGroup.alpha = 0;
	}

	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	public void OnLaunchingAd(CEvent e){
		if(lbSecondsToShowAd){
			bool notInterrupt = (bool) e.data;
			StartCoroutine(showProgress(notInterrupt));
		}
	}
}