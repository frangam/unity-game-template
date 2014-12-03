using UnityEngine;
using System.Collections;


/// <summary>
/// Este sensor sirve para avisar al gestor de oleadas que el objeto generado en una zona concreta ya ha pasado por el sensor
/// dejando libre la zona, para que el gestor pueda generar más
/// </summary>
public class SensorObjetoPasa : MonoBehaviour {
	[SerializeField]
	private GeneradorOleadas generadorOleadas;

	/// <summary>
	/// Desocupa la zona donde fue generada la oleada
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other){
		GOOleada oleada = null;

		if(other.transform.parent != null)
			oleada = other.transform.parent.GetComponent<GOOleada> ();

		if(oleada != null && oleada.DatosOleada != null && oleada.DatosOleada.Zona != null){
//			Debug.Log("desocupando: "+oleada);
			Debug.DrawLine(oleada.transform.position, oleada.DatosOleada.Zona.transform.position, Color.white, 0.5f);
			generadorOleadas.PosSalidaOcupadas[oleada.DatosOleada.Zona] = false; //desocupar la zona
		}
	}
}
