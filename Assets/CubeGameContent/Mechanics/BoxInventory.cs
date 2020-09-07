﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CubeTypes;
using Random = UnityEngine.Random;

namespace Assets.VR_Controller_scripts
{
    public class BoxInventory: MonoBehaviour
    {
        public CubeType[] CubeTypes ;
        public int[] Quantities;
        public GameObject SphereDisplayer;
        public GameObject SecondarySphereDisplayer;
        public GameObject playBox;
        public float OrbitForce = 5;
        public float OrbitMax = .06f;
        public float OrbiterSize = .2f;
        private int index=0;
        private List<GameObject> primaryOrbiters;
        private List<GameObject> secondaryOrbiters;
        public void Start()
        {
            primaryOrbiters = new List<GameObject>();
            secondaryOrbiters = new List<GameObject>();
            PopulateCubeDisplays();
        }


        public void Update()
        {
          //  Debug.Log("Updating minibox positions");
            foreach (GameObject orbiter in primaryOrbiters)
            {
                Vector3 directionTowardsSource = orbiter.transform.position - SphereDisplayer.transform.position;
              //  Debug.Log("Direction for primary: " + directionTowardsSource);
                
               // Debug.Log("current minicube position: "+orbiter.transform.position);
               // Debug.Log("direction towards source: " + directionTowardsSource);
               // Debug.Log("Source location: " + SphereDisplayer.transform.position);
               // Debug.Log("Current velocity: " + orbiter.GetComponent<Rigidbody>().velocity);
                orbiter.GetComponent<Rigidbody>().velocity = orbiter.GetComponent<Rigidbody>().velocity + -directionTowardsSource * OrbitForce * Time.deltaTime;
               // Debug.Log("Velocity after applying force: " + orbiter.GetComponent<Rigidbody>().velocity);
            }
            foreach (GameObject orbiter in secondaryOrbiters)
            {
                Vector3 directionTowardsSource = orbiter.transform.position - SecondarySphereDisplayer.transform.position;
              //  Debug.Log("Direction for secondary: " + directionTowardsSource);
                orbiter.GetComponent<Rigidbody>().velocity = orbiter.GetComponent<Rigidbody>().velocity + -directionTowardsSource * OrbitForce*Time.deltaTime;
            }
        }


        public CubeType GetCurrentCubeType()
        {
            SetCubeDisplayColors(CubeTypes[index]);
           // Debug.Log("Selector returning: " + CubeTypes[index]);
            return CubeTypes[index];
        }
        public CubeType GetCurrentCube()
        {
            
            if (Quantities[index] <= 0)
            {
                return null;
            }
            else
            {
                Quantities[index] = Quantities[index] - 1;
                GameObject primary = primaryOrbiters[0];
                GameObject secondary = secondaryOrbiters[0];
                primaryOrbiters.RemoveAt(0);
                secondaryOrbiters.RemoveAt(0);
                GameObject.Destroy(primary);
                GameObject.Destroy(secondary);
                return CubeTypes[index];
            }
        }
        private void PopulateCubeDisplays()
        {
            foreach(GameObject orbiter in primaryOrbiters)
            {
              //  Debug.Log("Destroying:" + orbiter);
                GameObject.Destroy(orbiter);
            }
            foreach (GameObject orbiter in secondaryOrbiters)
            {
                //Debug.Log("Destroying:" + orbiter);
                GameObject.Destroy(orbiter);
            }
            primaryOrbiters = new List<GameObject>();
            secondaryOrbiters = new List<GameObject>();
            for (int i=0; i<Quantities[index]; i++)
            {
              //  Debug.Log("Adding cube to primary");
                AddCubeTypeToDisplay(CubeTypes[index], SphereDisplayer, primaryOrbiters);
               // Debug.Log("Adding cube to secondary");
                AddCubeTypeToDisplay(CubeTypes[index], SecondarySphereDisplayer, secondaryOrbiters);
            }
        }
        public void AddCubeTypeToDisplay(CubeType type, GameObject display, List<GameObject> orbiters)
        {
            GameObject instance = Instantiate(playBox, display.transform);
            MeshRenderer renderer = instance.GetComponent<MeshRenderer>();
            Material material = renderer.material;
            renderer.material.SetColor("_EmissionColor", type.GetLiveColor());

            Light light = instance.GetComponentInChildren<Light>();
            light.color = type.GetLiveColor();
            instance.transform.parent = display.transform;
            instance.transform.localScale = new Vector3(OrbiterSize, OrbiterSize, OrbiterSize);
           
            float dx = Random.Range(-OrbitMax, OrbitMax);
            float dy = Random.Range(-OrbitMax, OrbitMax);
            float dz = Random.Range(-OrbitMax, OrbitMax);
            Vector3 position = new Vector3(display.transform.position.x+dx, display.transform.position.y + dy, display.transform.position.z + dz);
            //Debug.Log("position of display : " + display.transform.position);
            //Debug.Log("Should shift by: " + dx + " " + dy + dz);
            //Debug.Log("position of new minicube : " + position);

            instance.transform.position = position;
            instance.GetComponent<Rigidbody>().isKinematic = false;
            instance.GetComponent<Rigidbody>().useGravity = false;
            instance.GetComponent<Rigidbody>().angularDrag = 0;
            orbiters.Add(instance);

        }
        
        private void SetCubeDisplayColors(CubeType cubeType)
        {

            Color color = cubeType.GetLiveColor();
            Debug.Log("Changing particle color to " + color);
            ParticleSystem sys1 = SphereDisplayer.GetComponentInChildren<ParticleSystem>();
            ParticleSystem sys2 = SecondarySphereDisplayer.GetComponentInChildren<ParticleSystem>();
            
            ParticleSystem.MainModule settings = sys1.main;
            settings.startColor = new ParticleSystem.MinMaxGradient(new Color(color.r,color.g, color.b, 255));
            settings = sys2.main;
            settings.startColor = new ParticleSystem.MinMaxGradient(new Color(color.r, color.g, color.b, 255));

        }

        public CubeType ScrollLeft()
        {
            index = index - 1;
            if (index < 0)
            {
                index = CubeTypes.Length - 1;
            }
            //Debug.Log("Index: " + index);
            PopulateCubeDisplays();
            return GetCurrentCubeType();
        }
        public CubeType ScrollRight()
        {
            index = index + 1;
            if (index > CubeTypes.Length - 1)
            {
                index = 0 ;
            }
            //Debug.Log("Index: " + index);
            PopulateCubeDisplays();
            return GetCurrentCubeType();
        }
        public void AddCube(CubeType addingType)
        {
            for(int i= 0; i<CubeTypes.Length; i++)
            {
                if(CubeTypes[i].GetType() == addingType.GetType())
                {
                    Quantities[i] = Quantities[i] + 1;
                    if (CubeTypes[index].GetType() == addingType.GetType())
                    {
                        AddCubeTypeToDisplay(addingType, SphereDisplayer, primaryOrbiters);
                        AddCubeTypeToDisplay(addingType, SecondarySphereDisplayer, secondaryOrbiters);
                    }
                    return;
                }
            }
            Array.Resize<CubeType>(ref CubeTypes, CubeTypes.Length + 1);
            Array.Resize<int>(ref Quantities, CubeTypes.Length);
            CubeTypes[CubeTypes.Length - 1] = addingType;
            Quantities[CubeTypes.Length - 1] = 1;
            
            
        }
    }
}
