using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBaseMIssionCompletedWin : UIBaseWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Text lbMoneyReward;
	
	[SerializeField]
	private Text lbGemsReward;
	
	[SerializeField]
	private Transform pnlMoneyRewards;
	
	[SerializeField]
	private Transform pnlGemsReward;
	
	[SerializeField]
	protected Text lbTotalMoneyRewards;
	
	[SerializeField]
	protected Text lbTotalGemsRewards;
	
	[SerializeField]
	private Transform pnlTotalMoneyRewards; 
	
	[SerializeField]
	private Transform pnlTotalGemsRewards;
	
	[SerializeField]
	private bool loadInfoWhenQuesCompleted = true;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected BaseLevel level;
	protected int totalMoneyRw;
	protected int totalGemsRw;
	
	//--------------------------------------
	// Getters && Setters
	//--------------------------------------
	public bool LoadInfoWhenQuesCompleted {
		get {
			return this.loadInfoWhenQuesCompleted;
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void open (){
		if(level != null && !loadInfoWhenQuesCompleted){
			showInfo();
		}
		
		base.open ();
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void init(BaseLevel pLevel){
		level = pLevel;
	}
	
	public virtual void setTotalMoneyRw(int val){
		totalMoneyRw = val;
	}
	
	public virtual void setTotalGemsRw(int val){
		totalGemsRw = val;
	}
	
	public virtual void showInfo(){
		if(level != null){
			lbMoneyReward.text = level.RealMoneyReward.ToString();
			lbGemsReward.text = level.RealGemsReward.ToString();
			pnlGemsReward.gameObject.SetActive(level.RealGemsReward > 0);
			showTotalMoneyReward();
			showTotalGemsReward();
		}
		else if(GameSettings.Instance.showTestLogs){
			Debug.Log("UIBaseMIssionCompletedWin - Level not inited");
		}
	}
	
	public virtual void showTotalMoneyReward(){
		bool show = totalMoneyRw > 0;
		pnlTotalMoneyRewards.gameObject.SetActive(show);
		
		if(show)
			lbTotalMoneyRewards.text = totalMoneyRw.ToString();
	}
	
	public virtual void showTotalGemsReward(){
		bool show = totalGemsRw > 0;
		pnlTotalGemsRewards.gameObject.SetActive(show);
		
		if(show)
			lbTotalGemsRewards.text = totalGemsRw.ToString();
	}
}
