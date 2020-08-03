using UnityEngine;

/// <summary>
/// Generic class to handle singleton instances behaviour.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Variables
    private static bool applicationIsQuitting = false;
    private static object _lock = new object();

    private static T instance;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (instance == null)
                {
                    var instances = FindObjectsOfType<T>();

                    if (instances.Length > 1)
                    {
                        Debug.LogWarning("[Singleton] There are too many Singletons... deleting them: ");

                        for (int i = 1; i < instances.Length; i++)
                        {
                            Debug.LogWarning($"Deleting {instances[i].gameObject.name}");
                            Destroy(instances[i].gameObject);
                        }

                        instance = FindObjectOfType<T>();
                        return instance;
                    }

                    if (instances.Length > 0)
                    {
                        instance = instances[0];
                    }

                    // Create new instance if one doesn't already exist
                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();

                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        Debug.Log($"[Singleton] An instance of {typeof(T)} is needed in the scene, so '{singletonObject}' was created with DontDestroyOnLoad.");
                    }
                }

                return instance;
            }
        }
    }
    #endregion Variables

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    private void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}