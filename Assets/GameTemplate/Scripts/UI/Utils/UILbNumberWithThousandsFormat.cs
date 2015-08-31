/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;
using System.Collections;


[RequireComponent(typeof(Text))]
public class UILbNumberWithThousandsFormat : MonoBehaviour {
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Text lb;
	private long number;

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity	
	public void Awake(){
		lb = GetComponentInParent<Text>();
	}
	#endregion
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public void changeValue(long pNumber){
		number = pNumber;

		if(lb)
			lb.text =  number.ToString("N0", CultureInfo.CurrentCulture);
	}
}
