using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIButtonOpenWin : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private List<UIBaseWindow> openWindows;
	
	[SerializeField]
	private List<UIBaseWindow> closeWindows;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		base.doPress ();
		
		if(closeWindows != null && closeWindows.Count > 0){
			foreach(UIBaseWindow w in closeWindows)
				UIController.Instance.Manager.close(w);
		}
		
		if(openWindows != null && openWindows.Count > 0){
			foreach(UIBaseWindow w in openWindows)
				UIController.Instance.Manager.open(w);
		}
		else
			Debug.Log("Not found any window to open");
	}
}
