using UnityEngine;
using System.Collections;

/// <summary>
/// Singleton pattern for object we want to exist in all of the Scenes
/// </summary>
public class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour {
	protected static T instance;
	
	/// <summary>
	/// Returns instance of this class
	/// </summary>
	/// <value>
	/// The instance.
	/// </value>
	public static T Instance {
		get {
			if (FindObjectsOfType(typeof(T)).Length > 1 ){
				Debug.LogError("[PersistentSingleton] Something went really wrong " +
				               " - there should never be more than 1 singleton!" +
				               " Reopenning the scene might fix it.");
			}
			else if (instance == null) {
				instance = (T)FindObjectOfType(typeof(T));
				
				if (instance == null) {
					GameObject container = new GameObject();
					container.name = typeof(T)+"(PersistentSingleton)";
					instance = (T)container.AddComponent(typeof(T));
					DontDestroyOnLoad(instance.gameObject); //dont destroy instant to persist in every scene
				}
			}
			return instance;
		}
	}
}
