using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {
	[SerializeField]
	private string[] tagDestruibles;

	[SerializeField]
	/// <summary>
	/// True si se tienen que decrementar puntos con el gestor de juego al destruirse el objeto
	/// </summary>
	private bool descontarPuntos = false;

	void OnTriggerEnter(Collider other){
//		Debug.Log (esDestruible (other.tag));
		if(tagDestruibles.Length == 0 || esDestruible(other.tag)){


			Destroy(other.gameObject);

			if(descontarPuntos){

			}
		}
	}

	void OnCollisionEnter(Collision other){
//		Debug.Log (esDestruible (other.gameObject.tag));
		if(tagDestruibles.Length == 0 || esDestruible(other.gameObject.tag)){


			Destroy(other.gameObject);

			if(descontarPuntos){

			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
//		Debug.Log (esDestruible (other.tag));
		if(tagDestruibles.Length == 0 || esDestruible(other.tag)){



			Destroy(other.gameObject);

			if(descontarPuntos){

			}
		}
	}
	
	void OnCollisionEnter2D(Collision2D other){
//		Debug.Log (esDestruible (other.gameObject.tag));
		if(tagDestruibles.Length == 0 || esDestruible(other.gameObject.tag)){


			Destroy(other.gameObject);

			if(descontarPuntos){

			}
		}
	}

	private bool esDestruible(string tag){
		bool res = false;

		foreach(string t in tagDestruibles){
			res = t == tag;

			if(res)
				break;
		}

		return res;
	}
}
