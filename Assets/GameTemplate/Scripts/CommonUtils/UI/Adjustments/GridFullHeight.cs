using UnityEngine;
using System.Collections;

public class GridFullHeight : MonoBehaviour {
	private UIGrid grid;

	// Use this for initialization
	void Awake () {
		grid = GetComponent<UIGrid> ();

		grid.cellHeight = (float) Screen.height;
	}
}
