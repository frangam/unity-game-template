using UnityEngine;
using System.Collections;

/// <summary>
/// Singleton pattern for object we want to exist in all of the Scenes
/// </summary>
public abstract class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour {
	
	private static T _instance = null;
	
	
	
	public static T Instance {
		
		get {
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
	
	public static bool IsDestroyed {
		get {
			if(_instance == null) {
				return true;
			} else {
				return false;
			}
		}
	}

	public virtual void Start(){
		DontDestroyOnLoad(_instance.gameObject); //dont destroy instance to persist in every scene
	}
}
