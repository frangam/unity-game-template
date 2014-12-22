using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UIBaseController : UIBaseController<UIBaseController>{
	
}

public class UIBaseController<T> : Singleton<T> where T: MonoBehaviour {
	//--------------------------------------
	// Constants
	//--------------------------------------
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private List<UIBaseWindow> windows;
	
	[SerializeField]
	private CanvasGroup[] guiBlended;
	
	[SerializeField]
	private float alphaForGuiBlended = 0.75f;
	
	[SerializeField]
	private float delayForGuiBlended = 2f;
	
	[SerializeField]
	private EventSystem eventSystm;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public List<UIBaseWindow> Windows {
		get {
			return this.windows;
		}
	}
	
	public EventSystem EventSystm {
		get {
			return this.eventSystm;
		}
	}
	
	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public virtual void Awake(){
		if(eventSystm == null){
			eventSystm = FindObjectOfType<EventSystem>() as EventSystem;
			
			if(eventSystm == null){
				Debug.LogWarning("There is not any EventSystem on the scene");
			}
		}
		
		if(guiBlended != null && guiBlended.Length > 0){
			foreach(CanvasGroup c in guiBlended)
				c.alpha = 1f;
			
			StartCoroutine(setAlphaGuiBlended());
		}
	}
	public virtual void Start(){}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void close(string id){
		openWin(id, false);
	}
	
	public virtual void open(string id){
		openWin(id);
	}
	
	public virtual void openWin(string id, bool show = true){
		UIBaseWindow win = null;
		
		foreach(UIBaseWindow w in windows){
			if(w.Id == id){
				win = w;
				break;
			}
		}
		
		if(win != null){
			open(win, show);
		}
		else{
			Debug.LogError("Windows with id " + id + " does not exist");
		}
	}
	
	
	public virtual void open(UIBaseWindow window, bool show = true){
		if(windows.Contains(window)){
			window.gameObject.SetActive(show);
			
			if(show){
				window.open();
			}
			else
				window.close();
		}
	}
	
	public virtual IEnumerator setAlphaGuiBlended(bool blended = true){
		yield return new WaitForSeconds(delayForGuiBlended);
		
		float al = blended ? alphaForGuiBlended: 1f;
		foreach(CanvasGroup c in guiBlended)
			c.alpha = al;
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
}
