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
	[SerializeField]
	private Vector3 proPos;
	
	private RectTransform noProTransform;
	private RectTransform myT;
	public virtual void Awake(){
		noProTransform = GetComponent<RectTransform>();
		myT = GetComponent<RectTransform>();
		
		if(GameSettings.Instance.IS_PRO_VERSION){
			myT.anchoredPosition3D = proPos;
		}
	}
}
