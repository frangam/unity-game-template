using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UIBaseManager : MonoBehaviour {
	//--------------------------------------
	// Constants
	//--------------------------------------
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool locateAllWindows = true;
	
	[SerializeField]
	[Tooltip("If locateAllWindows is true does not must fill this because automatically it will be filled. Be care with started closed windows.")]
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
		//first load windows
		loadWindows();
		
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
	public virtual void OnDestroy(){}
	public virtual void Update(){}
	#endregion
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private IEnumerator waitForCloseWin(UIBaseWindow win, float wait, bool willClose = true){
		yield return new WaitForSeconds(wait);
		
		if(willClose)
			close(win);
		else
			open(win);
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void loadWindows(){
		if(locateAllWindows){
			UIBaseWindow[] windowsArr = FindObjectsOfType<UIBaseWindow>() as UIBaseWindow[];
			windows = new List<UIBaseWindow>();
			
			foreach(UIBaseWindow w in windowsArr){
				if(!windows.Contains(w))
					windows.Add(w);
			}
		}
		
		if(windows != null && windows.Count > 0){
			foreach(UIBaseWindow w in windows){
				open(w, !w.StartClosed);
			}
		}
	}
	
	public virtual UIBaseWindow getWindow(string id){
		UIBaseWindow win = null;
		
		foreach(UIBaseWindow w in windows){
			if(w.Id == id){
				win = w;
				break;
			}
		}
		
		return win;
	}
	
	public virtual void waitForOpen(string id, float wait){
		UIBaseWindow win = getWindow(id);
		
		if(win != null)
			StartCoroutine(waitForCloseWin(win, wait, false));
	}
	
	public virtual void waitForOpen(UIBaseWindow win, float wait){
		if(windows.Contains(win)){
			StartCoroutine(waitForCloseWin(win, wait, false));
		}
	}
	
	public virtual void waitForClose(string id, float wait){
		UIBaseWindow win = getWindow(id);
		
		if(win != null)
			StartCoroutine(waitForCloseWin(win, wait));
	}
	
	public virtual void waitForClose(UIBaseWindow win, float wait){
		if(windows.Contains(win)){
			StartCoroutine(waitForCloseWin(win, wait));
		}
	}
	
	public virtual void close(string id){
		openWin(id, false);
	}
	
	public virtual void close(UIBaseWindow window){
		open(window, false);
	}
	
	public virtual void forceClose(string id){
		openWin(id, false, true);
	}
	
	public virtual void forceClose(UIBaseWindow window){
		openWin(window.Id, false, true);
	}
	
	public virtual void open(string id){
		openWin(id);
	}
	
	public virtual void openWin(string id, bool show = true, bool forceCloseWin = false){
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
	
	
	public virtual void open(UIBaseWindow window, bool show = true, bool forceCloseWin = false){
		if(windows.Contains(window)){			
			if(show)
				window.open();
			else{
				if(!forceCloseWin)
					window.close();
				else
					window.forceClose();
				
				//open new window when close
				if(window.OpenNewWinWhenClose != null){
					window.OpenNewWinWhenClose.open();
					window.OpenNewWinWhenClose.gameObject.SetActive(true);
				}
			}
			
			
			//finally do it visible or not
			window.gameObject.SetActive(show);
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
