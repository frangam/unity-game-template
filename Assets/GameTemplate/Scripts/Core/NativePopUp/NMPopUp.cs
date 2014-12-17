using UnityEngine;
using UnionAssets.FLE;
using System.Collections;
using System.Collections.Generic;

public class NMPopUp :  EventDispatcher{

	public string title;
	public string message;

	public void onDismissed(string data) {
		dispatch(BaseEvent.COMPLETE, NMDialogResult.CLOSED);
		Destroy(gameObject);
	}
}
