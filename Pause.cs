using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public int savedSceneIndex = 1;

    public Image background;
    public Text text;
    public Button b1;
    public Button b2;
    public Button b3;
    public Button bb;
    public Text info;

    void Start()
    {
        background.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        b1.gameObject.SetActive(true);
        b2.gameObject.SetActive(true);
        b3.gameObject.SetActive(true);
        bb.gameObject.SetActive(false);
        info.gameObject.SetActive(false);


        /*
        string savedLang = PlayerPrefs.GetString("lang", "en");
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(savedLang);
        LocalizationSettings.SelectedLocale = locale;
        */
    }


    void Update()
    {
        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
    }

    public void SaveAndExit()
    {
        PlayerPrefs.SetInt("savedSceneIndex", savedSceneIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void InformotionPause()
    {
        background.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        b1.gameObject.SetActive(false);
        b2.gameObject.SetActive(false);
        b3.gameObject.SetActive(false);
        bb.gameObject.SetActive(true);
        info.gameObject.SetActive(true);
    }

    public void BackInformotionPause()
    {
        background.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        b1.gameObject.SetActive(true);
        b2.gameObject.SetActive(true);
        b3.gameObject.SetActive(true);
        bb.gameObject.SetActive(false);
        info.gameObject.SetActive(false);
    }
}
