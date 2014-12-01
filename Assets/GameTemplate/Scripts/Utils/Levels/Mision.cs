using UnityEngine;
using System.Collections;

public class Mision : MonoBehaviour {
	//--------------------------------------
	// Atributos de configuracion
	//--------------------------------------
	[SerializeField]
	/// <summary>
	/// La cantidad objetivo a completar
	/// </summary>
	private int cantidadObjetivo;

	[SerializeField]
	/// <summary>
	/// La cantidad maxima que se genera
	/// </summary>
	private int cantidadMaxGenerada;

	[SerializeField]
	private TipoMision tipo;


	[SerializeField]
	private int puntosTresEstrellas;


	//--------------------------------------
	// Atributos privados
	//--------------------------------------
	private string description;
	private int progreso;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public int Cantidad {
		get {
			return this.cantidadObjetivo;
		}
	}

	public int Progreso {
		get {
			return this.progreso;
		}
		set {
			progreso = value;
		}
	}

	public TipoMision Tipo {
		get {
			return this.tipo;
		}
	}


	public string Description {
		get {
			string res = "";

			switch(tipo){
			case TipoMision.RECOGE_MONEDAS:
				res = Localization.Localize(ExtraLocalizations.OBJETIVO_RECOGE_MONEDAS) + " " +  cantidadObjetivo + Localization.Localize(ExtraLocalizations.OBJETIVO_RECOGE_MONEDAS_FIN);
				break;

			case TipoMision.AGUANTAR_TIEMPO:
				res = Localization.Localize(ExtraLocalizations.OBJETIVO_AGUANTAR_TIEMPO) + " " +  cantidadObjetivo + Localization.Localize(ExtraLocalizations.OBJETIVO_AGUANTAR_TIEMPO_FIN);
				break;

			case TipoMision.DISTANCIA_RECORRIDA:
				res = Localization.Localize(ExtraLocalizations.OBJETIVO_DISTANCIA_RECORRIDA) + " " +  cantidadObjetivo + Localization.Localize(ExtraLocalizations.OBJETIVO_DISTANCIA_RECORRIDA_FIN);
				break;
			}

			return res;
		}
	}

	public int PuntosTresEstrellas {
		get {
			return this.puntosTresEstrellas;
		}
	}





	//--------------------------------------
	// Metodos publicos
	//--------------------------------------
	public bool superada(){
		return progreso >= cantidadObjetivo;
	}
	public bool fallida(){
		return false; //TODO revisar
	}


	//--------------------------------------
	// Metodos privados
	//--------------------------------------

}
