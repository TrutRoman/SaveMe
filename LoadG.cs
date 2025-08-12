using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadG : MonoBehaviour
{
    
    int savingSceneIndex;
    public Text textTips;
    float TimeToLoad = 0;



    void Awake()
    {
        savingSceneIndex = PlayerPrefs.GetInt("savedSceneIndex", 0);
        TimeToLoad = 0;
    }
    void Update()
    {
        TimeToLoad += Time.deltaTime;
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
    }


}
