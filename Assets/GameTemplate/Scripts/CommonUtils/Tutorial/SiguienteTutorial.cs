using UnityEngine;
using System.Collections;

public class SiguienteTutorial : MonoBehaviour {


	void OnPress(bool pressed){
		if(!pressed)
			GestorTutorial.Instance.siguiente();
	}

}
