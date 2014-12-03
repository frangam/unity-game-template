using UnityEngine;
using System.Collections;

public class PanelCargando : MonoBehaviour {
	public float alfa = 0.748f;
	
	private UIPanel panel;
	
	void Awake(){
		panel = GetComponent<UIPanel>();
		ocultar();
	}
	
	public void mostrar(){
		panel.alpha = alfa;	
	}
	
	public void ocultar(){
		panel.alpha = 0;
	}
}
