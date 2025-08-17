using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Arra : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isMove = false;
    private bool isMoving = false;
    public static bool isRotateArra;

    [Header("Is Grounded")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Pause Menu")]
    public GameObject PauseObject;

    [Header("Animator")]
    public Animator animator;

    [Header("Attack")]
    public GameObject bollPrefab;
    public Transform spawnBoll;
    private float cooldownTime;
    public float cooldownMax = 3f;
    private float attackCooldownTime;

    [Header("Block")]
    public float blockCooldownTime;
    public float blockCooldownMax = 15f;
    public GameObject block;
    public static int BlockHPArra;
    public static bool isBlockArra;

    [Header("Damage")]
    public SpriteRenderer sr;
    bool isDamage = false;
    float damageTime = 0f; 

    [Header("Characteristics")]
    public int maxHealthArra = 100;
    public int HealthArra;
    public static int XPArra;
    public static int maxXPArra = 100;
    public int LevelArra = 1;

    [Header("UI Characteristics")]
    public Image HealthImage;
    public Image XPImage;
    public Text LevelText;
    public Image blockCooldownTimeImage;
    public Image attackCooldownTimeImage;
    public int savedSceneIndexArra;

    [Header("Settings Audio")]
    public AudioSource AudioWalk;
    public AudioClip impact;
    public AudioClip attackClip;   
    public AudioClip damageClip;  
    public AudioClip blockClip;
    public AudioClip deathClip;
    public AudioClip flyClip;
    public AudioClip LevelUpClip;
    public float VolumeAudioSoundd;

    // Таймер для кроків
    private float walkSoundTimer = 0f;
    public float walkSoundInterval = 0.35f;

    public static bool isPause = false;

    void Start()
    {
        cooldownTime = 3f;
        attackCooldownTime = cooldownTime;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        HealthArra = maxHealthArra;
        blockArra.BlockHP = blockArra.staticMaxBlockHP;

        isMove = true;
        isMoving = false;
        isPause = false;
        isBlockArra = false;

        HealthArra = PlayerPrefs.GetInt("HealthArra", maxHealthArra);
        XPArra = PlayerPrefs.GetInt("XPArra", 0);
        maxXPArra = PlayerPrefs.GetInt("maxXPArra", 100);
        LevelArra = PlayerPrefs.GetInt("LevelArra", 0);
        maxHealthArra = PlayerPrefs.GetInt("maxHealthArra", 100);
        FloraBoll.DamageBoll = PlayerPrefs.GetInt("DamageBoll", 10);
        blockArra.staticMaxBlockHP = PlayerPrefs.GetInt("MaxBlockHP", 100);
        AudioWalk.volume = PlayerPrefs.GetFloat("gameVolume", 0.5f) * 1.5f;
        savedSceneIndexArra = PlayerPrefs.GetInt("savedSceneIndex", 2);
        PlayerPrefs.SetInt("savedSceneIndex", savedSceneIndexArra);
    }

    void Update()
    {
        if (isPause == false)
        { 
            VolumeAudioSoundd = AudioWalk.volume;
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
            MoveArra();
            RotateArra();
            Attack();
            Block();
            LevelUp();
            UpHealthArra();
            CharacterUI();

            if (isDamage) Damage();
            else sr.color = new Color(1f, 1f, 1f, 1f);

            PlayWalkSound();

            if (Input.GetKey(KeyCode.Escape))
            {
                PauseObject.SetActive(true);
                isPause = true;
            }    
            if (Pause.isExit) SaveDataArra();  
        }
        

    }

    
    void PlayWalkSound()
    {
        if (isMoving && isGrounded)
        {
            walkSoundTimer += Time.deltaTime;
            if (walkSoundTimer >= walkSoundInterval)
            {
                AudioWalk.PlayOneShot(impact, AudioWalk.volume);
                walkSoundTimer = 0f;
            }
        }
        else
        {
            walkSoundTimer = walkSoundInterval;
        }
    }

    // Перевірка тригера
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            isMove = false;
        }
        if (other.CompareTag("Trap"))
        {
            if (!isBlockArra)
            {
                HealthArra -= 10;
                if (HealthArra <= 0)
                {
                    DeathArra();
                }
                isDamage = true;
                damageTime = 0.5f;
                if (damageClip != null) AudioWalk.PlayOneShot(damageClip, AudioWalk.volume);
            }

        }
        if (other.CompareTag("Boulder"))
        {
            DeathArra();
        }

        if (other.CompareTag("Enemy"))
        {
            if (!isBlockArra)
            {
                HealthArra -= 10;
                if (HealthArra <= 0)
                {
                    DeathArra();
                }
                isDamage = true;
                damageTime = 0.5f;
                if (damageClip != null) AudioWalk.PlayOneShot(damageClip, AudioWalk.volume);

            }
        }

        if (other.CompareTag("Gul"))
        {
            if (!isBlockArra)
            {
                HealthArra -= 10;
                if (HealthArra <= 0)
                {
                    DeathArra();
                }
                isDamage = true;
                damageTime = 0.5f;
                if (damageClip != null) AudioWalk.PlayOneShot(damageClip, AudioWalk.volume);

            }
        }

        if (other.CompareTag("Ork"))
        {
            if (!isBlockArra)
            {
                HealthArra -= 20;
                if (HealthArra <= 0)
                {
                    DeathArra();
                }
                isDamage = true;
                damageTime = 0.5f;
                if (damageClip != null) AudioWalk.PlayOneShot(damageClip, AudioWalk.volume);

            }
        }
    }

    // Рух
    void MoveArra()
    {
        if (Input.GetKey(KeyCode.D) && isDead == false)
        {
            isMoving = true;
            if (isMove) transform.localPosition += transform.right * speed * Time.deltaTime;
            animator.SetBool("isWalk", true);

            if (!isGrounded)
            {
                animator.SetBool("isJump", true);
                animator.SetBool("isWalk", false);
                
            }
        }
        else if (Input.GetKey(KeyCode.A) && isDead == false)
        {
            isMoving = true;
            if (isMove) transform.localPosition += transform.right * -speed * Time.deltaTime;
            animator.SetBool("isWalk", true);

            if (!isGrounded)
            {
                animator.SetBool("isJump", true);
                animator.SetBool("isWalk", false);
                
            }
        }
        else
        {
            isMoving = false;
            animator.SetBool("isWalk", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && isDead == false)
        {
            isMoving = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
        }
        if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A)))
        {
            speed = 8f;
            walkSoundInterval = 0.4f; 
        }
        else
        {
            speed = 5f;
            walkSoundInterval = 0.6f; 
        }

        animator.SetBool("isJump", !isGrounded);
    }

    void RotateArra()
    {
        float mouseX = Input.mousePosition.x;
        float screenMiddle = Screen.width / 2f;

        if (mouseX < screenMiddle)
        {
            isRotateArra = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            isRotateArra = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Атака
    void Attack()
    {
        cooldownTime += Time.deltaTime;
        attackCooldownTime = cooldownTime; 

        if (Input.GetMouseButtonDown(0) && cooldownTime >= cooldownMax && isMoving == false)
        {
            cooldownTime = 0f;
            attackCooldownTime = cooldownTime; 
            animator.SetTrigger("isAttack");
            
            if (attackClip != null) AudioWalk.PlayOneShot(attackClip, AudioWalk.volume);
        }
    }

    public void OnAttackButtonPressed()
    {
        Instantiate(bollPrefab, spawnBoll.position, spawnBoll.rotation);
    }

    //Block
    void Block()
    {
        block.transform.localPosition = transform.localPosition;
        blockCooldownTime += Time.deltaTime;
        if (Input.GetMouseButtonDown(1) && blockCooldownTime >= blockCooldownMax && isMoving == false)
        {
            if (blockClip != null) AudioWalk.PlayOneShot(blockClip, AudioWalk.volume);
            blockCooldownTime = 0f;
            animator.SetTrigger("isBlock");
            isBlockArra = true;
            block.SetActive(true);
            blockArra.BlockHP = blockArra.staticMaxBlockHP;
        }
    }

    public void OnBlockButtonPressed()
    {
        block.SetActive(true);
        isBlockArra = true;
        blockArra.BlockHP = blockArra.staticMaxBlockHP;
        blockCooldownTime = 0f;
    }

    // Смерть персонажа
    bool isDead = false;
    public void DeathArra()
    {
        if (isDead) return; // якщо вже мертвий, вихід
        isDead = true;

        if (deathClip != null) AudioWalk.PlayOneShot(deathClip, AudioWalk.volume);
        animator.SetTrigger("isDeath");
        animator.SetBool("isWalk", false);
        animator.SetBool("isJump", false);
        isMove = false;
    }

    public void OnDeathAnimationFinished()
    {
        SceneManager.LoadScene(0);
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

    void LevelUp()
    {
        switch (LevelArra)
        {
            case 1:
                maxXPArra = 100;
                maxHealthArra = 100;
                blockCooldownMax = 15;
                cooldownMax = 3f;
                blockArra.staticMaxBlockHP = 100;
                FloraBoll.DamageBoll = 10;
                break;
            case 2:
                maxXPArra = 200;
                maxHealthArra = 120;
                blockCooldownMax = 14;
                cooldownMax = 2.75f;
                blockArra.staticMaxBlockHP = 110;
                FloraBoll.DamageBoll = 15;
                break;
            case 3:
                maxXPArra = 300;
                maxHealthArra = 140;
                blockCooldownMax = 13;
                cooldownMax = 2.5f;
                blockArra.staticMaxBlockHP = 120;
                FloraBoll.DamageBoll = 20;
                break;
            case 4:
                maxXPArra = 400;
                maxHealthArra = 160;
                blockCooldownMax = 12;
                cooldownMax = 2.25f;
                blockArra.staticMaxBlockHP = 130;
                FloraBoll.DamageBoll = 25;
                break;
            case 5:
                maxXPArra = 500;
                maxHealthArra = 180;
                blockCooldownMax = 11;
                cooldownMax = 2f;
                blockArra.staticMaxBlockHP = 140;
                FloraBoll.DamageBoll = 30;
                break;
            case 6:
                maxXPArra = 600;
                maxHealthArra = 200;
                blockCooldownMax = 10;
                cooldownMax = 1.75f;
                blockArra.staticMaxBlockHP = 150;
                FloraBoll.DamageBoll = 35;
                break;
            case 7:
                maxXPArra = 700;
                maxHealthArra = 220;
                blockCooldownMax = 9;
                cooldownMax = 1.5f;
                blockArra.staticMaxBlockHP = 160;
                FloraBoll.DamageBoll = 40;
                break;
            case 8:
                maxXPArra = 800;
                maxHealthArra = 240;
                blockCooldownMax = 8;
                cooldownMax = 1.25f;
                blockArra.staticMaxBlockHP = 170;
                FloraBoll.DamageBoll = 45;
                break;
            case 9:
                maxXPArra = 900;
                maxHealthArra = 260;
                blockCooldownMax = 7;
                cooldownMax = 1f;
                blockArra.staticMaxBlockHP = 180;
                FloraBoll.DamageBoll = 50;
                break;
            case 10:
                maxXPArra = 1000;
                maxHealthArra = 300;
                blockCooldownMax = 6;
                cooldownMax = 0.75f;
                blockArra.staticMaxBlockHP = 190;
                FloraBoll.DamageBoll = 55;
                break;
        }
        if (XPArra >= maxXPArra && LevelArra < 10)
        { 
            LevelArra++;
            HealthArra = maxHealthArra;
            blockArra.BlockHP = blockArra.staticMaxBlockHP;
            if (LevelUpClip != null) AudioWalk.PlayOneShot(LevelUpClip, AudioWalk.volume);
        }
        if (LevelArra >= 10)
        {
            LevelArra = 10; 
            
        }
    }

    float UpHealthArraTime = 0f;
    void UpHealthArra()
    {
        UpHealthArraTime += Time.deltaTime;
        if (UpHealthArraTime >= 1f)
        {
            HealthArra += 2;
            if (HealthArra > maxHealthArra) HealthArra = maxHealthArra;
            UpHealthArraTime = 0f;
        }
    }

    void CharacterUI()
    {
        if (LevelArra < 10)LevelText.text = LevelArra.ToString();
        else LevelText.text = "MAX";

        float healthPercent = Mathf.Clamp01((float)HealthArra / maxHealthArra);
        HealthImage.rectTransform.sizeDelta = new Vector2(580 * healthPercent, HealthImage.rectTransform.sizeDelta.y);

        float xpPercent = Mathf.Clamp01((float)XPArra / maxXPArra);
        XPImage.rectTransform.sizeDelta = new Vector2(580 * xpPercent, XPImage.rectTransform.sizeDelta.y);

        float blockPercent = Mathf.Clamp01(blockCooldownTime / blockCooldownMax);
        blockCooldownTimeImage.rectTransform.sizeDelta = new Vector2(blockCooldownTimeImage.rectTransform.sizeDelta.x, 290 * blockPercent);

        float attackPercent = Mathf.Clamp01(attackCooldownTime / cooldownMax);
        attackCooldownTimeImage.rectTransform.sizeDelta = new Vector2(attackCooldownTimeImage.rectTransform.sizeDelta.x, 290 * attackPercent);
    }

    public void SaveDataArra()
    {
        PlayerPrefs.SetInt("XPArra", XPArra);
        PlayerPrefs.SetInt("maxXPArra", maxXPArra);
        PlayerPrefs.SetInt("LevelArra", LevelArra);
        PlayerPrefs.SetInt("maxHealthArra", maxHealthArra);
        PlayerPrefs.SetInt("DamageBoll", FloraBoll.DamageBoll);
        PlayerPrefs.SetInt("MaxBlockHP", blockArra.staticMaxBlockHP);
        PlayerPrefs.SetInt("savedSceneIndex", savedSceneIndexArra);
        PlayerPrefs.Save();
    }
}
