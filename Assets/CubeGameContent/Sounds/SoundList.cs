using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundList : MonoBehaviour
{
    public List<AudioClip> clips = new List<AudioClip>();
    void Start()
    {

    }
    public int Size()
    {
        return clips.Count;
    }


    public  AudioClip this[int index]
    {
        get => clips[index];
        set => clips[index] = value;
    }
    void Update()
    {
        
    }
}
