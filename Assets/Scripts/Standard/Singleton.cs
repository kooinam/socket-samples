using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                throw new System.Exception(string.Format("Singleton not found - {0}", typeof(T)));
            }

            return instance;
        }
    }

    public static bool Exists()
    {
        if (instance == null)
        {
            return false;
        }

        return true;
    }

    // Use this for initialization
    protected virtual void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this as T;
    }
}
