﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnionAssets.FLE;

public class UIBaseManager : MonoBehaviour {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string ALL_WINDOWS_LOADED_AT_SCENE_AWAKE = "gt_all_windows_loaded_at_scene_awake";
	
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	/// <summary>
	/// Observer pattern
	/// </summary>
	private EventDispatcherBase _dispatcher  = new EventDispatcherBase ();
	
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
	//	private bool allWindowsLoadWhen
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public EventDispatcherBase dispatcher {
		get {
			return _dispatcher;
		}
	}
	
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
				if(w.StartClosed)
					close (w);
				else
					open(w);
			}
			
			dispatcher.dispatch(ALL_WINDOWS_LOADED_AT_SCENE_AWAKE);
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
	
	public virtual void close(string id, bool handleStart = true){
		openWin(id, handleStart, false);
	}
	
	public virtual void close(UIBaseWindow window, bool handleStart = true){
		open(window, handleStart, false);
	}
	
	public virtual void forceClose(string id, bool handleStart = true){
		openWin(id, handleStart, false, true);
	}
	
	public virtual void forceClose(UIBaseWindow window, bool handleStart = true){
		openWin(window.Id, handleStart, false, true);
	}
	
	public virtual void open(string id, bool handleStart = true){
		openWin(id, handleStart);
	}
	
	public virtual void openWin(string id, bool handleStart = true, bool show = true, bool forceCloseWin = false){
		UIBaseWindow win = null;
		
		foreach(UIBaseWindow w in windows){
			if(w.Id == id){
				win = w;
				break;
			}
		}
		
		if(win != null){
			open(win, handleStart, show, forceCloseWin);
		}
		else{
			Debug.LogWarning("Windows with id " + id + " does not exist or is not loaded yet");
		}
	}
	
	
	public virtual void open(UIBaseWindow window, bool handleStart = true, bool show = true, bool forceCloseWin = false){
		if(window != null && (!handleStart || (handleStart && !window.NotStartWithManager))){
			//		if(windows.Contains(window)){
			//first do it visible or not
			window.gameObject.SetActive(show);
			
			if(show){
				window.open();
				
				//close windows when open the current one
				if(window.CloseWinsWhenOpen != null && window.CloseWinsWhenOpen.Count > 0){
					foreach(UIBaseWindow w in window.CloseWinsWhenOpen){
						w.gameObject.SetActive(false);
						w.close();
					}
				}
				//hide gameobjects when the current window are open
				if(window.HideObjsWhenOpen != null && window.HideObjsWhenOpen.Count > 0){
					foreach(GameObject g in window.HideObjsWhenOpen){
						g.SetActive(false);
					}
				}
			}
			else{
				
				
				if(!forceCloseWin)
					window.close();
				else
					window.forceClose();
				
				//open new window when close
				if(window.OpenNewWinsWhenClose != null && window.OpenNewWinsWhenClose.Count > 0){
					foreach(UIBaseWindow w in window.OpenNewWinsWhenClose){
						w.gameObject.SetActive(true);
						w.open();
					}
				}
				
				//show gameobjects when the current window are close
				if(window.ShowObjsWhenClose != null && window.ShowObjsWhenClose.Count > 0){
					foreach(GameObject g in window.ShowObjsWhenClose){
						g.SetActive(true);
					}
				}
			}
			
			
			
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
