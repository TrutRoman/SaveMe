using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGG : MonoBehaviour
{
    public GameObject Arra;
    public GameObject Grus;
    private bool isPerson;

    void Start()
    {
        isPerson = PlayerPrefs.GetInt("isPerson", 0) == 1;
        if (isPerson)
        {
            Arra.SetActive(true);
            Grus.SetActive(false);
            
        }
        else
        {
            Grus.SetActive(true);
            Arra.SetActive(false);
        }
    }
}
