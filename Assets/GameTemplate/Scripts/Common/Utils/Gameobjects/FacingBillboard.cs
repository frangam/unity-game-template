/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

/// <summary>
/// Useful if we need to face an object when camera is moving every time
/// </summary>
public class FacingBillboard : MonoBehaviour {
	public Transform target;

	[Tooltip("If set to true does not matter target")]
	public bool mainCamera = true;

	void Awake(){
		if(mainCamera)
			target = Camera.main.transform;
	}

	void Update(){
		if(target != null)
			transform.LookAt(transform.position + target.rotation * Vector3.back,
		                 target.rotation * Vector3.up);
	}
}
