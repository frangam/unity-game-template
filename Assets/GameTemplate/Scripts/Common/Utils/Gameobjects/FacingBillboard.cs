using UnityEngine;
using System.Collections;

public class FacingBillboard : MonoBehaviour {
	public Transform target;

	[Tooltip("If set to true does not matter target")]
	public bool mainCamera = true;

	void Awake(){
		if(mainCamera)
			target = Camera.main.transform;
	}

	void Update(){
		if(target != null)
			transform.LookAt(transform.position + target.rotation * Vector3.back,
		                 target.rotation * Vector3.up);
	}
}
