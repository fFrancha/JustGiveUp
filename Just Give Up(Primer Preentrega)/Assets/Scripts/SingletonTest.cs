using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonTest : MonoBehaviour
{
    public static SingletonTest instancia;
    void Awake()
    {
        

        if(instancia == null)
        {
            SingletonTest.instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
