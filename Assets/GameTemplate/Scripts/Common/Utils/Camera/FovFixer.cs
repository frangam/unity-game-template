using UnityEngine;
using System.Collections;

public class FovFixer : MonoBehaviour {
	public float originalWidth = 1024.0f;
	public float originalHeight = 768.0f;
	
	private float originalAspectRatio;
	
	public enum ADJUST_MODE {
		FIXED_W,
		FIXED_H,
		FIXED_MINIMUM
	}
	public ADJUST_MODE adjustMode = ADJUST_MODE.FIXED_MINIMUM;
	
	public float originalFovH{get; private set;}
	public float originalFovV{get; private set;}
	
	protected float previousAspect;
	protected ADJUST_MODE previousMode;
	
	
	public Camera cam {get; private set;}
	
	public delegate void OnFovChanged();
	public event OnFovChanged onFovChanged;
	
	// Use this for initialization
	public virtual void Awake () {
		originalAspectRatio = originalWidth / originalHeight;
		
		cam = GetComponent<Camera>();
		if(cam == null)
			Debug.LogError("FovFixer in " + gameObject.name + " needs a camera");
		
		
		originalFovV = cam.fieldOfView;
		originalFovH = GetFovX(originalAspectRatio, cam.fieldOfView);
		
		Fit();
	}
	
	
	// Update is called once per frame
	void Update () {
		if(cam.aspect != previousAspect || adjustMode != previousMode) {
			Fit();
		}
	}
	
	protected virtual void Fit() {
		//Unity keeps fovV constant, but I want fovH constant
		previousAspect = cam.aspect;
		previousMode = adjustMode;
		float aspectRatio = cam.aspect;
		
		switch(adjustMode)
		{
		case ADJUST_MODE.FIXED_W:
			SetFovY( 2.0f * Mathf.Atan( Mathf.Tan( originalFovH  * Mathf.Deg2Rad / 2.0f ) / aspectRatio) * Mathf.Rad2Deg);
			break;
			
			//This is unity default mode
		case ADJUST_MODE.FIXED_H:
			SetFovY(originalFovV);
			break;
			
		case ADJUST_MODE.FIXED_MINIMUM:
			if(aspectRatio < originalAspectRatio)
				SetFovY( 2.0f * Mathf.Atan( Mathf.Tan( originalFovH * Mathf.Deg2Rad / 2.0f ) / aspectRatio) * Mathf.Rad2Deg);
			else
				SetFovY(originalFovV);
			break;
		}
		
		if(onFovChanged != null)
			onFovChanged();
	}
	
	protected void SetFovY(float fovY) {
		// The trick, and why there is no longer two cases, one for orthographic cameras and another for perspective cameras
		// Ortho cameras doens't have fov (obviously) but we are going to assume that fov is the one selected in perspective
		// Then calculate the distance at which we have orthographic size with that fov
		//
		//                                             _
		//                                       *****|
		//                                  *****     |
		//                             *****          |
		//                        ***\*               | cam.orthographicSize
		//                   *****    \ cam.fov/2     | 
		//              *****         |               |
		//         ************************************_
		//                      orthoDist
		//
		// When we change the fov we change also the orthographicSize
		
		float orthoDist = (cam.orthographicSize) / Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2.0f);
		cam.fieldOfView = fovY;
		cam.orthographicSize = Mathf.Tan(fovY * Mathf.Deg2Rad / 2.0f) * orthoDist;
	}
	
	public float GetFovX(float _aspectRatio, float _fovY) {
		return Mathf.Atan(_aspectRatio * Mathf.Tan(_fovY * Mathf.Deg2Rad / 2.0f)) * Mathf.Rad2Deg * 2.0f;
	}
	
	void OnDrawGizmos()	{
		if(!gameObject.activeInHierarchy)
			return;
		
		//Show camera safe frame (it will appear both in Scene and Game windows)		
		float ar;
		if(Application.isPlaying) {	
			ar = cam.pixelRect.width / cam.pixelRect.height;
		}else {
			//we don't have camera cached yet con cam, so let's do it here
			cam = GetComponent<Camera>();
			if(cam == null)
				return;
			
			ar = originalAspectRatio;
		}
		
		//Cam.near plane dimensions
		float nW, nH;
		if(cam.orthographic) {
			nH = cam.orthographicSize * 2.0f;
		} else {
			nH = Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2.0f) * cam.nearClipPlane * 2.0f;
		}
		
		if(ar < originalAspectRatio) {
			nW = ar * nH;	
			nH = nW / originalAspectRatio;
		} else {
			nW = nH * originalAspectRatio;
		}
		
		Gizmos.color = Color.yellow;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube(Vector3.forward * (cam.nearClipPlane + 0.001f), new Vector3(nW, nH, 0.0f));
		Gizmos.matrix = Matrix4x4.identity;
	}
}
