using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneradorOleadas : MonoBehaviour {
	[SerializeField]
	private GameDifficulty difficulty = GameDifficulty.NONE;

	[SerializeField]
	private GestorZonasSpawn gestorZonas; 

	[SerializeField]
	private SensorTerrenoSig sensorTerreno;

	private Oleada[] oleadas;
	private bool siguiente;
	private int oleada = 0;
	public int cont=0; //public para test
	private bool finGeneracionOleadas;
	private Dictionary<ZonaGeneracion, bool> posSalidaOcupadas;

	public GestorZonasSpawn ZonesManager {
		get {
			return this.gestorZonas;
		}
	}

	public SensorTerrenoSig SensorTerreno {
		get {
			return this.sensorTerreno;
		}
	}

	public Dictionary<ZonaGeneracion, bool> PosSalidaOcupadas {
		get {
			return this.posSalidaOcupadas;
		}
		set {
			posSalidaOcupadas = value;
		}
	}


	public bool iniciada(){
		return oleadas != null;
	}

	void Awake(){
		gameObject.SetActive (GameManager.Instance.Difficulty == difficulty);
	}

	void Start(){
		posSalidaOcupadas = new Dictionary<ZonaGeneracion, bool>();
		foreach(ZonaGeneracion zona in gestorZonas.Zones)
			posSalidaOcupadas.Add(zona, false);


		finGeneracionOleadas = false;

		cargarOleadas ();

		if(iniciada()){
			siguiente = true;
			StartCoroutine (generarOleadas ());

		}

	}

//	void LateUpdate(){
//		Debug.Log("todas zonas salida ocupadas ? "+todasOcupadas());
//	}

	public bool todasOcupadas(){
		bool ocupada = true;
		
		foreach(ZonaGeneracion zona in PosSalidaOcupadas.Keys){
			ocupada = PosSalidaOcupadas[zona];
			
			if(!ocupada)
				break;
		}
		
		return ocupada;
	}

	public List<ZonaGeneracion> zonasSalidaLibres(){
		List<ZonaGeneracion> res = new List<ZonaGeneracion> ();

		foreach(ZonaGeneracion zona in PosSalidaOcupadas.Keys){
			if(!PosSalidaOcupadas[zona])
				res.Add(zona);
		}

		return res;
	}

	private void cargarOleadas(){
		List<Oleada> aux = new List<Oleada> ();
		Oleada[] todas = GetComponentsInChildren<Oleada>() as Oleada[];

		foreach(Oleada o in todas){
			o.init(this);
			aux.Add(o);
		}

		if(aux.Count > 0)
			oleadas = aux.ToArray ();
		else
			Debug.LogError("No se han podido cargar las oleadas");
	}

	private IEnumerator generarOleadas(){
		yield return new WaitForSeconds(1);

		while(!GameManager.gameStart)
			yield return null;
		
		while(cont<oleadas.Length && !GameManager.gameOver){
			if(siguiente){
				siguiente = false;
				oleadas[cont].generar();
			}
			
			yield return new WaitForSeconds(2);
		}
	}
	
	public void siguienteOleada(){
		siguiente = true;
		cont++;
		oleada++;
	}

	public bool finGeneracion(){
		bool finGeneracion = false;
		
		if(oleada >= oleadas.Length && oleadas[oleadas.Length-1].finOleada()){
//			finGeneracion = GameObject.FindGameObjectsWithTag(Configuracion.TAG_OLEADA).Length == 0;
			finGeneracionOleadas = finGeneracion;

//			Debug.Log("fin generacion: "+finGeneracion);
		}
		
		return finGeneracion;
	}
}
