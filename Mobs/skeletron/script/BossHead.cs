using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : MonoBehaviour
{
    static public int Health = 1500;
    public int HealthV;

    public AudioSource AudioBoss; 
    public AudioClip DamageClip;  


    void Update()
    {
        AudioBoss.volume = PlayerPrefs.GetFloat("gameVolume", 0.5f) * 1.5f;
        HealthV = Health;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Arra"))
        {
            Health -= FloraBoll.DamageBoll;
            if (DamageClip != null) AudioBoss.PlayOneShot(DamageClip, AudioBoss.volume);
        }
        if (other.CompareTag("Grus"))
        {
            Health -= Grus.powerDamage;
            if (DamageClip != null) AudioBoss.PlayOneShot(DamageClip, AudioBoss.volume);
        }
    }
}
