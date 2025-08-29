using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicProg : MonoBehaviour
{
    public AudioSource MusicRadio;
    public AudioClip[] MusicClips;
    public AudioClip BossMusicClip;

    private Coroutine musicCoroutine;

    void Start()
    {
        MusicRadio.volume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        musicCoroutine = StartCoroutine(PlayRadio());
    }

    void Update()
    {
        // Перевіряємо, чи треба перемкнути музику
        if (TriggerBoss.isBoss)
        {
            if (musicCoroutine != null)
            {
                StopCoroutine(musicCoroutine);
                musicCoroutine = null;
            }

            if (MusicRadio.clip != BossMusicClip)
            {
                musicCoroutine = StartCoroutine(PlayBossMusic());
            }
        }
        else
        {
            if (musicCoroutine == null || MusicRadio.clip == BossMusicClip)
            {
                if (musicCoroutine != null) StopCoroutine(musicCoroutine);
                musicCoroutine = StartCoroutine(PlayRadio());
            }
        }
    }

    IEnumerator PlayRadio()
    {
        while (!TriggerBoss.isBoss)
        {
            int i = Random.Range(0, MusicClips.Length);
            if (MusicClips[i] != null)
            {
                MusicRadio.clip = MusicClips[i];
                MusicRadio.Play();
                yield return new WaitForSeconds(MusicClips[i].length);
            }
            else
            {
                Debug.LogWarning("Null AudioClip at index " + i);
                yield return null;
            }
        }
    }

    IEnumerator PlayBossMusic()
    {
        while (TriggerBoss.isBoss)
        {
            if (BossMusicClip != null)
            {
                MusicRadio.clip = BossMusicClip;
                MusicRadio.Play();
                yield return new WaitForSeconds(BossMusicClip.length);
            }
            else
            {
                yield return null;
            }
        }
    }
}
