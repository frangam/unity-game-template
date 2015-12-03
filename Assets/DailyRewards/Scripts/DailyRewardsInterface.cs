/***************************************************************************\
Project:      Mobile Interface Template
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)

Modified by: Francisco Manuel García Moreno (garmodev@gmail.com)
\***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

/* 
 * Daily Rewards Canvas is the User interface to show Daily rewards using Unity 4.6
 */
public class DailyRewardsInterface : UIBaseWindow {
	public const float MAX_TIME_WAIT_FOR_INIT = 15;
	public UIBaseWindow loadingWin;
	public UIBaseWindow pnlInternetLost;
	public ScrollRectEnsureVisible scroll;

	// Prefab containing the daily reward
	public GameObject dailyRewardPrefab;

	// Claim Button
	public Button btnClaim;

	// How long until next claim
	public Text txtTimeDue;

	// The Grid that contains the rewards
	public GridLayoutGroup dailyRewardsGroup;

//	private DailyRewards dailyRewards;
	private float timer = 0;

	public override void Awake ()
	{
		timer = 0;
		base.Awake();
//		dailyRewards = FindObjectOfType<DailyRewards> ();
//		DailyRewards.Instance.CheckRewards ();
//		dailyRewards.CheckRewards ();
//		UpdateUI ();
	}

	public void initialization(){
		DailyRewards.Instance.CheckRewards ();
		UpdateUI (true);

	}


	public override void open ()
	{
		base.open ();
		UpdateUI (true);

		timer = 0;

		bool inited = InternetChecker.Instance.IsconnectedToInternet 
			&& (DailyRewards.Instance.IsInitialized && DailyRewards.Instance.rewards != null);

		if(!inited){
			StartCoroutine(tryToInitEverySeconds());
			UIController.Instance.Manager.open(loadingWin);
			StartCoroutine(waitForInitRewards());
		}
	}

	private IEnumerator doInitialization(){
		yield return new WaitForSeconds(3);
		UIController.Instance.Manager.waitForClose(loadingWin,2);
		StartCoroutine(centerScrollOntheCurrentDailyAvailable());
	}

	private IEnumerator tryToInitEverySeconds(float secondsForTryInit = 2.5f, int totalTimes = 4, int timer = 0){
		if(!DailyRewards.Instance.IsInitialized && timer < totalTimes){
			DailyRewards.Instance.Initialize();
			yield return new WaitForSeconds(secondsForTryInit);
			timer++;
			StartCoroutine(tryToInitEverySeconds(secondsForTryInit,totalTimes,timer));
		}
	}

	private IEnumerator waitForInitRewards(){
		bool inited = (DailyRewards.Instance.IsInitialized && DailyRewards.Instance.rewards != null);
		while(timer < MAX_TIME_WAIT_FOR_INIT && !inited){
			timer += Time.deltaTime;
			inited = InternetChecker.Instance.IsconnectedToInternet && (DailyRewards.Instance.IsInitialized && DailyRewards.Instance.rewards != null);
			yield return null;
		}

		if(!inited){
			DailyRewards.Instance.Initialize();
			UIController.Instance.Manager.open(pnlInternetLost);
			UIController.Instance.Manager.waitForClose(pnlInternetLost, 3);
			UIController.Instance.Manager.waitForClose(loadingWin, 3);
			UIController.Instance.Manager.waitForClose(this, 3);
		}
		else{
			initialization();

//			yield return new WaitForSeconds(3);
//			UIController.Instance.Manager.waitForClose(loadingWin,2);
//			StartCoroutine(centerScrollOntheCurrentDailyAvailable());
		}
	}

	public IEnumerator centerScrollOntheCurrentDailyAvailable(){
		while(!DailyRewards.Instance.IsInitialized){
			yield return null;
		}
		int availableIndex = DailyRewards.Instance.AvailableReward;
		DailyReward[] rwButtons = GetComponentsInChildren<DailyReward>();

		if(scroll && availableIndex >= 0 && DailyRewards.Instance.rewards != null && DailyRewards.Instance.rewards.Count > availableIndex && rwButtons != null && rwButtons.Length > availableIndex){			
			RectTransform rt = rwButtons[availableIndex].GetComponent<RectTransform>();
				
			if(rt != null)
				scroll.CenterOnItem(rt);
		}
	}

