using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class OleadaBase {
	public GOOleada go;
	public Priority prioridad = Priority.MINIMUM;
	public List<TerrainType> whereCanSpawn;
	public bool canMove = false;
	public LayerMask maskForCollision; 
	public float minDistanciaColision = 3f;

	public Vector3 dirMov = Vector3.left; //izq
	public float minVelocidad = 1f;
	public float maxVelocidad = 10;
	public float decVelocidad = 0.001f;






	private float velocidad = 0;
	private float velInicial = 0;
	private bool vivo = true;
	private GeneradorOleadas gestorOleadas;

	public OleadaBase(OleadaBase o, ZonaGeneracion _zona):this (o.gestorOleadas, o.canMove, o.whereCanSpawn, o.minDistanciaColision, o.dirMov, o.minVelocidad, o.maxVelocidad
	                                                            , o.decVelocidad, o.prioridad, o.go, o.maskForCollision, _zona){}

	public OleadaBase(GeneradorOleadas _gestor, bool _canMove, List<TerrainType> _whereCanSpawn, float _minDistCol, Vector3 _dirMov, float _minVel, float _maxVel
	                  , float _decVel, Priority _prioridad, GOOleada _go, LayerMask _mask, ZonaGeneracion _zona){
		gestorOleadas = _gestor;
		canMove = _canMove;
		whereCanSpawn = _whereCanSpawn;
		minVelocidad = _minVel;
		minDistanciaColision = _minDistCol;
		dirMov = _dirMov;
		maxVelocidad = _maxVel;
		decVelocidad = _decVel;
		prioridad = _prioridad;
		go = _go;
		zona = _zona;
		maskForCollision = _mask;

		if(canMove){
			velocidad = Random.Range (minVelocidad, maxVelocidad);
			velInicial = velocidad;
		}
	}

	/// <summary>
	/// la zona donde se ha generado
	/// </summary>
	private ZonaGeneracion zona;

	public GeneradorOleadas GestorOleadas {
		get {
			return this.gestorOleadas;
		}
	}

	public ZonaGeneracion Zona {
		get {
			return this.zona;
		}
	}

	public float Velocidad {
		get {
			return this.velocidad;
		}
		set{
			velocidad = value;
		}
	}

	public float VelInicial {
		get {
			return this.velInicial;
		}
	}

	public void inicializar(ZonaGeneracion _zona){
		zona = _zona;
		go.inicializar (this);
	}

	public bool Vivo {
		get {
			return this.vivo;
		}
		set {
			vivo = value;
		}
	}

	public override string ToString ()
	{
		return string.Format ("[OleadaBase: go={0}, prioridad={1}, canMove={2}, maskForCollision={3}, velocidad={4}, zona={5}]", go, prioridad, canMove, maskForCollision, velocidad, zona);
	}
	
	
}
