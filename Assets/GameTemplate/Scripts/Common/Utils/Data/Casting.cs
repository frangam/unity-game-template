using UnityEngine;
using System.Collections;

public class Casting : MonoBehaviour {
	public static bool TryCast<T>(object obj, out T result){
		if (obj is T){
			result = (T)obj;
			return true;
		}
		
		result = default(T);
		return false;
	}
}
