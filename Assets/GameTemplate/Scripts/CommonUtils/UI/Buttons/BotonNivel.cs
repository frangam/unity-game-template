using UnityEngine;
using System.Collections;

public class BotonNivel : MonoBehaviour {
	[SerializeField]
	private int id = 1;

	[SerializeField]
	private UISprite spLock;


	private UIButton button;
	private bool unlocked;

	void Awake () {
		button = GetComponent<UIButton> ();

		if(id == 1){
			unlocked = true;
		}
		else{
			int lastLevelUnlocked = PlayerPrefs.GetInt(Configuration.PP_LAST_LEVEL_UNLOCKED);
			unlocked = id <= lastLevelUnlocked; 
		}

		spLock.gameObject.SetActive (!unlocked); //show the lock
		button.isEnabled = unlocked; //enable the buttons if it it is not locked
	}

	void OnPress(bool pressed){
		if(!pressed){
			PlayerPrefs.SetInt(Configuration.PP_SELECTED_LEVEL, id);
			((PanelCargando) GameObject.FindObjectOfType(typeof(PanelCargando))).mostrar();
			StartCoroutine( ScreenLoaderIndicator.Instance.Load (Configuration.SCENE_GAME));
		}
	}
}
