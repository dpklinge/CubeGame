using Assets.VR_Controller_scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using CubeTypes;

public class BoxCreationScript : MonoBehaviour
{
    public SteamVR_Action_Boolean BoxCreate; 
    public GameObject playBox;
    public float maxGrabDistance      = 0.1f;
    [Header("Controllers")]
    public SteamVR_Input_Sources leftHand;
    public SteamVR_Input_Sources rightHand;
    public float DistanceFromHands = 0.1f;
    public BoxInventory inventory;
    public LayerMask layersToAccessWhileGrabbing;
    private bool leftDown;
    private bool rightDown;
  
    
    private GameObject currentBox;
    private Vector3 lastCurrentBoxPosition;
    private Quaternion lastCurrentBoxRotation;
    private float lastDistance;
    

    void Start()
    {
        Debug.Log("Initializing boxcreate");
        BoxCreate.AddOnStateDownListener(TriggerDown, leftHand);
        BoxCreate.AddOnStateUpListener(TriggerUp, leftHand);
        BoxCreate.AddOnStateDownListener(TriggerDown, rightHand);
        BoxCreate.AddOnStateUpListener(TriggerUp, rightHand);

    }

    private void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (fromSource == SteamVR_Input_Sources.LeftHand)
        {
            Debug.Log("Left up");
            leftDown = false;
           
        }
        else if (fromSource == SteamVR_Input_Sources.RightHand)
        {
            Debug.Log("right up");
            rightDown = false;
            
        }
        if(!leftDown && !rightDown && currentBox != null)
        {
            Debug.Log("Both triggers released");
            ReleaseBox();
            
        }
    }
    private void ReleaseBox()
    {
        Debug.Log("Releasing box " + currentBox);
        currentBox.transform.SetParent(null);
        
        Vector3 velocity = (currentBox.transform.position - lastCurrentBoxPosition) / Time.deltaTime;
        Quaternion rotationDelta = Quaternion.Inverse(lastCurrentBoxRotation) * currentBox.transform.rotation;

        Vector3 angularVelocity = rotationDelta.eulerAngles / Time.deltaTime;

        Debug.Log("Beginning box behaviour");
        currentBox.GetComponent<CubeType>().BeginBehaviour(velocity, angularVelocity);
        currentBox = null;
    }

    private void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if(fromSource == SteamVR_Input_Sources.LeftHand)
        {
            Debug.Log("Left down");
            leftDown = true;
        }else if (fromSource == SteamVR_Input_Sources.RightHand)
        {
            Debug.Log("Right down");
            rightDown = true;
        }
        if(((leftDown && !rightDown)||(rightDown&&!leftDown)) && currentBox == null)
        {
            if (leftDown)
            {
                AttemptGrab(GameObject.Find("LeftHand"));
            }else if (rightDown)
            {
                AttemptGrab(GameObject.Find("RightHand"));
            }
        }
        if (leftDown && rightDown && currentBox==null)
        {
            Debug.Log("Making box?");

            CreateBox();
        }
    }

    private void CreateBox()
    {
        Debug.Log("Beginning create box");
        CubeType type = inventory.GetCurrentCube();
        Debug.Log("Creating box of type: " + type);
        if (type == null)
        {
            return;
        }
        Transform leftHandPos = GameObject.Find("LeftHand").transform;
        Transform rightHandPos = GameObject.Find("RightHand").transform;
        Vector3 direction = (rightHandPos.position - leftHandPos.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Debug.Log("Instantiating: " + playBox);
        GameObject instance = Instantiate(playBox, (leftHandPos.position + rightHandPos.position) / 2, rotation);
        Debug.Log("Getting type");
        
        Debug.Log("Type: " + type);
        instance.AddComponent(type.GetType());
        instance.GetComponent<CubeType>().Initialize(type);
        float distance = Vector3.Distance(rightHandPos.position, leftHandPos.position);
        instance.transform.localScale = new Vector3(distance, distance, distance);
        currentBox = instance;
        
    }

    private void AttemptGrab(GameObject hand)
    {
        

        Collider[] hitColliders = Physics.OverlapSphere(hand.transform.position, maxGrabDistance, layersToAccessWhileGrabbing);
        GameObject nearestBox = null;
        float nearestDistance = 100;
        foreach(Collider collider in hitColliders)
        {
            if (collider.gameObject.GetComponent(typeof(CubeType))!=null)
            {
                float distance = Vector3.Distance(collider.gameObject.transform.position, hand.transform.position);
                if (distance < nearestDistance)
                {
                    nearestBox = collider.gameObject;
                    nearestDistance = distance;
                }
            }
        }
        if(nearestBox != null)
        {
            currentBox = nearestBox;
            currentBox.transform.SetParent(hand.transform);
            currentBox.GetComponent<CubeType>().DisableBehaviour("grab");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (leftDown && rightDown && currentBox != null)
        {
            Transform leftHandPos = GameObject.Find("LeftHand").transform;
            Transform rightHandPos = GameObject.Find("RightHand").transform;
            Vector3 direction = (rightHandPos.position - leftHandPos.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            currentBox.transform.rotation = rotation;
            float distanceBetweenHands = Vector3.Distance(rightHandPos.position, leftHandPos.position);
            currentBox.transform.localScale = new Vector3(distanceBetweenHands, distanceBetweenHands, distanceBetweenHands);
            currentBox.transform.position = (leftHandPos.position + rightHandPos.position) / 2;
            lastDistance = distanceBetweenHands/2;

        }
        else if(leftDown && currentBox!=null)
        {
            Transform leftHandPos = GameObject.Find("LeftHand").transform;
            OneHandBoxHoldPositionUpdate(leftHandPos, -1);
            //currentBox.transform.position = (currentBox.transform.position+(leftHandPos.position - lastLeftPosition));
     
            //lastLeftPosition = leftHandPos.position;
        }
        else if (rightDown && currentBox != null)
        {
            Transform rightHandPos = GameObject.Find("RightHand").transform;
            OneHandBoxHoldPositionUpdate(rightHandPos, 1);
            //currentBox.transform.position = (currentBox.transform.position + (rightHandPos.position - lastRightPosition));
            //lastRightPosition = rightHandPos.position;
        }
    }

    private void OneHandBoxHoldPositionUpdate(Transform handPos, int sign)
    {

        //currentBox.transform.position = (currentBox.transform.position+(leftHandPos.position - lastLeftPosition));

        lastCurrentBoxPosition = currentBox.transform.position;
        lastCurrentBoxRotation = currentBox.transform.rotation;
        currentBox.transform.SetParent(handPos.transform);
        
    }

    private void OneHandBoxHoldPositionUpdateNoVerticalAngle(Transform handPos, int sign)
    {

        //currentBox.transform.position = (currentBox.transform.position+(leftHandPos.position - lastLeftPosition));

        lastCurrentBoxPosition = currentBox.transform.position;
        lastCurrentBoxRotation = currentBox.transform.rotation;
        Vector3 direction = handPos.forward;
        Debug.Log("Vector between hand and body: " + direction);
        Vector3 perpendicular = Vector3.Cross(direction, new Vector3(0, 1, 0));
        Debug.Log("Vector of perpendicular: " + perpendicular);
        Quaternion rotation = Quaternion.LookRotation(perpendicular);
        currentBox.transform.rotation = rotation;
        currentBox.transform.position = handPos.position + (sign * perpendicular.normalized * lastDistance);

    }
}
