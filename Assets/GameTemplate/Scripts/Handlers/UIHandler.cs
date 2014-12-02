using UnityEngine;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;

public class UIHandler : Singleton<UIHandler> {
	public UIPanel pnlSalir;
	public UIPanel pnlGameover;
	public UIPanel pnlQuest;

	public UILabel lbNivelVentanaMision;
	public UILabel lbNivelVentanaMisionSuperada;
	public UILabel lbNivelVentanaGameOver;
	public UILabel lbDescMision;
	public UILabel lbCantidadMision;
	public UILabel lbScore;
	public UILabel lbMetros;
	public UILabel lbCurrentScore;
	public UILabel lbBestScore;
	public UILabel lbContadorAperturas;
	public UILabel lbProgresoMision;
	public UI2DSprite icoObjetivo;
	public UI2DSprite icoObjVentanaMision;
	public IconoObjetivoMision[] iconosObjetivoMision;
	public IconoObjetivoMision[] iconosObjetivoMisionCompartimento;
	public Transform estrellaCentro;
	public UILabel lbPuntosConseguidos;
	public UILabel lbCombos;
	public UILabel lbMotivoGO;
	public UIPanel pnlPelotasPerdidas;
	public UIPanel pnlPausa;
	public UI2DSprite icoNoPerder;
	public UIPanel pnlConectandoFB;
	public UIPanel pnlFBConectado;
	public UIPanel pnlFBErrorConexion;
	public UISprite icoConectandoFB;
	public UISprite icFBConectado;
	public BotonLogin btnLoginFB;
	public Texture texFotoFBParaMarco;
	public UIScrollView scrollNiveles;
	public UIGrid gridInvitaAmigos;
	public UIPanel pnlInvitaAmigos;
	public Sprite spMarcoAmigo;
	public Sprite spMarcoMio;
	public Camera UICamara;
	public Camera GameCamera;
	public float retrasoAceptarMision = 1.5f;
	public UILabel lbBackCounter;
	public UILabel lbMonedasTotal;
	public MensajeUI[] mensajes;
	public IcoBoosterActivo[] icos;
	public UIPanel pnlAvisoCompraCoche;
//	public BotonConfirmaCompraCoche botonConfirmaCompraCoche;
	public UIPuntos[] puntos;

	public GameObject[] crashParticles; //particulas para cuando el coche se choca
	public GameObject[] particulasDerribo; //particualas para cuando el coche derriba obstaculos
	public GameObject[] particulasSangre; //particualas para cuando el coche derriba zombies
	public GameObject[] particulasSalirseFuera; //particualas para cuando el coche sale de la pista

	UILabel lbPuntosCarro;
	UILabel lbCombosCarro;
   

	void Awake(){
		switch (StartScene.Instance.Section) {
			case  Section.GAME:
				//cerrar
				abrir (GameScreen.GAMEOVER, false);
				abrir (GameScreen.COMPLETED_MISSION, false);
				abrir (GameScreen.EXIT, false);
			break;
		}

	}

	void Start(){
//        if(StartScene.Instance.Seccion == Seccion.ELECCION_NIVEL){
//            //mover scroll hasta boton ultimo nivel desbloqueado
//            int ultimoNivel = PlayerPrefs.GetInt(Configuracion.CLAVE_ULTIMO_NIVEL_DESBLOQUEADO);
//            BotonSeleccionNivel boton = botonNivel(ultimoNivel); //el boton del ultimo nivel desbloqueado
//
//
//            Vector3 pos = new Vector3(scrollNiveles.transform.localPosition.x, -(boton.transform.localPosition.y+boton.transform.parent.parent.localPosition.y), scrollNiveles.transform.localPosition.z);
//			
////			Debug.Log(boton.transform.parent.parent.localPosition.y);
//
//
//            scrollNiveles.MoveRelative(pos); //movemos el scroll
//            scrollNiveles.RestrictWithinBounds (false); //restringimos el movimiento del scroll a los limites del scroll para que no se salga al moverlo
//        }
	}

	void Update(){
		if(lbScore)
			lbScore.text = GameManager.score.ToString();
		if(lbProgresoMision && GameManager.Instance.CurrentLevel != null)
			lbProgresoMision.text = GameManager.Instance.CurrentLevel.Mision.Progreso.ToString () + "/"+GameManager.Instance.CurrentLevel.Mision.Cantidad.ToString();
		if(lbMonedasTotal)
			lbMonedasTotal.text = PlayerPrefs.GetInt(Configuration.PP_TOTAL_COINS).ToString();
	}




