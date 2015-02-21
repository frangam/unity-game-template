using UnityEngine;
using System.Collections;

/// <summary>
/// Singleton pattern for object we want to exist in all of the Scenes
/// </summary>
public abstract class PersistentSingleton<T> : Singleton<T> where T : MonoBehaviour {
	protected virtual void Awake(){
		DontDestroyOnLoad(Instance.gameObject); //dont destroy instance to persist in every scene
	}
}
