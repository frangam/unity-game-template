////////////////////////////////////////////////////////////////////////////////
//  
// @module V2D
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;

public class GoogleMobileAdMenu : EditorWindow {
	


	#if UNITY_EDITOR

	//--------------------------------------
	//  GENERAL
	//--------------------------------------

	[MenuItem("Window/GoogleMobileAd/Edit Settings")]
	public static void Edit() {
		Selection.activeObject = GoogleMobileAdSettings.Instance;
	}


	[MenuItem("Window/GoogleMobileAd/Documentation/Setup/Plugin setup")]
	public static void s1() {
		Application.OpenURL("http://goo.gl/aLCzeD");
	}


	[MenuItem("Window/GoogleMobileAd/Documentation/Setup/Update Guide")]
	public static void s2() {
		Application.OpenURL("http://goo.gl/Tx5NyB");
	}


	[MenuItem("Window/GoogleMobileAd/Documentation/Setup/Manifest Requirements")]
	public static void s3() {
		Application.OpenURL("http://goo.gl/nqP5MU");
	}


	[MenuItem("Window/GoogleMobileAd/Documentation/Getting Started/Before you begin")]
	public static void s4() {
		Application.OpenURL("http://goo.gl/1e4aU2");
	}


	[MenuItem("Window/GoogleMobileAd/Documentation/Getting Started/Setup for IOS")]
	public static void s5() {
		Application.OpenURL("http://goo.gl/iW3fAH");
	}


	[MenuItem("Window/GoogleMobileAd/Documentation/Getting Started/Setup for Android")]
	public static void s6() {
		Application.OpenURL("http://goo.gl/mo57Zc");
	}


	[MenuItem("Window/GoogleMobileAd/Documentation/Getting Started/Setup for WP8")]
	public static void s7() {
		Application.OpenURL("http://goo.gl/E6AhOM");
	}


	[MenuItem("Window/GoogleMobileAd/Documentation/Implementation/Initialization")]
	public static void s8() {
		Application.OpenURL("http://goo.gl/krTk5z");
	}


	[MenuItem("Window/GoogleMobileAd/Documentation/Implementation/Banners")]
	public static void s9() {
		Application.OpenURL("http://goo.gl/BXh5y2");
	}


	[MenuItem("Window/GoogleMobileAd/Documentation/Implementation/Interstitial")]
	public static void s10() {
		Application.OpenURL("http://goo.gl/PxUkms");
	}


	[MenuItem("Window/GoogleMobileAd/Documentation/Implementation/In App Purchase Listener")]
	public static void s11() {
		Application.OpenURL("http://goo.gl/7HmcBX");
	}

	[MenuItem("Window/GoogleMobileAd/Documentation/Implementation/Prefab Solution")]
	public static void s21() {
		Application.OpenURL("http://goo.gl/jLyLwX");
	}


	[MenuItem("Window/GoogleMobileAd/Documentation/Implementation/Video Tutorials")]
	public static void s13() {
		Application.OpenURL("http://goo.gl/0NNf3q");
	}

	[MenuItem("Window/GoogleMobileAd/Documentation/More/Released Apps with the plugin")]
	public static void s14() {
		Application.OpenURL("http://goo.gl/LmGKqu");
	}

	[MenuItem("Window/GoogleMobileAd/Documentation/More/Playmaker Support")]
	public static void s15() {
		Application.OpenURL("http://goo.gl/WW0I8X");
	}

	[MenuItem("Window/GoogleMobileAd/Documentation/More/Frequently Asked Questions")]
	public static void s16() {
		Application.OpenURL("http://goo.gl/sDrbMj");
	}
	
	[MenuItem("Window/GoogleMobileAd/Documentation/More/Using Plugins with Java Script")]
	public static void s17() {
		Application.OpenURL("http://goo.gl/NYK8S7");
	}
	
	#endif

}
