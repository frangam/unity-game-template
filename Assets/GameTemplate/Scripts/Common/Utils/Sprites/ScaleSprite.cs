/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

public class ScaleSprite : MonoBehaviour {
	public enum ScaleType{
		ONLY_WIDTH,
		ONLY_HEIGHT,
		WIDTH_AND_HEIGHT,
		WIDTH_RELATIVE_TO_HEIGHT
	}
	
	public ScaleType scaleType = ScaleType.WIDTH_AND_HEIGHT;
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
		
		
		
		switch(scaleType){
		case ScaleType.WIDTH_AND_HEIGHT:
			transform.localScale = new Vector3( worldScreenWidth / width, worldScreenHeight / height, transform.localScale.z);
			
			
			break;
			
		case ScaleType.ONLY_WIDTH:
			transform.localScale = new Vector3( worldScreenWidth / width, transform.localScale.y, transform.localScale.z);
			break;
			
		case ScaleType.WIDTH_RELATIVE_TO_HEIGHT:
			transform.localScale = new Vector3( worldScreenWidth / width, transform.localScale.y/height/2, transform.localScale.z);
			break;
			
		case ScaleType.ONLY_HEIGHT:
			transform.localScale = new Vector3( transform.localScale.x/width*2, worldScreenHeight, transform.localScale.z);
			break;
		}
	}
	
}
