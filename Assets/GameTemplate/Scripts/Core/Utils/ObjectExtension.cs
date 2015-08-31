/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

/// <summary>
/// Extensions for object type instances
/// </summary>
public static class ObjectExtension {
	public static string NameOf(this object o)
	{
		return o.GetType().Name;
	}
}
