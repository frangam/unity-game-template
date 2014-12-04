using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TransformExtensions {
	/// <summary>
	/// Obtiene el primer hijo del gameobject padre indicado que sea del tipo dado, evita devolver el propio objeto padre
	/// como hace el metodo GetComponetsInChildren de Unity
	/// </summary>
	/// <returns>El primer hijo que coincida con el componente buscado.</returns>
	/// <param name="go">El padre</param>
	/// <typeparam name="T">El componente que debe tener el primer hijo.</typeparam>
	public static Transform getFirstComponentInChildren<T>(this Transform t) {
		Transform res = default(Transform);
		Transform aux = default(Transform);
		
		foreach(Transform child in t){
			aux = child.GetComponent<Transform>();
			
			//si el hijo es el tipo de componente que buscamos
			if(aux != null){
				res = aux;
				break;
			}
		}
		
		return res;
	}
	
	/// <summary>
	/// Obtiene exclusivamente, sin incluir el propio padre, todos los hijos del gameobject padre indicado que sea del tipo dado
	/// </summary>
	/// <returns>todos los hijos que coincidan con el componente buscado.</returns>
	/// <param name="go">El padre</param>
	/// <typeparam name="T">El componente que deben tener los hijos.</typeparam>
	public static Transform[] getComponentsInChildren(this Transform t){
		List<Transform> list = new List<Transform> ();
		Transform[] res = default(Transform[]);
		Transform aux = default(Transform);
		
		foreach(Transform child in t){
			aux = child.GetComponent<Transform>();
			
			//si el hijo es el tipo de componente que buscamos
			if(aux != null){
				list.Add(aux);
			}
		}
		
		res = list.ToArray ();
		
		return res;
	}
}
