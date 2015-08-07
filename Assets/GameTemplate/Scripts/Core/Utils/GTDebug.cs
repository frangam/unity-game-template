using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class GTDebug {
	public enum DebugMessageType{
		NORMAL 	= 0,
		WARNING = 1,
		ERROR	= 2
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Log the specified message if flag show test logs in GameSettings is true
	/// </summary>
	/// <param name="message">Message.</param>
	public static void log(string message, bool includeMethodCalling = true, bool includeClassNameCalling = true){
		if(GameSettings.Instance.showTestLogs){
			StackTrace st = new StackTrace();
			string mthName = st.FrameCount > 0 && st.GetFrame(0) != null && st.GetFrame(0).GetMethod() != null ? st.GetFrame(0).GetMethod().ToString() : "";
			string className = !string.IsNullOrEmpty(mthName) && st.GetFrame(0).GetMethod().ReflectedType != null ? st.GetFrame(0).GetMethod().ReflectedType.Name : "";
			string finalMessage = getFinalMessage (message, className, mthName, includeClassNameCalling && !string.IsNullOrEmpty(className), includeMethodCalling && !string.IsNullOrEmpty(mthName)); 
			showDebugLog(finalMessage, DebugMessageType.NORMAL); //finally show debug log
		}
	}
	/// <summary>
	/// Log the specified warning message if flag show test logs in GameSettings is true
	/// </summary>
	/// <param name="message">Message.</param>
	public static void logWarning(string message, bool includeMethodCalling = true, bool includeClassNameCalling = true){
		if(GameSettings.Instance.showTestLogs){
			StackTrace st = new StackTrace();
			string mthName = st.FrameCount > 0 && st.GetFrame(0) != null && st.GetFrame(0).GetMethod() != null ? st.GetFrame(0).GetMethod().ToString() : "";
			string className = !string.IsNullOrEmpty(mthName) && st.GetFrame(0).GetMethod().ReflectedType != null ? st.GetFrame(0).GetMethod().ReflectedType.Name : "";
			string finalMessage = getFinalMessage (message, className, mthName, includeClassNameCalling && !string.IsNullOrEmpty(className), includeMethodCalling && !string.IsNullOrEmpty(mthName)); 
			showDebugLog(finalMessage, DebugMessageType.WARNING); //finally show debug log
		}
	}
	/// <summary>
	/// Log the specified error message if flag show test logs in GameSettings is true
	/// </summary>
	/// <param name="message">Message.</param>
	public static void logError(string message, bool includeMethodCalling = true, bool includeClassNameCalling = true){
		if(GameSettings.Instance.showTestLogs){
			StackTrace st = new StackTrace();
			string mthName = st.FrameCount > 0 && st.GetFrame(0) != null && st.GetFrame(0).GetMethod() != null ? st.GetFrame(0).GetMethod().ToString() : "";
			string className = !string.IsNullOrEmpty(mthName) && st.GetFrame(0).GetMethod().ReflectedType != null ? st.GetFrame(0).GetMethod().ReflectedType.Name : "";
			string finalMessage = getFinalMessage (message, className, mthName, includeClassNameCalling && !string.IsNullOrEmpty(className), includeMethodCalling && !string.IsNullOrEmpty(mthName)); 
			showDebugLog(finalMessage, DebugMessageType.ERROR); //finally show debug log
		}
	}
	
	/// <summary>
	/// Log always the specified message WITHOUT checking if flag show test logs in GameSettings is true
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="includeMethodCalling">If set to <c>true</c> include method calling.</param>
	/// <param name="includeClassNameCalling">If set to <c>true</c> include class name calling.</param>
	/// <param name="messageType">Message type.</param>
	public static void logAlways(string message, bool includeMethodCalling = true, bool includeClassNameCalling = true){
		StackTrace st = new StackTrace();
		string mthName = st.FrameCount > 0 && st.GetFrame(0) != null && st.GetFrame(0).GetMethod() != null ? st.GetFrame(0).GetMethod().ToString() : "";
		string className = !string.IsNullOrEmpty(mthName) && st.GetFrame(0).GetMethod().ReflectedType != null ? st.GetFrame(0).GetMethod().ReflectedType.Name : "";
		string finalMessage = getFinalMessage (message, className, mthName, includeClassNameCalling && !string.IsNullOrEmpty(className), includeMethodCalling && !string.IsNullOrEmpty(mthName)); 
		showDebugLog(finalMessage, DebugMessageType.NORMAL); //finally show debug log
	}
	/// <summary>
	/// Log always the specified warning message WITHOUT checking if flag show test logs in GameSettings is true
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="includeMethodCalling">If set to <c>true</c> include method calling.</param>
	/// <param name="includeClassNameCalling">If set to <c>true</c> include class name calling.</param>
	/// <param name="messageType">Message type.</param>
	public static void logWarningAlways(string message, bool includeMethodCalling = true, bool includeClassNameCalling = true){
		StackTrace st = new StackTrace();
		string mthName = st.FrameCount > 0 && st.GetFrame(0) != null && st.GetFrame(0).GetMethod() != null ? st.GetFrame(0).GetMethod().ToString() : "";
		string className = !string.IsNullOrEmpty(mthName) && st.GetFrame(0).GetMethod().ReflectedType != null ? st.GetFrame(0).GetMethod().ReflectedType.Name : "";
		string finalMessage = getFinalMessage (message, className, mthName, includeClassNameCalling && !string.IsNullOrEmpty(className), includeMethodCalling && !string.IsNullOrEmpty(mthName)); 
		showDebugLog(finalMessage, DebugMessageType.WARNING); //finally show debug log
	}
	/// <summary>
	/// Log always the specified error message WITHOUT checking if flag show test logs in GameSettings is true
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="includeMethodCalling">If set to <c>true</c> include method calling.</param>
	/// <param name="includeClassNameCalling">If set to <c>true</c> include class name calling.</param>
	/// <param name="messageType">Message type.</param>
	public static void logErrorAlways(string message, bool includeMethodCalling = true, bool includeClassNameCalling = true){
		StackTrace st = new StackTrace();
		string mthName = st.FrameCount > 0 && st.GetFrame(0) != null && st.GetFrame(0).GetMethod() != null ? st.GetFrame(0).GetMethod().ToString() : "";
		string className = !string.IsNullOrEmpty(mthName) && st.GetFrame(0).GetMethod().ReflectedType != null ? st.GetFrame(0).GetMethod().ReflectedType.Name : "";
		string finalMessage = getFinalMessage (message, className, mthName, includeClassNameCalling && !string.IsNullOrEmpty(className), includeMethodCalling && !string.IsNullOrEmpty(mthName)); 
		showDebugLog(finalMessage, DebugMessageType.ERROR); //finally show debug log
	}
	
	
	//--------------------------------------
	// Private Methods
	//--------------------------------------
	private static void showDebugLog(string message, DebugMessageType messageType = DebugMessageType.NORMAL){
		switch(messageType){
		case DebugMessageType.NORMAL: UnityEngine.Debug.Log(message); break;
		case DebugMessageType.WARNING: UnityEngine.Debug.LogWarning(message); break;
		case DebugMessageType.ERROR: UnityEngine.Debug.LogError(message); break;
		}
	}
	
	private static string getFinalMessage(string message, string classIntialValue, string methodInitialValue, bool includeClassNameCalling, bool includeMethodCalling){
		string className = getClassNameParsingResult(classIntialValue, includeClassNameCalling, includeMethodCalling);
		string methodName = getMethodNameParsingResult(methodInitialValue, includeClassNameCalling, includeMethodCalling);
		string finalMessage = className + methodName + message;
		return finalMessage;
	}
	
	private static string getClassNameParsingResult(string initialValue, bool includeClassNameCalling, bool includeMethodCalling){
		string className = includeClassNameCalling ? initialValue : "";
		if(includeClassNameCalling && !includeMethodCalling)
			className += ": ";
		return className;
	}
	private static string getMethodNameParsingResult(string initialValue, bool includeClassNameCalling, bool includeMethodCalling){
		string methodName = includeMethodCalling ? initialValue + ": " : "";
		if(!includeClassNameCalling && includeMethodCalling)
			methodName = "On " + methodName + ": ";
		else if(includeClassNameCalling && includeMethodCalling)
			methodName = " on " + methodName + ": ";
		return methodName;
	}
}