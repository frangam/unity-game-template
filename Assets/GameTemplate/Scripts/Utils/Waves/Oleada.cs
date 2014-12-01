using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Oleada : MonoBehaviour {
	[SerializeField]
	private string descripcion = "";

	/// <summary>
	/// El numero total de objetos que genera la oleada
	/// 0: Infinito
	/// </summary>
	[SerializeField]
	private int totalObjetosGenerados = 0;

	/// <summary>
	/// minimo de objetos que se generan al mismo tiempo
	/// </summary>
	[SerializeField]
	private int minimoObjetosMismoTiempo = 1;

	
	/// <summary>
	/// como maximo debe ser el numero de salidas. 0: maximo de zonas
	/// </summary>
	[SerializeField]
	private int maximoObjetosMismoTiempo = 0;
	
	[SerializeField]
	private bool pro;

	/// <summary>
	/// El ritmo de generacion de oleadas
	/// </summary>
	public float ritmo;

	[SerializeField]
	private float esperaInicial = 0.6f;

	[SerializeField]
	private OleadaBase[] oleadasDisponibles;

	[SerializeField]
	private bool ultimo = false;




	public int cont = 0; //public para test
	private List<ZonaGeneracion> posSalida;
	private GeneradorOleadas gestorOleadas;
	private Dictionary<ZonaGeneracion, GOOleada> generacionPrev; //previas


	void Awake(){


//		Debug.Log ("limite ? " + esLimiteEscenario);
	}

	public void init(GeneradorOleadas gestor){
		gestorOleadas = gestor;
		posSalida = gestor.ZonesManager.Zones;
		generacionPrev = new Dictionary<ZonaGeneracion, GOOleada> ();
	}
	
	public void generar(){
		cont = 0;
		StartCoroutine(generarOleada());	
	}
	
	public IEnumerator generarOleada(){
		yield return new WaitForSeconds (esperaInicial);

		//Generacion FINITA
		if(totalObjetosGenerados > 0){
			for(cont=0; cont<totalObjetosGenerados && !GameManager.gameOver; ){
				//si no hay ninguna zona libre no generamos
				if(!gestorOleadas.todasOcupadas())
					generarObjetos();
				
				
				cont++;
				
				if (finOleada()) {
					gestorOleadas.siguienteOleada();
				}
				
				yield return new WaitForSeconds(ritmo);
			}
		}
		//Generacion INFINITA
		else{
//			Debug.Log("generacion infinita");
			while(!GameManager.gameOver){
				//si no hay ninguna zona libre no generamos
				if(!gestorOleadas.todasOcupadas())
					generarObjetos();
			
				yield return new WaitForSeconds(ritmo);
			}
		}

	}



	private void generarObjetos(){
		int min = minimoObjetosMismoTiempo;
		maximoObjetosMismoTiempo = maximoObjetosMismoTiempo == 0 ? gestorOleadas.ZonesManager.Zones.Count : maximoObjetosMismoTiempo;
		int numObjetosGenerar = Random.Range(min, Mathf.Clamp(maximoObjetosMismoTiempo, min, posSalida.Count)); //eleccion del numero de objetos a generar a la vez
		Dictionary<ZonaGeneracion, GOOleada> generacion = new Dictionary<ZonaGeneracion, GOOleada> ();

//		Debug.Log ("objetos a la vez: " + numObjetosGenerar);

		//objetos a la vez
		for(int j=0; j<numObjetosGenerar; j++){

			//no podemos generar si las zonas de generacion estan ocupadas por otros objetos
			if(gestorOleadas.todasOcupadas())
				break;

			int iSalida = 0; //izquierda en caso de que sea limite de escenario. 0: izquierda, 1:derecha
			ZonaGeneracion zona = null; 

			//no repetir la zona
			do{
				iSalida = Random.Range(0, posSalida.Count);//eleccion de la zona de inicio
				zona = posSalida[iSalida]; //la zona de salida
			}while(generacion.ContainsKey(zona) );

			


			OleadaBase oleadaAux = null;
			bool terrenoInvalido = false;
			int intentos = 0;
			bool variosIntentos = false;

			//si la generacion previa solo hay un hueco libre para que pase el coche
			//tenemos que asegurar que no se generara un objeto que tenga velocidad en la nueva generacion
			//porque podria obstaculizar el paso
//			do{

				List<OleadaBase> objetosDisponiblesEnTerreno = getObjetosValidosEnTerroActual(zona); //obtenemos solo los objetos validos en el terreno actual y zona elegida
				List<Priority> prioridadesValidas = getPrioridadesValidas(objetosDisponiblesEnTerreno); //obtenemos las prioridades validas para la generacion
				oleadaAux = conseguirObjetoAGenerar(objetosDisponiblesEnTerreno, prioridadesValidas);

//				Debug.Log(oleadaAux.velocidad);
//				terrenoInvalido = gestorOleadas.SensorTerreno != null && oleadaAux.whereCanSpawn != null && oleadaAux.whereCanSpawn.Count > 0 && !oleadaAux.whereCanSpawn.Contains(gestorOleadas.SensorTerreno.terrenoSig);
			
			
//				if(terrenoInvalido){
//					oleadaAux = null;
//					intentos++;
//
//					if(intentos == 5){
//						Debug.LogWarning("Varios intentos generando objeto " + oleadaAux +" en terrero " + gestorOleadas.SensorTerreno.terrenoSig);
//						variosIntentos = true;
//						break;
//					}
//				}


//			}
//			while(
//				//Cuando se intenta generar en la misma zona que se genero antes y es la unica libre y el objeto se va a mover
//				generacionPrev.Count == posSalida.Count-1 && oleadaAux.canMove && !generacionPrev.ContainsKey(zona));

//			if(!variosIntentos && oleadaAux != null){

			if(oleadaAux != null){

				//instanciar objeto generado
				OleadaBase oleada = new OleadaBase(oleadaAux, zona);
				Vector3 pos = zona.transform.position + oleada.go.transform.position; //posicion para colocar la oleada
				GOOleada go = Instantiate(oleada.go, pos, oleada.go.transform.rotation) as GOOleada;
				go.inicializar(oleada);

				//add a la generacion
				generacion.Add(zona, go);
			}
		}

		//actualizar generacion previa
		generacionPrev = generacion;
	}

	private OleadaBase conseguirObjetoAGenerar(List<OleadaBase> objetosDispEnterreno, List<Priority> prioridadesValidas){
		GOOleada goOleada = null;
		int prioridad = Random.Range(0, 100); //probabilidad eleccion del objeto a generar
		List<OleadaBase> objetosConPrioridadElegida = new List<OleadaBase>(); //los objetos que tienen la prioridad elegida
		Priority prioridadElegida = Priority.MINIMUM;

		//Prioridad RARA 2%
		if(prioridad < 2){
			prioridadElegida = Priority.RARE;
		}
		//Prioridad MINIMA 18%
		else if(prioridad >= 2 && prioridad < 20){
			prioridadElegida = Priority.MINIMUM;
		}
		//Prioridad INTERMEDIA 35%
		else if(prioridad >= 20 && prioridad < 55){
			prioridadElegida = Priority.MIDDLE;
		}
		//Prioridad MAXIMA 45%
		else if(prioridad >= 55 && prioridad < 100){
			prioridadElegida = Priority.MAXIMUM;
		}

		//obtenemos los indices de los regalos que tienen esta prioridad elegida
		foreach(OleadaBase r in objetosDispEnterreno){
			if(r.prioridad == prioridadElegida){
				objetosConPrioridadElegida.Add(r);
			}
		}

		int iOleada = Random.Range (0, objetosConPrioridadElegida.Count); //elegimos al azar uno de esas oleadas que tienen la misma prioridad que se ha elegido
		OleadaBase oleada = null;

		oleada = objetosConPrioridadElegida != null && objetosConPrioridadElegida.Count > 0  ? objetosConPrioridadElegida [iOleada] : getOleadaBasePrioridadMinima(objetosDispEnterreno);//la oleada elegida

		return oleada; 
	}

	public bool finOleada(){
		return cont >= totalObjetosGenerados;
	}

	private List<OleadaBase> getObjetosValidosEnTerroActual(ZonaGeneracion zona){
		List<OleadaBase> res = new List<OleadaBase> ();
		TerrainType terreoActual = gestorOleadas.SensorTerreno.terrenoSig;
		bool genPrevUnicaZonaLibre = generacionPrev.Count == posSalida.Count - 1; //true si la generacion previa solo tiene una zona libre para que pase el jugador

		foreach(OleadaBase o in oleadasDisponibles){
			bool excluirObjsMov = genPrevUnicaZonaLibre && o.canMove && !generacionPrev.ContainsKey(zona); //excluir objetos que se mueven si se va a generar el objeto en una zona donde en la generacion previa se genero otro objeto

			if(!excluirObjsMov 
			   && ((o.whereCanSpawn != null && o.whereCanSpawn.Count > 0 && o.whereCanSpawn.Contains(terreoActual))
			   		|| (o.whereCanSpawn != null && o.whereCanSpawn.Count == 0)
			   		|| (o.whereCanSpawn == null))
				){
				res.Add(o);
			}
		}

		return res;
	}


	private List<Priority> getPrioridadesValidas(List<OleadaBase> objetosDispEnTerreno){
		List<Priority> res = new List<Priority> ();

		foreach(OleadaBase o in objetosDispEnTerreno){
			Priority p = o.prioridad;

			if(!res.Contains(p)){
				res.Add(p);
			}
		}

		return res;
	}

	private OleadaBase getOleadaBasePrioridadMinima(List<OleadaBase> objetos){
		OleadaBase res = null;
		Priority prioridadMinimaDisp = Priority.MAXIMUM;

		foreach(OleadaBase o in objetos){
			if(o.prioridad < prioridadMinimaDisp){
				prioridadMinimaDisp = o.prioridad;
				res = o;
			}
		}

		return res;
	}
}
