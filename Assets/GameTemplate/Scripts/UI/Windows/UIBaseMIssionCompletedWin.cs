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
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected BaseLevel level;
	protected int totalMoneyRw;
	protected int totalGemsRw;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void open (){
		if(level != null){
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
		lbMoneyReward.text = level.RealMoneyReward.ToString();
		lbGemsReward.text = level.RealGemsReward.ToString();
		pnlGemsReward.gameObject.SetActive(level.RealGemsReward > 0);
		showTotalMoneyReward();
		showTotalGemsReward();
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
