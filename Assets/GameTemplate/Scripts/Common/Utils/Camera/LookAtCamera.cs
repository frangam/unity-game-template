/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

[AddComponentMenu("Utility/Look At Camera")]

public class LookAtCamera : MonoBehaviour {
	public GameObject target;
	
	void LateUpdate() {
		transform.LookAt(target.transform);
	}
}
