using CubeTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTrigger : MonoBehaviour
{

    [Header("Leave blank to accept any type of trigger.")]
    public CubeType type;
    public List<Triggerable> triggerables = new List<Triggerable>();
    private List<CubeType> cubesOnTrigger = new List<CubeType>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        CubeType collidingType = collision.gameObject.GetComponent<CubeType>();
        if (type == null || collidingType.GetType() == type.GetType())
        {

            if (cubesOnTrigger.Count == 0)
            {

                foreach (Triggerable triggerable in triggerables)
                {
                    triggerable.Trigger();
                }
            }
            cubesOnTrigger.Add(collidingType);
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        CubeType collidingType = collision.gameObject.GetComponent<CubeType>();
        if (type == null || collidingType.GetType() == type.GetType())
        {
            cubesOnTrigger.Remove(collidingType);
            if (cubesOnTrigger.Count ==0)
            {
                foreach (Triggerable triggerable in triggerables)
                {
                    triggerable.Untrigger();
                }
            }
        }
    }
}
