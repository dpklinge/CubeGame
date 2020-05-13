using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CubeTypes
{
    class StaticCube : CubeType
    {

        public override void BeginBehaviour(Vector3 velocity, Vector3 angularVelocity)
        {
            Debug.Log("Beginning static box; Setting kinematic to true");
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
           
        }

        public override void DisableBehaviour()
        {
            Debug.Log("Disabling static box; Setting kinematic to true");
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }


    }
}
