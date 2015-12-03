/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(LevelPacks))]
public class LevelPacksEditor : Editor {
	private LevelPacks settings;

	//--------------------------------------
	// Unity
	//--------------------------------------
	public override void OnInspectorGUI() {
		#if UNITY_WEBPLAYER
		EditorGUILayout.HelpBox("Editing Game Settings not avaliable with web player platfrom. Please swith to any other platfrom under Build Seting menu", MessageType.Warning);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		if(GUILayout.Button("Switch To Android Platfrom",  GUILayout.Width(180))) {
			EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
		}
		if(GUILayout.Button("Switch To iOS Platfrom",  GUILayout.Width(180))) {
			EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.iPhone);
		}
		if(GUILayout.Button("Switch To WP8 Platfrom",  GUILayout.Width(180))) {
			EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WP8Player);
		}
		EditorGUILayout.EndHorizontal();
		
		if(Application.isEditor) {
			return;
		}
		
		
		#endif
		
		
		settings = target as LevelPacks;
		
		GUI.changed = false;
		
	
		levelsSettings();
		EditorGUILayout.Space();

		
		if(GUI.changed) {
			DirtyEditor();
		}
	}

	protected virtual void levelsSettings(){
		LevelPacks.Instance.showLevelsSettings = EditorGUILayout.Foldout(LevelPacks.Instance.showLevelsSettings, "Levels Settings");
		if (LevelPacks.Instance.showLevelsSettings) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Defaul level pack size");
			LevelPacks.Instance.DEFAULT_LEVEL_PACKS_SIZE = EditorGUILayout.IntField(LevelPacks.Instance.DEFAULT_LEVEL_PACKS_SIZE);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			
			EditorGUI.indentLevel++;
			levelPacksSettings();
			EditorGUI.indentLevel--;
		}
	}
	protected virtual void levelPacksSettings(){
		if(LevelPacks.Instance.packs != null && LevelPacks.Instance.packs.Count == 0) {
			EditorGUILayout.HelpBox("No level packs registred",MessageType.None);
		}

		if(LevelPacks.Instance.packs != null && LevelPacks.Instance.packs.Count > 0){
			foreach(BaseLevelPack levelPack in LevelPacks.Instance.packs) {
				if(levelPack != null){
					if(!LevelPacks.Instance.showLevelPacks.ContainsKey(levelPack))
						LevelPacks.Instance.showLevelPacks.Add(levelPack, true);

					LevelPacks.Instance.showLevelPacks[levelPack] = EditorGUILayout.Foldout(LevelPacks.Instance.showLevelPacks[levelPack], "Pack " + levelPack.Id);
					if(LevelPacks.Instance.showLevelPacks[levelPack]){

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Id");
						levelPack.Id = EditorGUILayout.TextField(levelPack.Id).Trim();
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Initial Level");
						levelPack.InitialLevel = EditorGUILayout.IntField(levelPack.InitialLevel);
						EditorGUILayout.EndHorizontal();

						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Total levels");
						levelPack.TotalLevelsInPack = EditorGUILayout.IntField(levelPack.TotalLevelsInPack);
						EditorGUILayout.EndHorizontal();


						EditorGUILayout.BeginHorizontal();
						if(GUILayout.Button("Delete Pack " + levelPack.Id,  GUILayout.Width(120))) {
							LevelPacks.Instance.packs.Remove(levelPack);
							break;
						}
						EditorGUILayout.EndHorizontal();
					}//end_if(levelPack != null)
				}//end_foreach
			}//end_if

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("Delete all packs",  GUILayout.Width(180))) {
				if(LevelPacks.Instance.packs != null && LevelPacks.Instance.packs.Count > 0){
					List<BaseLevelPack> packsRemove = new List<BaseLevelPack>();
					foreach(BaseLevelPack levelPack in LevelPacks.Instance.packs)
						packsRemove.Add(levelPack);
					foreach(BaseLevelPack levelPack in packsRemove)
						LevelPacks.Instance.packs.Remove(levelPack);
				}
			}

		}
		else{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
		}

		
		
//		EditorGUILayout.BeginHorizontal();
//		EditorGUILayout.Space();

		if(GUILayout.Button("Add new level pack",  GUILayout.Width(180))) {
			if(LevelPacks.Instance.packs == null || (LevelPacks.Instance.packs != null && LevelPacks.Instance.packs.Count == 0))
				LevelPacks.Instance.packs.Add(new BaseLevelPack("1", 1, LevelPacks.Instance.DEFAULT_LEVEL_PACKS_SIZE, null));
			else{
				BaseLevelPack lastPack = LevelPacks.Instance.packs[LevelPacks.Instance.packs.Count-1];
				int lastPackId;

				if(int.TryParse(lastPack.Id, out lastPackId))
					LevelPacks.Instance.packs.Add(new BaseLevelPack((lastPackId+1).ToString(), lastPack.FinalLevel+1, LevelPacks.Instance.DEFAULT_LEVEL_PACKS_SIZE, null));
			}
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
	}

	private static void DirtyEditor() {
		#if UNITY_EDITOR
		EditorUtility.SetDirty(LevelPacks.Instance);
		#endif
	}
}
