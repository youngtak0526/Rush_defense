using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager instance;
    void Start()
    {
        
        if (instance != null )
        {
            Destroy(this.gameObject);
            
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        
    }
}
