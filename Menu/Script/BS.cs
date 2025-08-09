using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BS : MonoBehaviour
{
    public Image image;
    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        image.color = new Color(1f, 1f, 1f, MainMenu.brightness);
    }
}
