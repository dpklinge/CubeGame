using CubeTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnulmentCube : CubeType
{
    
    public float radiusMultiplier = 7.5f;

    private GameObject disableSphereInstance;
    public override void BeginBehaviour(Vector3 velocity, Vector3 angularVelocity)
    {
        Debug.Log("Beginning behaviour annulment cube; Setting kinematic to false");
        Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = false;
            rigidbody.velocity = velocity;
            rigidbody.angularVelocity = angularVelocity;
            Debug.Log("Beginning annullment cube- creating DisableSphere");
            GameObject disableSphere = GameObject.Find("DisableSphere");
            disableSphereInstance = Instantiate(disableSphere, transform);
            float radius = radiusMultiplier * gameObject.transform.localScale.magnitude;
            disableSphereInstance.transform.localScale *= radiusMultiplier;


            Debug.Log("Getting colliders in vicinity");
            Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, radius);
            Debug.Log("Looping through colliders");
            foreach (Collider collider in colliders)
            {
                Debug.Log("Collider: " + collider + " " + collider.gameObject.name);
                CubeType cube = collider.gameObject.GetComponent<CubeType>();
                if (cube != null && cube.gameObject != gameObject)
                {
                    cube.TurnOff();
                }
            }
        }
        base.BeginBehaviour(velocity, angularVelocity);
        

    }

    public override void DisableBehaviour(string disableType)
    {
        Debug.Log("Disabling behaviour generic box; Setting kinematic to true");
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        float radius = radiusMultiplier * gameObject.transform.localScale.magnitude;
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, radius);
        foreach (Collider collider in colliders)
        {
            CubeType cube = collider.gameObject.GetComponent<CubeType>();
            if (cube != null && cube.gameObject != gameObject)
            {
                cube.TurnOn();
            }
        }
        GameObject.Destroy(disableSphereInstance);
        base.DisableBehaviour(disableType);

    }


  
}
