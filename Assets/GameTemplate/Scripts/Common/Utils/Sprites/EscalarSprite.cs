using UnityEngine;
using System.Collections;

public class EscalarSprite : MonoBehaviour {
	public enum Escalado{
		ANCHO,
		ALTO,
		ANCHO_Y_ALTO,
		ANCHO_RELATIVO_ALTURA
	}

	public Escalado escalado = Escalado.ANCHO_Y_ALTO;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer == null) return;
		
		transform.localScale = new Vector3(1f,1f,1f);
		
		float width = spriteRenderer.sprite.bounds.size.x;
		float height = spriteRenderer.sprite.bounds.size.y;
		
		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;



		switch(escalado){
		case Escalado.ANCHO_Y_ALTO:
			transform.localScale = new Vector3( worldScreenWidth / width, worldScreenHeight / height, transform.localScale.z);


			break;

		case Escalado.ANCHO:
			transform.localScale = new Vector3( worldScreenWidth / width, transform.localScale.y, transform.localScale.z);
			break;

		case Escalado.ANCHO_RELATIVO_ALTURA:
			transform.localScale = new Vector3( worldScreenWidth / width, transform.localScale.y/height/2, transform.localScale.z);
			break;

		case Escalado.ALTO:
			transform.localScale = new Vector3( transform.localScale.x/width*2, worldScreenHeight, transform.localScale.z);
			break;
		}
	}

}
