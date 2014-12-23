using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GOExtensions {
	/// <summary>
	/// Gets only the first child of gameobject parent indicated of given type. It does not return the first object that is the parent
	/// like it does Unity method "GetComponetsInChildren"
	/// </summary>
	/// <returns>The first child matches with searched component</returns>
	/// <param name="parent">The parent</param>
	/// <typeparam name="T">The component</typeparam>
	public static T getFirstComponentInChildren<T>(this Transform parent) where T: Component {
		T res = default(T);
		T aux = default(T);
		
		foreach(Transform child in parent){
			aux = child.GetComponent<T>();
			
			//si el hijo es el tipo de componente que buscamos
			if(aux != null){
				res = aux;
				break;
			}
		}
		
		return res;
	}
	/// <summary>
	/// Gets only the first child of gameobject parent indicated of given type. It does not return the first object that is the parent
	/// like it does Unity method "GetComponetsInChildren"
	/// </summary>
	/// <returns>The first child matches with searched component</returns>
	/// <param name="parent">The parent</param>
	/// <typeparam name="T">The component</typeparam>
	public static T getFirstComponentInChildren<T>(this GameObject parent) where T: Component{
		return parent.transform.getFirstComponentInChildren<T>();
	}
	
	/// <summary>
	/// Gest exclusively, without including the parent, all children of the given parent
	/// </summary>
	/// <returns>All children match with searched component</returns>
	/// <param name="parent">The parent</param>
	/// <typeparam name="T">The component</typeparam>
	public static T[] getComponentsInChildren<T>(this Transform parent, bool inDeph = true) where T: Component{
		List<T> list = new List<T> ();
		T[] res = default(T[]);
		T aux = default(T);
		
		foreach(Transform child in parent){
			aux = child.GetComponent<T>();
			
			//si el hijo es el tipo de componente que buscamos
			if(aux != null){
				list.Add(aux);
			}
		}
		
		res = list.ToArray ();
		
		return res;
	}
	/// <summary>
	/// Gest exclusively, without including the parent, all children of the given parent
	/// </summary>
	/// <returns>All children match with searched component</returns>
	/// <param name="parent">The parent</param>
	/// <typeparam name="T">The component</typeparam>
	public static T[] getComponentsInChildren<T>(this GameObject parent, bool inDeph = true) where T: Component{
		return parent.transform.getComponentsInChildren<T>(inDeph);
	}

	/// <summary>
	/// Destroies all children.
	/// </summary>
	/// <param name="parent">Parent.</param>
	public static void DestroyAllChildren(this Transform parent){
		foreach (Transform childTransform in parent.transform){
			GameObject.Destroy(childTransform.gameObject);
		}
	}

	/// <summary>
	/// Destroies all children.
	/// </summary>
	/// <param name="parent">Parent.</param>
	public static void DestroyAllChildren(this GameObject parent){
		foreach (Transform childTransform in parent.transform){
			GameObject.Destroy(childTransform.gameObject);
		}
	}

	public static void GetComponentsInChildrenRecursive<T> (this GameObject parent, List<T> buffer) where T : Component{
		parent.transform.GetComponentsInChildrenRecursive<T>(buffer);
	}

	public static void GetComponentsInChildrenRecursive<T> (this Transform parent, List<T> buffer) where T : Component{
		foreach (Transform t in parent) {
			var c = t.GetComponent<T> ();
			if (c) {
				buffer.Add (c);
			}
			GetComponentsInChildrenRecursive (t, buffer);
		}
	}
}
