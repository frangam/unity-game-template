using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIProTransform : MonoBehaviour {
	[SerializeField]
	private Vector3 proPos;

	private RectTransform noProTransform;
	private RectTransform myT;
	public virtual void Awake(){
		noProTransform = GetComponent<RectTransform>();
		myT = GetComponent<RectTransform>();

		if(GameSettings.Instance.IS_PRO_VERSION){
			myT.anchoredPosition3D = proPos;
		}
	}
}
