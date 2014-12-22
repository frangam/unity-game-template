using UnityEngine;
using System.Collections;
 
/// <summary>
/// Singleton pattern for object we want to exist only in the current Scene
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	
	private static T _instance = null;
	
	
	
	public static T Instance {
		
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType(typeof(T)) as T;
				if (_instance == null) {
					_instance = new GameObject ().AddComponent<T> ();
					_instance.gameObject.name = _instance.GetType ().Name+" (Singleton)";
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
}