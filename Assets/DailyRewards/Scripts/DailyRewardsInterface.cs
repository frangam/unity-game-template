/***************************************************************************\
Project:      Mobile Interface Template
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)
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
	
	private DailyRewards dailyRewards;
	private float timer = 0;
	
	public override void Awake ()
	{
		timer = 0;
		base.Awake();
		dailyRewards = FindObjectOfType<DailyRewards> ();
		dailyRewards.CheckRewards ();
		UpdateUI ();
	}
	
	public void initialization(){
		dailyRewards.CheckRewards ();
		UpdateUI ();
		StartCoroutine(doInitialization());
	}
	
	
	public override void open ()
	{
		base.open ();
		timer = 0;
		
		bool inited = InternetChecker.Instance.IsconnectedToInternet 
			&& (dailyRewards != null && dailyRewards.IsInitialized && dailyRewards.rewards != null);
		
		if(!inited){
			StartCoroutine(tryToInitEverySeconds());
			UIController.Instance.Manager.open(loadingWin);
			StartCoroutine(waitForInitRewards());
		}
		else{
			StartCoroutine(centerScrollOntheCurrentDailyAvailable());
		}
	}
	
	private IEnumerator doInitialization(){
		yield return new WaitForSeconds(3);
		UIController.Instance.Manager.waitForClose(loadingWin,2);
		StartCoroutine(centerScrollOntheCurrentDailyAvailable());
	}
	
	private IEnumerator tryToInitEverySeconds(float secondsForTryInit = 2.5f, int totalTimes = 4, int timer = 0){
		if(!dailyRewards.IsInitialized && timer < totalTimes){
			dailyRewards.Initialize();
			yield return new WaitForSeconds(secondsForTryInit);
			timer++;
			StartCoroutine(tryToInitEverySeconds(secondsForTryInit,totalTimes,timer));
		}
	}
	
	private IEnumerator waitForInitRewards(){
		bool inited = (dailyRewards != null && dailyRewards.IsInitialized && dailyRewards.rewards != null);
		while(timer < MAX_TIME_WAIT_FOR_INIT && !inited){
			timer += Time.deltaTime;
			inited = InternetChecker.Instance.IsconnectedToInternet && (dailyRewards != null && dailyRewards.IsInitialized && dailyRewards.rewards != null);
			yield return null;
		}
		
		if(!inited){
			dailyRewards.Initialize();
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
		while(!dailyRewards.IsInitialized){
			yield return null;
		}
		int availableIndex = dailyRewards.AvailableReward;
		DailyReward[] rwButtons = GetComponentsInChildren<DailyReward>();
		
		if(scroll && availableIndex >= 0 && dailyRewards.rewards != null && dailyRewards.rewards.Count > availableIndex && rwButtons != null && rwButtons.Length > availableIndex){			
			RectTransform rt = rwButtons[availableIndex].GetComponent<RectTransform>();
			
			if(rt != null)
				scroll.CenterOnItem(rt);
		}
	}
	
	// Clicked the claim button
	public void OnClaimClick() {
		dailyRewards.ClaimPrize (dailyRewards.availableReward);
		dailyRewards.CheckRewards ();
		UpdateUI ();
	}
	
	private IEnumerator doUpdate(){
		bool inited = (dailyRewards != null && dailyRewards.IsInitialized && dailyRewards.rewards != null);
		while(timer < MAX_TIME_WAIT_FOR_INIT && !inited){
			timer += Time.deltaTime;
			inited = (dailyRewards != null && dailyRewards.IsInitialized && dailyRewards.rewards != null);
			yield return null;
		}
		
		
		if(inited){
			foreach(Transform child in dailyRewardsGroup.transform) {
				Destroy(child.gameObject);
			}
			
			bool isRewardAvailableNow = false;
			
			for(int i = 0; i< dailyRewards.rewards.Count; i++) {
				int reward = dailyRewards.rewards[i];
				int day = i+1;
				
				GameObject dailyRewardGo = GameObject.Instantiate(dailyRewardPrefab) as GameObject;
				
				DailyReward dailyReward = dailyRewardGo.GetComponent<DailyReward>();
				dailyReward.transform.SetParent(dailyRewardsGroup.transform);
				dailyRewardGo.transform.localScale = Vector2.one;
				
				dailyReward.day = day;
				dailyReward.reward = reward;
				
				dailyReward.isAvailable = day == dailyRewards.availableReward;
				dailyReward.isClaimed = day <= dailyRewards.lastReward;
				
				if(dailyReward.isAvailable) {
					isRewardAvailableNow = true;
				}
				
				dailyReward.Refresh();
			}
			
			btnClaim.gameObject.SetActive(isRewardAvailableNow);
			txtTimeDue.gameObject.SetActive(!isRewardAvailableNow);
		}
	}
	
	public void UpdateUI () {
		StartCoroutine(doUpdate());
	}
	
	void FixedUpdate() {
		if(txtTimeDue.IsActive()) {
			TimeSpan difference = (dailyRewards.lastRewardTime - dailyRewards.timer).Add(new TimeSpan (0, 24, 0, 0));
			
			// Is the counter below 0? There is a new reward then
			if(difference.TotalSeconds <= 0) {
				dailyRewards.CheckRewards();
				UpdateUI();
				return;
			}
			
			string formattedTs = string.Format("{0:D2}:{1:D2}:{2:D2}", difference.Hours, difference.Minutes, difference.Seconds);
			
			txtTimeDue.text = Localization.Get("daily_rw_return_in") + " " + formattedTs; //"Return in " + formattedTs + " to claim your reward";
		}
	}
	
	// Resets player preferences
	public void OnResetClick() {
		dailyRewards.Reset ();
		
		dailyRewards.lastRewardTime = System.DateTime.MinValue;
		
		dailyRewards.CheckRewards ();
		UpdateUI ();
	}
	
	// Simulates the next day
	public void OnAdvanceDayClick() {
		dailyRewards.timer = dailyRewards.timer.AddDays (1);
		dailyRewards.CheckRewards ();
		UpdateUI ();
	}
	
	// Simulates the next hour
	public void OnAdvanceHourClick() {
		dailyRewards.timer = dailyRewards.timer.AddHours (1);
		dailyRewards.CheckRewards ();
		UpdateUI ();
	}
	
}
