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
            Debug.Log("Beginning behaviour generic box; Setting kinematic to false");
            Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            rigidbody.velocity = velocity;
            rigidbody.angularVelocity = angularVelocity;
        }

        public override void DisableBehaviour()
        {
            Debug.Log("Disabling behaviour generic box; Setting kinematic to true");
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }

      
    }
}
