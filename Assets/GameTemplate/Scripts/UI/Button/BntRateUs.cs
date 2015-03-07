using UnityEngine;
using System.Collections;

public class BntRateUs : UIBaseButton {
	protected override void doPress ()
	{
		base.doPress ();
		RateApp.Instance.rateUsNow();
	}
}
