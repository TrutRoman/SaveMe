using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{
    int Health = 50;
    bool GG;
    bool isDamage = true;
    float damageTime = 0f;
    public SpriteRenderer[] sr;
    public GameObject Boss;
    void Start()
    {
        sr[0] = GetComponent<SpriteRenderer>();
        sr[1] = GetComponent<SpriteRenderer>();
        sr[2] = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        GG = PlayerPrefs.GetInt("isPerson", 0) == 1;

        sr[0].color = new Color(1f, 0.25f, 0.25f, 1f);
        sr[1].color = new Color(1f, 0.25f, 0.25f, 1f);
        sr[2].color = new Color(1f, 0.25f, 0.25f, 1f);
        damageTime -= Time.deltaTime;
        if (damageTime <= 0)
        {
            isDamage = false;
            sr[0].color = new Color(1f, 1f, 1f, 1f);
            sr[1].color = new Color(1f, 1f, 1f, 1f);
            sr[2].color = new Color(1f, 1f, 1f, 1f);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AttackGG"))
        {
            if (GG == false)
            {
                Health -= FloraBoll.DamageBoll;
            }
            else
            {
                Health -= Grus.powerDamage;
            }
            if (Health <= 0)
            {
                Destroy(Boss);
            }
            Health -= Grus.powerDamage;

            Arra.XPArra += 50;
            Grus.XPGrus += 50;
            isDamage = true;
        }
    }
}


