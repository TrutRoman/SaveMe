using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skeletron : MonoBehaviour
{


    public Animator anim;
    public AudioSource audioSourceBoss;

    int Health;

    public Image HealthImage;
    public GameObject CanvasBoss;

    public AudioSource AudioBoss;
    public AudioClip DeathClip;

    void Update()
    {
        AudioBoss.volume = PlayerPrefs.GetFloat("gameVolume", 0.5f) * 1.5f;
        Health = BossHead.Health;
        if (TriggerBoss.isBoss == true)
        {
            anim.SetTrigger("StartFight");
            CanvasBoss.SetActive(true);
        }
        if (isShake) StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(0.5f, 0.2f));




        if (Health <= 1000 && Health > 500)
        {
            anim.SetTrigger("State2");
        }
        if (Health <= 500 && Health > 0)
        {
            anim.SetTrigger("State3");
        }
        if (Health <= 0)
        {
            anim.SetTrigger("Death");
            TriggerBoss.isBoss = false;
            if (DeathClip != null) AudioBoss.PlayOneShot(DeathClip, AudioBoss.volume);
        }

        float healthPercent = Mathf.Clamp01((float)Health / 1500);
        HealthImage.rectTransform.sizeDelta = new Vector2(580 * healthPercent, HealthImage.rectTransform.sizeDelta.y);
    }



    bool isShake = false;
    public void StartBossFightAnimation()
    {
        isShake = true;

    }
    public void FinishBossFightAnimation()
    {
        isShake = false;

    }

    void DeadBoss()
    {
        Destroy(gameObject);
        TriggerBoss.isBoss = false;
        CanvasBoss.SetActive(false);
    }
}
