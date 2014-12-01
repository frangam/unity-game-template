using UnityEngine;
using System.Collections;

public class BTime : Booster {
	protected override void apply (){
		base.apply ();


		Time.timeScale = Units;
	}

	public override void finish (){
		Time.timeScale = 1;


		base.finish ();
	}


}
