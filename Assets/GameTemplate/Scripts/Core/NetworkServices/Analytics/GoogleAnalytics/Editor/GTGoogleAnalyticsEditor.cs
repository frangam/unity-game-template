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

[CustomEditor(typeof(GTGoogleAnalyticsSettings))]
public class GTGoogleAnalyticsEditor : Editor {
	private GTGoogleAnalyticsSettings settings;


	//--------------------------------------
	// Unity
	//--------------------------------------
	public override void OnInspectorGUI() {
		#if UNITY_WEBPLAYER
		EditorGUILayout.HelpBox("Editing GTGoogleAnalytics Settings not avaliable with web player platfrom. Please swith to any other platfrom under Build Seting menu", MessageType.Warning);
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
		
		
		settings = target as GTGoogleAnalyticsSettings;
		
		GUI.changed = false;
		
		
		googleAnalyticsSettings();
		EditorGUILayout.Space();
		
		
		if(GUI.changed) {
			DirtyEditor();
		}
	}

	protected virtual void googleAnalyticsSettings(){
		GTGoogleAnalyticsSettings.Instance.showSettings = EditorGUILayout.Foldout(GTGoogleAnalyticsSettings.Instance.showSettings, "Google Analytics Settings");
		EditorGUILayout.BeginVertical();
		if (GTGoogleAnalyticsSettings.Instance.showSettings) {


//			EditorGUI.indentLevel++;
			allSettings();
//			EditorGUI.indentLevel--;


		}
		EditorGUILayout.EndVertical();
	}
	protected virtual void allSettings(){
		Color prevCol = GUI.color;

		if(GTGoogleAnalyticsSettings.Instance.idsPacks != null && GTGoogleAnalyticsSettings.Instance.idsPacks.Count == 0) {
			EditorGUILayout.HelpBox("No packs registred",MessageType.None);
		}
		
		if(GTGoogleAnalyticsSettings.Instance.idsPacks != null && GTGoogleAnalyticsSettings.Instance.idsPacks.Count > 0){
			foreach(GTGoogleAnalyticsIDsPack pack in GTGoogleAnalyticsSettings.Instance.idsPacks) {
				if(pack != null){
					if(!GTGoogleAnalyticsSettings.Instance.showPacks.ContainsKey(pack))
						GTGoogleAnalyticsSettings.Instance.showPacks.Add(pack, true);

					EditorGUILayout.BeginVertical(GUI.skin.box);
					GTGoogleAnalyticsSettings.Instance.showPacks[pack] = EditorGUILayout.Foldout(GTGoogleAnalyticsSettings.Instance.showPacks[pack], "Game Version: " + pack.gameVersion.ToString());
					if(GTGoogleAnalyticsSettings.Instance.showPacks[pack]){
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Android ID",GUILayout.Width(130));
						pack.androidTrackingID = EditorGUILayout.TextField(pack.androidTrackingID).Trim();
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("iOS ID",GUILayout.Width(130));
						pack.iOSTrackingID = EditorGUILayout.TextField(pack.iOSTrackingID).Trim();
						EditorGUILayout.EndHorizontal();
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Other ID",GUILayout.Width(130));
						pack.otherTrackingID = EditorGUILayout.TextField(pack.otherTrackingID).Trim();
						EditorGUILayout.EndHorizontal();
						
						
						EditorGUILayout.BeginHorizontal();
						GUI.color = Color.red;
						if(GUILayout.Button("Delete",  GUILayout.Width(120))) {
							GTGoogleAnalyticsSettings.Instance.idsPacks.Remove(pack);
							break;
						}
						GUI.color = prevCol;
						EditorGUILayout.EndHorizontal();
					}//end_if(GTGoogleAnalyticsSettings.Instance.showPacks[pack])
					EditorGUILayout.EndVertical();
				}//end_if(pack != null)
			}//end_foreach

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();

			GUI.color = Color.red;
			if(GUILayout.Button("Delete all",  GUILayout.Width(180))) {
				if(GTGoogleAnalyticsSettings.Instance.idsPacks != null && GTGoogleAnalyticsSettings.Instance.idsPacks.Count > 0){
					List<GTGoogleAnalyticsIDsPack> idsPacksRemove = new List<GTGoogleAnalyticsIDsPack>();
					foreach(GTGoogleAnalyticsIDsPack pack in GTGoogleAnalyticsSettings.Instance.idsPacks)
						idsPacksRemove.Add(pack);
					foreach(GTGoogleAnalyticsIDsPack levelPack in idsPacksRemove)
						GTGoogleAnalyticsSettings.Instance.idsPacks.Remove(levelPack);
				}
			}
			GUI.color = prevCol;
//			EditorGUILayout.EndHorizontal();

			
		}
//		else{
//			EditorGUILayout.BeginHorizontal();
//			EditorGUILayout.Space();
//		}
		
		
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		GUI.color = Color.green;
		if(GUILayout.Button("Add new level pack",  GUILayout.Width(180))) {
			if(GTGoogleAnalyticsSettings.Instance.idsPacks == null || (GTGoogleAnalyticsSettings.Instance.idsPacks != null && GTGoogleAnalyticsSettings.Instance.idsPacks.Count == 0))
				GTGoogleAnalyticsSettings.Instance.idsPacks.Add(new GTGoogleAnalyticsIDsPack());
			else{
				GTGoogleAnalyticsIDsPack lastPack = GTGoogleAnalyticsSettings.Instance.idsPacks[GTGoogleAnalyticsSettings.Instance.idsPacks.Count-1];
				GTGoogleAnalyticsSettings.Instance.idsPacks.Add(new GTGoogleAnalyticsIDsPack(lastPack.gameVersion+1));
			}
		}
		GUI.color = prevCol;
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
	}
	
	private static void DirtyEditor() {
		#if UNITY_EDITOR
		EditorUtility.SetDirty(GTGoogleAnalyticsSettings.Instance);
		#endif
	}
}
