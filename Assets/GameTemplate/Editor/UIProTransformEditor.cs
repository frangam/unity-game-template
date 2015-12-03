using UnityEngine;
using UnityEditor;
using System.Collections;

#if UNITY_3_5
[CustomEditor(typeof(UIProTransform))]
#else
[CustomEditor(typeof(UIProTransform), true)]
#endif
public class UIProTransformEditor : Editor {
	GUIContent copyT   = new GUIContent("Set PRO[?]", "Move your transform in Scene view and copy it automatically");
	GUIContent showSetNoProPrev   = new GUIContent("Save NO Pro[?]", "Only for show preview when you move your transform in scene view and you want to click on Clear Preview Button");

	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public override void OnInspectorGUI() {
		UIProTransform mTarget = (target as UIProTransform);
		GUI.changed = false;
		Color prevColor = GUI.color;

		EditorGUILayout.HelpBox("Move your transform in Scene view, press 'Set PRO' for make it for PRO version. Then, move again the transform for No pro version and press 'Save NO Pro'. Or Make it manually Setting every atrribute below.", MessageType.Info); 

//		EditorGUILayout.Space();

		GUI.color = Color.green;
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button(copyT)){
			mTarget.copyCurrentProperties();
		}
		EditorGUILayout.EndHorizontal();
		GUI.color = prevColor;

		if(mTarget.HasCopiedTransform){
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button(showSetNoProPrev)){
				mTarget.setPropertiesForClearPreview();
			}
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Left", GUILayout.Width(80));
		EditorGUILayout.LabelField("Top", GUILayout.Width(80));
//		EditorGUILayout.LabelField("Pos Z", GUILayout.Width(80));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		mTarget.setLeft(EditorGUILayout.FloatField(mTarget.Left, GUILayout.Width(80)));
		mTarget.setTop(EditorGUILayout.FloatField(mTarget.Top, GUILayout.Width(80)));
//		mTarget.setPosZ(EditorGUILayout.FloatField(mTarget.ProPos.z, GUILayout.Width(80)));
		EditorGUILayout.EndHorizontal();


		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Right", GUILayout.Width(80));
		EditorGUILayout.LabelField("Bottom", GUILayout.Width(80));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		mTarget.setRight(EditorGUILayout.FloatField(mTarget.Right, GUILayout.Width(80)));
		mTarget.setBottom(EditorGUILayout.FloatField(mTarget.Bottom, GUILayout.Width(80)));
		EditorGUILayout.EndHorizontal();

		if(mTarget.HasCopiedTransform){
			EditorGUILayout.Space();

//			if(mTarget.NewValue){
				EditorGUILayout.BeginHorizontal();
				if(!mTarget.ShownPreview){
					GUI.color = Color.cyan;
					if(GUILayout.Button("Sow Preview")){
						mTarget.showPreview();
					}
					GUI.color = prevColor;
				}
				else {
					GUI.color = Color.red;
					if(GUILayout.Button("Clear Preview"))
						mTarget.clearPreview();
					GUI.color = prevColor;
				}
				EditorGUILayout.EndHorizontal();
//			}
		}


		if(GUI.changed) {
			#if UNITY_EDITOR
			EditorUtility.SetDirty(mTarget);
			#endif
		}
	}


}
