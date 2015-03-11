using UnityEngine;
using UnityEditor;
using System.Collections;

public class GameTemplateMenu : EditorWindow {
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	#if UNITY_EDITOR
	
	[MenuItem("Window/Game Template/Game Settings", false, 1)]
	public static void Edit() {
		Selection.activeObject = GameSettings.Instance;
	}
	[MenuItem("Window/Game Template/Level Packs Settings", false, 1)]
	public static void EditLevelPacks() {
		Selection.activeObject = LevelPacks.Instance;
	}
	
	//--------------------------------------
	//  Documentation
	//--------------------------------------
	
	[MenuItem("Window/Game Template/Pukepixel GDrive", false)]
	public static void PukepixelGDrive() {
		string url = "https://drive.google.com/drive/u/0/#folders/0B9SJ925Use_uT2hBUzhBcnBIbUE";
		Application.OpenURL(url);
	}
	
	[MenuItem("Window/Game Template/Documentation/All Docs", false)]
	public static void AllDocs() {
		string url = "https://drive.google.com/drive/u/0/#folders/0B9SJ925Use_uT2hBUzhBcnBIbUE/0B9SJ925Use_uVlJQel93WHRzYU0/0B9SJ925Use_uSjlXZFU2d2ZfODQ";
		Application.OpenURL(url);
	}
	
	//--------------------------------------
	//  Achievemente Documentation
	//--------------------------------------
	
	[MenuItem("Window/Game Template/Documentation/Achievements/Getting Started")]
	public static void AchievementSystemGettingStartedDoc() {
		string url = "https://docs.google.com/document/d/17kOcOnMRrLfwEKrG9xs-shzGYEzBCSOTwJe8akRC0vo/edit#heading=h.ujroj168a0eg";
		Application.OpenURL(url);
	}
	
	[MenuItem("Window/Game Template/Documentation/Achievements/Workflow")]
	public static void AchievementSystemWorkflowDoc() {
		string url = "https://docs.google.com/document/d/17kOcOnMRrLfwEKrG9xs-shzGYEzBCSOTwJe8akRC0vo/edit#heading=h.4rrfds5d75hd";
		Application.OpenURL(url);
	}
	
	[MenuItem("Window/Game Template/Documentation/Achievements/Setup")]
	public static void AchievementSystemSetupDoc() {
		string url = "https://docs.google.com/document/d/17kOcOnMRrLfwEKrG9xs-shzGYEzBCSOTwJe8akRC0vo/edit#heading=h.wynqvbtoesnn";
		Application.OpenURL(url);
	}
	
	
	
	#endif
	
}
