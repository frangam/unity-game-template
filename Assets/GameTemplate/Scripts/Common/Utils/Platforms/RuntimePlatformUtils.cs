using UnityEngine;
using System.Collections;

public static class RuntimePlatformUtils {
	private static RuntimePlatform _platform;
	
	static RuntimePlatformUtils(){
		_platform = Application.platform;
	}
	
	public static bool IsWebPlayer(){
		return Application.isWebPlayer;
	}
	
	public static bool IsEditor(){
		return Application.isEditor;
	}
	
	public static bool IsFlash(){
		return _platform == RuntimePlatform.FlashPlayer;
	}


	#region mobile
	public static bool IsMobile(){
		return IsIOS() || IsAndroid() || IsWP8() || IsBlackberry();
	}
	
	public static bool IsIOS(){
		return Application.platform == RuntimePlatform.IPhonePlayer;
	}
	
	public static bool IsAndroid(){
		return Application.platform == RuntimePlatform.Android;
	}
	
	public static bool IsWP8(){
		return Application.platform == RuntimePlatform.WP8Player;
	}
	
	public static bool IsBlackberry(){
		return Application.platform == RuntimePlatform.BB10Player;
	}
	#endregion

	
	#region console
	public static bool IsGameConsole(){
		return IsPlaystation3() || IsPlaystation4() || IsXBox360();
	}
	
	public static bool IsXBox360(){
		return _platform == RuntimePlatform.XBOX360;
	}
	
	public static bool IsPlaystation3(){
		return _platform == RuntimePlatform.PS3;
	}

	public static bool IsPlaystation4(){
		return _platform == RuntimePlatform.PS4;
	}

	public static bool IsPlaystationPSP(){
		return _platform == RuntimePlatform.PSP2;
	}

	public static bool IsPlaystationMobile(){
		return _platform == RuntimePlatform.PSMPlayer;
	}
	#endregion

	
	#region desktop
	public static bool IsDesktopPlayer(){
		return _platform == RuntimePlatform.LinuxPlayer ||
			_platform == RuntimePlatform.WindowsPlayer ||
				_platform == RuntimePlatform.OSXPlayer;
	}
	#endregion
}
