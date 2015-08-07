using UnityEngine;
using System.Collections;

public static class ObjectExtension {
	public static string NameOf(this object o)
	{
		return o.GetType().Name;
	}
}