	public void abrir(GameScreen ventana, bool abre = true, object data = null){
		//gestion sonido
		if(ventana != GameScreen.SHOW_MISSION && ventana != GameScreen.BACK_COUNTER && ventana != GameScreen.GAMEOVER){
			if(abre)
				GestorSonidos.Instance.play(GestorSonidos.ID_SONIDO.FX_ABRIR_VENTANA);
			else
				GestorSonidos.Instance.play(GestorSonidos.ID_SONIDO.FX_CERRAR_VENTANA);
		}

		//gestion apertura
		switch(ventana){
//		case Ventana.AVISO_COMPRA_COCHE:
//			if(abre){
//				BotonEligeCoche boton = (BotonEligeCoche) data;
//				botonConfirmaCompraCoche.iniciar(boton);
//			}
//
//			if(pnlAvisoCompraCoche)
//				pnlAvisoCompraCoche.gameObject.SetActive(abre);
//
//			break;

		case GameScreen.PAUSE:
			if(pnlPausa)
				pnlPausa.gameObject.SetActive(abre);
			break;
		
		

		case GameScreen.EXIT:
			GameManager.Instance.pause(abre);
			pnlSalir.gameObject.SetActive(abre);
			break;

		case GameScreen.GAMEOVER:
			if(abre){
				
				if(GameManager.Instance.CurrentLevel != null){
					int n = GameManager.Instance.CurrentLevel.Id;

					if(lbNivelVentanaGameOver)
						lbNivelVentanaGameOver.text = Localization.Localize (ExtraLocalizations.NIVEL) + " " + n.ToString ();
				}

				if(lbCurrentScore)
					lbCurrentScore.text = GameManager.score.ToString();

				if(lbBestScore)
					lbBestScore.text = RankingHandler.Instance.mejorPuntuacion().ToString();

				//el motivo del gameover
				if(lbMotivoGO){
					string motivoGO = "";

//					switch(GameManager.Instance.GameoverType){
//					
//
//					}

					lbMotivoGO.text = motivoGO;
				}

				
				pnlGameover.gameObject.SetActive(true); 
				
			}
			else if(pnlGameover)
                pnlGameover.gameObject.SetActive(false);

			break;

		

		case GameScreen.SHOW_MISSION:
//			pnlPelotasPerdidas.gameObject.SetActive(GestorJuego.Instance.Nivel.Mision.CantidadNoSePuedenPerderPelotas > 0); //se muestra este panel si se tiene una mision de no perder un tipo de bolas

			if(abre){
				int nivel = GameManager.Instance.CurrentLevel.Id;
				lbNivelVentanaMision.text = Localization.Localize (ExtraLocalizations.NIVEL) + " " + nivel.ToString ();
				lbDescMision.text = GameManager.Instance.CurrentLevel.Mision.Description;
				lbCantidadMision.text = GameManager.Instance.CurrentLevel.Mision.Cantidad.ToString();
			}
			else{
				StartCoroutine(aceptarMision());
			}

			pnlQuest.gameObject.SetActive(abre);
			break;

		case GameScreen.CONNECTING_FACEBOOK:
			pnlFBErrorConexion.gameObject.SetActive(false);
			pnlConectandoFB.gameObject.SetActive(abre);

//			if(abre)
//				icoConectandoFB.GetComponent<Animator>().SetBool("conectado", );
			break;

		case GameScreen.FACEBOOK_ACCOUNT_CONNECTED:
			if(abre){
				abrir(GameScreen.CONNECTING_FACEBOOK, false); //cerrar primero la de conectando
//				btnLoginFB.gameObject.SetActive(false); //ocultar boton de login

				pnlFBConectado.gameObject.SetActive(true); //abrir fb conectado
				Animator anim = icFBConectado.GetComponent<Animator>();
				anim.SetBool("conectado", true); //y animar el ico de fb
			}
			else
				pnlFBConectado.gameObject.SetActive(false); //abrir fb conectado



			break;

		case GameScreen.FACEBOOK_FAILED_CONNECTION:
			if(abre){
				pnlFBConectado.gameObject.SetActive(false);
				pnlConectandoFB.gameObject.SetActive(false);
			}

			pnlFBErrorConexion.gameObject.SetActive(abre);
			break;

		case GameScreen.FACEBOOK_FRIENDS_INVITATION:
			pnlInvitaAmigos.gameObject.SetActive(abre);
			break;
		}
	}

    //public BotonSeleccionNivel botonNivel(int nivel){
    //    BotonSeleccionNivel res = null;

    //    foreach(BotonSeleccionNivel b in botonesSeleccionNivel){
    //        if(b.Nivel == nivel){
    //            res = b;
    //            break;
    //        }
    //    }

    //    return res;
    //}

	private IEnumerator aceptarMision(){
		yield return new WaitForSeconds (retrasoAceptarMision);
		GameManager.gameStart = true;
	}



	public void mostrarEstrellaCentro(){
		estrellaCentro.gameObject.SetActive (true);
		StartCoroutine (ocultarEstrellaCentral ());
	}

	private IEnumerator ocultarEstrellaCentral(){
		yield return new WaitForSeconds (3);
		estrellaCentro.gameObject.SetActive (false);
	}

	



	public void mostrarPuntos(int puntos, int combos){
		UILabel lp = null;
		UILabel lc = null;

		if(lbPuntosCarro == null){
			lp = Instantiate (lbPuntosConseguidos) as UILabel;
			lbPuntosCarro = lp;
		}
		else
			lp = lbPuntosCarro;

		lp.text = "+"+puntos.ToString ();
		StartCoroutine (destruirLabelPuntosOCombosUI (lp, 1f));

		//mostramos combos solo en los casos adecuados
		if(combos > 1 && (combos == 2 || combos == 3 || combos == 4 || combos == 5 || combos == 10)){
			if(lbCombosCarro == null){
				lc = Instantiate (lbCombos) as UILabel;
				lbCombosCarro = lc;
			}
			else
				lc = lbCombosCarro;

			lc.text = "x"+combos.ToString ();
			StartCoroutine (destruirLabelPuntosOCombosUI (lc,1f));
		}

	}

	private IEnumerator destruirLabelPuntosOCombosUI(UILabel lb, float delay = 2){
		yield return new WaitForSeconds(delay);

		if(lb != null)
			Destroy (lb.gameObject);
	}


	public void mostrarMensaje(TipoMensajeUI tipo){
		foreach(MensajeUI m in mensajes){
			if(m.Tipo == tipo){
				m.init();
				break;
			}
		}
	}

	public void mostrarPuntos(TipoPunto tipo){
		foreach(UIPuntos p in puntos){
			if(p.Tipo == tipo){
				UIPuntos punto = Instantiate(p) as UIPuntos;

				break;
			}
		}
	}



}
