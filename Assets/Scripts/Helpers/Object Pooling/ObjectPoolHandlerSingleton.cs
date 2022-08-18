using UnityEngine;

public abstract class ObjectPoolHandlerSingleton<T, O> : ObjectPoolHandler<O> where T : Component where O : PoolObject
{
    private static T instance;

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


    public override void Awake()
    {
        if (instance)
        {
            Debug.LogError("There is more than one " + typeof(T).Name + " singleton in the scene!");
            return;
        }
        instance = this as T;

        base.Awake();
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
