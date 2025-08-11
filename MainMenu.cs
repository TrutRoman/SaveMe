using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class MainMenu : MonoBehaviour
{
    public Animator animStart;
    void Start()
    {
        animStart = GetComponent<Animator>();
    }


    void Update()
    {
        SetBrightness();
        MusicV();
        GameV();
        SaveSettings();
    }
    void Awake()
    {
        if (brightness == 0) brightness = 0;

        LoadSettings();
    }


    //Головне меню

    //Кнопка нової гри
    public void ButtonStart()
    {
        animStart.SetTrigger("start");
    }

    //Кнопка продовження гри
    public void ButtonContinue()
    {
        animStart.SetTrigger("Continue");
    }

    //Кнопка налаштувань
    public void ButtonOptions()
    {
        animStart.SetTrigger("setting");
    }

    //Кнопка інформації
    public void ButtonInformation()
    {
        animStart.SetTrigger("info");
    }

    //Кнопка повернення з інформації
    public void ButtonBackInformation()
    {
        animStart.SetTrigger("backinfo");
    }

    //Кнопка виходу з гри
    public void ButtonExit()
    {
        Application.Quit();
    }

    //Кнопка повернення до головного меню з налаштувань
    public void ButtonBack()
    {
        animStart.SetTrigger("backSetting");
    }

    //Кнопка повернення до головного меню з вибору персонажу
    public void ButtonBack2()
    {
        animStart.SetTrigger("backStart");
    }






    //Налаштування


    //Текст повноекранного режиму
    public Text textFullscreen;
    public bool isFullscreen = false;
    public void SetFullscreen()
    {
        Screen.fullScreen = isFullscreen;
        if (LocalizationSettings.SelectedLocale.Identifier.Code == "en")
        {
            textFullscreen.text = isFullscreen ? "Fullscreen: On" : "Fullscreen: Off";
        }
        else if (LocalizationSettings.SelectedLocale.Identifier.Code == "uk")
        {
            textFullscreen.text = isFullscreen ? "Повноекранний режим: Увімкнено" : "Повноекранний режим: Вимкнено";
        }
        isFullscreen = !isFullscreen;
    }

    //Слайдер яскравості
    public Slider sliderBrightness;
    public static float brightness;
    public float tt;
    public void SetBrightness()
    {
        brightness = Mathf.Clamp(sliderBrightness.value, 0f, 0.4f);
        tt = brightness;
    }

    //Слайдер музики
    public Slider sliderMusic;
    public static float musicVolume;
    public void MusicV()
    {
        musicVolume = Mathf.Clamp(sliderMusic.value, 0f, 1f);
    }

    //Слайдер звуку в грі
    public Slider sliderGame;
    public static float gameVolume;
    public void GameV()
    {
        gameVolume = Mathf.Clamp(sliderGame.value, 0f, 1f);
    }

    //Кнопка вибору мови
    public Button buttonEn;
    public Button buttonUA;


    public void ChangeLanguageToEnglish()
    {
        buttonEn.gameObject.SetActive(false);
        buttonUA.gameObject.SetActive(true);
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale("uk");
        LocalizationSettings.SelectedLocale = locale;
        textFullscreen.text = isFullscreen ? "Повноекранний режим: Увімкнено" : "Повноекранний режим: Вимкнено";

        PlayerPrefs.SetString("lang", "uk");
        PlayerPrefs.Save();
    }

    public void ChangeLanguageToUkrainian()
    {
        buttonEn.gameObject.SetActive(true);
        buttonUA.gameObject.SetActive(false);
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale("en");
        LocalizationSettings.SelectedLocale = locale;
        textFullscreen.text = isFullscreen ? "Fullscreen: On" : "Fullscreen: Off";

        PlayerPrefs.SetString("lang", "en");
        PlayerPrefs.Save();
    }





    //Збереження даних і їх завантаження

    public void SaveSettings()
    {
        PlayerPrefs.SetString("lang", LocalizationSettings.SelectedLocale.Identifier.Code);
        PlayerPrefs.SetFloat("brightness", brightness);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("gameVolume", gameVolume);
        PlayerPrefs.SetFloat("fullscreen", isFullscreen ? 1f : 0f);


        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
    
        string savedLang = PlayerPrefs.GetString("lang", "en");
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(savedLang);
        LocalizationSettings.SelectedLocale = locale;

        brightness = PlayerPrefs.GetFloat("brightness", 0.4f);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        gameVolume = PlayerPrefs.GetFloat("gameVolume", 1f);
        isFullscreen = PlayerPrefs.GetFloat("fullscreen", 0f) == 1f;

    
        sliderBrightness.value = brightness;
        sliderMusic.value = musicVolume;
        sliderGame.value = gameVolume;

    
        textFullscreen.text = savedLang == "en"
        ? (isFullscreen ? "Fullscreen: On" : "Fullscreen: Off")
        : (isFullscreen ? "Повноекранний режим: Увімкнено" : "Повноекранний режим: Вимкнено");

        
        buttonEn.gameObject.SetActive(savedLang == "en");
        buttonUA.gameObject.SetActive(savedLang == "uk");
    }
}
