using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderLayerMod : MonoBehaviour
{
    public int RenderPriority;
    public Material Material;
    // Start is called before the first frame update
    void Start()
    {
        Material.renderQueue = RenderPriority;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
