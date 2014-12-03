using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	[SerializeField]
	private float cantidadMov = 1f;

	[SerializeField]
	private float defaultSpeed = 2f;

	[SerializeField]
	private bool invencible = false;

	private int track = 0;
	private Animator anim;
	private float speed;
	private bool desplazando = false;
	private Vector3 posDestino = Vector3.zero;
	private Vector3 movimiento = Vector3.zero;
	private SpriteRenderer spr;

	public bool Invencible {
		get {
			return this.invencible;
		}
		set {
			invencible = value;
		}
	}

	void Awake(){
		anim = GetComponent<Animator> ();

	}
}