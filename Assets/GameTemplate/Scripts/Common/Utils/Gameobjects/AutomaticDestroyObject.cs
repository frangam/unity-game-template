/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class AutomaticDestroyObject : MonoBehaviour 
{
	public float timeBeforeObjectDestroys;
	
	void Start () {
		// the function destroyGO() will be called in timeBeforeObjectDestroys seconds
		Invoke("destroyGO",timeBeforeObjectDestroys);
	}
	
	void destroyGO () {
		// destroy this gameObject
		Destroy(gameObject);
	}
}
