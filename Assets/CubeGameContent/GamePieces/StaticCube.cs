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
            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            base.BeginBehaviour(velocity, angularVelocity);

        }

        public override void DisableBehaviour(String disableType)
        {
            Debug.Log("Disabling static box; Setting kinematic to true");
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            base.DisableBehaviour(disableType);
        }


    }
}
