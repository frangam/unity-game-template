using UnityEngine;
using System.Collections;

public class ButtonChangeInputBackButtonBehaviour : UIBaseButton {
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
		if(openWindow){
			InputBackButton.Instance.OpenWindow = openWindow;
			
			if(closeWindow)
				InputBackButton.Instance.CloseWindow = closeWindow;
		}
		else if(closeWindow){
			InputBackButton.Instance.CloseWindow = closeWindow;
		}
		else if(!string.IsNullOrEmpty(specificScreenToGO)){
			InputBackButton.Instance.SpecificScreenToGO = specificScreenToGO;
		}
		else{
			InputBackButton.Instance.CurrentAction = action;
		}
	}
	
	protected override void doPress ()
	{
		base.doPress ();
		change();
	}
}
