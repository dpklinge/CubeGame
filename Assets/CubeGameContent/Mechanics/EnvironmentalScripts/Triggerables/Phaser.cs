using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phaser : Triggerable
{
    public Boolean StartsPhased = false;
    private Boolean phaseTriggered = false;
    private float phasingTime = 0;
    public float PhaseOutTime = 0;
    public float PhaseInTime = 0;
    public void Update()
    {
        if (phaseTriggered)
        {
            phasingTime += Time.deltaTime;
            if (phasingTime >= PhaseOutTime)
            {
                Debug.Log("Phase gate completely phased");
                Color color = gameObject.GetComponent<MeshRenderer>().material.color;
                color.a =0;
                gameObject.GetComponent<MeshRenderer>().material.color = color;
                gameObject.GetComponent<Collider>().enabled = false;

            }
            else
            {
                Debug.Log("Phasing transparency: alpha = 1- "+phasingTime+"/"+PhaseOutTime+"="+(1-phasingTime / PhaseOutTime));
                Color color = gameObject.GetComponent<MeshRenderer>().material.color;
                color.a = 1 - (phasingTime / PhaseOutTime);
                gameObject.GetComponent<MeshRenderer>().material.color = color;
            }
        }
        else if (phasingTime > 0)
        {
            phasingTime -= Time.deltaTime;
            if (phasingTime >= PhaseOutTime)
            {
                gameObject.GetComponent<Collider>().enabled = true;
            }
            Debug.Log("Unphasing transparency");
            Color color = gameObject.GetComponent<MeshRenderer>().material.color;
                color.a = (phasingTime / PhaseInTime);
                gameObject.GetComponent<MeshRenderer>().material.color = color;
            
        }
    }
    public override void Trigger()
    {
        Debug.Log("Triggering phase");
        phaseTriggered = true;
    }

    internal override void Untrigger()
    {
        Debug.Log("Untriggering phase");
        phaseTriggered = false;
    }

}
