using CubeTypes;
using UnityEngine;

internal class DisableSphere:MonoBehaviour
{
    internal void Start()
    {
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        CubeType cube = other.GetComponent<CubeType>();
        if(cube != null)
        {
            cube.TurnOff();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        CubeType cube = other.GetComponent<CubeType>();
        if (cube != null)
        {
            cube.TurnOn();
        }
    }
}