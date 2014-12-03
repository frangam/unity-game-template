using UnityEngine;
using System.Collections;

public class Nivel : MonoBehaviour {
	//--------------------------------------
	// Atributos de configuracion
	//--------------------------------------
	[SerializeField]
	private int id = 1;
	
	[SerializeField]
	private Mision mision;

	[SerializeField]
	private int puntosPorObjetivo = 25;

	[SerializeField]
	private int puntosPorNoObjetivo = 10;


	//--------------------------------------
	// Atributos privados
	//--------------------------------------
	/// <summary>
	/// El mejor numero de estrellas conseguidos
	/// </summary>
	private int estrellas = 0;

	/// <summary>
	/// las estrellas que se consigues al jugar el nivel
	/// </summary>
	private int estrellasJugadas = 0;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public int Id {
		get {
			return this.id;
		}
	}

	public Mision Mision {
		get {
			return this.mision;
		}
	}

	public int PuntosPorObjetivo {
		get {
			return this.puntosPorObjetivo;
		}
	}

	public int PuntosPorNoObjetivo {
		get {
			return this.puntosPorNoObjetivo;
		}
	}

	public int Estrellas {
		get {
			return this.estrellas;
		}
	}

	public int EstrellasJugadas {
		get {
			return this.estrellasJugadas;
		}
	}

	void Awake(){
		estrellas = PlayerPrefs.GetInt (Configuration.PP_LEVEL_STARS + id.ToString ());
		mision = FindObjectOfType<Mision> ();
	}

	public void comprobarConseguirEstrella(){
		int maxPuntos = mision.PuntosTresEstrellas;

		bool unaEstrella = estrellasJugadas == 0 && GameManager.score >= maxPuntos*0.25f && GameManager.score < maxPuntos*0.5f;
		bool dosEstrellas = estrellasJugadas == 1 && GameManager.score >=maxPuntos*0.5f && GameManager.score < mision.PuntosTresEstrellas;
		bool tresEstrellas = estrellasJugadas == 2 && GameManager.score >= mision.PuntosTresEstrellas;

		if(unaEstrella){
			estrellasJugadas = 1;
		}
		else if(dosEstrellas){
			estrellasJugadas = 2;
		}
		else if(tresEstrellas){
			estrellasJugadas = 3;
		}

		if(unaEstrella  || dosEstrellas || tresEstrellas){
			GestorSonidos.Instance.play(GestorSonidos.ID_SONIDO.FX_CONSIGUE_ESTRELLA);
//			BarraProgresoEstrellas.Instance.consigueEstrella(estrellasJugadas);
			UIHandler.Instance.mostrarEstrellaCentro();
		}
	}

	public bool aumentaProgreso(){
		bool res = false;

//		if(mision.Progreso+1 <= mision.Cantidad && pelotaRecogida.Tipo == mision.TipoPelota && poderValido(pelotaRecogida.Poder)){
//			res = true;
//			mision.Progreso++;
//		
//			if(mision.superada()){
//				if(estrellas < Configuracion.MAX_ESTRELLAS_POR_NIVEL && estrellasJugadas > estrellas){
//					PlayerPrefs.SetInt(Configuracion.CLAVE_ESTRELLAS_NIVEL+id.ToString(), estrellasJugadas);
//					estrellas = estrellasJugadas;
//				}
//
//				//marcar como ultimo nivel desbloqueado el nivel actual
//				if(PlayerPrefs.GetInt(Configuracion.CLAVE_ULTIMO_NIVEL_DESBLOQUEADO) == id && id != Configuracion.MAX_NIVELES)
//					PlayerPrefs.SetInt(Configuracion.CLAVE_ULTIMO_NIVEL_DESBLOQUEADO, id+1);
//				
//				//comprobamos si hay logro de desbloqueo de nivel
//				GestorLogros.Instance.comprobarLogros(id, GestorLogros.TipoLogro.DESBLOQUEAR_NIVELES);
//
//				//notificar al gestor de juego que la mision ha sido superada y provocar el gameover
//				GestorJuego.misionSuperada = true;
//			}
//		}

		return res;
	}


}
