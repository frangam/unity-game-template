using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Transforms {
	/// <summary>
	/// Obtiene el primer hijo del gameobject padre indicado que sea del tipo dado, evita devolver el propio objeto padre
	/// como hace el metodo GetComponetsInChildren de Unity
	/// </summary>
	/// <returns>El primer hijo que coincida con el componente buscado.</returns>
	/// <param name="go">El padre</param>
	/// <typeparam name="T">El componente que debe tener el primer hijo.</typeparam>
	public static T getFirstComponentInChildren<T>(GameObject go) where T: Component{
		T res = default(T);
		T aux = default(T);
		
		foreach(Transform hijo in go.transform){
			aux = hijo.GetComponent<T>();
			
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
	public static T[] getComponentsInChildren<T>(GameObject go) where T: Component{
		List<T> list = new List<T> ();
		T[] res = default(T[]);
		T aux = default(T);
		
		foreach(Transform hijo in go.transform){
			aux = hijo.GetComponent<T>();
			
			//si el hijo es el tipo de componente que buscamos
			if(aux != null){
				list.Add(aux);
			}
		}
		
		res = list.ToArray ();
		
		return res;
	}
	public static T[] getComponentsInChildren<T>(Transform t) where T: Component{		
		return getComponentsInChildren<T>(t.gameObject);
	}
}
