using UnityEngine;
using System;
using System.Collections;

public class ZonaGeneracion : MonoBehaviour, IComparable<ZonaGeneracion> {
	[SerializeField]
	private int prioridad = 100;



	public int Prioridad {
		get {
			return this.prioridad;
		}
	}



	public int CompareTo (ZonaGeneracion other){
		return prioridad.CompareTo (other.prioridad);
	}
}
