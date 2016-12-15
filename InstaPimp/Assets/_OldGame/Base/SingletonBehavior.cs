using UnityEngine;
using System.Collections;

public class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogErrorFormat("Trying to access {0} after being destroyed!", typeof(T));
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        instance = null;
    }
}
