using CubeTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideCube : CubeType
{
    public override void BeginBehaviour(Vector3 velocity, Vector3 angularVelocity)
    {
        Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        BoxCollider collider = this.gameObject.GetComponent<BoxCollider>();
        collider.material.dynamicFriction = 0;
        collider.material.staticFriction = 0;
        collider.material.bounciness = 0;
        collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        collider.material.bounceCombine = PhysicMaterialCombine.Minimum;
        rigidbody.velocity = velocity;
        //rigidbody.angularVelocity = angularVelocity;
    }

    public override void DisableBehaviour()
    {
        Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }
   
}
