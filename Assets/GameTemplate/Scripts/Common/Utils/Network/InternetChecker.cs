using UnityEngine;
using System.Collections;
using UnionAssets.FLE;

public class InternetChecker : PersistentSingleton<InternetChecker> {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string NO_INTERNET_CONNECTION = "gt_no_internet_connection";
	public const string RESUMED_INTERNET_CONNECTION = "gt_resumed_internet_connection";
	
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	private static EventDispatcherBase _dispatcher  = new EventDispatcherBase ();
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private bool checkFromURL = false;
	
	[SerializeField]
	private float CheckTimer = 5f;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private bool isconnectedToInternet = false;
	private bool firstInternetConnectionLost = false;
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public static EventDispatcherBase dispatcher {
		get {
			return _dispatcher;
		}
	}
	public bool IsconnectedToInternet {
		get {
			return (Application.internetReachability != NetworkReachability.NotReachable);
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	protected override void Awake ()
	{
		base.Awake ();
		
		if(checkFromURL)
			InvokeRepeating("PingService",0,CheckTimer);
	}
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private void PingService(){
		StartCoroutine(PingGOOGLE());
	}
	
	private IEnumerator PingGOOGLE(){
		WWW www = new WWW("www.google.com");
		
		yield return www;
		
		if(www.error == "" || www.error == null){
			isconnectedToInternet = true;
			
			//if it was lost previously the connection we notify it has been resumed
			if(firstInternetConnectionLost){
				firstInternetConnectionLost = false;
				dispatcher.dispatch(RESUMED_INTERNET_CONNECTION);
			}
			
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("Internet is available!");
		}
		else{
			isconnectedToInternet = false;
			if(!firstInternetConnectionLost)
				firstInternetConnectionLost = true;
			
			if(GameSettings.Instance.showTestLogs)
				Debug.Log("No Internet");
			dispatcher.dispatch(NO_INTERNET_CONNECTION);
		}
	}
}
