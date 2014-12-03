using UnityEngine;
using System.Collections;

public class BotonSocial : MonoBehaviour {


	[SerializeField]
	private SocialNetwork redSocial;

	void Awake(){

	}

	// Update is called once per frame
	void OnPress (bool pressed) {
		if(!pressed){
//			GestorSocial.Instancia.post(redSocial); //realizamos el post
		}
	}
}
