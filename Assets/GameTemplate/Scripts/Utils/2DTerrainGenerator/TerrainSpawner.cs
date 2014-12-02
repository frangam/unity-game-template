using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainSpawner : MonoBehaviour 
{
	public bool atrezo = false;

	// terrain references	
	public Terrain[] carreteras;

	private float margenCambio = 10;
	

	public float startSpawnPosition = 40.96f;

	private int spawnYPos = 0;
	

	// keep track of the last position terrain was generated
	float lastPosition;
	// camera reference
	GameObject cam;
	// used to check if terrain can be generated depending on the camera position and lastposition
	bool canSpawn = true;

	Terrain carreteraActual;

	void Awake(){
		carreteraActual = FindObjectOfType<Terrain> () as Terrain;
	}

	void Start()
	{
//		startSpawnPosition = (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)).x*4)+Camera.main.transform.position.x;

		// make the lastposition start at start spawn position
		lastPosition = startSpawnPosition;
		// pair camera to camera reference
		cam = Camera.main.gameObject;
	}
	
	void Update()
	{
		// if the camera is farther than the number last position minus 16 terrain is spawned
		// a lesser number may make the terrain 'pop' into the scene too early
		// showing the player the terrain spawning which would be unwanted
		if ((atrezo || (GameManager.gameStart && !GameManager.isGameOver)) && cam.transform.position.x+margenCambio >= lastPosition - startSpawnPosition && canSpawn == true)
		{
			// turn off spawning until ready to spawn again
			canSpawn = false;

			// SpawnTerrain is called and passed the randomchoice number
			SpawnTerrain();
		}
	}
	
	// spawn terrain based on the rand int passed by the update method
	void SpawnTerrain()
	{
		TerrainType destino = TerrainType.LAWN;
		Terrain carreteraDest = null;
		Terrain carreteraPrioridad = null;

		do{
			 //elegimos por el sistema de prioridades una carretera
			carreteraPrioridad = getCarreteraPorPrioridad();

			//obtenemos la carretera que cumple los parámetros
			carreteraDest = getCarretera (carreteraActual.Destination, carreteraPrioridad.Destination);
		}
		while(carreteraDest == null);



		if(carreteraDest != null){
			carreteraActual = carreteraDest;
			Instantiate(carreteraDest, new Vector3(lastPosition, spawnYPos, 0), Quaternion.Euler(0, 0, 0));
			// same as start spawn position as starting terrain
			// is the same length as the rest of the terrain prefabs
			lastPosition += startSpawnPosition;

			// script is now ready to spawn more terrain
			canSpawn = true;
		}
	}

	private Terrain getCarretera(TerrainType origen, TerrainType destino){
		Terrain carretera = null;

		foreach(Terrain c in carreteras){
			if(c.Source == origen && c.Destination == destino){
				carretera = c;
				break;
			}
		}

		return carretera;
	}


	private Terrain getCarreteraPorPrioridad(){
		int prioridad = Random.Range(0, 100); //probabilidad eleccion del objeto a generar
		List<Terrain> prioridades = new List<Terrain>();
		Priority prioridadElegida = Priority.MINIMUM;
		
		//Prioridad MINIMA 20%
		if(prioridad >= 0 && prioridad < 20){
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
		foreach(Terrain c in carreteras){
			if(!c.isHybrid() && c.Priority == prioridadElegida){
				prioridades.Add(c);
			}
		}
		
		int iCarretera = Random.Range (0, prioridades.Count); //elegimos al azar una de esas carreteras que tiene la misma prioridad que se ha elegido
		Terrain carretera = prioridades.Count > 0 ? prioridades [iCarretera] : carreteras[0];//la elegida

		return carretera; 
	}
}

