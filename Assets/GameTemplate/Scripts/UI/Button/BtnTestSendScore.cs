/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class BtnTestSendScore : UIBaseButton {
	[SerializeField]
	private long initialScoreToSend;

	[SerializeField]
	private ScoreOrder order;

	[SerializeField]
	private int scoreIndex = 0;

	[SerializeField]
	private int increment = 10;

	protected override void doPress ()
	{
		base.doPress ();
		int index = Mathf.Clamp(scoreIndex, 0, GameSettings.Instance.CurrentScores.Count-1);
		long send = order == ScoreOrder.ASCENDING ? initialScoreToSend-increment : initialScoreToSend+increment;
		ScoresHandler.Instance.sendScoreToServerByIndex(index, send);
	}
}
