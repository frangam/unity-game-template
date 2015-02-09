using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIBaseUpgradableStatListItem : UIBasePurchasableListItem {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private List<UpgradableStat> stats;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public List<UpgradableStat> Stats {
		get {
			return this.stats;
		}
		set {
			stats = value;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="UIBaseUpgradableStatListItem"/> class.
	/// 
	/// STAT OWNER ID,STAT ID,NAME,PRICE,GEMSPRICE, STATS LIST
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="anim">Animation.</param>
	public UIBaseUpgradableStatListItem(string pPPKey, string data, Animator anim = null) :base(pPPKey, data, anim){
		string[] atts = data.Split(ATTRIBUTES_SEPARATOR);

		if(atts.Length > 4)
			init (atts[4]);
	}

	public UIBaseUpgradableStatListItem(string pPPKey, string pId, string pName, bool pPurchased, int pPrice, int pGemsPrice, List<UpgradableStat> pStats)
		: base(pPPKey, pId, pName, pPurchased, pPrice, pGemsPrice){
		this.stats = pStats;
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void save ()
	{
		base.save();
	}

	public override void load ()
	{
		base.load ();

		string stats = getContent();
		
		//load stats
		if(!string.IsNullOrEmpty(stats)){
			string[] att = stats.Split(ATTRIBUTES_SEPARATOR);
			
			if(att.Length > 4)
				init (att[4]);
		}
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(string data){
		string[] uStatsList = data.Split(LIST_CONTAINER_SEPARATOR);

		if(uStatsList != null && uStatsList.Length > 0){
			this.stats = new List<UpgradableStat>();

			foreach(string uStat in uStatsList){
				UpgradableStat us = new UpgradableStat(ppKey, uStat);

				if(us != null && !this.stats.Contains(us))
					this.stats.Add(us);
			}
		}
		else{
			Debug.LogError("Not found any UpgradableStat List");
		}
	}

	public UpgradableStat getUpgradableStatByID(string uStatID){
		UpgradableStat res = null;

		foreach(UpgradableStat s in stats){
			if(s.StatId.Equals(uStatID)){
				res = s;
				break;
			}
		}

		return res;
	}

	public virtual void upgrade(UpgradableStat stat){
		//Save the current upgrade index of the stat

	}

	public virtual bool allUpgradesCompleted(){
		bool res = false;

		if(stats != null && stats.Count > 0){
			foreach(UpgradableStat s in stats){
				res = s.completed();

				if(!res)
					break;
			}
		}

		return res;
	}
}
