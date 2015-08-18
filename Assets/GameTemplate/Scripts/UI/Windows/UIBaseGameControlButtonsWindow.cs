using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class UIBaseGameControlButtonsWindow : UIBaseWindow {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private AxisTouchButton[] buttons;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		buttons = GetComponentsInChildren<AxisTouchButton>();
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Releases the touch down.
	/// </summary>
	public void releaseTouchDown(){
		if(buttons != null && buttons.Length > 0){
			foreach(AxisTouchButton b in buttons){
				GTDebug.log("Releasing button " +b.name);
				b.OnPointerUp(null);
			}
		}
	}
}
