using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestorZonasSpawn : MonoBehaviour {
	private  List<ZonaGeneracion> zones;

	public List<ZonaGeneracion> Zones {
		get {
			return this.zones;
		}
	}

	void Awake(){
		zones = new List<ZonaGeneracion> ();
		ZonaGeneracion[] zonesArray = GetComponentsInChildren<ZonaGeneracion>() as ZonaGeneracion[];
		
		foreach(ZonaGeneracion z in zonesArray)
			zones.Add(z);
		
		zones.Sort ();
	}

	
	public ZonaGeneracion getZonaAleatoria(GOOleada oleada = null){
		ZonaGeneracion res = zones [0];
		int decision = Random.Range (0, 100); //probabilidad para decidir si usar las prioridades para la generacion
		
		//60% de probabilidad de generar oleada objetivo de mision
		//en zones de mayor prioridad (los compartimentos)
		//solo en caso que los compartimentos tengan espacio, al menos uno de ellos
		if(decision >= 0 && decision < 60){
			ZonaGeneracion[] zonesAux = new ZonaGeneracion[2];
			zonesAux[0] = zones[zones.Count-1];
			zonesAux[1] = zones[zones.Count-2];
			
			int i = Random.Range(0, 1);			
			
			res = zonesAux[i];
		}
		//20% de probabilidad para generar
		//en zones de mayor prioridad
		else if(decision >= 60 && decision < 80){ //20% se usan prioridades
			int inicio = zones.Count/2;
			int i = Random.Range (inicio, zones.Count);
			res = zones[i];
		}
		//el resto en cualquier zona aleatoria
		else{
			int i = Random.Range (0, zones.Count);
			res = zones[i];
		}
		
		return res;
	}
	
	public GOOleada generarGameObjectEnZonaAleatoria(GOOleada oleada){
		ZonaGeneracion zonaAleatoria = getZonaAleatoria (oleada);
		Vector3 pos = zonaAleatoria.transform.position + oleada.transform.position; //new Vector3 (zonaAleatoria.transform.position.x,zonaAleatoria.transform.position.y+ oleada.transform.position.y, zonaAleatoria.transform.position.z);
		GOOleada p = Instantiate (oleada, pos, oleada.transform.rotation) as GOOleada;

		return p;
	}
	
	public GOOleada generarGameObjectEnZonaAleatoria(GOOleada oleada, Transform padre){
		ZonaGeneracion zonaAleatoria = getZonaAleatoria (oleada);
		GOOleada p = Instantiate (oleada, zonaAleatoria.transform.position, oleada.transform.rotation) as GOOleada;
		p.transform.parent = padre;
		return p;
	}
}
