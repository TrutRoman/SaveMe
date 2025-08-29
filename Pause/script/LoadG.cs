using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadG : MonoBehaviour
{
    
    int savingSceneIndex;
    public Text loadText;
    float TimeToLoad = 0;
    float tit = 0;
    int pip = 0;


    void Awake()
    {
        savingSceneIndex = PlayerPrefs.GetInt("savedSceneIndex", 0);
        TimeToLoad = 0;
    }
    void Update()
    {
        TimeToLoad += Time.deltaTime;
        tit += Time.deltaTime;
        if (TimeToLoad >= 5f)
        {
            if (MainMenu.isContinue == false)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(savingSceneIndex);
                MainMenu.isContinue = false;

            }

        }
        if (tit >= 0.6f)
        {
            tit = 0;
            pip++;
            if (pip > 4) pip = 1;
            if (pip == 1) loadText.text = " ";
            else if (pip == 2) loadText.text = ".";
            else if (pip == 3) loadText.text = "..";
            else if (pip == 4) loadText.text = "...";
        }
    }

}
