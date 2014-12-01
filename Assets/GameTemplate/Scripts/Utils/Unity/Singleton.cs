using UnityEngine;
using System.Collections;
 
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    protected static T instance;
 
    /// <summary>
    /// Returns instance of this class
    /// </summary>
    /// <value>
    /// The instance.
    /// </value>
    public static T Instance {
        get {
            if (instance == null) {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null) {
                    GameObject contenedor = new GameObject();
                    contenedor.name = typeof(T)+"Container";
                    instance = (T)contenedor.AddComponent(typeof(T));
                }
            }
            return instance;
        }
    }
}