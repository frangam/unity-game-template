using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EasterEggUIButton : MonoBehaviour {
	[SerializeField]
	private EasterEggCode code;

	void OnPress(bool pressed){
		if(!pressed){
			EasterEggsController.Instance.checkCode(code);
		}
	}
}
