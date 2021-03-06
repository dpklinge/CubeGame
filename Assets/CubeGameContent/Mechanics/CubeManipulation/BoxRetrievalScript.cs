﻿using Assets.VR_Controller_scripts;
using CubeTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BoxRetrievalScript : MonoBehaviour
{
    public SteamVR_Action_Boolean BoxRemove;
    public SteamVR_Input_Sources handSource;
    public float DelayUntilRetrieval = 1;
    public float MaximumDistance = 100f;
    public float Thickness = .01f;
    public Color PassiveColor = Color.red;
    public Color ActiveColor = Color.cyan;
    public LayerMask TraceLayerMask;
    private bool isActive = false;
    private bool targetIsCube = false;
    private GameObject laser = null;
    public GameObject hand;
    public float ShakeDistance = 0.001f;
    public BoxInventory inventory;
    private GameObject lastTarget;
    private float timeIncrement=0;
    public float ShakeTime = 25;
    
    
    // Start is called before the first frame update
    void Start()
    {
        BoxRemove.AddOnStateDownListener(TriggerDown, handSource);
        BoxRemove.AddOnStateUpListener(TriggerUp, handSource);
        Debug.Log("Finding hand : " + hand.name);
        hand = GameObject.Find(hand.name);
        
    }


    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
           // Debug.Log("Laser is active");
            RaycastHit raycastHit;
            if (Physics.Raycast(hand.transform.position, hand.transform.forward, out raycastHit, MaximumDistance, TraceLayerMask))
            {
                GameObject target = raycastHit.collider.gameObject;
             //  Debug.Log("Raycast hit " + target);
                CubeType type = target.GetComponentInParent<CubeType>();
                    targetIsCube = type != null;
                RenderLaser(target, raycastHit);
            }
            else
            {
               // Debug.Log("Raycast hit nothing");
            }
        }
    }

    private void RenderLaser(GameObject target, RaycastHit collisionPoint)
    {
      //Debug.Log("Rendering laser");
        Color color;
        if (targetIsCube)
        {

            color = ActiveColor;
            ShakeCube(target);
            TryRetrieval(target);
        }
        else
        {
            color = PassiveColor;
        }
        if (laser == null)
        {
            //Debug.Log("Created laser, attached to "+hand.transform +" at " +hand.transform.position);
            laser = GameObject.CreatePrimitive(PrimitiveType.Cube);
            laser.transform.parent = hand.transform;
            laser.transform.localScale = new Vector3(Thickness, Thickness, collisionPoint.distance);
            laser.transform.localPosition = new Vector3(0f, 0f, collisionPoint.distance/2);
            laser.transform.localRotation = Quaternion.identity;
            Material newMaterial = new Material(Shader.Find("Unlit/Color"));
            Destroy(laser.GetComponent<BoxCollider>());


            newMaterial.SetColor("_Color", color);
            laser.GetComponent<MeshRenderer>().material = newMaterial;
        }
        else
        {
           // Debug.Log("Updating laser");
            laser.transform.localScale = new Vector3(Thickness, Thickness, Vector3.Distance(target.transform.position, hand.transform.position));
            laser.transform.localPosition = new Vector3(0f, 0f, Vector3.Distance(target.transform.position, hand.transform.position)/2);
         //   Debug.Log("Parent transform position: " + laser.transform.parent.position);
         //  Debug.Log("Transform: " + laser.transform);
         //   Debug.Log("Scale: " +laser.transform.localScale);
         //   Debug.Log("Position: " + transform.position);
            laser.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        }
        lastTarget = target;
    }

    private void TryRetrieval(GameObject target)
    {
        if(target == lastTarget) {
            timeIncrement += Time.deltaTime;
            if (timeIncrement >= DelayUntilRetrieval)
            {
                CubeType type = target.GetComponentInParent<CubeType>();
                inventory.AddCube(type);
                if (type.attachedObjects != null)
                {
                    List<GameObject> copy = new List<GameObject>(type.attachedObjects);
                    foreach(GameObject obj in copy){
                        type.Detach(obj);
                    }
                }
                if (target.transform.parent != null)
                {
                    target = target.transform.parent.gameObject;
                }
                
                Debug.Log("Destroying target: " + target);
                target.GetComponent<CubeType>().DisableBehaviour("initial");
                Destroy(target);
                timeIncrement = 0f;
                lastTarget = null;
            }
        }
        else
        {
            
            timeIncrement = 0f;
        }
    }

    private void ShakeCube(GameObject target)
    {
        if (target.transform.parent != null)
        {
            target = target.transform.parent.gameObject;
        }
        float x = Mathf.Sin(Time.time*ShakeTime) * ShakeDistance;
        target.transform.Translate(new Vector3(x, 0, 0), Space.World);
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Laser button depressed");
        isActive = true;
    }
    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Laser button released");
        isActive = false;
        targetIsCube = false;
        Destroy(laser);
        laser = null;
        timeIncrement = 0f;
        lastTarget = null;
    }
}
