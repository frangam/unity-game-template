using UnityEngine;
using System.Collections;

/// <summary>
/// Shows if the active platform is in platforms array of this gameobject
/// if not disables it
/// </summary>
public class ExclusivePlatform : MonoBehaviour {
	[SerializeField]
	private RuntimePlatform[] platforms;

	void Awake(){
		bool show = false;

		foreach (RuntimePlatform p in platforms) {
			show = Application.platform == p;

			if(show)
				break;
		}

		gameObject.SetActive (show);
	}
}
