using CubeTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideCube : CubeType
{
    public override void BeginBehaviour(Vector3 velocity, Vector3 angularVelocity)
    {
        Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.isKinematic = false;
            rigidbody.velocity = velocity;
        }
        BoxCollider collider = this.gameObject.GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.material.dynamicFriction = 0;
            collider.material.staticFriction = 0;
            collider.material.bounciness = 0;
            collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
            collider.material.bounceCombine = PhysicMaterialCombine.Minimum;
        }
        
        //rigidbody.angularVelocity = angularVelocity;
        base.BeginBehaviour(velocity, angularVelocity);
    }

    public override void DisableBehaviour(String disableType)
    {
        Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        base.DisableBehaviour(disableType);
    }

}
