﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CubeTypes
{

    public class FloatCube : CubeType
    {
        private Vector3 velocity;
        private Vector3 lastVelocity = Vector3.zero;


        public override void BeginBehaviour(Vector3 velocity, Vector3 angularVelocity)
        {
            MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                Debug.Log(material.name + " has color at beginbehaviour " + material.color);
            }
            if (velocity.magnitude > 5)
            {
                velocity = velocity.normalized * 2.5f;
            }
            this.velocity = velocity;
            Debug.Log("Beginning behaviour float box; Setting kinematic to false");
            Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
            }
            if (lastVelocity != Vector3.zero)
            {
                this.velocity = lastVelocity;
            }
            //rigidbody.useGravity = false;
            //rigidbody.velocity = velocity;
            //rigidbody.drag = 0;
            //rigidbody.freezeRotation = true;
            lastVelocity = Vector3.zero;

            base.BeginBehaviour(velocity, angularVelocity);
        }



        public override void DisableBehaviour(String disableType)
        {
            Debug.Log("Disabling beahaviour generic box; Setting kinematic to true");
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            if (disableType.Equals("turnOff"))
            {
                lastVelocity = velocity;
            }
            else
            {
                lastVelocity = Vector3.zero;
            }
            velocity = Vector3.zero;
            base.DisableBehaviour(disableType);
        }
        public void Update()
        {
            transform.Translate(velocity * Time.deltaTime, Space.World);
            base.Update();

        }
        public override void ApplyCollision(GameObject obj)
        {
            Rigidbody rb = obj.GetComponentInChildren<Rigidbody>();
            if (rb != null && !rb.isKinematic)
            {
                if (!attachedObjects.Contains(obj))
                {
                    Debug.Log("Attaching " + obj.name + " to moving platform");
                    attachedObjects.Add(obj.gameObject);
                    obj.transform.parent = this.gameObject.transform;
                }
            }
            else
            {
                velocity = velocity * -1;
                Debug.Log("Collided with: " + obj.name);
            }
        }




    }





}
