using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CubeTypes
{
    public abstract class CubeType : MonoBehaviour
    {
        public static bool levelReady = false;
        public Color CubeColor;
        public Color CubeSpeakingColor;
        public float MinTimeBetweenAudio = 2;
        public float MaxTimeBetweenAudio = 5;
        private SoundList list;
        private AudioSource source;
        public List<GameObject> attachedObjects = new List<GameObject>();
        private Vector3 velocityBeforeTurnOff;
        private Vector3 angularVelocityBeforeTurnOff;
        public bool active = false;
        private float timeToSoundPulse = 0;
        private float nextPulseTime;
        private float timePlayingSound;
        private AudioClip currentlyPlayingSound;
        private float baseLightValue;
        private float baseLightRange;
        public float maxLightMultiplier = 1.15f;


        public Color GetLiveColor()
        {
            return CubeColor;
        }
        public virtual void DisableBehaviour(String disableType)
        {
            active = false;
        }
        public virtual void BeginBehaviour(Vector3 velocity, Vector3 angularVelocity)
        {
            active = true;
        }
        public virtual void Initialize(CubeType type)
        {
            Debug.Log("Initializing "+this.name);
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
         
            CubeColor = type.GetLiveColor();
            list = type.list;
            source = Instantiate<AudioSource>(GameObject.Find("CubeAudioSource").GetComponent<AudioSource>());
            source.transform.SetParent(transform);
            //Debug.Log("Finding: " + this.GetType().Name + "Audio");
            try
            {
                GameObject soundList = GameObject.Find(this.GetType().Name + "Audio");
              
                    list = soundList.GetComponent<SoundList>();

                //Debug.Log("Sounds post find " + list);
                //Debug.Log("Sounds size: " + list.Size());
                //Debug.Log("First sound: " + list[0]);
                //Debug.Log("Audio sources present at initialization: " + list.clips);
            }
            catch(NullReferenceException e)            {
                Debug.LogWarning("Soundlist not found for " + this.GetType().Name);
               
            }

            CubeSpeakingColor = type.CubeSpeakingColor;
            MinTimeBetweenAudio = type.MinTimeBetweenAudio;
            MaxTimeBetweenAudio = type.MaxTimeBetweenAudio;
            
            MeshRenderer renderer = type.gameObject.GetComponent<MeshRenderer>();
           
            if (renderer != null)
            {
              //  Debug.Log("replacing: " + renderer.material.GetColor("_EmissionColor"));
                Material material = renderer.material;
                renderer.material.SetColor("_EmissionColor", CubeColor);
                Light light = type.gameObject.GetComponentInChildren<Light>();
                light.color = CubeColor;
                if (baseLightValue == 0.0)
                {
                   
                    baseLightValue = light.intensity;
                    baseLightRange = light.range;
                }
            }
           
            nextPulseTime = UnityEngine.Random.Range(MinTimeBetweenAudio, MaxTimeBetweenAudio) ;
           // Debug.Log("Time to next sound: " + nextPulseTime);
        }
        public void Start()
        {

        }

        public virtual void ApplyCollision(GameObject obj)
        {
           // Debug.Log("Applying collision between " + this + " and " + obj);
            Rigidbody rb = obj.GetComponentInChildren<Rigidbody>();
            if (rb != null && !rb.isKinematic)
            {
                if (!attachedObjects.Contains(obj))
                {
                 //   Debug.Log("Attaching " + obj.name + " to " + this.name);
                    attachedObjects.Add(obj.gameObject);
                    obj.transform.parent = this.gameObject.transform;
                }
            }

        }


        public void OnTriggerEnter(Collider other)
        {
          //  Debug.Log("Object entered cube " + name + " trigger: " + other.name);
            if (other.gameObject.layer == 9)
            {
            //    Debug.Log("Colliding with standard collision ignoring surface - aborting contact");
                return;
            }

            // if (other.transform.parent != null) { }
            //Debug.Log("Velocity at collision time of floatcube: "+ velocity);

            ApplyCollision(other.gameObject);


        }
        public void OnTriggerExit(Collider other)
        {

            Detach(other.gameObject);

        }
        public void Detach(GameObject obj)
        {
            if (attachedObjects.Contains(obj))
            {
                Debug.Log("Detaching " + obj.name + " from moving platform");
                obj.transform.parent = null;
                attachedObjects.Remove(obj);
            }
        }

        public void TurnOff()
        {
            if (active)
            {
                Debug.Log("Turning off: " + this.name);
                source.Stop();
                timePlayingSound = 0;
                currentlyPlayingSound = null;
                timeToSoundPulse = 0;
                nextPulseTime = 10;
                Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
                velocityBeforeTurnOff = rigidbody.velocity;
                angularVelocityBeforeTurnOff = rigidbody.angularVelocity;
                Debug.Log("Paused with velocity: " + velocityBeforeTurnOff + " and angular velocity " + angularVelocityBeforeTurnOff);
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                Debug.Log("Confirming they weren't zeroed - velocity: " + velocityBeforeTurnOff + " and angular velocity " + angularVelocityBeforeTurnOff);

                MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    Material material = renderer.material;
                    renderer.material.SetColor("_EmissionColor", Color.grey);

                    Light light = this.gameObject.GetComponentInChildren<Light>();
                    light.color = Color.black;

                }
                DisableBehaviour("turnOff");

            }
        }
        public void TurnOn()
        {
            if (!active)
            {
                Initialize(this);
                Debug.Log("Resuming with velocity: " + velocityBeforeTurnOff + " and angular velocity " + angularVelocityBeforeTurnOff);
                BeginBehaviour(velocityBeforeTurnOff, angularVelocityBeforeTurnOff);

            }
        }
        public void Update()
        {
            if (active && list !=null && list.Size() > 0)
            {
                timeToSoundPulse += Time.deltaTime;
                if (currentlyPlayingSound != null && timePlayingSound >= currentlyPlayingSound.length)
                {
                  //  Debug.Log("Ending sound");
                    source.Stop();
                    currentlyPlayingSound = null;
                }
                if (currentlyPlayingSound != null && timePlayingSound < currentlyPlayingSound.length)
                {
                   // Debug.Log("Sound is playing");
                    timePlayingSound += Time.deltaTime;

                   
                    MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
                    if (renderer != null)
                    {
                        
                       // Debug.Log("Updating color");
                        float fourth = currentlyPlayingSound.length / 4;
                        float threeFourths = 3* currentlyPlayingSound.length / 4;
                        float lightMultiplier = 1;
                        Color thisColor = CubeColor;
                        if (timePlayingSound <= fourth)
                        {
                            float percentage = timePlayingSound / fourth;
                            lightMultiplier += maxLightMultiplier* percentage;
                            thisColor = Color.Lerp(CubeColor, CubeSpeakingColor, percentage);
                            

                        }else if(timePlayingSound>fourth && timePlayingSound<= threeFourths)
                        {
                            lightMultiplier += maxLightMultiplier;
                            thisColor = CubeSpeakingColor;
                        }
                        else
                        {
                            float percentage = 1 - ((timePlayingSound - threeFourths) / fourth);
                            lightMultiplier += maxLightMultiplier * percentage;
                            thisColor = Color.Lerp(CubeColor, CubeSpeakingColor, percentage);
                        }
                        Material material = renderer.material;
                        renderer.material.SetColor("_EmissionColor", thisColor);
                       // Debug.Log("Color was: "+thisColor);
                        Light light = this.gameObject.GetComponentInChildren<Light>();
                        //light.color = thisColor;
                        light.intensity = baseLightValue * lightMultiplier;
                        light.range = baseLightRange * lightMultiplier;
                    }
                }
                if (timeToSoundPulse >= nextPulseTime)
                {
                   // Debug.Log(this.name+" triggering pulse");
                    TriggerSoundPulse();
                   
                    timeToSoundPulse = 0;
                    nextPulseTime = Random.Range(MinTimeBetweenAudio, MaxTimeBetweenAudio);
                }
             

            }
        }

 
        private void TriggerSoundPulse()
        {
            currentlyPlayingSound = list[Random.Range(0, list.Size() - 1)];
           // Debug.Log("Picking sound from sounds: " + list);
          //  Debug.Log("Sound picked: " + currentlyPlayingSound);
            source.PlayOneShot(currentlyPlayingSound);
            timePlayingSound = 0;

        }
        public void Awake()
        {
            if (!levelReady && active)
            {
                Debug.Log(this.name + " awakening");
                Initialize(GameObject.Find(this.GetType().Name+ "Type").GetComponent<CubeType>());
                BeginBehaviour(Vector3.zero, Vector3.zero);
                Debug.Log("Is kinematic at the endof awakening?" + this.GetComponent<Rigidbody>().isKinematic);
                StartCoroutine(Output());
            }
        }
        public IEnumerator Output()
        {
            
            yield return new WaitForSeconds(3f); // waits 3 seconds
            Debug.Log("Is kinematic at the end of delay?" + this.GetComponent<Rigidbody>().isKinematic);
        }
    }
}