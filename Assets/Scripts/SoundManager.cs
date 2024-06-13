using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _SFXSource;

    [Header("Music")]
    public AudioClip forestSpooky;

    [Header("Ambience")]
    public AudioClip crickets;

    [Header("SFX")]
    public AudioClip whoosh;
    public AudioClip owl;
    public AudioClip wolfHowl;
    public AudioClip rest;

    [Header("Combat Sounds")]
    public AudioClip slash;
    public AudioClip strike;
    public AudioClip bite;

    public AudioClip heal;
    public AudioClip bleed;
    public AudioClip burn;
    public AudioClip stun;

    [Header("Movement Sounds")]
    public AudioClip moveSound;

    private AudioClip _selectedSound;


    private static SoundManager instance;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator PlayCampSounds()
    {
        int timer;
        while (true)
        {
            timer = Random.Range(20, 40);
            yield return new WaitForSeconds(timer);
            if(Random.Range(0, 2) == 0)
            {
                _SFXSource.PlayOneShot(owl);
            }
            else
            {
                _SFXSource.PlayOneShot(wolfHowl);
            }
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlayMoveSound()
    {
        PlaySound(moveSound);
    }

    public void PrepareSound(AudioClip clip)
    {
        _selectedSound = clip;
    }

    public void PlayPreparedSound()
    {
        _SFXSource.PlayOneShot(_selectedSound);
    }

    public void PlayAttackSound()
    {
        if(Random.Range(0, 2) == 0)
        {
            PlaySound(slash);
        }
        else
        {
            PlaySound(strike);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        _SFXSource.PlayOneShot(clip);
    }
}
