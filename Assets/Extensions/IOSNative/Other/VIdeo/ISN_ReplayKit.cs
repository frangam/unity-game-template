//#define REPLAY_KIT

using UnityEngine;
using System;
using System.Collections;
#if (UNITY_IPHONE && !UNITY_EDITOR && REPLAY_KIT) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class ISN_ReplayKit : ISN_Singleton<ISN_ReplayKit> {

	#if (UNITY_IPHONE && !UNITY_EDITOR && REPLAY_KIT) || SA_DEBUG_MODE
	
	[DllImport ("__Internal")]
	private static extern void _ISN_StartRecording(bool microphoneEnabled);
	
	
	[DllImport ("__Internal")]
	private static extern void _ISN_StopRecording();


	[DllImport ("__Internal")]
	private static extern void _ISN_ShowVideoShareDialog(int ipadViewMode);

	[DllImport ("__Internal")]
	private static extern bool ISN_IsReplayKitAvaliable();


	
	#endif

	private bool _IsRecording = false;

	public static event Action<ISN_Result> ActionRecordStarted =  delegate {};
	public static event Action<ISN_Result> ActionRecordStoped =  delegate {};

	public static event Action<ReplayKitVideoShareResult> ActionShareDialogFinished =  delegate {};

	public static event Action<ISN_Error> ActionRecordInterrupted =  delegate {};
	


	//--------------------------------------
	// Public Methods
	//--------------------------------------
	void Awake(){
		DontDestroyOnLoad (gameObject);
	}

	public void StartRecording(bool microphoneEnabled = true) {
		#if (UNITY_IPHONE && !UNITY_EDITOR && REPLAY_KIT) || SA_DEBUG_MODE
		_ISN_StartRecording(microphoneEnabled);
		#endif
	}
	
	public void StopRecording() {
		#if (UNITY_IPHONE && !UNITY_EDITOR && REPLAY_KIT) || SA_DEBUG_MODE
		_ISN_StopRecording();
		#endif
	}

	public void ShowVideoShareDialog() {
		#if (UNITY_IPHONE && !UNITY_EDITOR && REPLAY_KIT) || SA_DEBUG_MODE
		_ISN_ShowVideoShareDialog(IOSNativeSettings.Instance.RPK_iPadViewType);
		#endif

	}


	//--------------------------------------
	// Get / Set
	//--------------------------------------

	public bool IsRecording {
		get {
			return _IsRecording;
		}
	}

	public bool IsAvailable {
		get {
			#if (UNITY_IPHONE && !UNITY_EDITOR && REPLAY_KIT) || SA_DEBUG_MODE
			return ISN_IsReplayKitAvaliable();
			#else
			return false;
			#endif


		}
	}
	

	//--------------------------------------
	// Objective-C Delegates
	//--------------------------------------

	private void OnRecorStartSuccess(string data) {

		_IsRecording = true;

		ISN_Result result =  new ISN_Result(true);
		ActionRecordStarted(result);
	}

	private void OnRecorStartFailed(string errorData) {
		ISN_Result result =  new ISN_Result(errorData);
		ActionRecordStarted(result);
	}


	private void OnRecorStopFailed(string errorData) {
		ISN_Result result =  new ISN_Result(errorData);
		ActionRecordStoped(result);
	}

	private void OnRecorStopSuccess() {
		ISN_Result result =  new ISN_Result(true);
		ActionRecordStoped(result);
	}


	private void OnRecordInterrupted(string errorData) {
		_IsRecording = false;

		ISN_Error e =  new ISN_Error(errorData);
		ActionRecordInterrupted(e);
	}


	private void OnSaveResult(string sourcesData) {

		_IsRecording = false;
		string[] sources = IOSNative.ParseArray(sourcesData);

		ReplayKitVideoShareResult result = new ReplayKitVideoShareResult(sources);
		ActionShareDialogFinished(result);
	}



}
