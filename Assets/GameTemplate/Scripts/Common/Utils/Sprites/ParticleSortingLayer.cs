using UnityEngine;
using System.Collections;

public class ParticleSortingLayer : MonoBehaviour {
	//--------------------------------------
	// Atributos de configuracion
	//--------------------------------------
	[SerializeField]
	private string layer = "Default";

	[SerializeField]
	private int order = 0;

	//--------------------------------------
	// Atributos privados
	//--------------------------------------
	private ParticleRenderer partRenderer;
	private ParticleAnimator anim;

	// Use this for initialization
	void Start () {
		partRenderer = GetComponent<ParticleRenderer> ();
		anim = GetComponent<ParticleAnimator> ();

		// Set the sorting layer of the particle system.
		if(partRenderer != null){
			partRenderer.sortingLayerName = layer;
			partRenderer.sortingOrder = order;
		}
		else if(GetComponent<ParticleSystem>() != null){
			GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = layer;
			GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = order;
		}
	}

}
