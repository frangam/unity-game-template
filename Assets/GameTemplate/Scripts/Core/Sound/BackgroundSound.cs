/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class BackgroundSound : MonoBehaviour {
	void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}
}
