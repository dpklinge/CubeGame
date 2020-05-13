using UnityEngine;
namespace CubeTypes
{
    public abstract class CubeType : MonoBehaviour
    {
        public Color CubeColor;
        public abstract void BeginBehaviour(Vector3 velocity, Vector3 angularVelocity);
        public abstract void DisableBehaviour();
        public virtual void Initialize() {
            CubeColor = GameObject.Find(this.GetType().Name).GetComponent<CubeType>().CubeColor;
            Debug.Log("Initializing with color " + CubeColor);
            MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
            Material material = renderer.material;
            renderer.material.SetColor("_EmissionColor", CubeColor);

            Light light = this.gameObject.GetComponentInChildren<Light>();
            light.color = CubeColor;
        }
    }
}