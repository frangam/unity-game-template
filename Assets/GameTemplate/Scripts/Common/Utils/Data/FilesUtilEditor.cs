using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System;
using System.IO;
using System.Threading;

public class FilesUtilEditor {
	public static void CreateFolder(string path) {
		if (!folderExists (path)) {
			Directory.CreateDirectory (getFullPath (path));
		}
	}

	public static void DeleteFile(string fileName) {
		if (fileExists (fileName)) {
			File.Delete(getFullPath(fileName));

			AssetDatabase.Refresh();
		}
	}

	public static bool fileExists(string fileName) {
		if (fileName.Equals (string.Empty)) {
			return false;
		}

		return File.Exists (getFullPath (fileName));
	}

	public static bool folderExists(string path) {
		if (path.Equals (string.Empty)) {
			return false;
		}

		return Directory.Exists (getFullPath(path));
	}


	public static string getFullPath(string name) {
		if (name.Equals (string.Empty)) {
			return Application.dataPath;
		}

		if (name [0].Equals ('/')) {
			name.Remove(0, 1);
		}

		return Application.dataPath + "/" + name;
	}
}
#endif
