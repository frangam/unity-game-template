using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeStatButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	protected UIBaseStatsUpgradesWin window;
	
	[SerializeField]
	private GameObject[] hideWhenCompletedUpgrades;
	
	[SerializeField]
	protected string statID;
	
	[SerializeField]
	private bool priceIntegerValue = true;
	
	[SerializeField]
	private string currentValueFormat = "#.#";
	
	[SerializeField]
	[Tooltip("An adding string precedes to the current value string")]
	private string currentValuePrevStringAdding;
	
	[SerializeField]
	[Tooltip("An adding string follows to the current value string")]
	private string currentValuePostStringAdding;
	
	[SerializeField]
	/// <summary>
	/// The progress of upgrade
	/// </summary>
	private Image[] spStatProgress;
	
	[SerializeField]
	private Color progressNotReachedColor = Color.white;
	
	[SerializeField]
	private Color progressReachedColor = Color.green;
	
	[SerializeField]
	private Text lbCurrentValue;
	
	[SerializeField]
	private Text lbCurrentUpgradePrice;
	
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected UpgradableStat stat;
	protected int currentObjectIndex;
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void showInfo(){
		bool activeBtnUpgrade = stat != null && hideWhenCompletedUpgrades != null && hideWhenCompletedUpgrades.Length > 0 && !stat.completed();
		foreach(GameObject g in hideWhenCompletedUpgrades)
			g.SetActive(activeBtnUpgrade);
		
		
		if(lbCurrentUpgradePrice){
			if(priceIntegerValue)
				lbCurrentUpgradePrice.text = ((int)stat.currentPrice()).ToString();
			else
				lbCurrentUpgradePrice.text = stat.currentPrice().ToString("#.#");
		}
		
		showCurrentValue();
		showProgress();
	}
	
	public virtual void showCurrentValue(){
		if(lbCurrentValue){
			if(stat.CurrentSimValue >= 0)
				lbCurrentValue.text = currentValuePrevStringAdding + stat.CurrentSimValue.ToString(currentValueFormat) + currentValuePostStringAdding;
			else
				lbCurrentValue.text = currentValuePrevStringAdding + stat.CurrentValue.ToString(currentValueFormat) + currentValuePostStringAdding;
		}
	}
	
	public virtual void showProgress(){
		if(spStatProgress != null && spStatProgress.Length >0 && stat != null){
			//first reset
			for(int i=0; i<spStatProgress.Length; i++){
				spStatProgress[i].color = progressNotReachedColor;
			}
			for(int i=0; i<stat.CurrentUpgradeIndex; i++){
				spStatProgress[i].color = progressReachedColor;
			}
		}
	}
	
	
	public virtual void init(){
		if(window){
			UIBaseUpgradableStatListItem uItem = (UIBaseUpgradableStatListItem) window.CurrentItemSelected;
			
			if(uItem != null && uItem.Stats != null){
				foreach(UpgradableStat s in uItem.Stats){
					if(s.StatId == statID){
						stat = s;
						break;
					}
				}
				
				if(stat != null){
					showInfo();
				}
				else
					Debug.LogError("Not loaded stat");
			}
			else
				Debug.LogError("Not loaded stat");
		}
	}
	
	public virtual void upgrade(bool payWithMoney = true){
		if(stat.apply(payWithMoney)){
			window.upgrade(stat);
			showInfo();
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		
		if(!window){
			window = GetComponentInParent<UIBaseStatsUpgradesWin>();
			
			if(window == null)
				Debug.LogError("Not found UIBaseStatsUpgradesWin");
		}
	}
	
	public override void Start ()
	{
		base.Start ();
		
		init();
	}
	
	protected override void doPress ()
	{
		base.doPress ();
		
		if(window != null && stat != null){
			if(GameMoneyManager.Instance.hasEnoughMoney((int)stat.currentPrice())){
				upgrade();
			}
			else if(GameMoneyManager.Instance.hasEnoughGems((int)stat.currentPrice(), true)){
				window.openConfirmPayWithGems((int) stat.currentPrice(), this);
			}
			else {
				window.openAddMoreCredit();
			}
		}
	}
}
