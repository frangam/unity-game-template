using UnityEngine;
using System.Collections;

public class UIListNavigationButton : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private UIBaseListWindow window;

	[SerializeField]
	[Tooltip("True if show the next item. False if show the previous item")]
	private bool 			doNext = true;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();

		if(!window){
			window = GetComponentInParent<UIBaseListWindow>();

			if(window == null){
				Debug.LogError("Not found the window to handle the navigation");
			}
		}
	}

	protected override void doPress ()
	{
		base.doPress ();

		if(window != null){
			if(doNext)
				window.nextItem();
			else
				window.previousItem();
		}
	}
}
