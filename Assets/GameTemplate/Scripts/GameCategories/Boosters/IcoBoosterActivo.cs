using UnityEngine;
using System.Collections;

public class IcoBoosterActivo : MonoBehaviour {
	public BoosterType type;
	public UILabel lbProgress;
	private Booster booster;

	private float progress;

	public void init(Booster b){
		booster = b;
		progress = booster.Duration;
		gameObject.SetActive (true);
	}

	void Update(){
		lbProgress.text = (booster.Duration - booster.Progress).ToString ();
	}
	
	public void finish(){
		gameObject.SetActive (false);
	}
}
