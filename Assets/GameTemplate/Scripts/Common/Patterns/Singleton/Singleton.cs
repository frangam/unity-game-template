﻿/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
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
					_instance.gameObject.name = _instance.GetType ().Name;
				}
			}
			
			return _instance;
			
		}
		
	}
	
	public static bool HasInstance {
		get {
			return !IsDestroyed;
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
	
	
	
//	/// <summary>
//	/// When Unity quits, it destroys objects in a random order.
//	/// In principle, a Singleton is only destroyed when application quits.
//	/// If any script calls Instance after it have been destroyed, 
//	///   it will create a buggy ghost object that will stay on the Editor scene
//	///   even after stopping playing the Application. Really bad!
//	/// So, this was made to be sure we're not creating that buggy ghost object.
//	/// </summary>
//	protected virtual void OnDestroy () {
//		_instance = null;
//		applicationIsQuitting = true;
//		Debug.Log(typeof(T) + " [Mog.Singleton] instance destroyed with the OnDestroy event");
//	}
	
//	protected virtual void OnApplicationQuit () {
//		Debug.Log("OnApplicationQuit");
//		_instance = null;
//		applicationIsQuitting = true;
//		Debug.Log(typeof(T) + " [Mog.Singleton] instance destroyed with the OnApplicationQuit event");
//	}
}