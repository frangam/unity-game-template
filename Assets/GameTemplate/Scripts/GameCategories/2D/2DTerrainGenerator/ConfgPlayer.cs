using UnityEngine;
using System.Collections;

public class ConfgPlayer : MonoBehaviour {

	public GameDifficulty difficulty = GameDifficulty.NONE;
	
	// script to move camera
	public float speed = 7;
	public float rate = 0.2f;
	public float incSpeed = 0.001f;
	public float finalSpeed = 15;
	public bool isProps = false;
}
