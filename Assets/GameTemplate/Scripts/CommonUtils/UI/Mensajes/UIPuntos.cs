using UnityEngine;
using System.Collections;

public class UIPuntos : MonoBehaviour {
	[SerializeField]
	private int puntos;

	[SerializeField]
	private TipoPunto tipo;

	private Animator animator;

	public int Puntos {
		get {
			return this.puntos;
		}
	}

	public TipoPunto Tipo {
		get {
			return this.tipo;
		}
	}

	void Awake(){
		animator = GetComponent<Animator> ();
	}

	void Start(){
		animator.speed = Random.Range(0.1f, 1.5f);
		StartCoroutine (finmostrarpunto ());
	}

	private IEnumerator finmostrarpunto(){
		yield return new WaitForSeconds(0.75f);
		
		Destroy (gameObject);
	}
}
