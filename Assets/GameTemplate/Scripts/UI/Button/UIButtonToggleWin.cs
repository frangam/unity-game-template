/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/// <summary>
/// Open / close windows and set button color depending of if windows are opened
/// or not
/// </summary>
public class UIButtonToggleWin : UIButtonOpenWin {

	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Color windowsCloseColor=Color.white;
	[SerializeField]
	private Color windowsOpenColor=Color.cyan;
	[SerializeField]
	private bool isOpenWindows=false;

	//--------------------------------------
	// Getters / Setters
	//--------------------------------------
	public bool IsOpenWindows(){
		return this.isOpenWindows;
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------

	public void closeWindowsAreOpened(){
		if(OpenWindows != null && OpenWindows.Count > 0){
			foreach(UIBaseWindow w in OpenWindows){		
				UIController.Instance.Manager.close(w);
			}

			button.image.color = windowsCloseColor;
		}
	}

	public void openWindowsAreClosed(){
		if(CloseWindows != null && CloseWindows.Count > 0){
			foreach(UIBaseWindow w in OpenWindows){		
				UIController.Instance.Manager.open(w);
			}
			
			button.image.color = windowsOpenColor;
		}
	}

	public virtual void notifyWindowIsOpen(UIBaseWindow window){

	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();

		if ( OpenWindows != null && OpenWindows.Count > 0) {
			
			foreach (UIBaseWindow w in OpenWindows) {
				if (w.IsOpen) {
					button.image.color=windowsOpenColor;
				}else{
					button.image.color=windowsCloseColor;
				}
				break;
			}
		}
	
	}

	protected override void doPress ()
	{
		base.doPress ();

		if(OpenWindows != null && OpenWindows.Count > 0){
			foreach(UIBaseWindow w in OpenWindows){
				//notify this w window is the current new active
				notifyWindowIsOpen(w);
			}
		}
	}
}
