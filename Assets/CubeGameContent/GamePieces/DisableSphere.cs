using CubeTypes;
using UnityEngine;

internal class DisableSphere:MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered disableShere trigger");
        CubeType cube = other.GetComponentInChildren<CubeType>();
        if(cube != null)
        {
            cube.TurnOff();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name+ " exited disableShere trigger");
        CubeType cube = other.GetComponentInChildren<CubeType>();
        if (cube != null)
        {
            cube.TurnOn();
        }
    }
}