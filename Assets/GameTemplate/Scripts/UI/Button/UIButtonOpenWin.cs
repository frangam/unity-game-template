using UnityEngine;
using System.Collections;

public class UIButtonOpenWin : UIBaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private UIBaseWindow window;

	[SerializeField]
	private UIBaseWindow closeWindow;

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void doPress ()
	{
		base.doPress ();

		if(closeWindow)
			UIController.Instance.Manager.close(closeWindow);

		if(window)
			UIController.Instance.Manager.open(window);
		else
			Debug.Log("Not found window to open");
	}
}
