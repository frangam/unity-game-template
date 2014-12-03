using UnityEngine;
using System.Collections;

public class AjustaSpritePantalla : MonoBehaviour {
	public enum Ajuste{
		ALTURA	= 1,
		ANCHURA	= 2
	}

	//--------------------------------------
	// Atributos de configuracion
	//--------------------------------------
	[SerializeField]
	private Ajuste ajuste;
	
	//--------------------------------------
	// Atributos privados
	//--------------------------------------
	private Sprite sprite;
	
	//--------------------------------------
	// Metodos Unity
	//--------------------------------------
	#region Unity
	void Start(){
		sprite = GetComponent<SpriteRenderer> ().sprite;
		ajustar ();
	}
	#endregion
	
	//--------------------------------------
	// Metodos privados
	//--------------------------------------
	private void ajustar(){
		if(ajuste == Ajuste.ALTURA){
			int altura = Screen.height;


		}
	}
}
