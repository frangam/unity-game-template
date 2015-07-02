/**
 * ScreenBounds.cs
 * 
 * ScreenBounds scale and locate the four screen bounds if exist (top, bottom, left and right).
 * File location: Assets/Scripts/Utils/Unity/ScreenBounds.cs
 * 
 * 
 * (c) Copyright 2014 fgarmo.com All Rights Reserved.
 * 
 * @version 1.0.0
 * @Unity3D 4.3
 *
 */

using UnityEngine;
using System.Collections;

public class ScreenBounds : MonoBehaviour {
	public BoxCollider top;
	public BoxCollider bottom;
	public BoxCollider left;
	public BoxCollider right;

	public BoxCollider2D top2D;
	public BoxCollider2D bottom2D;
	public BoxCollider2D left2D;
	public BoxCollider2D right2D;

	void Awake(){
		//top bound
		if(top){
			top.size = new Vector3 (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f, 1f);
			top.center = new Vector3 (0f, Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y + 0.5f, 0f);
		}
		if(top2D){
			top2D.size = new Vector2 (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
			top2D.offset = new Vector2 (0f, Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y + 0.5f);
		}
		
		//bottom bound
		if(bottom){
			bottom.size = new Vector3 (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f, 1f);
			bottom.center = new Vector3 (0f, Camera.main.ScreenToWorldPoint(Vector3.zero).y - 0.5f, 0f);
		}
		if(bottom2D){
			bottom2D.size = new Vector2 (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
			bottom2D.offset = new Vector2 (0f, Camera.main.ScreenToWorldPoint(Vector3.zero).y - 0.5f);
		}
//
//		//right bound
//		if(right){
//			right.size = new Vector2 (1f, Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y);
//			right.center = new Vector2 (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + 0.5f, 0f);
//		}
//
//		//left bound
//		if(left){
//			left.size = new Vector2 (1f, Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y);
//			left.center = new Vector2 (Camera.main.ScreenToWorldPoint(Vector3.zero).x - 0.5f, 0f);
//		}
	}
}
