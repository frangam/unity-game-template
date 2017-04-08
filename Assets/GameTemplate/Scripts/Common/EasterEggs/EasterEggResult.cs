/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class EasterEggResult : BaseDataResult {
	private EasterEgg easterEgg;

	public EasterEggResult(EasterEgg pEasterEgg):base(true){
		easterEgg = pEasterEgg;
	}

	public EasterEgg EasterEgg {
		get {
			return this.easterEgg;
		}
	}
}
