using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SingletonPersistent<T> : MonoBehaviour where T : Component
{
    public static T instance;

    /// <summary>
    /// Only accesible in the start function or later
    /// </summary>
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                Debug.LogError(
                    "Failed to get " + typeof(T).Name + " instance.\n" +
                    "Please make sure you are not trying to access the instance before start!"
                    );
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this as T)
        {
            instance = null;
        }
    }
}
