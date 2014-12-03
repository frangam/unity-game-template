using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Booster : MonoBehaviour {
	[SerializeField]
	private BoosterType type;

	[SerializeField]
	private float duration = 0;

	[SerializeField]
	private float units = 0;

	[SerializeField]
	private List<string> tags;

	[SerializeField]
	private float price = 0;

	[SerializeField]
	private TipoMensajeUI tipoMensajeUI;

	[SerializeField]
	private GestorSonidos.ID_SONIDO sonido;

	[SerializeField]
	private float rangeToNotifyFinishing = 0.5f;

	private bool applying = true;
	private SpriteRenderer spRenderer;
	private IcoBoosterActivo icoActivo; //incono gui que muestra el cooldown
	private float progress = 0;

	public delegate void notificarFinEfecto();
	public static event notificarFinEfecto OnFinEfecto;

	public bool Applying {
		get {
			return this.applying;
		}
		set {
			applying = value;
		}
	}

	public float Units {
		get {
			return this.units;
		}
	}

	public float Price {
		get {
			return this.price;
		}
	}

	public float Duration {
		get {
			return this.duration;
		}
	}

	protected virtual void apply(){
		GestorSonidos.Instance.play (sonido);
		BoostersHandler.Instance.activarBooster (this);
		initIcoActivo ();

		if(icoActivo != null)
			icoActivo.init(this);

		UIHandler.Instance.mostrarMensaje (tipoMensajeUI);

		applying = true;

		if(spRenderer != null){
			spRenderer.enabled = false;
		}

		StartCoroutine (checkIfIsFinishing ());
		StartCoroutine (checkDuration ());
	}

	public BoosterType Type {
		get {
			return this.type;
		}
	}

	public float Progress {
		get {
			return this.progress;
		}
	}


	public virtual void finish(){
		applying = false;

		if(icoActivo != null)
			icoActivo.finish();

		//dispatch event
		OnFinEfecto ();

		Destroy (gameObject);
	}

	protected virtual void notifyIsFinishing(){
		
	}

	protected virtual void Awake(){
		spRenderer = GetComponent<SpriteRenderer> ();
	}

	public void iniciar(){
		apply ();
	}

	protected virtual void OnTriggerEnter2D(Collider2D collider){
		if(tags != null && tags.Count > 0 && tags.Contains(collider.tag)){
			apply();
		}
	}

	protected virtual IEnumerator checkIfIsFinishing(){
		float range = 0f;

		do{
			range = (progress*1f)/duration;
			yield return null;
		}
		while(range < rangeToNotifyFinishing);

		notifyIsFinishing();
	}

	private IEnumerator checkDuration(){
		while(progress < duration){
			yield return new WaitForSeconds(1);
			progress++;
		}

		finish ();
	}


	private void initIcoActivo(){
		foreach(IcoBoosterActivo ico in UIHandler.Instance.icos){
			if(ico.type == type){
				icoActivo = ico;
				break;
			}
		}
	}
}
