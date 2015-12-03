/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// User interface transform for the pro game version
/// </summary>
public class UIProTransform : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Vector2 offsetMin;
	
	[SerializeField]
	private Vector2 offsetMax;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private RectTransform myT;
	private Vector2 offsetMinPrev;
	private Vector2 offsetMaxPrev;
	private bool hasCopiedTransform = false;
	private bool shownPreview = false;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public float Left{get{return offsetMin.x;}}
	public float Right{get{return offsetMax.x;}}
	public float Bottom{get{return offsetMin.y;}}
	public float Top{get{return offsetMax.y;}}
	public bool HasCopiedTransform {
		get {
			return this.hasCopiedTransform;
		}
	}
	public bool ShownPreview {
		get {
			return this.shownPreview;
		}
	}
	//	public bool NewValue{
	//		get{return !((offsetMin.x.Equals(myT.offsetMin.x) && offsetMin.y.Equals(myT.offsetMin.y)) 
	//			            && (offsetMax.x.Equals(-myT.offsetMax.x) && offsetMax.y.Equals(-myT.offsetMax.y)));}
	//	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void Awake(){
		myT = GetComponent<RectTransform>();
		
		if(GameSettings.Instance.IS_PRO_VERSION){
			myT.offsetMin = offsetMin;
			myT.offsetMax = -offsetMax;
		}
	}
	
	public void copyCurrentProperties(){
		if(!hasCopiedTransform){
			RectTransform myTAux = GetComponent<RectTransform>();
			if(myTAux != null){
				offsetMinPrev = myTAux.offsetMin;
				offsetMaxPrev = myTAux.offsetMax;
			}
		}
		
		myT = GetComponent<RectTransform>();
		
		offsetMin = myT.offsetMin;
		offsetMax = -myT.offsetMax;
		
		hasCopiedTransform = true;
		shownPreview = false;
	}
	public void setPropertiesForClearPreview(){
		offsetMinPrev = myT.offsetMin;
		offsetMaxPrev = myT.offsetMax;
	}
	public void showPreview(){
		myT.offsetMin = offsetMin;
		myT.offsetMax = -offsetMax;
		shownPreview = true;
	}
	
	public void clearPreview(){
		myT.offsetMin =	offsetMinPrev;
		myT.offsetMax = offsetMaxPrev;
		shownPreview = false;
	}
	
	public void setLeft(float value){
		offsetMin = new Vector2(value, offsetMin.y);
	}
	public void setTop(float value){
		offsetMax = new Vector2(offsetMax.x, value);
	}
	//	public void setPosZ(float value){
	//		proPos = new Vector3(proPos.x, proPos.y, value);
	//	}
	public void setRight(float value){
		offsetMax = new Vector2(value, offsetMax.y);
	}
	public void setBottom(float value){
		offsetMin = new Vector2(offsetMin.x, value);
	}
}
