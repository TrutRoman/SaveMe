using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrkW : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public Transform[] player;
    public bool isMove = false;
    public float Distanse = 5f;

    [Header("Health")]
    public int Health = 50;
    private bool isDead = false;

    [Header("Damage")]
    bool GG;
    public SpriteRenderer sr;
    public Animator anim;

    [Header("Attack")]
    public float DistanseKick = 10f;
    public GameObject Kick;
    private float cooldownTime = 0f;
    private float cooldownDelay = 2f;
    private bool isAttacking = false;

    private bool isDamage = false;
    private float damageTime = 0f;


    [Header("Audio")]
    public AudioSource audioSourceOrk;
    public AudioClip audioClipKickOrk;
    public AudioClip audioClipDamageOrk;
    public AudioClip audioClipDeathOrk;
    public AudioClip audioClipWalkOrk;
    public AudioClip audioClipidleOrk;
    private float walkSoundTimer = 0f;
    float walkSoundInterval = 0.6f;
    public float idleSoundInterval = 0;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSourceOrk.volume = PlayerPrefs.GetFloat("gameVolume", 0.5f) * 1.5f;
    }

    void Update()
    {
        if (isDead) return;

        
        if (cooldownTime > 0)
            cooldownTime -= Time.deltaTime;

        
        if (isDamage)
            Damage();

        KickOrk();
        MoveTowardsPlayer();

        GG = PlayerPrefs.GetInt("isPerson", 0) == 1;
        
    }

    private void MoveTowardsPlayer()
    {
        if (!isMove || isAttacking)
        {
            anim.SetBool("isWalk", false);
            return;
        }

        if (Vector2.Distance(transform.position, player[0].position) <= Distanse)
        {
            anim.SetBool("isWalk", true);
            Vector2 targetPosition = new Vector2(player[0].position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            PlayWalkSound();

            if (transform.position.x < player[0].position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                Kick.transform.position = transform.position + new Vector3(4f, 0f, 0f);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                Kick.transform.position = transform.position + new Vector3(-4f, 0f, 0f);
            }
        }

        else if (Vector2.Distance(transform.position, player[1].position) <= Distanse)
        {
            anim.SetBool("isWalk", true);
            Vector2 targetPosition = new Vector2(player[1].position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            PlayWalkSound();

            if (transform.position.x < player[1].position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                Kick.transform.position = transform.position + new Vector3(4f, 0f, 0f);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                Kick.transform.position = transform.position + new Vector3(-4f, 0f, 0f);
            }
        }
        else
        {
            anim.SetBool("isWalk", false);

        }

        idleSoundInterval += Time.deltaTime;

        if (idleSoundInterval >= 10f && !isMove && !isAttacking && !isDamage && !isDead)
        {
            audioSourceOrk.PlayOneShot(audioClipidleOrk, audioSourceOrk.volume);
            idleSoundInterval = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Arra"))
        {
            isDamage = true;
            damageTime = 0.5f;
            Health -= FloraBoll.DamageBoll;
            if (audioClipDamageOrk != null) audioSourceOrk.PlayOneShot(audioClipDamageOrk, audioSourceOrk.volume);
            if (Health <= 0 && !isDead) Die();
        }
        if (other.CompareTag("Grus"))
        {
            isDamage = true;
            damageTime = 0.5f;
            Health -= Grus.powerDamage;
            if (audioClipDamageOrk != null) audioSourceOrk.PlayOneShot(audioClipDamageOrk, audioSourceOrk.volume);
            if (Health <= 0 && !isDead) Die();
        }        

        if (other.CompareTag("Boulder") && !isDead)
        {
            Die();
        }

        if (other.CompareTag("Block") && !isDead)
        {
            isMove = false;
        }
    }

    void KickOrk()
    {
        if (isDead || isAttacking) return;

        bool closeToPlayer = Vector2.Distance(transform.position, player[0].position) <= DistanseKick ||
                             Vector2.Distance(transform.position, player[1].position) <= DistanseKick;

        if (closeToPlayer && cooldownTime <= 0)
        {
            isAttacking = true;
            isMove = false;
            cooldownTime = cooldownDelay;
            anim.SetTrigger("Kick");
            anim.SetBool("isWalk", false);
            if (audioClipKickOrk != null) audioSourceOrk.PlayOneShot(audioClipKickOrk, audioSourceOrk.volume);
        }
        else
        {
            isMove = true;
        }
    }

    public void StartKick()
    {
        Kick.SetActive(true);
    }

    public void EndKick()
    {
        Kick.SetActive(false);
    }

    public void EndKickAnimation()
    {
        isAttacking = false;
        isMove = true;
    }

    public void FinalKick()
    {
        isMove = true;
        isAttacking = false;
    }

    private void Die()
    {
        isDead = true;
        isMove = false;
        anim.SetTrigger("Dead");
        anim.SetBool("isWalk", false);
        if (audioClipDeathOrk != null) audioSourceOrk.PlayOneShot(audioClipDeathOrk, audioSourceOrk.volume);
        Arra.XPArra += 50;
        Grus.XPGrus += 50;
    }

    public void DeadEnemy()
    {
        Destroy(gameObject);
    }

    void Damage()
    {
        sr.color = new Color(1f, 0.25f, 0.25f, 1f);
        damageTime -= Time.deltaTime;

        if (damageTime <= 0)
        {
            isDamage = false;
            sr.color = new Color(1f, 1f, 1f, 1f);
        }
    }
    
    void PlayWalkSound()
    {
        if (isMove)
        {
            walkSoundTimer += Time.deltaTime;
            if (walkSoundTimer >= walkSoundInterval)
            {
                audioSourceOrk.PlayOneShot(audioClipWalkOrk, audioSourceOrk.volume);
                walkSoundTimer = 0f;
            }
        }
        else
        {
            walkSoundTimer = walkSoundInterval;
        }
    }
}
