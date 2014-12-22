using UnityEngine;
using System.Collections;

/// <summary>
/// Singleton pattern for object we want to exist in all of the Scenes
/// </summary>
public abstract class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour {
	
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
					_instance.gameObject.name = _instance.GetType ().Name+" (PersistentSingleton)";
				}
			}
			
			return _instance;
			
		}
		
	}

	public virtual void Start(){
		DontDestroyOnLoad(_instance.gameObject); //dont destroy instance to persist in every scene
	}
}
