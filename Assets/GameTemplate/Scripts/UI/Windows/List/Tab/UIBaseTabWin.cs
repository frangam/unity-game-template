using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIBaseTabWin : UIBaseWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Text lbTitle;

	[SerializeField]
	private string initialTabTitleLocKey = "lb_tab_title";

	[SerializeField]
	private List<UIBaseTabButton> buttons;

	[SerializeField]
	private GameObject pnlTabButtons;

	[SerializeField]
	private GameObject pnlCurrentTabAllContent;

	[SerializeField]
	private List<UIBaseWindow> tabContents;

	[SerializeField]
	/// <summary>
	/// Button shows all tab buttons
	/// </summary>
	private GameObject btnBack;

	[SerializeField]
	/// <summary>
	/// If true only it will be shown in the window the tab buttons
	/// </summary>
	private bool onlyShowTabButtons;

	[SerializeField]
	/// <summary>
	/// If true the tab buttons will be hiden when the tab is changed
	/// </summary>
	private bool hideTabButtonsWhenChangeTab;


	private int currentTabIndex = -1;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();

		if(pnlCurrentTabAllContent && ( tabContents == null || (tabContents != null && tabContents.Count == 0))){
			UIBaseWindow[] aux = pnlCurrentTabAllContent.GetComponentsInChildren<UIBaseWindow>() as UIBaseWindow[];

			if(aux != null)
				tabContents = new List<UIBaseWindow>(aux);


		}

		if(tabContents == null || (tabContents != null && tabContents.Count == 0))
			Debug.LogError("UIBaseTabWin - Not found any BUILevelSelectorWin");


		//close all windows except the start window if it is set
		for(int i=0; i<tabContents.Count; i++){
			if(currentTabIndex >= 0 && currentTabIndex == i)
				UIController.Instance.Manager.open(tabContents[i]);
			else
				UIController.Instance.Manager.close(tabContents[i]);
		}

		if(lbTitle)
			lbTitle.text = Localization.Get(initialTabTitleLocKey);

		if(pnlCurrentTabAllContent)
			pnlCurrentTabAllContent.SetActive(!onlyShowTabButtons);

		btnBack.SetActive(false);
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void changeTab(int index){
		if(hideTabButtonsWhenChangeTab){
			pnlTabButtons.SetActive(false);

			if(btnBack)
				btnBack.SetActive(true);

			pnlCurrentTabAllContent.SetActive(true);
		}

		if(tabContents != null && tabContents.Count > index){
			if(currentTabIndex >= 0)
				UIController.Instance.Manager.close( tabContents[currentTabIndex]);

			currentTabIndex = index;
			UIController.Instance.Manager.open( tabContents[currentTabIndex]);
		}
	}

	public virtual void getBack(){
		pnlTabButtons.SetActive(true);

		if(btnBack)
			btnBack.SetActive(false);

		UIController.Instance.Manager.close( tabContents[currentTabIndex]);
		lbTitle.text = Localization.Get(initialTabTitleLocKey);
	}

	public virtual void setCurrentTabName(string name){
		if(lbTitle)
			lbTitle.text = name;
	}
}
