/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class GPlusButton : UIBaseButton {
	[SerializeField]
	private AN_PlusBtnSize size = AN_PlusBtnSize.SIZE_TALL;
	
	[SerializeField]
	private AN_PlusBtnAnnotation annotation = AN_PlusBtnAnnotation.ANNOTATION_BUBBLE;
	
	[SerializeField]
	private TextAnchor anchor = TextAnchor.LowerCenter;
	
	[SerializeField]
	private bool useOnlyAnchor = true;
	
	[SerializeField]
	private bool combineAnchors = false;
	
	[SerializeField]
	private int xPosition = 0;
	
	[SerializeField]
	private int yPosition = 0;
	
	
	private AN_PlusButton gPlusButton;
	
	public override void Awake ()
	{
		base.Awake ();
		
		gPlusButton =  new AN_PlusButton(GameSettings.Instance.CurrentAndroidAppLink, size, annotation);
		
		if(gPlusButton != null){
			if(useOnlyAnchor)
				gPlusButton.SetGravity(anchor);
			//			gPlusButton.SetPosition(gPlusButton.x+xOffset, gPlusButton.y+yOffset);
			else{
				if(combineAnchors){
					xPosition += getValueCombinedWithAnchor();
					yPosition += getValueCombinedWithAnchor(false);
				}
				gPlusButton.SetPosition(xPosition, yPosition);
			}
			
			gPlusButton.ButtonClicked += ButtonClicked;
		}
	}
	
	private int getValueCombinedWithAnchor(bool width = true){
		int res =  0;
		
		switch(anchor){
		case TextAnchor.UpperCenter: res = width ? Screen.width/2 : 0; break;
		case TextAnchor.MiddleCenter: res = width ? Screen.width/2 : Screen.height/2; break;
		case TextAnchor.LowerCenter: res = width ? Screen.width/2 : Screen.height; break;
			
		case TextAnchor.UpperLeft: res = width ? 0 : 0; break;
		case TextAnchor.MiddleLeft:	res = width ? 0 : Screen.height/2; break;
		case TextAnchor.LowerLeft:	res = width ? 0 : Screen.height; break;
			
		case TextAnchor.UpperRight: res = width ? Screen.width : 0; break;
		case TextAnchor.MiddleRight: res = width ? Screen.width : Screen.height/2; break;
		case TextAnchor.LowerRight: res = width ? Screen.width :Screen.height; break;
		}
		
		return res;
	}
	
	void ButtonClicked () {
		base.doPress();
	}
	
	void OnEnable(){
		if(gPlusButton != null && !gPlusButton.IsShowed)
			gPlusButton.Show();
	}
	
	void OnDisable(){
		if(gPlusButton != null && gPlusButton.IsShowed)
			gPlusButton.Hide();
	}
	
	void OnDestroy(){
		if(gPlusButton != null && gPlusButton.IsShowed)
			gPlusButton.Hide();
	}
	
}
