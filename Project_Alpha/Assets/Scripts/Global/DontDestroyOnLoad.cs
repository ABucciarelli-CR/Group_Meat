using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{

    public static DontDestroyOnLoad instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
