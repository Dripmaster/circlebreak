using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBridge : MonoBehaviour
{
    public static DataBridge Singleton { get { return instance; } }
    static DataBridge instance = null;

    public int currentPoint = 0;
    protected void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<DataBridge>();
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
