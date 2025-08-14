using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockArra : MonoBehaviour
{
    public AudioSource AudioCrack;
    public AudioClip impact;

    public int maxBlockHP = 100;

    public static int BlockHP;
    public int visibleBlockHP;
    public static int staticMaxBlockHP;
    public float blockCooldownMax = 15f;
    public SpriteRenderer blockSprite;
    public Sprite[] versionBlockSprite;

    // Додаємо змінну для контролю стадії, щоб звук не грався кожен кадр
    private int lastSoundStage = -1;

    void Start()
    {
        blockSprite = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
        BlockHP = maxBlockHP;
        lastSoundStage = GetSoundStage(BlockHP);
    }

    void Update()
    {
        AudioCrack.volume = PlayerPrefs.GetFloat("gameVolume", 0.5f);
        staticMaxBlockHP = maxBlockHP;

        int currentStage = GetSoundStage(BlockHP);

        if (currentStage == 0)
        {
            blockSprite.sprite = versionBlockSprite[0];
        }
        else if (currentStage == 1)
        {
            blockSprite.sprite = versionBlockSprite[1];
        }
        else if (currentStage == 2)
        {
            blockSprite.sprite = versionBlockSprite[2];
        }
        else if (currentStage == 3)
        {
            blockSprite.sprite = versionBlockSprite[3];
        }
        else if (currentStage == 4)
        {
            blockSprite.sprite = versionBlockSprite[4];
        }

        if (currentStage == 5)
        {
            AudioCrack.PlayOneShot(impact, AudioCrack.volume);
            gameObject.SetActive(false);
            Arra.isBlockArra = false;
        }

        visibleBlockHP = BlockHP;
        // Відтворювати звук тільки якщо стадія змінилася
        if (currentStage != lastSoundStage && currentStage > 0)
        {
            AudioCrack.PlayOneShot(impact, AudioCrack.volume);
            lastSoundStage = currentStage;
        }

        
    }

    int GetSoundStage(int hp)
    {
        if (hp >= (maxBlockHP * 0.8f))
            return 0;
        else if (hp >= (maxBlockHP * 0.6f))
            return 1;
        else if (hp >= (maxBlockHP * 0.4f))
            return 2;
        else if (hp >= (maxBlockHP * 0.2f))
            return 3;
        else if (hp > 0)
            return 4;
        else
            return 5;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Trap"))
        {
            BlockHP -= 10; // Замінити на силу удару моба
            if (BlockHP <= 0)
            {
                Arra.isBlockArra = false;
            }
        }
    }
}