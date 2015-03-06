using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBaseTabButton : UIButtonOpenWin {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool useTextButtonLikeTabTitle = true;

	[SerializeField]
	private Text lbButtonText;

	[SerializeField]
	private string titleForTab;

	[SerializeField]
	private UIBaseTabWin tabWin;

	[SerializeField]
	private int indexTabContent;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();

		if(lbButtonText == null)
			lbButtonText = GetComponentInChildren<Text>();

		if(tabWin == null)
			tabWin = GetComponentInParent<UIBaseTabWin>();

		if(tabWin == null)
			Debug.LogError("UIBaseTabButton - Not found UIBaseTabWin");

		bool valid = (useTextButtonLikeTabTitle && lbButtonText != null && !string.IsNullOrEmpty(lbButtonText.text))
			|| (!useTextButtonLikeTabTitle && !string.IsNullOrEmpty(titleForTab));

		if(!valid)
			Debug.LogError("Not valid title");
	}

	protected override bool canPress ()
	{
		bool valid = (useTextButtonLikeTabTitle && lbButtonText != null)
			|| (!useTextButtonLikeTabTitle && !string.IsNullOrEmpty(titleForTab));

		return base.canPress () && valid;
	}

	protected override void doPress ()
	{
		base.doPress ();

		if(tabWin != null){
			string titleToSend = titleForTab;

			if(useTextButtonLikeTabTitle)
				titleToSend = lbButtonText.text;

			tabWin.setCurrentTabName(titleToSend);
			tabWin.changeTab(indexTabContent);
		}
	}
}
