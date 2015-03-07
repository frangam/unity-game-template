using UnityEngine;
using System.Collections;

public class UIInAppShopWindow : UIBaseWindow {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private UIBaseInAppWin gralInAppWin;
	
	
	public override void Awake ()
	{
		base.Awake ();
		
		if(!gralInAppWin)
			gralInAppWin = GetComponentInParent<UIBaseInAppWin>();
		
		if(gralInAppWin == null)
			Debug.LogError("UIInAppShopWindow - not found UIBaseInAppWin");
	}
	
	public override void open ()
	{
		//GA
		GA.API.Design.NewEvent(GAEvents.INAPP_WIN_OPEN);
		
		base.open ();
		
		if(gralInAppWin ){
			gralInAppWin.showPanelLoading();
			StartCoroutine(gralInAppWin.waitAtStartForCloseIfNotLoaded(true));
		}
	}
	
	public override void close ()
	{
		if(IsOpen && gralInAppWin)
			gralInAppWin.closeLoading(true);
		
		base.close ();
	}
	
	private void closeWinWhenErrorAtInit(){
		if(gralInAppWin )
			gralInAppWin.closeWinWhenErrorAtInit(true);
		
	}
	
	
}
