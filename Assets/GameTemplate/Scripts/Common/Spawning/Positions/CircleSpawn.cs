using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleSpawn : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private float radius = 2.5f;

	[SerializeField]
	private bool freezeY = true;

	[SerializeField]
	private float yFrozen = 1.05f;

	[SerializeField]
	private Color gizmoColor = Color.red;
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------	
	Vector3 RandomCircle ( Vector3 center ,   float radius  ){
		float ang = Random.value * 360;
		Vector3 pos;
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.y = freezeY ? yFrozen : center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		return pos;
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public Vector3 getARandomSpawnPosition(){
		GameObject res = null;
		Vector3 center = transform.position;
		Vector3 pos = RandomCircle(center, radius);
//		Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center-pos);


		return pos;
	}


	void OnDrawGizmosSelected() {
		Gizmos.color = gizmoColor;
		Gizmos.DrawSphere(transform.position, radius);
	}
}
