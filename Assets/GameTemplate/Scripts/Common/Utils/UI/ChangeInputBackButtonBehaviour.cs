using UnityEngine;
using System.Collections;

[System.Serializable]
public class ChangeInputBackButtonBehaviour {
	[SerializeField]
	private bool active = false;

	[SerializeField]
	private InputBackButton.Action action;
	
	[SerializeField]
	[Tooltip("Leave empty if do navigate with Action. If we go to an specific scene represented by another game section fill it")]
	private string specificScreenToGO;
	
	[SerializeField]
	private UIBaseWindow openWindow;
	
	[SerializeField]
	private UIBaseWindow closeWindow;
	

	public void change(){
		if(active){
			InputBackButton.Instance.OpenWindow = openWindow;
			InputBackButton.Instance.CloseWindow = closeWindow;
			InputBackButton.Instance.SpecificScreenToGO = specificScreenToGO;
			InputBackButton.Instance.CurrentAction = action;
		}
	}
}
