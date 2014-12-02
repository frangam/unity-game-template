using UnityEngine;
using System.Collections;

public class GTButton : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool hacerScroll = true;

	[SerializeField]
	/// <summary>
	/// Add a delay after press button
	/// </summary>
	private float retrasoPostPulsar = 0f;

	[SerializeField]
	private UIFunction function;

	[SerializeField]
	private UIPanel pnlInfo;

	[SerializeField]
	private UIPanel pnlResetMisiones;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Animator anim;
	private bool enMovimiento;



	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	void Awake(){
		anim = GetComponent<Animator> ();

		if(anim != null)
			anim.SetBool("press", false);

		switch (function) {
		case UIFunction.ABRIR_MENU_INVITAR_AMIGOS_FB:
			gameObject.SetActive(SPFacebook.instance.IsLoggedIn);
			break;

		case UIFunction.RANKING_SCREEN_SWITCH_DIFFICULT:
#if UNITY_ANDROID || UNITY_IPHONE
			GameDifficulty dif = (GameDifficulty) PlayerPrefs.GetInt(Configuration.PP_GAME_DIFFICULTY);
            gameObject.SetActive(dif != GameDifficulty.NONE);

#elif UNITY_WP8
            gameObject.SetActive(false);
#endif
			break;

            case UIFunction.ACHIEVEMENTS_SCREEN:
            case UIFunction.RANKINGS_SCREEN:
                #if UNITY_WP8
                    gameObject.SetActive(false);
                #endif
            break;
		}
	}

	void Start(){
		switch(function){
			case UIFunction.CONTINUE_HISTORY_MODE:
			GetComponent<UIButton>().isEnabled = false;
			GetComponent<UIWidget>().enabled = false;
			foreach(Transform t in transform)
				t.GetComponent<UIWidget>().enabled = false;
			StartCoroutine(iniciar(2.05f));
			break;
		}
	}
	#endregion



	//--------------------------------------
	// NGUI Methods
	//--------------------------------------
	#region NGUI
	
	void OnPress (bool inicioToque){
		if(inicioToque){
			if(anim != null){
				anim.SetBool("press", true);
			}
		}

		//touch up
		if(!inicioToque && !enMovimiento){
			pulsar();
		}
			else 		
		if(!inicioToque){
			enMovimiento = false; //fin del movimiento
		}
	}

	void OnDrag(Vector2 delta){
		if(hacerScroll){
			enMovimiento = true;
		}
	}
	#endregion

	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private IEnumerator iniciar(float espera){
		yield return new WaitForSeconds (espera);
		GetComponent<UIButton>().isEnabled = true;
		GetComponent<UIWidget>().enabled = true;
		foreach(Transform t in transform)
			t.GetComponent<UIWidget>().enabled = true;
	}

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void pulsar(){
		switch(function){
//		case UIFunction.MODO_FACIL:
//			PlayerPrefs.SetInt(Configuration.PP_GAME_DIFFICULTY, (int) GameDifficulty.EASY);
//			((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
//			StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.ESCENA_GARAJE));
//			break;
//		case UIFunction.MODO_DIFICIL:
//			PlayerPrefs.SetInt(Configuration.PP_GAME_DIFFICULTY, (int) GameDifficulty.HARD);
//			((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
//			StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.ESCENA_GARAJE));
//			break;
//		case UIFunction.MODO_NORMAL:
//			PlayerPrefs.SetInt(Configuration.PP_GAME_DIFFICULTY, (int) GameDifficulty.NORMAL);
//			((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
//			StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.ESCENA_GARAJE));
//			break;
			
			//Ir a la pantalla de juego
		case UIFunction.GAME_SCREEN:
			((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
			StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_GAME));
			break;
			
		case UIFunction.MAIN_MENU_WITH_COMFIRMATION:
			UIHandler.Instance.abrir(GameScreen.EXIT);
			break;
			
		case UIFunction.INFO_SCREEN:
			((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
			StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_INFO));
			break;
			
		case UIFunction.TEST_RESET_HISTORY:
			PlayerPrefs.DeleteKey(Configuration.PP_LAST_LEVEL_UNLOCKED);
			
			((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
			StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_MAINMENU));
			break;
			
		case UIFunction.TEST_INC_POINTS:
			GameManager.Instance.CurrentLevel.Mision.Progreso += 5;
			
			break;
			
		case UIFunction.ACCEPT_QUEST:
			UIHandler.Instance.abrir(GameScreen.SHOW_MISSION, false);
			break;
			
		case UIFunction.ELECCION_NIVEL:
			if(StartScene.Instance.Section == Section.MAIN_MENU && !Configuration.firstTimePlayerInviteFriends)
				Configuration.firstTimePlayerInviteFriends = true;
			
			((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
			StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_LEVEL_SELECTION));
			break;
			
			
		case UIFunction.MAIN_MENU_SCREEN_FROM_GAME_SCREEN:

			((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
			StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_MAINMENU));
			break;
			
		case UIFunction.MORE_GAMES_SCREEN:
			#if !UNITY_EDITOR && UNITY_IPHONE || UNITY_ANDROID
			GestorChartboost.Instance.mostrarMoreApps();
			#endif
			break;
			
		case UIFunction.MAIN_MENU_SCREEN:
			((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
			StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_MAINMENU));
			break;
			
		case UIFunction.PAUSE_GAME:
			GameManager.Instance.pause();
			break;
			
		case UIFunction.RESUME_GAME:
			GameManager.Instance.pause(false);
			break;
			
		case UIFunction.RANKINGS_SCREEN:
			RankingHandler.Instance.mostrarClasificacionGeneral();
			break;

		case UIFunction.RANKING_SCREEN_SWITCH_DIFFICULT:
			GameDifficulty dif = (GameDifficulty) PlayerPrefs.GetInt(Configuration.PP_GAME_DIFFICULTY);
			RankingHandler.Instance.mostrarClasificacionGeneral(dif);
			break;
			
		case UIFunction.ACHIEVEMENTS_SCREEN:
			AchievementsHandler.Instance.mostrarLogros();
			break;
			
		case UIFunction.ABRIR_MENU_INVITAR_AMIGOS_FB:
			if(SPFacebook.instance.IsLoggedIn )
				SPFacebook.instance.AppRequest (Localization.Get(ExtraLocalizations.FB_INVITACION));
			//				GestorUI.Instancia.abrir(Ventana.FB_INVITA_AMIGOS);
			break;
			
		case UIFunction.INVITAR_AMIGOS_FB:
			
			break;
		}
	}



	private IEnumerator esperar(float delay, string escena){
		yield return new WaitForSeconds(delay);

		StartCoroutine( ScreenLoaderIndicator.Instance.Load (escena));
	}


}
