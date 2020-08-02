using CubeTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRTeleporter : MonoBehaviour
{

    public GameObject positionMarker; // marker for display ground position
    public GameObject invalidPositionMarker;
    public float distanceFromSurface = 0.01f;

    public Transform bodyTransform; // target transferred by teleport

    public SteamVR_Input_Sources TeleportHand;

    public SteamVR_Action_Boolean TeleportBoolean;

    public LayerMask excludeLayers; // excluding for performance

    public float maximumLandableAngle = 30f;

    public float teleportArcAngle = 40f; // Arc take off angle

    public float strength = 5f; // Increasing this value will increase overall arc length


    int maxVertexcount = 100; // limitation of vertices for performance. 

    private float vertexDelta = 0.08f; // Delta between each Vertex on arc. Decresing this value may cause performance problem.

    private LineRenderer arcRenderer;

    private Vector3 velocity; // Velocity of latest vertex

    private Vector3 groundPos; // detected ground position

    private Vector3 lastNormal; // detected surface normal

    private bool groundDetected = false;
    private Boolean canTeleport = false;
    private CubeType cubeTarget = null;
    private CubeType lastCubeTarget = null;

    private List<Vector3> vertexList = new List<Vector3>(); // vertex on arc

    private bool displayActive = false; // don't update path when it's false.


    // Teleport target transform to ground position
    public void Teleport()
    {
        if (groundDetected && canTeleport)
        {
            
            bodyTransform.position = groundPos + lastNormal * 0.1f;
            if (lastCubeTarget != null)
            {
                lastCubeTarget.Detach(bodyTransform.gameObject);
                lastCubeTarget = null;
            }
            if (cubeTarget != null)
            {
                cubeTarget.ApplyCollision(bodyTransform.gameObject);
                lastCubeTarget = cubeTarget;
            }
           
        }
        else
        {
            Debug.Log("Ground wasn't detected");
        }
    }

    // Active Teleporter Arc Path
    public void ToggleDisplay(bool active)
    {
       
        if (!active)
        {
            arcRenderer.enabled = active;
            positionMarker.SetActive(active);
            invalidPositionMarker.SetActive(active);
        }
        displayActive = active;

    }

    private void TeleportUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        ToggleDisplay(false);
        Teleport();
        
    }

    private void TeleportDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        ToggleDisplay(true);
    }

    private void Awake()
    {
        arcRenderer = GetComponent<LineRenderer>();
        arcRenderer.enabled = false;
        positionMarker.SetActive(false);
        invalidPositionMarker.SetActive(false);

        TeleportBoolean.AddOnStateDownListener(TeleportDown, TeleportHand);
        TeleportBoolean.AddOnStateUpListener(TeleportUp, TeleportHand);
    }

    private void FixedUpdate()
    {
        if (displayActive)
        {
            UpdatePath();
        }
    }


    private void UpdatePath()
    {
        groundDetected = false;

        vertexList.Clear(); // delete all previouse vertices


        velocity = Quaternion.AngleAxis(-teleportArcAngle, transform.right) * transform.forward * strength;

        RaycastHit hit;


        Vector3 pos = transform.position; // take off position

        vertexList.Add(pos);

        while (!groundDetected && vertexList.Count < maxVertexcount)
        {
            Vector3 newPos = pos + velocity * vertexDelta
                + 0.5f * Physics.gravity * vertexDelta * vertexDelta;

            velocity += Physics.gravity * vertexDelta;

            vertexList.Add(newPos); // add new calculated vertex

            // linecast between last vertex and current vertex
            if (Physics.Linecast(pos, newPos, out hit, ~excludeLayers))
            {

               
                groundPos = hit.point;
                lastNormal = hit.normal;
               // Debug.Log("Hit normal: " + hit.normal);
                //Debug.Log("Angle between hit normal and (0,1,0): " + Vector3.Angle(hit.normal, new Vector3(0,1,0)));
                //Debug.Log("Is it less than maximum landable angle? " + (Math.Abs(Vector3.Angle(hit.normal, new Vector3(0, 1, 0))) < maximumLandableAngle));
                canTeleport =  Math.Abs(Vector3.Angle(hit.normal, new Vector3(0,1,0))) < maximumLandableAngle;
                cubeTarget = hit.transform.gameObject.GetComponent<CubeType>();
                groundDetected = true;

            }
            pos = newPos; // update current vertex as last vertex
        }


        


        positionMarker.transform.position = groundPos + lastNormal * distanceFromSurface;
        positionMarker.transform.LookAt(groundPos);
        invalidPositionMarker.transform.position = groundPos + lastNormal * distanceFromSurface;
        invalidPositionMarker.transform.LookAt(groundPos);

        if (groundDetected && canTeleport)
        {
            positionMarker.SetActive(true);
            invalidPositionMarker.SetActive(false);

        }
        if (groundDetected && !canTeleport)
        {
            invalidPositionMarker.SetActive(true);
            positionMarker.SetActive(false);

        }
        if (!groundDetected)
        {
            positionMarker.SetActive(false);
            invalidPositionMarker.SetActive(false);
        }

        // Update Line Renderer

        arcRenderer.positionCount = vertexList.Count;
        arcRenderer.SetPositions(vertexList.ToArray());
        arcRenderer.enabled = true;
    }


}