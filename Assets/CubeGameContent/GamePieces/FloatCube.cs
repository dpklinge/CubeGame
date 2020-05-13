using System;
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
        public List<GameObject> attachedObjects = new List<GameObject>();

        public override void BeginBehaviour(Vector3 velocity, Vector3 angularVelocity)
        {
            MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
            Material material = renderer.material;
            Debug.Log(material.name + " has color at beginbehaviour " + material.color);
            if (velocity.magnitude > 5)
            {
                velocity = velocity.normalized * 2.5f;
            }
            this.velocity = velocity;
            Debug.Log("Beginning behaviour float box; Setting kinematic to false");
            Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            //rigidbody.useGravity = false;
            //rigidbody.velocity = velocity;
            //rigidbody.drag = 0;
            //rigidbody.freezeRotation = true;
            


        }
       


        public override void DisableBehaviour()
        {
            Debug.Log("Disabling beahaviour generic box; Setting kinematic to true");
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            velocity = Vector3.zero;
        }
        public void Update()
        {
            transform.Translate(velocity * Time.deltaTime, Space.World);

        }
        public void ApplyCollision(GameObject obj)
        {
            Rigidbody rb = obj.GetComponentInChildren<Rigidbody>();
            if (rb!=null && !rb.isKinematic)
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
        public void OnTriggerEnter(Collider other)
        {
            Debug.Log("Object entered floatcube trigger: "+other.name);
            // if (other.transform.parent != null) { }
            //Debug.Log("Velocity at collision time of floatcube: "+ velocity);
            Transform parent = other.transform.parent;
            if (parent == null)
            {
                ApplyCollision(other.gameObject);
            }
            else
            {
                ApplyCollision(parent.gameObject);
            }
            

        }
        public void OnTriggerExit(Collider other)
        {
            Transform parent = other.transform.parent;
            if (parent == null)
            {
                Detach(other.gameObject);
            }
            else
            {
                Detach(parent.gameObject);
            }
        }
        public void Detach(GameObject obj)
        {
            if (attachedObjects.Contains(obj))
            {
                Debug.Log("Dettaching " + obj.name + " from moving platform");
                obj.transform.parent = null;
                attachedObjects.Remove(obj);
            }
        }

 
    }
   
    

    
    
}