	// Clicked the claim button
	public void OnClaimClick() {
		DailyRewards.Instance.ClaimPrize (DailyRewards.Instance.availableReward);
		DailyRewards.Instance.CheckRewards ();
		UpdateUI ();
	}

	private IEnumerator doUpdate(bool centerScroll = false){
		bool inited = (DailyRewards.Instance.IsInitialized && DailyRewards.Instance.rewards != null);
		while(timer < MAX_TIME_WAIT_FOR_INIT && !inited){
			timer += Time.deltaTime;
			inited = (DailyRewards.Instance.IsInitialized && DailyRewards.Instance.rewards != null);
			yield return null;
		}


		if(inited){
			foreach(Transform child in dailyRewardsGroup.transform) {
				Destroy(child.gameObject);
			}
			
			bool isRewardAvailableNow = false;
			
			for(int i = 0; i< DailyRewards.Instance.rewards.Count; i++) {
				int reward = DailyRewards.Instance.rewards[i];
				int day = i+1;
				
				GameObject dailyRewardGo = GameObject.Instantiate(dailyRewardPrefab) as GameObject;
				
				DailyReward dailyReward = dailyRewardGo.GetComponent<DailyReward>();
				dailyReward.transform.SetParent(dailyRewardsGroup.transform);
				dailyRewardGo.transform.localScale = Vector2.one;
				
				dailyReward.day = day;
				dailyReward.reward = reward;
				
				dailyReward.isAvailable = day == DailyRewards.Instance.availableReward;
				dailyReward.isClaimed = day <= DailyRewards.Instance.lastReward;
				
				if(dailyReward.isAvailable) {
					isRewardAvailableNow = true;
				}
				
				dailyReward.Refresh();
			}
			
			btnClaim.gameObject.SetActive(isRewardAvailableNow);
			txtTimeDue.gameObject.SetActive(!isRewardAvailableNow);

			if(centerScroll)
				StartCoroutine(doInitialization());
		}
	}

	public void UpdateUI (bool centerScroll = false) {
		if(isActiveAndEnabled)
			StartCoroutine(doUpdate(centerScroll));
	}

	void FixedUpdate() {
		if(txtTimeDue.IsActive()) {
			TimeSpan difference = (DailyRewards.Instance.lastRewardTime - DailyRewards.Instance.timer).Add(new TimeSpan (0, 24, 0, 0));

			// Is the counter below 0? There is a new reward then
			if(difference.TotalSeconds <= 0) {
				DailyRewards.Instance.CheckRewards();
				UpdateUI();
				return;
			}

			string formattedTs = string.Format("{0:D2}:{1:D2}:{2:D2}", difference.Hours, difference.Minutes, difference.Seconds);

			txtTimeDue.text = Localization.Get("daily_rw_return_in") + " " + formattedTs; //"Return in " + formattedTs + " to claim your reward";
		}
	}

	// Resets player preferences
	public void OnResetClick() {
		DailyRewards.Instance.Reset ();
		
		DailyRewards.Instance.lastRewardTime = System.DateTime.MinValue;
		
		DailyRewards.Instance.CheckRewards ();
		UpdateUI ();
	}
	
	// Simulates the next day
	public void OnAdvanceDayClick() {
		DailyRewards.Instance.timer = DailyRewards.Instance.timer.AddDays (1);
		DailyRewards.Instance.CheckRewards ();
		UpdateUI ();
	}
	
	// Simulates the next hour
	public void OnAdvanceHourClick() {
		DailyRewards.Instance.timer = DailyRewards.Instance.timer.AddHours (1);
		DailyRewards.Instance.CheckRewards ();
		UpdateUI ();
	}

}
