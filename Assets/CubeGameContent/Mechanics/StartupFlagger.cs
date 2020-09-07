using CubeTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupFlagger : MonoBehaviour
{
    void Start()
    {
        CubeType.levelReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
