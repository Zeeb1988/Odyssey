using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<Type> : MonoBehaviour where Type : Singleton<Type>
{
    private static Type instance;

    public static Type Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (Type)this;
    }

    public static bool IsInitialized
    {
        get { return instance != null; }    
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        { 
          instance = null;
        }
    }










}
