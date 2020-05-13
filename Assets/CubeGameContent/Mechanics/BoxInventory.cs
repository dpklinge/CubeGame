
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CubeTypes;

namespace Assets.VR_Controller_scripts
{
    class BoxInventory: MonoBehaviour
    {
        public CubeType[] CubeTypes ;
        public int[] Quantities;
        public GameObject SphereDisplayer;
        public GameObject SecondarySphereDisplayer;
        private int index=0;



  
        public CubeType GetCurrentCubeType()
        {
            SetCubeDisplayColors(CubeTypes[index]);
            Debug.Log("Selector returning: " + CubeTypes[index]);
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
                
                return CubeTypes[index];
            }
        }

        private void SetCubeDisplayColors(CubeType cubeType)
        {
           
            Color color = GameObject.Find(cubeType.GetType().Name).GetComponent<CubeType>().CubeColor;
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
            Debug.Log("Index: " + index);
            return GetCurrentCubeType();
        }
        public CubeType ScrollRight()
        {
            index = index + 1;
            if (index > CubeTypes.Length - 1)
            {
                index = 0 ;
            }
            Debug.Log("Index: " + index);
            return GetCurrentCubeType();
        }
        public void AddCube(CubeType addingType)
        {
            for(int i= 0; i<CubeTypes.Length; i++)
            {
                if(CubeTypes[i].GetType() == addingType.GetType())
                {
                    Quantities[i] = Quantities[i] + 1;
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
