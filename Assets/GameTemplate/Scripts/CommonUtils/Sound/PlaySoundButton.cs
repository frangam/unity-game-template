using UnityEngine;
using System.Collections;

public class PlaySoundButton : MonoBehaviour {
	public enum Trigger{
		TOUCH_UP,
		TOUCH_DOWN,
	}

	[SerializeField]
	private Trigger trigger;

	[SerializeField]
	private GestorSonidos.ID_SONIDO sonido = GestorSonidos.ID_SONIDO.FX_CLICK_BOTON_UI;

	private bool enMovimiento;

	void OnPress (bool inicioToque){
		//touch up
		if(trigger == Trigger.TOUCH_UP && !inicioToque && !enMovimiento){
			GestorSonidos.Instance.play(sonido);
		}
		//touch down
		else if(trigger == Trigger.TOUCH_DOWN && inicioToque && !enMovimiento){
			GestorSonidos.Instance.play(sonido);
			enMovimiento = false; //fin del movimiento
		}
		else if(!inicioToque){
			enMovimiento = false; //fin del movimiento
		}
	}

	void OnDrag(Vector2 delta){
		enMovimiento = true;
	}
}
