using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextLevel : MonoBehaviour
{
    public String sceneToBeLoaded;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Endzone triggered with object " + collision.gameObject);
        if (collision.gameObject.name.Equals("Player"))
        {
            Debug.Log("Object was player");
            Load();
        }
        else
        {
            Debug.Log("Object was not player");
        }
    }

    public void Load()
    {
        SceneManager.LoadScene(sceneToBeLoaded);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
