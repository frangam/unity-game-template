using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoostersHandler : Singleton<BoostersHandler> {
	private Booster currentBooster; //booster que esta utilizando el player en este momento

	void OnEnable(){
		Booster.OnFinEfecto += HandleOnFinEfecto;
	}

	void OnDisable(){
		Booster.OnFinEfecto -= HandleOnFinEfecto;
	}

	public void activarBooster(Booster b){
		if(currentBooster != null)
			currentBooster.finish();

		currentBooster = b;
	}
	
	void HandleOnFinEfecto (){
		currentBooster = null;
	}
}
