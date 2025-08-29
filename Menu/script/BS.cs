using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BS : MonoBehaviour
{
    public Image image;
    float brightnessV = 0f;
    void Start()
    {
        image = GetComponent<Image>();
        brightnessV = PlayerPrefs.GetFloat("brightness", 0f);
    }

    void Update()
    {
        image.color = new Color(1f, 1f, 1f, brightnessV);
        image.color = new Color(1f, 1f, 1f, MainMenu.brightness);
    }
}
