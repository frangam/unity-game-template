using UnityEngine;
using System.Collections;

public class UIConfirmationDialog : UIBaseWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private UIBaseButton 	btnConfirm;
	
	[SerializeField]
	private UIBaseButton 	btnCancel;

	[SerializeField]
	private bool 			closeWhenCancel = true;

	[SerializeField]
	private bool 			closeWhenConfirm = true;

	[SerializeField]
	private UIBaseWindow	winToOpenWhenCancel;

	[SerializeField]
	private UIBaseWindow	winToOpenWhenConfirm;

	[SerializeField]
	private string			sceneToGoWhenCancel;

	[SerializeField]
	private string			sceneToGoWhenConfirm;

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void confirm(){
		if(!string.IsNullOrEmpty(sceneToGoWhenConfirm))
			ScreenLoaderVisualIndicator.Instance.LoadScene(sceneToGoWhenConfirm);
		else if(winToOpenWhenConfirm)
			UIController.Instance.Manager.open(winToOpenWhenConfirm);

		if(closeWhenConfirm)
			UIController.Instance.Manager.close(this);
	}

	public virtual void cancel(){
		if(!string.IsNullOrEmpty(sceneToGoWhenCancel))
			ScreenLoaderVisualIndicator.Instance.LoadScene(sceneToGoWhenCancel);
		else if(winToOpenWhenCancel)
			UIController.Instance.Manager.open(winToOpenWhenCancel);

		if(closeWhenCancel)
			UIController.Instance.Manager.close(this);
	}
}
