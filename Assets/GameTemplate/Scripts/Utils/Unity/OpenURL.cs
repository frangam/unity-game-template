using UnityEngine;
using System.Collections;

public class OpenURL : MonoBehaviour {
	[SerializeField]
	private string url;

	void OnPress(bool pressed){
		if(!pressed){
			Application.OpenURL(url);
		}
	}
}
