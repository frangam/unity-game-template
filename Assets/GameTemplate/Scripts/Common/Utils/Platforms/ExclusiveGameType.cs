/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class ExclusiveGameType : MonoBehaviour {
	public enum GameType{
		OFFLINE_SINGLE_PLAYER,
		OFFLINE_MULTIPLAYER,
		ONLINE_MULTIPLAYER
	}

	[SerializeField]
	private GameType[] gameTypes;

	void Awake(){
		bool show = false;
		
		foreach (GameType t in gameTypes) {
			show = (GameController.Instance.Manager.IsOnlineMultiplayerGame && t == GameType.ONLINE_MULTIPLAYER)
				|| (GameController.Instance.Manager.IsLocalMultiplayerGame && t == GameType.OFFLINE_MULTIPLAYER)
				|| (!GameController.Instance.Manager.IsLocalMultiplayerGame && t == GameType.OFFLINE_SINGLE_PLAYER);
			
			if(show)
				break;
		}
		
		gameObject.SetActive (show);
	}
}
