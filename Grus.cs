using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Grus : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isMove = false;
    private bool isMoving = false;
    public static bool isRotateGrus;

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
    private float cooldownTime;
    public float cooldownMax = 3f;
    private float attackCooldownTime;
    public int powerDamage = 20;
    public GameObject attackEffect;

    [Header("Block")]
    public static bool isBlockGrus;

    [Header("Damage")]
    public SpriteRenderer sr;
    bool isDamage = false;
    float damageTime = 0f; 

    [Header("Characteristics")]
    public int maxHealthGrus = 100;
    public int HealthGrus;
    public static int XPGrus;
    public static int maxXPGrus = 100;
    public int LevelGrus = 1;

    [Header("UI Characteristics")]
    public Image HealthImage;
    public Image XPImage;
    public Text LevelText;
    public Image blockCooldownTimeImage;
    public Image attackCooldownTimeImage;

    [Header("Settings Audio")]
    public AudioSource AudioWalk;
    public AudioClip impact;
    public AudioClip attackClip;   
    public AudioClip damageClip;  
    public AudioClip deathClip;
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
        HealthGrus = maxHealthGrus;

        isMove = true;
        isMoving = false;
        isPause = false;

        HealthGrus = PlayerPrefs.GetInt("HealthGrus", maxHealthGrus);
        XPGrus = PlayerPrefs.GetInt("XPGrus", 0);
        maxXPGrus = PlayerPrefs.GetInt("maxXPGrus", 100);
        LevelGrus = PlayerPrefs.GetInt("LevelGrus", 0);
        maxHealthGrus = PlayerPrefs.GetInt("maxHealthGrus", 100);
        powerDamage = PlayerPrefs.GetInt("DamageGrus", 20);
        AudioWalk.volume = PlayerPrefs.GetFloat("gameVolume", 0.5f) * 1.5f;
    }

    void Update()
    {
        if (isPause == false)
        {
            VolumeAudioSoundd = AudioWalk.volume;
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
            if (isBlockGrus == false) MoveGrus();
            RotateGrus();
            Attack();
            Block();
            LevelUp();
            UpHealthGrus();
            CharacterUI();

            if (isDamage) Damage();
            else sr.color = new Color(1f, 1f, 1f, 1f);

            PlayWalkSound();

            if (Input.GetKey(KeyCode.Escape))
            {
                PauseObject.SetActive(true);
                isPause = true;
            }
        
            if (Pause.isExit) SaveDataGrus();


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
        }
        if (other.CompareTag("Trap"))
        {
            HealthGrus -= 10;
            if (HealthGrus <= 0)
            {
                DeathGrus();
            }
            isDamage = true;
            damageTime = 0.5f;
            if (damageClip != null) AudioWalk.PlayOneShot(damageClip, AudioWalk.volume);
        }
        if (other.CompareTag("Enemy"))
        {
            if (isBlockGrus == false)
            { 
                HealthGrus -= 10;
                if (HealthGrus <= 0)
                {
                    DeathGrus();
                }
                isDamage = true;
                damageTime = 0.5f; 
                if (damageClip != null) AudioWalk.PlayOneShot(damageClip, AudioWalk.volume);
            }
            
        }
    }

    // Рух
    void MoveGrus()
    {
        if (Input.GetKey(KeyCode.D) )
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
        else if (Input.GetKey(KeyCode.A))
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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isMoving = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
        }
        if ((Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A)))
        {
            speed = 8f;
            walkSoundInterval = 0.4f;
            animator.SetBool("isRun", true);
            animator.SetBool("isWalk", false);

            if (!isGrounded)
            {
                animator.SetBool("isJump", true);
                animator.SetBool("isRun", false);
            } 
        }
        else
        {
            animator.SetBool("isRun", false);
            speed = 5f;
            walkSoundInterval = 0.6f;
        }

        animator.SetBool("isJump", !isGrounded);
    }

    void RotateGrus()
    {
        float mouseX = Input.mousePosition.x;
        float screenMiddle = Screen.width / 2f;

        if (mouseX < screenMiddle)
        {
            isRotateGrus = false;
            transform.localScale = new Vector3(-1, 1, 1);
            attackEffect.transform.position = transform.position + new Vector3(-4.5f, 0f, 0f);
        }
        else
        {
            isRotateGrus = true;
            transform.localScale = new Vector3(1, 1, 1);
            attackEffect.transform.position = transform.position + new Vector3(4.5f, 0f, 0f);
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
            animator.SetTrigger("Attack");
            
            if (attackClip != null) AudioWalk.PlayOneShot(attackClip, AudioWalk.volume);
        }
    }
    public void OnAttackAnimationFinished()
    {
        attackEffect.SetActive(true);
    }
    public void OnAttackEffectFinished()
    {
        attackEffect.SetActive(false);
    }

    //Block
    void Block()
    {

        if (Input.GetMouseButton(1) && isMoving == false)
        {
            isBlockGrus = true;
        }
        else
        {
            isBlockGrus = false;
        }
        animator.SetBool("isBlock", isBlockGrus);
    }

    // Смерть персонажа
    bool isDead = false;
    public void DeathGrus()
    {
        if (isDead) return; // якщо вже мертвий, вихід
        isDead = true;

        if (deathClip != null) AudioWalk.PlayOneShot(deathClip, AudioWalk.volume);
        animator.SetTrigger("isDeath");
        isMove = false;
    }
    public void OnDeathAnimationFinished()
    {
        SceneManager.LoadScene(0);
        SaveDataGrus();
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
        if (XPGrus >= maxXPGrus && LevelGrus <= 10)
        {
            XPGrus -= maxXPGrus;
            maxXPGrus += 100;
            LevelGrus++;
            maxHealthGrus += 10;
            HealthGrus = maxHealthGrus;
            cooldownMax -= 0.25f;
            if (LevelUpClip != null) AudioWalk.PlayOneShot(LevelUpClip, AudioWalk.volume);
            powerDamage += 5;
        }
    }

    float UpHealthGrusTime = 0f;
    void UpHealthGrus()
    {
        UpHealthGrusTime += Time.deltaTime;
        if (UpHealthGrusTime >= 1f)
        {
            HealthGrus += 2;
            if (HealthGrus > maxHealthGrus) HealthGrus = maxHealthGrus;
            UpHealthGrusTime = 0f;
        }
    }

    void CharacterUI()
    {
        LevelText.text = LevelGrus.ToString();

        float healthPercent = Mathf.Clamp01((float)HealthGrus / maxHealthGrus);
        HealthImage.rectTransform.sizeDelta = new Vector2(580 * healthPercent, HealthImage.rectTransform.sizeDelta.y);

        float xpPercent = Mathf.Clamp01((float)XPGrus / maxXPGrus);
        XPImage.rectTransform.sizeDelta = new Vector2(580 * xpPercent, XPImage.rectTransform.sizeDelta.y);

        float blockPercent = 1;
        blockCooldownTimeImage.rectTransform.sizeDelta = new Vector2(blockCooldownTimeImage.rectTransform.sizeDelta.x, 290 * blockPercent);

        float attackPercent = Mathf.Clamp01(attackCooldownTime / cooldownMax);
        attackCooldownTimeImage.rectTransform.sizeDelta = new Vector2(attackCooldownTimeImage.rectTransform.sizeDelta.x, 290 * attackPercent);
    }

    public void SaveDataGrus()
    {
        PlayerPrefs.SetInt("XPGrus", XPGrus);
        PlayerPrefs.SetInt("maxXPGrus", maxXPGrus);
        PlayerPrefs.SetInt("LevelGrus", LevelGrus);
        PlayerPrefs.SetInt("maxHealthGrus", maxHealthGrus);
        PlayerPrefs.SetInt("DamageGrus", powerDamage);
        PlayerPrefs.Save();
    }
}
