using UnityEngine;
using System.Collections;
//using Chartboost;

public class AndroidBackButton : MonoBehaviour {	
	
	public enum TipoAccion{
		QUIT
		,MENU_PPAL_SIN_CONFIRMAR
		,POPUP_PAUSE
		,RESUME
		,JUEGO
		,ELEGIR_ESCENARIO
		,ELEGIR_JUGADOR
		,MENU_PPAL_CON_CONFIRMACION
	}
	
	public TipoAccion tipo;
	
	
	void LateUpdate () {
				
		#if UNITY_ANDROID || UNITY_WP8 || UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Escape)) {
			
			switch(tipo){
				case TipoAccion.QUIT:
					

				Application.Quit(); 
					break;

			case TipoAccion.JUEGO:

				StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_GAME));
				break;


			case TipoAccion.POPUP_PAUSE:
				if(GameManager.Instance.EnPausa && !GameManager.Instance.ShowingGOPanel){
					GameManager.Instance.pause (false);
				}
				else if(!GameManager.Instance.EnPausa && !GameManager.Instance.ShowingGOPanel){
					GameManager.Instance.pause (true);
				}
				else if(GameManager.Instance.ShowingGOPanel){
					((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
					StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_MAINMENU));
				}
				
				break;

				
			case TipoAccion.MENU_PPAL_SIN_CONFIRMAR:
					
				((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
				StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_MAINMENU));

				
				break;

			case TipoAccion.MENU_PPAL_CON_CONFIRMACION:
				if(GameManager.gameStart && GameManager.Instance.EnPausa && !GameManager.Instance.ShowingGOPanel){
					UIHandler.Instance.abrir(Ventana.SALIR, false);
				}
				else if(GameManager.gameStart && !GameManager.Instance.EnPausa && !GameManager.Instance.ShowingGOPanel){
					UIHandler.Instance.abrir(Ventana.SALIR);
				}
				else if(GameManager.Instance.ShowingGOPanel){
					((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
					StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_LEVEL_SELECTION));
				}



				break;


			
			}				
		}
		#endif
		
	}
}
