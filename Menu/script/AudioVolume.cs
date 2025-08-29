using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolume : MonoBehaviour
{
    public AudioSource[] musicSource;
    public AudioSource[] gameSource;



    void Update()
    {
        for (int i = 0; i < musicSource.Length; i++)
        {
            musicSource[i].volume = MainMenu.musicVolume;
            gameSource[i].volume = MainMenu.gameVolume;
        }
        
    }
}
