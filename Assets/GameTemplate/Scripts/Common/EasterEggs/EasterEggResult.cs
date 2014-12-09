using UnityEngine;
using System.Collections;

public class EasterEggResult : ISN_Result {
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
