using UnityEngine;
using System.Collections;
 
/// <summary>
/// Singleton pattern for object we want to exist only in the current Scene
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	
	private static T _instance = null;
	private bool applicationIsQuitting = false;
	
	
	public static T instance {
		
		get {
			if(applicationIsQuitting) {
				Debug.Log(typeof(T) + " [Mog.Singleton] is already destroyed. Returning null. Please check HasInstance first before accessing instance in destructor.");
				return null;
			}
			
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType(typeof(T)) as T;
				if (_instance == null) {
					_instance = new GameObject ().AddComponent<T> ();
					_instance.gameObject.name = _instance.GetType ().Name+" (Singlenton)";
				}
			}
			
			return _instance;
			
		}
		
	}

	protected virtual void OnApplicationQuit () {
		Debug.Log("OnApplicationQuit");
		_instance = null;
		applicationIsQuitting = true;
		Debug.Log(typeof(T) + " [Mog.Singleton] instance destroyed with the OnApplicationQuit event");
	}
}