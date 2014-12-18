using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AnimationUtils {
	/// <summary>
	/// Returns the list of animation clip names of the animation given
	/// </summary>
	/// <returns>The clip names.</returns>
	/// <param name="anim">Animation.</param>
	public static List<string> animationClipNames(Animation anim){
		List<string> animationStatesList = new List<string>();
		
		foreach (AnimationState clip in anim)
			animationStatesList.Add(clip.name);
		
		return animationStatesList;
	}

	/// <summary>
	/// Returns the list of animation clip names that are currently playing of the animation given
	/// </summary>
	/// <returns>The animation clip names.</returns>
	/// <param name="anim">Animation.</param>
	public static List<string> playingAnimationClipNames(Animation anim){
		List<string> clipNames = AnimationUtils.animationClipNames(anim);
		
		for (int index=clipNames.Count - 1; index >= 0; index--){
			string clipName = clipNames[index];
			
			// Remove anything that isn't playing
			if (!anim.IsPlaying(clipName) ||
			    clipName.EndsWith(" - Queued Clone")){
				clipNames.RemoveAt(index);
			}
		}
		
		return clipNames;
	}
}
