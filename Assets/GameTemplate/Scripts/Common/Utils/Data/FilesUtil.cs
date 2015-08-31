/// <summary>
/// Useful when we need to get a coroutine return value
/// Original idea from: http://answers.unity3d.com/questions/24640/how-do-i-return-a-value-from-a-coroutine.html
/// </summary>
using UnityEngine;
using System.Collections;

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
}
