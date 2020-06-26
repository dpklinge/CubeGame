﻿using CubeTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnulmentCube : CubeType
{
    public float radiusMultiplier = 5;
    public override void BeginBehaviour(Vector3 velocity, Vector3 angularVelocity)
    {
        Debug.Log("Beginning behaviour annulment cube; Setting kinematic to false");
        Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.velocity = velocity;
        rigidbody.angularVelocity = angularVelocity;
        Debug.Log("Beginning annullment cube- creating DisableSphere");
        DisableSphere disabler = gameObject.AddComponent<DisableSphere>();
        disabler.Start();
        Debug.Log("Disable sphere started, changing size");
        float radius = radiusMultiplier * gameObject.transform.localScale.magnitude;
        disabler.GetComponent<SphereCollider>().radius = radius;
        Debug.Log("Getting colliders in vicinity");
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, radius);
        Debug.Log("Looping through colliders");
        foreach (Collider collider in colliders)
        {
            Debug.Log("Collider: " + collider + " " + collider.gameObject.name);
            CubeType cube = collider.gameObject.GetComponent<CubeType>();
            if(cube != null && cube.gameObject != gameObject)
            {
                cube.TurnOff();
            }
        }
        

    }

    public override void DisableBehaviour()
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
        GameObject.Destroy(gameObject.GetComponent<DisableSphere>());
    }


  
}