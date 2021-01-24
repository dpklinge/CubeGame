using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CubeTypes
{
    class GenericCube : CubeType
    {

        public override void BeginBehaviour(Vector3 velocity, Vector3 angularVelocity)
        {
            Debug.Log("Beginning behaviour generic box");
            Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
            if (rigidbody != null)  
            {
                Debug.Log("Setting kinematic to false");
                rigidbody.isKinematic = false;
                rigidbody.velocity = velocity;
                rigidbody.angularVelocity = angularVelocity;
            }
            base.BeginBehaviour(velocity, angularVelocity);
        }

        public override void DisableBehaviour(String disableType)
        {
            Debug.Log("Disabling behaviour generic box; Setting kinematic to true");
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            base.DisableBehaviour(disableType);
        }

      
    }
}
