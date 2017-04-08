/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Threading;

public class FilesUtil  {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const char LINES_SEPARATOR = '\n';
	public const char ATTRIBUTES_SEPARATOR = ',';
	
	public static string getContent(string filename){
		string content = "";
		
		if(!string.IsNullOrEmpty(filename)){
			//Try to load list item from file
			TextAsset text = Resources.Load(filename) as TextAsset;
			
			if(text == null){
				Debug.LogError("You must provide a correct load filename");
			}
			//load all items from text file
			else{
				content = text.text;
				
				if(string.IsNullOrEmpty(content)){
					Debug.LogError("No data found in file " + filename);
				}
			}
		}
		else
			Debug.LogError("You must provide a correct load filename");
		
		return content;
	}
	
	public static string[] getLinesFromFile(string filename, char separator = LINES_SEPARATOR){
		string[] res = null;
		string content = getContent(filename);
		
		if(!string.IsNullOrEmpty(content)){
			res = getItemsFromContent(content, separator);
		}
		
		return res;
	}
	
	public static string[] getItemsFromContent(string content, char separator = ATTRIBUTES_SEPARATOR){
		string[] res = null;
		
		if(!string.IsNullOrEmpty(content)){
			res = content.Split(separator);
		}
		
		return res;
	}

	public static void CreateFolder(string path) {
		if (!folderExists (path)) {
			Directory.CreateDirectory (getFullPath (path));
		}
	}

	public static void DeleteFile(string fileName) {
		if (fileExists (fileName)) {
			File.Delete(getFullPath(fileName));
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
