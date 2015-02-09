using UnityEngine;
using System.Collections;

public class UIBasePlayLevelButton : UIBaseButton {
	//--------------------------------------
	// Settings Attributes
	//--------------------------------------
	[SerializeField]
	private UIBaseLevelSelectorWin window;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();

		if(window == null)
			window = GetComponentInParent<UIBaseLevelSelectorWin>();
	}
	protected override void doPress ()
	{
		base.doPress ();

		if(window != null)
			window.playLevel();
		else
			Debug.LogError("Not found UIBaseLevelSelectorWin");
	}
}
