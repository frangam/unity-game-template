using UnityEngine;
using System.Collections;

public class ConfigAchievements : MonoBehaviour{
	//--------------------------------------
	// Claves PlayerPrefs
	//--------------------------------------
	public const string NUM_MONEDAS 					= "pl_lg_num_monedas";
	public const string NUM_SURVIVAL_COMPLETADO 		= "pl_lg_num_survival_completado";


	//--------------------------------------
	// Constantes
	//--------------------------------------
	public const int PROGRESO_GET_1 					= 1;
	public const int PROGRESO_GET_2 					= 2;
	public const int PROGRESO_GET_3 					= 3;	
	public const int PROGRESO_GET_4 					= 4;	
	public const int PROGRESO_GET_5 					= 5;
	public const int PROGRESO_GET_6 					= 6;	
	public const int PROGRESO_GET_7 					= 7;	
	public const int PROGRESO_GET_8 					= 8;	
	public const int PROGRESO_GET_9 					= 9;	
	public const int PROGRESO_GET_10		 			= 10;
	public const int PROGRESO_GET_11		 			= 11;
	public const int PROGRESO_GET_12		 			= 12;
	public const int PROGRESO_GET_13		 			= 13;
	public const int PROGRESO_GET_25_PUNTOS 			= 25;
	public const int PROGRESO_GET_50_PUNTOS 			= 50;
	public const int PROGRESO_GET_100_PUNTOS 			= 100;
	public const int PROGRESO_GET_150_PUNTOS 			= 150;
	public const int PROGRESO_GET_200_PUNTOS 			= 200;
	public const int PROGRESO_GET_250_PUNTOS 			= 250;
	public const int PROGRESO_GET_500_PUNTOS 			= 500;
	public const int PROGRESO_GET_1000_PUNTOS 			= 1000;
	public const int PROGRESO_GET_1500_PUNTOS 			= 1500;
	public const int PROGRESO_GET_3000_PUNTOS 			= 3000;
	public const int PROGRESO_GET_5000_PUNTOS 			= 5000;
	public const int PROGRESO_GET_7500_PUNTOS 			= 7500;
	public const int PROGRESO_GET_10000_PUNTOS 			= 10000;
	public const int PROGRESO_GET_25000_PUNTOS 			= 25000;
	public const int PROGRESO_GET_50000_PUNTOS 			= 50000;
	public const int PROGRESO_GET_100000_PUNTOS 		= 100000;
	public const int PROGRESO_GET_500000_PUNTOS 		= 5000000;
	public const int PROGRESO_GET_1000000_PUNTOS 		= 1000000;
	public const int PROGRESO_GET_10000000_PUNTOS 		= 10000000;

	//--------------------------------------
	// Metodos Unity
	//--------------------------------------
	#region Unity
	void Start(){
		DontDestroyOnLoad(this); //para que no se destruyan los atributos al cargar escenas	
	}
	#endregion
}