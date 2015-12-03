/***************************************************************************\
Project:      Mobile Interface Template
Copyright (c) Niobium Studios.
Author:       Guilherme Nunes Barbosa (gnunesb@gmail.com)
\***************************************************************************/
using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

/* 
 * Daily Rewards keeps track of the user daily rewards based on the time he last selected a reward
 */
public class DailyRewards : Singleton<DailyRewards> {
	
	// Needed controls
	public bool userAServer	 = true;
	public string serverURL = "http://www.frillsgames.com/gettime.php";
	public List<int> rewards;			// Rewards list 

	public DateTime timer;				// Today timer
	public DateTime lastRewardTime;		// The last time the user clicked in a reward
	public int availableReward;			// The available reward position the user can click
	public int lastReward;				// the last reward the user clicked
	private float t;					// Timer seconds ticker
	private bool isInitialized;			// Is the timer initialized?

	// Needed Constants
	private const string LAST_REWARD_TIME = "pp_dr_last_reward_time";
	private const string LAST_REWARD = "pp_dr_last_reward";
	private const string FMT = "O";

	public int AvailableReward {
		get {
			return this.availableReward;
		}
	}

	public int LastReward {
		get {
			return this.lastReward;
		}
	}

	public bool IsInitialized {
		get {
			return this.isInitialized;
		}
	}

	void Awake(){
		if (!isInitialized)
			Initialize ();
	}


	void Update() {
		t += Time.deltaTime;
		if(t >= 1) {
			timer = timer.AddSeconds(1);
			t = 0;
		}
	}

	public void Initialize() {
		if(userAServer)
			StartCoroutine(initTimeFormServer());
		else{
			timer = DateTime.Now;
			isInitialized = true;
		}
	}

	private IEnumerator initTimeFormServer(){
		CoroutineWithData cd = new CoroutineWithData(this, TimeUtils.getDateTimeFromServer(serverURL) );
		yield return cd.coroutine;
		GTDebug.log("Daily rewards - Getting time from server with result: " + cd.result);

		string result = cd.result.ToString();

		if(!string.IsNullOrEmpty(result) && !result.Equals("fail")){
			GTDebug.logWarning("Daily rewards time from server: " +result );
			DateTime dt = Convert.ToDateTime(cd.result);
			timer = dt;
			isInitialized = true;
			GTDebug.logWarning("Daily rewards is inited. Result: " +result);
		}
		else{
			//try to connect again
			GTDebug.logWarning("Daily rewards not inited. Result: " +result +". Waiting for an internet connection");

			while(!InternetChecker.Instance.IsconnectedToInternet)
				yield return new WaitForSeconds(5f);

			//when we have an internet connection, trying to connect to server
			StartCoroutine(initTimeFormServer());
		}
	}

	private IEnumerator doCheckRewards(){
		if(!isInitialized){
			Initialize();

			while(!isInitialized) {
				yield return null;
			}
		}
		
		string lastClaimedTimeStr = PlayerPrefs.GetString (LAST_REWARD_TIME);
		lastReward = PlayerPrefs.GetInt(LAST_REWARD);
		
		// It is not the first time the user claimed.
		// I need to know if he can claim another reward or not
		if(!string.IsNullOrEmpty(lastClaimedTimeStr)) {
			lastRewardTime = DateTime.ParseExact(lastClaimedTimeStr, FMT, CultureInfo.InvariantCulture);
			
			TimeSpan diff = timer - lastRewardTime;
			GTDebug.log("Last claim was " + (long)diff.TotalHours + " hours ago.");
			
			int days = (int)(Math.Abs(diff.TotalHours)/24);
			
			if(days == 0) {
				// No claim for you. Try tomorrow
				availableReward = 0;
			}
			else{
				// The player can only claim if he logs between the following day and the next.
				if(days >= 1 && days < 2) {
					// If reached the last reward, resets to the first restarting the cicle
					if(lastReward == rewards.Count) {
						availableReward = 1;
						lastReward = 0;
					}
					else{
						availableReward = lastReward + 1;
					
						GTDebug.log("Player can claim prize " + availableReward);
					}
				}
				else if(days >= 2) {
					// The player loses the following day reward and resets the prize
					availableReward = 1;
					lastReward = 0;
					GTDebug.log("Prize reset ");
				}
			}
		} else {
			// Is this the first time? Shows only the first reward
			availableReward = 1;
		}
	}

	// Check if the player have unclaimed prizes
	public void CheckRewards() {
		StartCoroutine(doCheckRewards());
	}

	// Claims the prize and checks if the player can do it
	public void ClaimPrize(int day) {
		if(availableReward == day) {
			//apply reward
			//add money
			GameMoneyManager.Instance.addMoney(rewards[day - 1]);

			GTDebug.log("Reward [" + rewards[day - 1] + "] Claimed!");
			PlayerPrefs.SetInt (LAST_REWARD, availableReward);

			string lastClaimedStr = timer.ToString (FMT);
			//GTDebug.log("Setting date: " + lastClaimedStr);
			PlayerPrefs.SetString (LAST_REWARD_TIME, lastClaimedStr);

			BaseSoundManager.Instance.play(BaseSoundIDs.CLAIM_BILLS_FX);

			CheckRewards();
		} else if(day <= lastReward) {
			GTDebug.log("Reward already claimed. Try again tomorrow");
		} else {
			GTDebug.log("Cannot Claim this reward! Can only claim reward #" + availableReward);
		}
	}

	public void Reset() {
		PlayerPrefs.DeleteKey (DailyRewards.LAST_REWARD);
		PlayerPrefs.DeleteKey (DailyRewards.LAST_REWARD_TIME);
	}

}
