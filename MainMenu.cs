using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animStart;
    void Start()
    {
        animStart = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonStart()
    {
        animStart.SetTrigger("start");
    }

    public void ButtonContinue()
    {
        animStart.SetTrigger("Continue");
    }

    public void ButtonOptions()
    {
        animStart.SetTrigger("setting");
    }
    public void ButtonInformation()
    {
        animStart.SetTrigger("info");
    }

    public void ButtonExit()
    {
        Application.Quit();
    }

    public void ButtonBack()
    {
        animStart.SetTrigger("backSetting");
    }

    public void ButtonBack2()
    {
        animStart.SetTrigger("backStart");
    }


    public Text textFullscreen;
    public bool isFullscreen = false;
    public void SetFullscreen()
    {
        Screen.fullScreen = isFullscreen;
        textFullscreen.text = isFullscreen ? "Fullscreen: On" : "Fullscreen: Off";
        isFullscreen = !isFullscreen;
    }

    public Slider sliderBrightness;
    public void SetBrightness()
    { 
        
        float brightness = Mathf.Clamp01(sliderBrightness.value); // гарантія, що 0..1
        RenderSettings.ambientLight = new Color(brightness, brightness, brightness, 1f);
        RenderSettings.ambientLight = new Color(brightness, brightness, brightness, 1f);
    }
}
