using System.Collections.Generic;
using UnityEngine;
namespace CubeTypes
{
    public abstract class CubeType : MonoBehaviour
    {
        public Color CubeColor;
        public List<GameObject> attachedObjects = new List<GameObject>();
        private Vector3 velocityBeforeTurnOff;
        private Vector3 angularVelocityBeforeTurnOff;
        public abstract void BeginBehaviour(Vector3 velocity, Vector3 angularVelocity);
        public abstract void DisableBehaviour();
        public Color GetLiveColor()
        {
            return GameObject.Find(this.GetType().Name).GetComponent<CubeType>().CubeColor;
        }
        public virtual void Initialize() {
            CubeColor = GameObject.Find(this.GetType().Name).GetComponent<CubeType>().CubeColor;
            Debug.Log("Initializing with color " + CubeColor);
            MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
            if(renderer != null) { 
                Material material = renderer.material;
                renderer.material.SetColor("_EmissionColor", CubeColor);

                Light light = this.gameObject.GetComponentInChildren<Light>();
                light.color = CubeColor;
            }
        }
        public void Start()
        {
            Initialize();
        }

        public virtual void ApplyCollision(GameObject obj)
        {
            Debug.Log("Applying collision between " + this + " and " + obj);
            Rigidbody rb = obj.GetComponentInChildren<Rigidbody>();
            if (rb != null && !rb.isKinematic)
            {
                if (!attachedObjects.Contains(obj))
                {
                    Debug.Log("Attaching " + obj.name + " to "+this.name);
                    attachedObjects.Add(obj.gameObject);
                    obj.transform.parent = this.gameObject.transform;
                }
            }
            
        }

        public void OnTriggerEnter(Collider other)
        {
            Debug.Log("Object entered cube trigger: " + other.name);
            if(other.gameObject.layer == 9)
            {
                Debug.Log("Colliding with teleportIgnore surface - aborting contact");
                return;
            }
            // if (other.transform.parent != null) { }
            //Debug.Log("Velocity at collision time of floatcube: "+ velocity);
            
           ApplyCollision(other.gameObject);


        }
        public void OnTriggerExit(Collider other)
        {
            
                Detach(other.gameObject);
     
        }
        public void Detach(GameObject obj)
        {
            if (attachedObjects.Contains(obj))
            {
                Debug.Log("Detaching " + obj.name + " from moving platform");
                obj.transform.parent = null;
                attachedObjects.Remove(obj);
            }
        }

        public void TurnOff()
        {
            Debug.Log("Turning off: " + this.name);
            Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
            velocityBeforeTurnOff = rigidbody.velocity;
            angularVelocityBeforeTurnOff = rigidbody.angularVelocity;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
 
            MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                renderer.material.SetColor("_EmissionColor", Color.grey);

                Light light = this.gameObject.GetComponentInChildren<Light>();
                light.color = Color.black;
               
            }
            DisableBehaviour();
        }
        public void TurnOn()
        {
            Initialize();
            BeginBehaviour(velocityBeforeTurnOff, angularVelocityBeforeTurnOff);
        }
    }
}