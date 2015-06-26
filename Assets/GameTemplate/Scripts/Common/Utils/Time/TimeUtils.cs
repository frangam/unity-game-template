using UnityEngine;
using System.Collections;

public class TimeUtils{
	/// <summary>
	/// Waits for real seconds ignores Time.scale
	/// </summary>
	/// <returns>The for real seconds.</returns>
	/// <param name="delay">Delay.</param>
	public static IEnumerator WaitForRealSeconds( float delay )
	{
		float start = Time.realtimeSinceStartup;
		while( Time.realtimeSinceStartup < start + delay)
		{
			yield return null;
		}
	}
}
