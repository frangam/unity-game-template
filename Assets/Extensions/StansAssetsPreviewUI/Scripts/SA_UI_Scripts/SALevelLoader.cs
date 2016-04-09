using UnityEngine;
using System.Collections;

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2|| UNITY_5_3
#else
using UnityEngine.SceneManagement;
#endif

public class SALevelLoader : SA_Singleton<SALevelLoader> {

	private Texture2D bg;

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}


	public void LoadLevel(string name) {


				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2|| UNITY_5_3
		Application.LoadLevel(name);
		#else
		SceneManager.LoadScene(name);
		#endif


	}

	public void Restart() {

				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2|| UNITY_5_3
		Application.LoadLevel(Application.loadedLevelName);
		#else
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		#endif



	}
}
