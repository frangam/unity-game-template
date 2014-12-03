using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GestorTutorial : Singleton<GestorTutorial> {
	public Transform organizadorPaneles;
	public bool iniciarPausado = false;

	/// <summary>
	/// Deben estar ordenados como hijos del organizadorPaneles
	/// segun el orden de aparicion que deseemos
	/// </summary>
	private List<PanelTutorial> panelesTutoriales;



	
	private int progreso = 0;
	private int progresoCasilla = 0;
	private int progresoFicha = 0;
	private int progresoIA = 0;
	private int progresoTotal = 0;
	
	
	void Awake(){
		panelesTutoriales = new List<PanelTutorial> ();
		PanelTutorial[] aux = Transforms.getComponentsInChildren<PanelTutorial> (organizadorPaneles) as PanelTutorial[];
		foreach(PanelTutorial p in aux)
			panelesTutoriales.Add(p);
		panelesTutoriales.Sort ();

		int i = 0;
		foreach(PanelTutorial p in panelesTutoriales){
			p.gameObject.SetActive(i==0);
			i++;
		}

		progresoTotal = panelesTutoriales.Count;
	}

	void Start(){
		Time.timeScale = iniciarPausado ? 0f : 1f;
	}
	
	public void siguiente(){
		if(progreso < panelesTutoriales.Count)
			panelesTutoriales [progreso].gameObject.SetActive (false);
		
		progreso++;
		
		if(progreso < panelesTutoriales.Count)
			panelesTutoriales [progreso].gameObject.SetActive (true);
		
//		Debug.Log (progreso+"/"+progresoTotal);

		
		if (progreso >= progresoTotal)
			fin ();
	}
	
	private void fin(){
		Time.timeScale = 0f;
		PlayerPrefs.SetInt (Configuration.PP_COMPLETED_TUTORIAL, 1); //guardar la superacion del tutorial
		Configuration.mandatoryTutorial = false;

        //Debug.Log ("fin tutorial");
		StartCoroutine (ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_MAINMENU));
	}
}
