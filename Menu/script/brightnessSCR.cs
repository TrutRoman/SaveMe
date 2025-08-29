using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brightnessSCR : MonoBehaviour
{
    public float brightness = 1f;
    public 

    void Start()
    {
        brightness = PlayerPrefs.GetFloat("brightness", 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
