/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIButtomToggleWin : UIButtonOpenWin {

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

	public void cl(){
		doPress ();
	}

	public void closeMyWin(){
	
		if(openWindows != null && openWindows.Count > 0){
			foreach(UIBaseWindow w in openWindows){		
				UIController.Instance.Manager.close(w);
			}

			button.image.color=windowsCloseColor;
		}

	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Update ()
	{

		base.Update ();
		if (openWindows != null && openWindows.Count > 0) {
			
			foreach (UIBaseWindow w in openWindows) {
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


		if(closeWindows != null && closeWindows.Count > 0){
			foreach(UIBaseWindow w in closeWindows)
				UIController.Instance.Manager.close(w);
		}
		
		if(openWindows != null && openWindows.Count > 0){

			foreach(UIBaseWindow w in openWindows){
				if(!w.IsOpen){
					isOpenWindows=false;
				
				}

				if(!isOpenWindows){
					UIController.Instance.Manager.open(w);
					isOpenWindows=true;
					VynilManager.Instance.UICarVynilManager.setActualWin(w);
				}else{
					UIController.Instance.Manager.close(w);
					isOpenWindows=false;
				}
			}

//			if(button&&button.image){
//				if(isOpenWindows){
//					button.image.color=windowsOpenColor;
//				}else{
//					button.image.color=windowsCloseColor;
//				}
//			}

		}
	}
}
