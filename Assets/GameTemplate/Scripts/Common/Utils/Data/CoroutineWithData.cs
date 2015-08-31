/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

/// <summary>
/// Useful when we need to get a coroutine return value
/// Original idea from: http://answers.unity3d.com/questions/24640/how-do-i-return-a-value-from-a-coroutine.html
/// </summary>
public class CoroutineWithData {
	public Coroutine coroutine { get; private set; }
	public object result;
	private IEnumerator target;
	
	public CoroutineWithData(MonoBehaviour owner, IEnumerator target) {
		this.target = target;
		this.coroutine = owner.StartCoroutine(Run());
	}
	
	private IEnumerator Run() {
		while(target.MoveNext()) {
			result = target.Current;
			yield return result;
		}
	}
}